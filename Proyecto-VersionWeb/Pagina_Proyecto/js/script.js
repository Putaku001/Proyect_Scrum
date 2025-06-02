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
