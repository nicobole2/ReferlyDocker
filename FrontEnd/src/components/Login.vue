<script setup>
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { auth } from '../services/auth'

const router = useRouter()
const email = ref('')
const password = ref('')
const errorMessage = ref('')
const showPassword = ref(false)
const isLoading = ref(false)

const handleSubmit = async () => {
  if (email.value && password.value) {
    try {
      isLoading.value = true
      errorMessage.value = ''
      
      await auth.login(email.value, password.value)
      router.push('/profile')
    } catch (error) {
      errorMessage.value = error.message || 'Login failed. Please try again.'
    } finally {
      isLoading.value = false
    }
  } else {
    errorMessage.value = 'Please fill in all fields'
  }
}

const togglePassword = () => {
  showPassword.value = !showPassword.value
}

const goToRegister = () => {
  router.push('/register')
}
</script>

<template>
  <main class="login-page">
    <div class="login-container">
      <div class="login-content">
        <div class="login-header">
          <h1>Bienvenido a Referly</h1>
          <p>Inicia sesión para comenzar a ganar dinero refiriendo talento</p>
        </div>

        <form @submit.prevent="handleSubmit" class="login-form">
          <div class="form-group">
            <label for="email">Email</label>
            <div class="input-wrapper">
              <input
                id="email"
                style="color: #111827;"
                type="email"
                v-model="email"
                placeholder="Ingresa tu email"
                required
                :disabled="isLoading"
              />
              <span class="input-icon">📧</span>
            </div>
          </div>
          
          <div class="form-group">
            <label for="password">Password</label>
            <div class="input-wrapper">
              <input
                style="color: #111827;"
                id="password"
                :type="showPassword ? 'text' : 'password'"
                v-model="password"
                placeholder="Ingresa tu contraseña"
                required
                :disabled="isLoading"
              />
              <button 
                type="button" 
                class="toggle-password"
                @click="togglePassword"
              >
                {{ showPassword ? '👁️' : '👁️‍🗨️' }}
              </button>
            </div>
          </div>

          <div class="remember-forgot">
            <label class="remember-me">
              <input type="checkbox" :disabled="isLoading"> Recordarme
            </label>
            <a href="#" class="forgot-password">¿Olvidaste tu contraseña?</a>
          </div>

          <div v-if="errorMessage" class="error-message">
            {{ errorMessage }}
          </div>

          <button 
            type="submit" 
            class="submit-btn" 
            :disabled="isLoading"
          >
            {{ isLoading ? 'Iniciando sesión...' : 'Iniciar sesión' }}
          </button>
          
          <div class="additional-options">
            <p>¿No tienes una cuenta? <a href="#" @click.prevent="goToRegister">Regístrate</a></p>
          </div>
        </form>
      </div>

      <div class="login-image">
        <div class="image-content">
          <h2>Gana dinero refiriendo talento</h2>
          <p>Conecta profesionales con oportunidades laborales y recibe recompensas por cada contratación exitosa.</p>
        </div>
      </div>
    </div>
  </main>
</template>

<style scoped>
.login-page {
  min-height: 100vh;
  background: linear-gradient(135deg, #f9f0ff 0%, #e4f2ff 100%);
  display: flex;
  align-items: center;
  justify-content: center;
  padding: 2rem;
}

.login-container {
  display: grid;
  grid-template-columns: 1fr 1fr;
  max-width: 1200px;
  width: 100%;
  background: white;
  border-radius: 20px;
  overflow: hidden;
  box-shadow: 0 10px 25px rgba(0, 0, 0, 0.1);
}

.login-content {
  padding: 3rem;
}

.login-header {
  text-align: center;
  margin-bottom: 2rem;
}

.login-header h1 {
  font-size: 2.5rem;
  color: #1a1a1a;
  margin-bottom: 1rem;
}

.login-header p {
  color: #6b7280;
  font-size: 1.1rem;
}

.login-form {
  max-width: 400px;
  margin: 0 auto;
}

.form-group {
  margin-bottom: 1.5rem;
}

.form-group label {
  display: block;
  margin-bottom: 0.5rem;
  color: #374151;
  font-weight: 500;
}

.input-wrapper {
  position: relative;
}

.input-wrapper input {
  width: 100%;
  padding: 0.75rem 1rem;
  padding-right: 2.5rem;
  border: 1.5px solid #e5e7eb;
  border-radius: 8px;
  font-size: 1rem;
  transition: all 0.2s;
  background: #f9fafb;
}

.input-wrapper input:focus {
  border-color: #646cff;
  background: white;
  outline: none;
  box-shadow: 0 0 0 3px rgba(100, 108, 255, 0.1);
}

.input-icon,
.toggle-password {
  position: absolute;
  right: 1rem;
  top: 50%;
  transform: translateY(-50%);
  color: #6b7280;
  background: none;
  border: none;
  cursor: pointer;
  padding: 0;
}

.remember-forgot {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 1.5rem;
}

.remember-me {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  color: #6b7280;
}

.forgot-password {
  color: #646cff;
  text-decoration: none;
  font-size: 0.875rem;
}

.forgot-password:hover {
  text-decoration: underline;
}

.error-message {
  background: #fef2f2;
  color: #dc2626;
  padding: 0.75rem;
  border-radius: 6px;
  margin-bottom: 1rem;
  font-size: 0.875rem;
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
  transition: background-color 0.2s;
}

.submit-btn:hover:not(:disabled) {
  background: #535bf2;
}

.submit-btn:disabled {
  opacity: 0.7;
  cursor: not-allowed;
}

.additional-options {
  text-align: center;
  margin-top: 1.5rem;
  color: #6b7280;
}

.additional-options a {
  color: #646cff;
  text-decoration: none;
  font-weight: 500;
}

.additional-options a:hover {
  text-decoration: underline;
}

.login-image {
  background: linear-gradient(135deg, #4f46e5 0%, #646cff 100%);
  padding: 3rem;
  display: flex;
  align-items: center;
  justify-content: center;
  color: white;
}

.image-content {
  text-align: center;
  max-width: 400px;
}

.image-content h2 {
  font-size: 2rem;
  margin-bottom: 1rem;
}

.image-content p {
  font-size: 1.1rem;
  opacity: 0.9;
  line-height: 1.6;
}

@media (max-width: 1024px) {
  .login-container {
    grid-template-columns: 1fr;
  }

  .login-image {
    display: none;
  }
}

@media (max-width: 640px) {
  .login-page {
    padding: 1rem;
  }

  .login-content {
    padding: 2rem 1.5rem;
  }

  .login-header h1 {
    font-size: 2rem;
  }
}
</style>