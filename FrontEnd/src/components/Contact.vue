<script setup>
import { ref } from 'vue'

const form = ref({
  name: '',
  email: '',
  subject: '',
  message: ''
})

const status = ref('')

const handleSubmit = async () => {
  status.value = 'sending'
  
  // Simulate API call
  await new Promise(resolve => setTimeout(resolve, 1000))
  
  console.log('Form submitted:', form.value)
  status.value = 'sent'
  
  // Reset form
  form.value = {
    name: '',
    email: '',
    subject: '',
    message: ''
  }
  
  // Reset status after 3 seconds
  setTimeout(() => {
    status.value = ''
  }, 3000)
}
</script>

<template>
  <main class="contact">
    <section class="hero">
      <div class="container">
        <h1>Contacto</h1>
        <p class="subtitle">¿Tienes alguna pregunta? Estamos aquí para ayudarte</p>
      </div>
    </section>

    <section class="contact-form">
      <div class="container">
        <div class="form-container">
          <form @submit.prevent="handleSubmit">
            <div class="form-group">
              <label for="name">Nombre completo</label>
              <input
                id="name"
                type="text"
                v-model="form.name"
                required
                placeholder="Tu nombre"
              />
            </div>

            <div class="form-group">
              <label for="email">Email</label>
              <input
                id="email"
                type="email"
                v-model="form.email"
                required
                placeholder="tu@email.com"
              />
            </div>

            <div class="form-group">
              <label for="subject">Asunto</label>
              <input
                id="subject"
                type="text"
                v-model="form.subject"
                required
                placeholder="¿Sobre qué nos quieres contactar?"
              />
            </div>

            <div class="form-group">
              <label for="message">Mensaje</label>
              <textarea
                id="message"
                v-model="form.message"
                required
                placeholder="Tu mensaje..."
                rows="5"
              ></textarea>
            </div>

            <button 
              type="submit" 
              class="submit-btn"
              :disabled="status === 'sending'"
            >
              {{ status === 'sending' ? 'Enviando...' : 'Enviar mensaje' }}
            </button>

            <div v-if="status === 'sent'" class="success-message">
              ¡Mensaje enviado! Te contactaremos pronto.
            </div>
          </form>

          <div class="contact-info">
            <div class="info-card">
              <div class="icon">📧</div>
              <h3>Email</h3>
              <p>contacto@referly.com</p>
            </div>
            
            <div class="info-card">
              <div class="icon">📍</div>
              <h3>Ubicación</h3>
              <p>Buenos Aires, Argentina</p>
            </div>
          </div>
        </div>
      </div>
    </section>
  </main>
</template>

<style scoped>
.contact {
  padding-top: 64px;
}

.hero {
  background: linear-gradient(135deg, #f9f0ff 0%, #e4f2ff 100%);
  padding: 4rem 2rem;
  text-align: center;
}

.container {
  max-width: 1200px;
  margin: 0 auto;
  padding: 0 2rem;
}

.hero h1 {
  font-size: 3rem;
  color: #1a1a1a;
  margin-bottom: 1rem;
}

.subtitle {
  font-size: 1.25rem;
  color: #4b5563;
}

.contact-form {
  padding: 4rem 0;
}

.form-container {
  display: grid;
  grid-template-columns: 2fr 1fr;
  gap: 4rem;
  align-items: start;
}

form {
  background: white;
  padding: 2rem;
  border-radius: 12px;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
}

.form-group {
  margin-bottom: 1.5rem;
}

label {
  display: block;
  margin-bottom: 0.5rem;
  color: #374151;
  font-weight: 500;
}

input,
textarea {
  width: 100%;
  background: white;
  padding: 0.75rem 1rem;
  border: 1px solid #e5e7eb;
  border-radius: 8px;
  font-size: 1rem;
  color: #111827;
  transition: all 0.2s;
}

input:focus,
textarea:focus {
  outline: none;
  border-color: #646cff;
  box-shadow: 0 0 0 3px rgba(100, 108, 255, 0.1);
}

textarea {
  resize: vertical;
}

.submit-btn {
  width: 100%;
  padding: 0.875rem;
  background: #646cff;
  color: white;
  border: none;
  border-radius: 8px;
  font-size: 1rem;
  font-weight: 500;
  cursor: pointer;
  transition: all 0.2s;
}

.submit-btn:hover:not(:disabled) {
  background: #535bf2;
}

.submit-btn:disabled {
  opacity: 0.7;
  cursor: not-allowed;
}

.success-message {
  margin-top: 1rem;
  padding: 1rem;
  background: #ecfdf5;
  color: #047857;
  border-radius: 8px;
  text-align: center;
}

.contact-info {
  display: flex;
  flex-direction: column;
  gap: 1.5rem;
}

.info-card {
  background: white;
  padding: 1.5rem;
  border-radius: 12px;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
  text-align: center;
}

.info-card .icon {
  font-size: 2rem;
  margin-bottom: 1rem;
}

.info-card h3 {
  color: #1a1a1a;
  margin-bottom: 0.5rem;
}

.info-card p {
  color: #4b5563;
}

@media (max-width: 1024px) {
  .form-container {
    grid-template-columns: 1fr;
    gap: 2rem;
  }

  .contact-info {
    flex-direction: row;
    flex-wrap: wrap;
  }

  .info-card {
    flex: 1;
    min-width: 250px;
  }
}

@media (max-width: 768px) {
  .hero {
    padding: 3rem 1rem;
  }

  .hero h1 {
    font-size: 2.5rem;
  }

  .contact-form {
    padding: 2rem 1rem;
  }

  .info-card {
    min-width: 100%;
  }
}
</style>