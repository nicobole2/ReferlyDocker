<script setup>
const links = [
  { text: 'Términos y Condiciones', href: '#' },
  { text: 'Política de Privacidad', href: '#' },
  { text: 'Condiciones de contratación', href: '#' },
  { text: 'Preguntas frecuentes', href: '#' },
  { text: 'Ofertas de Empleo', href: '#' },
  { text: 'Política de Gestión de Calidad', href: '#' }
]
</script>

<template>
  <footer class="footer">
    <div class="footer-content">
    <div class="footer-links">
      <a v-for="link in links" 
          :key="link.text" 
          :href="link.href">
        {{ link.text }}
      </a>
    </div>
    </div>
  </footer>
</template>

<style scoped>
.footer {
  background-color: #f8fafc;
  padding: 3rem 0;
  margin-top: 4rem;
  border-top: 1px solid #e5e7eb;
}

.footer-content {
  max-width: 1200px;
  margin: 0 auto;
  padding: 0 2rem;
  display: flex;
  flex-direction: column;
  align-items: center;
  gap: 2rem;
}

.app-download {
  text-align: center;
}

.app-download h3 {
  font-size: 1.5rem;
  color: #1a1a1a;
  margin-bottom: 1rem;
}

.store-buttons {
  display: flex;
  gap: 1rem;
  justify-content: center;
  flex-wrap: wrap;
}

.store-button img {
  height: 40px;
  object-fit: contain;
}

.social-links {
  display: flex;
  gap: 1.5rem;
  margin: 2rem 0;
}

.social-icon {
  width: 40px;
  height: 40px;
  border-radius: 50%;
  background-color: #e5e7eb;
  display: flex;
  align-items: center;
  justify-content: center;
  transition: background-color 0.2s;
}

.social-icon:hover {
  background-color: #646cff;
}

.icon {
  width: 20px;
  height: 20px;
  background-size: contain;
  background-repeat: no-repeat;
  background-position: center;
  filter: invert(0.5);
}

.social-icon:hover .icon {
  filter: invert(1);
}

.footer-links {
  display: flex;
  gap: 1.5rem;
  flex-wrap: wrap;
  justify-content: center;
}

.footer-links a {
  color: #666;
  text-decoration: none;
  font-size: 0.9rem;
}

.footer-links a:hover {
  color: #646cff;
}

@media (max-width: 768px) {
  .footer-content {
    padding: 0 1rem;
  }

  .store-buttons {
    flex-direction: column;
    align-items: center;
  }

  .footer-links {
    flex-direction: column;
    align-items: center;
    text-align: center;
    gap: 1rem;
  }
}
</style>