<script setup>
defineProps({
  show: Boolean,
  title: {
    type: String,
    default: '¡Éxito!'
  },
  message: {
    type: String,
    required: true
  }
})

const emit = defineEmits(['close'])
</script>

<template>
  <div v-if="show" class="dialog-overlay" @click="emit('close')">
    <div class="dialog-content" @click.stop>
      <div class="success-icon">✓</div>
      <h3>{{ title }}</h3>
      <p>{{ message }}</p>
      <button class="close-btn" @click="emit('close')">Continuar</button>
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
  width: 400px;
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

h3 {
  color: #1a1a1a;
  font-size: 1.5rem;
  margin-bottom: 1rem;
}

p {
  color: #4b5563;
  margin-bottom: 1.5rem;
}

.close-btn {
  padding: 0.75rem 2rem;
  background: #646cff;
  color: white;
  border: none;
  border-radius: 8px;
  font-size: 1rem;
  cursor: pointer;
  transition: background-color 0.2s;
}

.close-btn:hover {
  background: #535bf2;
}
</style>
