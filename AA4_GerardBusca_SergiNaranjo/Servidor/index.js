const express = require("express");
const app = express();

app.set("port", process.env.PORT || 3000);
app.set("json spaces", 2);

const morgan = require("morgan");
app.use(morgan("dev"));

app.use(express.urlencoded({ extended: false }));
app.use(express.json());

const path = require("path");
app.use(express.static(path.join(__dirname, "public")));

const ipHelper = require("ip");
const http = require("http");

const server = http.createServer(app);
const { Server } = require("socket.io");
const io = new Server(server);

app.set("io", io);

app.use(require("./routes/_routes"));

const rooms = {};

io.on("connection", (socket) => {
  console.log("Usuario conectado");

  socket.on("joinRoom", ({ roomName, message }) => {
    if (!roomName) return;

    if (!rooms[roomName]) rooms[roomName] = [];
    rooms[roomName].push(socket.id);

    socket.join(roomName);

    io.to(roomName).emit("roomUpdate", {
      players: rooms[roomName].length,
      message: message || `Bienvenido a ${roomName}`
    });
  });

  socket.on("disconnect", () => {
    for (const room in rooms) {
      rooms[room] = rooms[room].filter(id => id !== socket.id);
      io.to(room).emit("roomUpdate", {
        players: rooms[room].length,
        message: `Jugador se ha desconectado de ${room}`
      });
    }
  });
});

server.listen(app.get("port"), () => {
    const ip = ipHelper.address();
    const port = app.get("port");
    const url = "http://" + ip + ":" + port + "/";
    console.log("Servidor arrancado en la url: " + url);
});
