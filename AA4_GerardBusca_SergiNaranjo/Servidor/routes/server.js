const express = require("express");
const http = require("http");
const { Server } = require("socket.io");
const path = require("path");

const app = express();
const server = http.createServer(app);
const io = new Server(server);

const PORT = 3000;

app.use(express.static(path.join(__dirname, "public")));

app.get("/", (req, res) => {
  res.sendFile(path.join(__dirname, "public/index.html"));
});

const rooms = {};

function updateRoomState(roomName) {
  const count = rooms[roomName]?.length || 0;

  if (count === 0) {
    io.to(roomName).emit("roomPaused", {});
    console.log(`Sala ${roomName} pausada`);
  } else {
    io.to(roomName).emit("roomResumed", {});
    console.log(`Sala ${roomName} reanudada`);
  }
}

io.on("connection", socket => {
  console.log("Usuario conectado:", socket.id);

  socket.on("joinRoom", ({ roomName }) => {
    if (!rooms[roomName]) rooms[roomName] = [];

    rooms[roomName].push(socket.id);
    socket.join(roomName);

    io.to(roomName).emit("roomUpdate", {
      players: rooms[roomName].length
    });

    updateRoomState(roomName);
  });

  socket.on("leaveRoom", ({ roomName }) => {
    if (!rooms[roomName]) return;

    rooms[roomName] = rooms[roomName].filter(id => id !== socket.id);
    socket.leave(roomName);

    io.to(roomName).emit("roomUpdate", {
      players: rooms[roomName].length
    });

    updateRoomState(roomName);

    if (rooms[roomName].length === 0) {
      delete rooms[roomName];
    }
  });

  socket.on("playerMove", direction => {
    socket.broadcast.emit("unityMove", direction);
  });

  socket.on("disconnect", () => {
    console.log("Usuario desconectado:", socket.id);

    for (const roomName in rooms) {
      if (rooms[roomName].includes(socket.id)) {
        rooms[roomName] = rooms[roomName].filter(id => id !== socket.id);

        io.to(roomName).emit("roomUpdate", {
          players: rooms[roomName].length
        });

        updateRoomState(roomName);

        if (rooms[roomName].length === 0) {
          delete rooms[roomName];
        }
      }
    }
  });
});

server.listen(PORT, () => {
  console.log(`Servidor activo en http://localhost:${PORT}`);
});
