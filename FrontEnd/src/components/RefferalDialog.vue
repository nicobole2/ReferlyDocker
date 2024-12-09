<script setup>
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { auth } from '../services/auth'
import { referralService } from '../services/referralService'
import { useUserStore } from '../stores/user'

const props = defineProps({
  show: Boolean,
  jobId: Number,
  jobTitle: String
})

const emit = defineEmits(['close'])

const router = useRouter()
const userStore = useUserStore()
const isLoading = ref(false)
const isSuccess = ref(false)
const successMessage = ref('')
const errorMessage = ref('')

const referralForm = ref({
  email: '',
  firstName: '',
  lastName: '',
  phone: '',
  linkedin: '',
  file: null
})

const errors = ref({})

const validateForm = () => {
  const newErrors = {}
  
  if (!referralForm.value.email?.trim()) {
    newErrors.email = 'El email es requerido'
  } else if (!/^[^\s@]+@[^\s@]+\.[^\s@]+$/.test(referralForm.value.email)) {
    newErrors.email = 'Email inválido'
  }
  
  if (!referralForm.value.firstName?.trim()) {
    newErrors.firstName = 'El nombre es requerido'
  }
  
  if (!referralForm.value.lastName?.trim()) {
    newErrors.lastName = 'El apellido es requerido'
  }
  
  errors.value = newErrors
  return Object.keys(newErrors).length === 0
}

const handleFileSelect = (event) => {
  const file = event.target.files[0]
  if (file) {
    if (file.type === 'application/pdf') {
      referralForm.value.file = file
      errors.value.file = ''
    } else {
      errors.value.file = 'Por favor, selecciona un archivo PDF'
      referralForm.value.file = null
      event.target.value = ''
    }
  }
}

const handleSubmit = async () => {
  if (!auth.isAuthenticated()) {
    router.push('/login')
    return
  }

  if (!validateForm()) return

  try {
    isLoading.value = true
    errorMessage.value = ''
    
    const currentUser = auth.getCurrentUser()
    if (!currentUser) {
      throw new Error('Usuario no autenticado')
    }

    const referralData = {
      jobId: props.jobId,
      hunterId: currentUser.id,
      firstName: referralForm.value.firstName,
      lastName: referralForm.value.lastName,
      email: referralForm.value.email,
      description: `${referralForm.value.phone || ''} ${referralForm.value.linkedin || ''}`.trim(),
      file: referralForm.value.file
    }

    await referralService.createReferral(referralData)
    
    // Add to local store
    userStore.addReferral(props.jobId, {
      firstName: referralForm.value.firstName,
      lastName: referralForm.value.lastName,
      email: referralForm.value.email,
      jobTitle: props.jobTitle
    })

    isSuccess.value = true
    successMessage.value = '¡Tu referido ha sido registrado correctamente! Puedes ver el estado en tu perfil.'

    // Reset form
    referralForm.value = {
      email: '',
      firstName: '',
      lastName: '',
      phone: '',
      linkedin: '',
      file: null
    }
    errors.value = {}
  } catch (error) {
    errorMessage.value = error.message
    console.error('Error submitting referral:', error)
  } finally {
    isLoading.value = false
  }
}

const handleClose = () => {
  isSuccess.value = false
  errorMessage.value = ''
  errors.value = {}
  emit('close')
}
</script>

<template>
  <div v-if="show" class="dialog-overlay" @click="handleClose">
    <div class="dialog-content" @click.stop>
      <div v-if="isSuccess" class="success-message">
        <div class="success-icon">✓</div>
        <h3>¡Referido exitosamente!</h3>
        <p>{{ successMessage }}</p>
        <button class="close-btn" @click="handleClose">Cerrar</button>
      </div>
      <div v-else>
        
        <div v-if="errorMessage" class="error-banner">
          {{ errorMessage }}
        </div>
        
        <div class="form-group">
          <label>Email del referido</label>
          <input 
            type="email" 
            v-model="referralForm.email"
            :class="{ 'error': errors.email }"
            placeholder="Ingresa el email"
          />
          <span v-if="errors.email" class="error-message">{{ errors.email }}</span>
        </div>

        <div class="form-group">
          <label>Nombre</label>
          <input 
            type="text" 
            v-model="referralForm.firstName"
            :class="{ 'error': errors.firstName }"
            placeholder="Nombre del referido"
          />
          <span v-if="errors.firstName" class="error-message">{{ errors.firstName }}</span>
        </div>

        <div class="form-group">
          <label>Apellido</label>
          <input 
            type="text" 
            v-model="referralForm.lastName"
            :class="{ 'error': errors.lastName }"
            placeholder="Apellido del referido"
          />
          <span v-if="errors.lastName" class="error-message">{{ errors.lastName }}</span>
        </div>

        <div class="form-group">
          <label>Teléfono</label>
          <input 
            type="tel" 
            v-model="referralForm.phone"
            placeholder="Teléfono de contacto"
          />
        </div>

        <div class="form-group">
          <label>LinkedIn</label>
          <input 
            type="url" 
            v-model="referralForm.linkedin"
            placeholder="URL del perfil de LinkedIn"
          />
        </div>

        <div class="form-group">
          <label>CV</label>
          <div class="file-upload">
            <input
              type="file"
              accept=".pdf"
              class="file-input"
              @change="handleFileSelect"
            />
            <div class="file-upload-button">
              <span class="icon">📎</span>
              {{ referralForm.file ? referralForm.file.name : 'Seleccionar archivo' }}
            </div>
          </div>
          <small class="file-info">Formato aceptado: PDF</small>
          <span v-if="errors.file" class="error-message">{{ errors.file }}</span>
        </div>

        <button 
          class="submit-btn" 
          @click="handleSubmit"
          :disabled="isLoading"
        >
          {{ isLoading ? 'Enviando...' : 'Referir candidato' }}
        </button>
      </div>
    </div>
  </div>
</template>

<style scoped>
.dialog-overlay {
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  bottom: 0;
  background: rgba(0, 0, 0, 0.5);
  display: flex;
  align-items: center;
  justify-content: center;
  z-index: 1000;
}

.dialog-content {
  background: white;
  padding: 2rem;
  border-radius: 12px;
  max-width: 90%;
  width: 500px;
}

.success-message {
  text-align: center;
}

.success-icon {
  width: 64px;
  height: 64px;
  background: #10b981;
  color: white;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 2rem;
  margin: 0 auto 1.5rem;
}

.error-banner {
  background: #fef2f2;
  color: #dc2626;
  padding: 1rem;
  border-radius: 8px;
  margin-bottom: 1.5rem;
  text-align: center;
  font-size: 0.875rem;
}

h3 {
  color: #1a1a1a;
  font-size: 1.5rem;
  margin-bottom: 1.5rem;
}

.form-group {
  margin-bottom: 1.5rem;
}

label {
  display: block;
  color: #374151;
  margin-bottom: 0.5rem;
  font-weight: 500;
}

input {
  width: 100%;
  padding: 0.75rem 1rem;
  border: 1px solid #e5e7eb;
  border-radius: 8px;
  font-size: 1rem;
  transition: all 0.2s;
  background: white;
  color: #111827;
}

input::placeholder {
  color: #9ca3af;
}

input:focus {
  outline: none;
  border-color: #646cff;
  box-shadow: 0 0 0 3px rgba(100, 108, 255, 0.1);
}

input.error {
  border-color: #fee2e2;
  background-color: #fef2f2;
}

.error-message {
  color: #dc2626;
  font-size: 0.875rem;
  margin-top: 0.5rem;
  display: block;
}

.file-upload {
  position: relative;
  margin-top: 0.5rem;
}

.file-input {
  position: absolute;
  top: 0;
  left: 0;
  width: 100%;
  height: 100%;
  opacity: 0;
  cursor: pointer;
  z-index: 2;
}

.file-upload-button {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  padding: 0.75rem 1rem;
  background: #f9fafb;
  border: 1px dashed #e5e7eb;
  border-radius: 8px;
  color: #6b7280;
  cursor: pointer;
}

.file-info {
  display: block;
  margin-top: 0.5rem;
  font-size: 0.875rem;
  color: #6b7280;
}

.submit-btn, .close-btn {
  width: 100%;
  padding: 0.75rem;
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

.submit-btn:hover:not(:disabled),
.close-btn:hover {
  background: #535bf2;
}

.submit-btn:disabled {
  opacity: 0.7;
  cursor: not-allowed;
}

@media (max-width: 640px) {
  .dialog-content {
    padding: 1.5rem;
    margin: 1rem;
  }
}
</style>