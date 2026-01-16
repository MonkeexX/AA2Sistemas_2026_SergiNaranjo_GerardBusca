const express = require("express");
const http = require("http");
const { Server } = require("socket.io");
const path = require("path");

const app = express();
const server = http.createServer(app);
const io = new Server(server);

app.use(express.json());
app.use(express.static(path.join(__dirname, "public"))); 

app.get("/", (req, res) => {
    res.sendFile(path.join(__dirname, "public/index.html"));
});

// Manejo de salas
const rooms = {};

io.on("connection", (socket) => {
    console.log("Usuario conectado");

    socket.on("joinRoom", ({ roomName, message }) => {
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

server.listen(3000, () => console.log("Servidor corriendo en http://localhost:3000"));
