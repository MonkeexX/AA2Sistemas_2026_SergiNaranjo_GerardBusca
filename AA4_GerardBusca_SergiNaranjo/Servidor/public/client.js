const socket = io();

document.getElementById("joinBtn").onclick = () => {
  const roomName = document.getElementById("roomName").value.trim();
  const roomMsg = document.getElementById("roomMsg").value.trim();

  if (!roomName) {
    alert("Debes escribir el nombre de la sala");
    return;
  }

  socket.emit("joinRoom", { roomName, message: roomMsg });
};

socket.on("roomUpdate", ({ players, message }) => {
  document.getElementById("players").innerText = players;
  document.getElementById("roomMessage").innerText = message;
});
