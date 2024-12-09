<script setup>
import { ref } from 'vue'
import { auth } from '../services/auth'

const email = ref('')
const password = ref('')
const errorMessage = ref('')
const showPassword = ref(false)
const isLoading = ref(false)

const emit = defineEmits(['login-success'])

const handleSubmit = async () => {
  if (email.value && password.value) {
    try {
      isLoading.value = true
      errorMessage.value = ''
      
      await auth.login(email.value, password.value)
      emit('login-success')
      
      // Redirect to profile
      window.location.href = '/profile'
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
</script>

<template>
  <!-- Rest of the template remains the same -->
</template>

<style scoped>
/* Styles remain the same */
</style>