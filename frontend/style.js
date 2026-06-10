const API_BASE = window.CLICKHOME_API_BASE;
const STORAGE_KEY = "clickhome_usuario";

// =========================
// CARROSSEL
// =========================
const anuncios = document.querySelector(".anuncios");
const prev = document.querySelector(".prev");
const next = document.querySelector(".next");

if (anuncios && prev && next) {
  prev.addEventListener("click", () => {
    anuncios.scrollBy({ left: -420, behavior: "smooth" });
  });

  next.addEventListener("click", () => {
    anuncios.scrollBy({ left: 420, behavior: "smooth" });
  });

  setInterval(() => {
    anuncios.scrollBy({ left: 420, behavior: "smooth" });

    if (anuncios.scrollLeft + anuncios.clientWidth >= anuncios.scrollWidth) {
      anuncios.scrollTo({ left: 0, behavior: "smooth" });
    }
  }, 4000);
}

// =========================
// TEMA
// =========================
const themeBtn = document.getElementById("themeBtn");
const body = document.body;

if (localStorage.getItem("theme") === "dark") {
  body.classList.add("dark");
  body.classList.remove("light");
  if (themeBtn) themeBtn.textContent = "☀️";
}

if (themeBtn) {
  themeBtn.addEventListener("click", () => {
    body.classList.toggle("dark");
    body.classList.toggle("light");

    const isDark = body.classList.contains("dark");
    themeBtn.textContent = isDark ? "☀️" : "🌙";
    localStorage.setItem("theme", isDark ? "dark" : "light");
  });
}

// =========================
// USUARIO LOGADO NO HEADER
// =========================
function renderizarUsuarioLogado() {
  const nav = document.querySelector("header nav");
  const savedUser = localStorage.getItem(STORAGE_KEY);

  if (!nav || !savedUser) return;

  const user = JSON.parse(savedUser);

  if (user.tipo === "Locador" && !window.location.pathname.endsWith("locador.html")) {
    window.location.href = "locador.html";
    return;
  }

  let welcome = document.getElementById("welcomeUser");

  if (!welcome) {
    welcome = document.createElement("span");
    welcome.id = "welcomeUser";
    welcome.style.color = "white";
    welcome.style.fontWeight = "bold";
    welcome.style.marginRight = "15px";
    welcome.style.display = "inline-flex";
    welcome.style.alignItems = "center";
    nav.insertBefore(welcome, themeBtn || nav.firstChild);
  }

  const primeiroNome = (user.nome || "").trim().split(" ")[0];
  welcome.textContent = `Bem-vindo, ${primeiroNome}`;

  if (user.tipo === "Locador" && !document.getElementById("locadorLink")) {
    const locadorLink = document.createElement("a");
    locadorLink.id = "locadorLink";
    locadorLink.href = "locador.html";
    locadorLink.textContent = "Painel locador";
    nav.insertBefore(locadorLink, themeBtn || nav.firstChild);
  }
}

// =========================
// BOTAO "VER IMOVEIS"
// =========================
const btnVerImoveis = document.getElementById("btnVerImoveis");

if (btnVerImoveis) {
  btnVerImoveis.addEventListener("click", () => {
    const secao = document.getElementById("imoveis") || document.querySelector(".imoveis");

    if (secao) {
      secao.scrollIntoView({ behavior: "smooth" });
    }
  });
}

// =========================
// MAPA
// =========================
let map;
let marker;

const mapContainer = document.getElementById("map");

if (mapContainer && typeof L !== "undefined") {
  const lat = -23.420999;
  const lon = -51.933056;

  map = L.map("map").setView([lat, lon], 13);

  L.tileLayer("https://{s}.tile.openstreetmap.org/{z}/{x}/{y}.png", {
    attribution: "&copy; OpenStreetMap",
  }).addTo(map);

  marker = L.marker([lat, lon])
    .addTo(map)
    .bindPopup("Imobiliaria ClickHome")
    .openPopup();
}

// =========================
// INTEGRACAO COM API
// =========================
async function carregarImoveis() {
  if (!anuncios) return;

  try {
    const response = await fetch(API_BASE + "/imovel");

    if (!response.ok) {
      throw new Error("Erro ao buscar imoveis");
    }

    const imoveis = await response.json();
    anuncios.innerHTML = "";

    imoveis.forEach((imovel) => {
      const card = document.createElement("div");
      card.className = "item";
      card.innerHTML = `
        <img src="${imovel.imagemUrl}" alt="${imovel.titulo}" style="width:100%; height:100%; object-fit:cover; border-radius:8px;" />
      `;
      anuncios.appendChild(card);
    });
  } catch (error) {
    console.error("Erro ao conectar com a API:", error);
    // Mantem as imagens estaticas do HTML caso a API nao responda.
  }
}

carregarImoveis();
renderizarUsuarioLogado();
