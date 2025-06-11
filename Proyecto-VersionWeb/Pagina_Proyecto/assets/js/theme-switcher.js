document.addEventListener('DOMContentLoaded', function() {
  // DETECCIÓN DEL TEMA INICIAL
  // Verifica si hay un tema guardado en localStorage
  const savedTheme = localStorage.getItem('theme');
  // Detecta las preferencias del sistema operativo
  const systemPrefersDark = window.matchMedia('(prefers-color-scheme: dark)').matches;
  // Decide el tema inicial (guardado > preferencia del sistema > oscuro por defecto)
  const initialTheme = savedTheme || (systemPrefersDark ? 'dark' : 'light');
  
  // APLICAR TEMA INICIAL
  // Añade el atributo data-theme al elemento <html>
  document.documentElement.setAttribute('data-theme', initialTheme);
  
  // CONFIGURAR EL BOTÓN DE CAMBIO
  const themeToggle = document.getElementById('theme-toggle');
  if (themeToggle) {
    // MANEJADOR DE CLICK PARA CAMBIAR TEMA
    themeToggle.addEventListener('click', function() {
      // Obtiene el tema actual
      const currentTheme = document.documentElement.getAttribute('data-theme');
      // Determina el nuevo tema (alterna entre oscuro/claro)
      const newTheme = currentTheme === 'dark' ? 'light' : 'dark';
      
      // APLICAR NUEVO TEMA
      // Actualiza el atributo en el HTML
      document.documentElement.setAttribute('data-theme', newTheme);
      // Guarda la preferencia en localStorage
      localStorage.setItem('theme', newTheme);
      
      // ANIMACIÓN DE FEEDBACK
      // Efecto de "presionado" en el botón
      themeToggle.style.transform = 'scale(0.9)';
      setTimeout(() => {
        themeToggle.style.transform = 'scale(1)';
      }, 200);
    });
  }
  
  // DETECCIÓN DE CAMBIOS EN PREFERENCIAS DEL SISTEMA (OPCIONAL)
  // Si el usuario cambia las preferencias de su sistema, se actualiza (solo si no había elegido un tema manualmente)
  window.matchMedia('(prefers-color-scheme: dark)').addEventListener('change', e => {
    if (!localStorage.getItem('theme')) {
      const newTheme = e.matches ? 'dark' : 'light';
      document.documentElement.setAttribute('data-theme', newTheme);
    }
  });
});