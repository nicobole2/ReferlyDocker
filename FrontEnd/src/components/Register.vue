<script setup>
import { ref, computed } from 'vue'
import { useRouter } from 'vue-router'
import { auth } from '../services/auth'
import SuccessDialog from '../components/SuccessDialog.vue'

const router = useRouter()
const form = ref({
  email: '',
  firstName: '',
  lastName: '',
  linkedinProfile: '',
  shortSummary: '',
  phone: '',
  password: '',
  passwordConfirm: ''
})
const errors = ref({})
const isLoading = ref(false)
const showSuccessDialog = ref(false)

const passwordStrength = computed(() => {
  const password = form.value.password
  if (!password) return { score: 0, message: '' }

  let score = 0
  let message = ''

  if (password.length >= 8) score++
  if (/\d/.test(password)) score++
  if (/[a-z]/.test(password)) score++
  if (/[A-Z]/.test(password)) score++
  if (/[!@#$%^&*(),.?":{}|<>]/.test(password)) score++

  switch (score) {
    case 0:
    case 1:
      message = 'Muy débil'
      break
    case 2:
      message = 'Débil'
      break
    case 3:
      message = 'Media'
      break
    case 4:
      message = 'Fuerte'
      break
    case 5:
      message = 'Muy fuerte'
      break
  }

  return { score, message }
})

const validateForm = () => {
  const newErrors = {}
  
  if (!form.value.firstName.trim()) {
    newErrors.firstName = 'El nombre es requerido'
  }
  
  if (!form.value.lastName.trim()) {
    newErrors.lastName = 'El apellido es requerido'
  }
  
  if (!form.value.email.trim()) {
    newErrors.email = 'El email es requerido'
  } else if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(form.value.email)) {
    newErrors.email = 'Email inválido'
  }

  if (!form.value.phone.trim()) {
    newErrors.phone = 'El teléfono es requerido'
  }

  if (!form.value.shortSummary.trim()) {
    newErrors.shortSummary = 'La descripción breve es requerida'
  }
  
  if (!form.value.password) {
    newErrors.password = 'La contraseña es requerida'
  } else if (form.value.password.length < 8) {
    newErrors.password = 'La contraseña debe tener al menos 8 caracteres'
  }
  
  if (form.value.password !== form.value.passwordConfirm) {
    newErrors.passwordConfirm = 'Las contraseñas no coinciden'
  }

  errors.value = newErrors
  return Object.keys(newErrors).length === 0
}

const handleSubmit = async () => {
  if (!validateForm()) return

  try {
    isLoading.value = true
    
    await auth.register({
      email: form.value.email,
      firstName: form.value.firstName,
      lastName: form.value.lastName,
      linkedinProfile: form.value.linkedinProfile,
      shortSummary: form.value.shortSummary,
      phone: form.value.phone,
      password: form.value.password,
      passwordConfirm: form.value.passwordConfirm
    })
    
    showSuccessDialog.value = true
  } catch (error) {
    errors.value.submit = error.message || 'Error al registrar usuario'
  } finally {
    isLoading.value = false
  }
}

const handleDialogClose = () => {
  showSuccessDialog.value = false
  router.push('/profile')
}
</script>

<template>
  <main class="register-page">
    <div class="register-container">
      <div class="register-content">
        <div class="register-header">
          <h1>Crear cuenta</h1>
          <p>Únete a Referly y comienza a ganar dinero refiriendo talento</p>
        </div>

        <form @submit.prevent="handleSubmit" class="register-form">
          <div class="form-row">
            <div class="form-group">
              <label for="firstName">Nombre</label>
              <input
                id="firstName"
                type="text"
                v-model="form.firstName"
                :class="{ 'error': errors.firstName }"
                placeholder="Tu nombre"
              />
              <span v-if="errors.firstName" class="error-message">{{ errors.firstName }}</span>
            </div>

            <div class="form-group">
              <label for="lastName">Apellido</label>
              <input
                id="lastName"
                type="text"
                v-model="form.lastName"
                :class="{ 'error': errors.lastName }"
                placeholder="Tu apellido"
              />
              <span v-if="errors.lastName" class="error-message">{{ errors.lastName }}</span>
            </div>
          </div>

          <div class="form-group">
            <label for="email">Email</label>
            <input
              id="email"
              type="email"
              v-model="form.email"
              :class="{ 'error': errors.email }"
              placeholder="tu@email.com"
            />
            <span v-if="errors.email" class="error-message">{{ errors.email }}</span>
          </div>

          <div class="form-group">
            <label for="phone">Teléfono</label>
            <input
              id="phone"
              type="tel"
              v-model="form.phone"
              :class="{ 'error': errors.phone }"
              placeholder="Tu número de teléfono"
            />
            <span v-if="errors.phone" class="error-message">{{ errors.phone }}</span>
          </div>

          <div class="form-group">
            <label for="linkedinProfile">Perfil de LinkedIn (opcional)</label>
            <input
              id="linkedinProfile"
              type="url"
              v-model="form.linkedinProfile"
              placeholder="URL de tu perfil de LinkedIn"
            />
          </div>

          <div class="form-group">
            <label for="shortSummary">Descripción breve</label>
            <textarea
              id="shortSummary"
              v-model="form.shortSummary"
              :class="{ 'error': errors.shortSummary }"
              placeholder="Cuéntanos brevemente sobre ti"
              rows="3"
            ></textarea>
            <span v-if="errors.shortSummary" class="error-message">{{ errors.shortSummary }}</span>
          </div>

          <div class="form-group">
            <label for="password">Contraseña</label>
            <input
              id="password"
              type="password"
              v-model="form.password"
              :class="{ 'error': errors.password }"
              placeholder="Mínimo 8 caracteres"
            />
            <div class="password-strength" v-if="form.password">
              <div class="strength-bar">
                <div 
                  class="strength-progress"
                  :style="{ width: `${(passwordStrength.score / 5) * 100}%` }"
                  :class="`strength-${passwordStrength.score}`"
                ></div>
              </div>
              <span class="strength-text">{{ passwordStrength.message }}</span>
            </div>
            <span v-if="errors.password" class="error-message">{{ errors.password }}</span>
          </div>

          <div class="form-group">
            <label for="passwordConfirm">Confirmar contraseña</label>
            <input
              id="passwordConfirm"
              type="password"
              v-model="form.passwordConfirm"
              :class="{ 'error': errors.passwordConfirm }"
              placeholder="Repite tu contraseña"
            />
            <span v-if="errors.passwordConfirm" class="error-message">{{ errors.passwordConfirm }}</span>
          </div>

          <div v-if="errors.submit" class="submit-error">
            {{ errors.submit }}
          </div>

          <button 
            type="submit" 
            class="submit-btn" 
            :disabled="isLoading"
          >
            {{ isLoading ? 'Creando cuenta...' : 'Crear cuenta' }}
          </button>

          <div class="login-link">
            ¿Ya tienes cuenta? <a href="#" @click.prevent="router.push('/login')">Inicia sesión</a>
          </div>
        </form>
      </div>
    </div>

    <SuccessDialog
      :show="showSuccessDialog"
      title="¡Registro exitoso!"
      message="Tu cuenta ha sido creada correctamente. Serás redirigido a tu perfil."
      @close="handleDialogClose"
    />
  </main>
</template>


<style scoped>
.register-page {
  padding-top: 84px;
  min-height: 100vh;
  background-color: #f8fafc;
}

.register-container {
  max-width: 800px;
  margin: 0 auto;
  padding: 2rem;
}

.register-content {
  background: white;
  padding: 2rem;
  border-radius: 12px;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
}

.register-header {
  text-align: center;
  margin-bottom: 2rem;
}

.register-header h1 {
  font-size: 2rem;
  color: #1a1a1a;
  margin-bottom: 0.5rem;
}

.register-header p {
  color: #6b7280;
  font-size: 1.1rem;
}

.register-form {
  max-width: 600px;
  margin: 0 auto;
}

.form-row {
  display: grid;
  grid-template-columns: repeat(2, 1fr);
  gap: 1rem;
  margin-bottom: 1rem;
}

.form-group {
  margin-bottom: 1.5rem;
}

.form-group label {
  display: block;
  color: #374151;
  font-weight: 500;
  margin-bottom: 0.5rem;
}

input,
textarea {
  width: 100%;
  padding: 0.75rem 1rem;
  border: 1px solid #e5e7eb;
  border-radius: 8px;
  font-size: 1rem;
  transition: all 0.2s;
  background-color: white;
  color: #111827;
}

input::placeholder,
textarea::placeholder {
  color: #9ca3af;
}

input:focus,
textarea:focus {
  outline: none;
  border-color: #646cff;
  box-shadow: 0 0 0 3px rgba(100, 108, 255, 0.1);
}

textarea {
  resize: vertical;
  min-height: 100px;
}

input.error,
textarea.error {
  border-color: #dc2626;
}

.error-message {
  color: #dc2626;
  font-size: 0.875rem;
  margin-top: 0.5rem;
}

.password-strength {
  margin-top: 0.5rem;
}

.strength-bar {
  height: 4px;
  background: #e5e7eb;
  border-radius: 2px;
  overflow: hidden;
}

.strength-progress {
  height: 100%;
  transition: width 0.3s ease;
}

.strength-0 { background-color: #dc2626; }
.strength-1 { background-color: #dc2626; }
.strength-2 { background-color: #f59e0b; }
.strength-3 { background-color: #10b981; }
.strength-4 { background-color: #059669; }
.strength-5 { background-color: #047857; }

.strength-text {
  font-size: 0.875rem;
  color: #6b7280;
  margin-top: 0.25rem;
}

.submit-error {
  background: #fef2f2;
  color: #dc2626;
  padding: 0.75rem;
  border-radius: 8px;
  margin-bottom: 1rem;
  text-align: center;
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
  margin-top: 1rem;
}

.submit-btn:hover:not(:disabled) {
  background: #535bf2;
}

.submit-btn:disabled {
  opacity: 0.7;
  cursor: not-allowed;
}

.login-link {
  text-align: center;
  margin-top: 1.5rem;
  color: #6b7280;
}

.login-link a {
  color: #646cff;
  text-decoration: none;
  font-weight: 500;
}

.login-link a:hover {
  text-decoration: underline;
}

@media (max-width: 640px) {
  .register-container {
    padding: 1rem;
  }

  .register-content {
    padding: 1.5rem;
  }

  .form-row {
    grid-template-columns: 1fr;
  }

  .register-header h1 {
    font-size: 1.75rem;
  }
}
</style>