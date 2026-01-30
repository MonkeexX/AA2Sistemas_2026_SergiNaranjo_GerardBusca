const { Router } = require("express");
const router = Router();
const path = require("path");

// Página principal
router.get("/", (req, res) => {
  res.sendFile(path.join(__dirname, "../public/index.html"));
});

// Página de login
router.get("/login", (req, res) => {
  res.sendFile(path.join(__dirname, "../public/login.html"));
});

// Endpoint para procesar login
router.post("/login", (req, res) => {
  const { username, password } = req.body;

  // Aquí puedes poner tu lógica real de autenticación
  if (username === "admin" && password === "1234") {
    // Usuario correcto
    res.json({ ok: true });
  } else {
    res.json({ ok: false });
  }
});

module.exports = router;