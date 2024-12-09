<script setup>
import { useRouter } from 'vue-router'
import { formatDate } from '../utils/dateFormatter';

const router = useRouter()

const props = defineProps({
  referenceId: {
    type: Number,
    required: true
  },
  positionName: {
    type: String,
    required: true
  },
  category: {
    type: String,
    required: true
  },
  location: {
    type: String,
    required: true
  },
  workMode: {
    type: String,
    required: true
  },
  description: {
    type: String,
    required: true
  },
  published: {
    type: String,
    required: true
  },
  share: {
    type: Number,
    default: 1
  }
})

const navigateToDetail = () => {
  router.push(`/job/${props.referenceId}`)
}
</script>

<template>
  <div class="job-card">
    <div class="job-header">
      <span class="published-date">{{ 
        formatDate(published) }}</span>
      <div class="referral-share">
        <span class="share-label">Premio por referir</span>
        <span class="share-amount">${{ share }}</span>
        <span class="share-currency">USD</span>
      </div>
    </div>

    <div class="job-content">
      <h2 class="job-title">{{ positionName }}</h2>
      <h3 class="category-name">{{ category }}</h3>
      
      <div class="job-details">
        <div class="detail-item">
          <span class="icon">📍</span>
          {{ location }}
        </div>
        <div class="detail-item">
          <span class="icon">🏢</span>
          {{ workMode }}
        </div>
      </div>

      <p class="job-description">{{ description }}</p>
      
      <button class="apply-btn" @click="navigateToDetail">Aplicar ahora</button>
    </div>
  </div>
</template>

<style scoped>
.job-card {
  background: white;
  border-radius: 12px;
  padding: 1.5rem;
  box-shadow: 0 2px 8px rgba(0, 0, 0, 0.1);
  transition: transform 0.2s, box-shadow 0.2s;
  margin-bottom: 1rem;
  text-align: left;
}

.job-card:hover {
  transform: translateY(-2px);
  box-shadow: 0 4px 12px rgba(0, 0, 0, 0.15);
}

.job-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 1rem;
}

.published-date {
  color: #666;
  font-size: 0.875rem;
}

.referral-share {
  background: linear-gradient(135deg, #46e546 0%, #64d6ff 100%);
  padding: 0.75rem 1.25rem;
  border-radius: 8px;
  text-align: center;
}

.share-label {
  display: block;
  color: rgba(255, 255, 255, 0.9);
  font-size: 0.75rem;
  text-transform: uppercase;
  letter-spacing: 0.05em;
  margin-bottom: 0.25rem;
}

.share-amount {
  color: white;
  font-size: 1.5rem;
  font-weight: bold;
  margin-right: 0.25rem;
}

.share-currency {
  color: rgba(255, 255, 255, 0.9);
  font-size: 0.875rem;
  font-weight: 500;
}

.job-content {
  display: flex;
  flex-direction: column;
  gap: 0.75rem;
}

.job-title {
  color: #1a1a1a;
  font-size: 1.25rem;
  margin: 0;
  line-height: 1.4;
}

.category-name {
  color: #646cff;
  font-size: 1rem;
  margin: 0;
  line-height: 1.4;
}

.job-details {
  display: flex;
  gap: 1rem;
  margin: 0.25rem 0;
}

.detail-item {
  display: flex;
  align-items: center;
  gap: 0.25rem;
  color: #4b5563;
  font-size: 0.875rem;
}

.icon {
  font-size: 1rem;
}

.job-description {
  color: #4b5563;
  line-height: 1.5;
  margin: 0;
  font-size: 0.875rem;
}

.apply-btn {
  background-color: #646cff;
  color: white;
  border: none;
  border-radius: 6px;
  padding: 0.75rem 1.25rem;
  font-size: 0.875rem;
  font-weight: 500;
  cursor: pointer;
  transition: background-color 0.2s;
  width: fit-content;
  margin-top: 0.5rem;
}

.apply-btn:hover {
  background-color: #535bf2;
}

@media (max-width: 640px) {
  .job-card {
    padding: 1rem;
  }

  .job-header {
    flex-direction: column;
    align-items: flex-start;
    gap: 1rem;
  }

  .referral-share {
    width: 100%;
  }

  .job-details {
    flex-direction: column;
    gap: 0.5rem;
  }
}
</style>