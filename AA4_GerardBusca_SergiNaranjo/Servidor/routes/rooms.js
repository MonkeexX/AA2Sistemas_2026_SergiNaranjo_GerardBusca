module.exports = (io) => {
  io.on("connection", (socket) => {
    console.log("Usuario conectado");

    socket.on("joinRoom", ({ roomName, message }) => {
      socket.join(roomName);

      const players = io.sockets.adapter.rooms.get(roomName)?.size || 0;

      io.to(roomName).emit("roomUpdate", {
        players,
        message: message || `Bienvenido a la sala ${roomName}`
      });
    });
  });
};