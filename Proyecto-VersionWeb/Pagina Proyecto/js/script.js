// Variable global para almacenar el PDF seleccionado del manga clickeado
let selectedPdf = "";

// ===== Código para el slider =====
let sliderPositions = {};

function slideLeft(sliderId) {
  const sliderContainer = document.getElementById(sliderId);
  const visibleWidth = sliderContainer.parentElement.offsetWidth;
  sliderPositions[sliderId] = sliderPositions[sliderId] || 0;
  sliderPositions[sliderId] = Math.min(sliderPositions[sliderId] + visibleWidth, 0);
  sliderContainer.style.transform = `translateX(${sliderPositions[sliderId]}px)`;
}

function slideRight(sliderId) {
  const sliderContainer = document.getElementById(sliderId);
  const visibleWidth = sliderContainer.parentElement.offsetWidth;
  const totalWidth = sliderContainer.scrollWidth;
  sliderPositions[sliderId] = sliderPositions[sliderId] || 0;
  sliderPositions[sliderId] = Math.max(sliderPositions[sliderId] - visibleWidth, -(totalWidth - visibleWidth));
  sliderContainer.style.transform = `translateX(${sliderPositions[sliderId]}px)`;
}

// ===== Acción para el botón "Descubre lo Increíble" =====
document.getElementById('explore-btn').addEventListener('click', function() {
  document.getElementById('recomendados').scrollIntoView({ behavior: 'smooth' });
});

// ===== Lógica para el Modal de Selección de Tomo =====
// Asigna un listener a todas las tarjetas para que, al hacer clic, se abra el modal
const mangaCards = document.querySelectorAll('.manga-card');
mangaCards.forEach(card => {
  card.addEventListener('click', function() {
    // Guarda el enlace PDF del manga seleccionado (atributo data-pdf)
    selectedPdf = this.getAttribute('data-pdf') || "";
    openModal();
  });
});

function openModal() {
  document.getElementById("volumeModal").style.display = "block";
}

// Cerrar el modal al hacer clic en la "X" o fuera del contenido
document.querySelector("#volumeModal .close").addEventListener('click', function() {
  closeModal();
});
window.addEventListener('click', function(event) {
  const modal = document.getElementById("volumeModal");
  if (event.target === modal) {
    closeModal();
  }
});

function closeModal() {
  document.getElementById("volumeModal").style.display = "none";
  resetModalContent();
}

// Asignar eventos a los botones de volumen
function attachVolumeListeners() {
  const volumeButtons = document.querySelectorAll(".volume-btn");
  volumeButtons.forEach(function(button) {
    button.addEventListener('click', function() {
      const volume = this.getAttribute("data-volume");
      loadVolume(volume);
    });
  });
}
attachVolumeListeners();

// Función para cargar el visor PDF según el tomo seleccionado
function loadVolume(volume) {
  const modalContent = document.querySelector("#volumeModal .modal-content");
  if (volume === "1") {
    // Usa el enlace del PDF almacenado en selectedPdf si existe, o un enlace por defecto.
    const pdfLink = selectedPdf ? selectedPdf : "https://drive.google.com/file/d/1yTpv2PD5css7IS9FSgSRsTI7PbcmVUj1/preview";
    modalContent.innerHTML = `
      <span class="close">&times;</span>
      <h2>Leer Manga: Tomo 1</h2>
      <div class="pdf-viewer">
        <iframe src="${pdfLink}" width="100%" height="600px" frameborder="0"></iframe>
      </div>
      <button id="back-btn">Volver</button>
    `;
  } else {
    modalContent.innerHTML = `
      <span class="close">&times;</span>
      <h2>Leer Manga: Tomo ${volume}</h2>
      <p>Este tomo no está disponible.</p>
      <button id="back-btn">Volver</button>
    `;
  }
  document.querySelector("#volumeModal .close").addEventListener('click', function() {
    closeModal();
  });
  document.getElementById("back-btn").addEventListener('click', function() {
    resetModalContent();
  });
}

function resetModalContent() {
  const modalContent = document.querySelector("#volumeModal .modal-content");
  modalContent.innerHTML = `
    <span class="close">&times;</span>
    <h2>Elige el tomo que quieras leer</h2>
    <div class="volume-options">
      <button class="volume-btn" data-volume="1">Tomo 1</button>
      <button class="volume-btn" data-volume="2">Tomo 2</button>
      <button class="volume-btn" data-volume="3">Tomo 3</button>
    </div>
  `;
  attachVolumeListeners();
  document.querySelector("#volumeModal .close").addEventListener('click', function() {
    closeModal();
  });
}

