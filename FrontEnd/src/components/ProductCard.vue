<script setup>
defineProps({
  title: String,
  price: Number,
  features: Array,
  buttonText: {
    type: String,
    default: 'Buy Now'
  }
})
</script>

<template>
  <div class="product-card">
    <h3>{{ title }}</h3>
    <div class="price">
      <span class="currency">$</span>
      <span class="amount">{{ price.toLocaleString() }}</span>
    </div>
    <ul class="features">
      <li v-for="feature in features" :key="feature">
        {{ feature }}
      </li>
    </ul>
    <button class="buy-btn">{{ buttonText }}</button>
  </div>
</template>

<style scoped>
.product-card {
  background: white;
  border-radius: 12px;
  padding: 2rem;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
  text-align: center;
  transition: transform 0.2s;
}

.product-card:hover {
  transform: translateY(-5px);
}

h3 {
  color: #1a1a1a;
  margin-bottom: 1rem;
}

.price {
  font-size: 2.5rem;
  color: #646cff;
  font-weight: bold;
  margin-bottom: 1.5rem;
}

.currency {
  font-size: 1.5rem;
  vertical-align: super;
}

.features {
  list-style: none;
  padding: 0;
  margin: 0 0 2rem 0;
  text-align: left;
}

.features li {
  margin-bottom: 0.75rem;
  padding-left: 1.5rem;
  position: relative;
}

.features li::before {
  content: "✓";
  color: #646cff;
  position: absolute;
  left: 0;
}

.buy-btn {
  width: 100%;
  padding: 0.875rem;
  background-color: #646cff;
  color: white;
  border: none;
  border-radius: 8px;
  font-size: 1rem;
  font-weight: 500;
  cursor: pointer;
  transition: background-color 0.2s;
}

.buy-btn:hover {
  background-color: #535bf2;
}
</style>