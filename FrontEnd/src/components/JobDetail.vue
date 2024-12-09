<script setup>
import { ref, onMounted } from 'vue';
import { useRoute, useRouter } from 'vue-router';
import { referralService } from '../services/referralService';
import { jobService } from '../services/jobService';
import { formatDate, formatSalaryRange } from '../utils/dateFormatter';
import ReferralDialog from '../components/RefferalDialog.vue';
import { auth } from '../services/auth';

const route = useRoute();
const router = useRouter();
const job = ref(null);
const isLoading = ref(false);
const error = ref(null);
const showReferralDialog = ref(false);

const fetchJobDetail = async () => {
  try {
    isLoading.value = true;
    error.value = null;
    
    console.log('Fetching job with ID:', route.params.id);
    const jobData = await jobService.getJobDetail(route.params.id);
    
    if (!jobData) {
      console.error('Job not found');
      error.value = 'Empleo no encontrado';
      return;
    }
    
    console.log('Setting job data:', jobData);
    job.value = jobData.jobs.item;
  } catch (err) {
    console.error('Error fetching job:', err);
    error.value = 'Error al cargar el detalle del empleo';
  } finally {
    isLoading.value = false;
  }
};

const handleLoginRedirect = () => {
  router.push('/login');
};

onMounted(() => {
  if (!route.params.id) {
    router.push('/');
    return;
  }
  fetchJobDetail();
});
</script>

<template>
  <main class="job-detail">
    <div class="container">
      <div v-if="isLoading" class="loading">
        <div class="loading-spinner"></div>
        <p>Cargando detalles del empleo...</p>
      </div>

      <div v-else-if="error" class="error">
        {{ error }}
      </div>

      <div v-else-if="job" class="content">
        <div class="job-info">
          <h1>{{ job.title }}</h1>
          
          <div class="job-header">
            <div class="metadata-row">
              <span class="metadata-item">
                <span class="icon">🏢</span>
                {{ job.workMode }}
              </span>
              <span class="metadata-item">
                <span class="icon">📍</span>
                {{ job.location }}
              </span>
              <span class="metadata-item">
                <span class="icon">🕒</span>
                {{ formatDate(job.published) }}
              </span>
            </div>
            <div class="referral-reward">
              <span class="reward-amount">${{ job.share }} USD</span>
              <span class="reward-label">Premio por referir</span>
            </div>
          </div>

          <div class="job-content">
            <section class="section">
              <h3>Descripción</h3>
              <p>{{ job.description }}</p>
            </section>

            <section v-if="job.responsibilities?.length" class="section">
              <h3>Responsabilidades</h3>
              <ul class="list">
                <li v-for="(resp, index) in job.responsibilities" :key="index">
                  {{ resp }}
                </li>
              </ul>
            </section>

            <section v-if="job.requirements?.length" class="section">
              <h3>Requisitos</h3>
              <ul class="list">
                <li v-for="(req, index) in job.requirements" :key="index">
                  {{ req }}
                </li>
              </ul>
            </section>

            <div class="details-grid">
              <div class="detail-item">
                <span class="detail-label">
                  <span class="icon">📁</span>
                  Categoría
                </span>
                <span class="detail-value">{{ job.category }}</span>
              </div>
              <div class="detail-item">
                <span class="detail-label">
                  <span class="icon">📈</span>
                  Nivel
                </span>
                <span class="detail-value">{{ job.level }}</span>
              </div>
              <div class="detail-item">
                <span class="detail-label">
                  <span class="icon">🏢</span>
                  Modalidad
                </span>
                <span class="detail-value">{{ job.workMode }}</span>
              </div>
              <div class="detail-item">
                <span class="detail-label">
                <span class="icon">👥</span>
                  Vacantes
                </span>
                <span class="detail-value">{{ job.vacancies || 1 }}</span>
              </div>
            </div>
          </div>
        </div>

        <div class="application-form">
          <div class="form-card">
            <template v-if="!auth.isAuthenticated()">
              <div class="login-prompt">
                <h3>¡Únete a Referly!</h3>
                <p>Crea tu cuenta o inicia sesión para comenzar a referir candidatos y ganar dinero.</p>
                <div class="login-actions">
                  <button class="login-btn" @click="handleLoginRedirect">
                    Iniciar sesión
                  </button>
                  <button class="register-btn" @click="router.push('/register')">
                    Crear cuenta
                  </button>
                </div>
              </div>
            </template>
            <template v-else>
              <ReferralDialog
                :show="showReferralDialog"
                :jobId="job.referenceId"
                :jobTitle="job.title"
                @close="showReferralDialog = false"
              />
              <button 
                class="apply-btn" 
                @click="showReferralDialog = true"
              >
                Referir candidato
              </button>
            </template>
          </div>
        </div>
      </div>
    </div>
  </main>
</template>

<style scoped>
.job-detail {
  padding-top: 84px;
  background-color: #f8fafc;
  min-height: calc(100vh - 84px);
}

.container {
  max-width: 1200px;
  margin: 0 auto;
  padding: 2rem;
}

.content {
  display: grid;
  grid-template-columns: 2fr 1fr;
  gap: 2rem;
}

.job-info {
  background: white;
  padding: 2rem;
  border-radius: 12px;
  box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);
  text-align: left;
}

h1 {
  font-size: 1.875rem;
  color: #111827;
  margin-bottom: 1rem;
  font-weight: 600;
}

.job-header {
  display: flex;
  justify-content: space-between;
  align-items: center;
  margin-bottom: 2rem;
  padding-bottom: 1.5rem;
  border-bottom: 1px solid #e5e7eb;
}

.metadata-row {
  display: flex;
  gap: 1.5rem;
}

.metadata-item {
  display: flex;
  align-items: center;
  gap: 0.5rem;
  color: #6b7280;
  font-size: 0.875rem;
}

.referral-reward {
  text-align: right;
}

.reward-amount {
  display: block;
  color: #10b981;
  font-size: 1.5rem;
  font-weight: 600;
}

.reward-label {
  display: block;
  color: #6b7280;
  font-size: 0.75rem;
  margin-top: 0.25rem;
}

.section {
  margin-bottom: 2rem;
}

.section h3 {
  font-size: 1.25rem;
  color: #111827;
  margin-bottom: 1rem;
  font-weight: 600;
}

.section p {
  color: #4b5563;
  line-height: 1.6;
}

.list {
  list-style: none;
  padding: 0;
  margin: 0;
}

.list li {
  position: relative;
  padding-left: 1.5rem;
  margin-bottom: 0.75rem;
  color: #4b5563;
}

.list li::before {
  content: "•";
  position: absolute;
  left: 0;
  color: #4f46e5;
}

.details-grid {
  display: grid;
  grid-template-columns: repeat(4, 1fr);
  gap: 1.5rem;
  margin-top: 2rem;
  padding-top: 1.5rem;
  border-top: 1px solid #e5e7eb;
}

.detail-item {
  text-align: center;
}

.detail-value {
  display: block;
  font-size: 1rem;
  color: #111827;
  font-weight: 500;
  margin-bottom: 0.5rem;
}

.detail-label {
  display: flex;
  align-items: center;
  justify-content: center;
  gap: 0.5rem;
  color: #6b7280;
  font-size: 0.875rem;
}

.icon {
  font-size: 1rem;
}

.application-form {
  position: sticky;
  top: 84px;
}

.form-card {
  background: white;
  padding: 1.5rem;
  border-radius: 12px;
  box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);
}

.login-prompt {
  text-align: center;
}

.login-prompt h3 {
  color: #111827;
  font-size: 1.25rem;
  margin-bottom: 1rem;
}

.login-prompt p {
  color: #4b5563;
  margin-bottom: 1.5rem;
}

.login-actions {
  display: flex;
  gap: 1rem;
}

.login-btn,
.register-btn {
  flex: 1;
  padding: 0.75rem;
  border-radius: 8px;
  font-size: 0.875rem;
  font-weight: 500;
  cursor: pointer;
  transition: all 0.2s;
}

.login-btn {
  background: #4f46e5;
  color: white;
  border: none;
}

.login-btn:hover {
  background: #4338ca;
}

.register-btn {
  background: white;
  color: #4f46e5;
  border: 1px solid #4f46e5;
}

.register-btn:hover {
  background: #f9fafb;
}

.apply-btn {
  width: 100%;
  background: #4f46e5;
  color: white;
  padding: 0.75rem;
  border: none;
  border-radius: 8px;
  font-size: 1rem;
  font-weight: 500;
  cursor: pointer;
  transition: background-color 0.2s;
}

.apply-btn:hover {
  background: #4338ca;
}

@media (max-width: 1024px) {
  .content {
    grid-template-columns: 1fr;
  }

  .application-form {
    position: static;
  }

  .details-grid {
    grid-template-columns: repeat(2, 1fr);
  }
}

@media (max-width: 640px) {
  .container {
    padding: 1rem;
  }

  .job-info,
  .form-card {
    padding: 1rem;
  }

  .job-header {
    flex-direction: column;
    align-items: flex-start;
    gap: 1rem;
  }

  .metadata-row {
    flex-direction: column;
    gap: 0.75rem;
  }

  .referral-reward {
    text-align: left;
    margin-top: 1rem;
  }

  .login-actions {
    flex-direction: column;
  }
}
</style>