<script setup>
import { ref, onMounted } from 'vue';
import { useRouter } from 'vue-router';
import { auth } from '../services/auth';
import { profileService } from '../services/profileService';

const router = useRouter();
const isLoading = ref(true);
const error = ref(null);
const user = ref(null);
const referrals = ref([]);

const getStatusClass = (status) => {
  switch (status.toLowerCase()) {
    case 'hired':
    case 'aceptado':
      return 'status-success';
    case 'in_process':
    case 'en proceso':
      return 'status-pending';
    case 'rejected':
    case 'rechazado':
      return 'status-rejected';
    default:
      return '';
  }
};

const getStatusText = (status) => {
  switch (status.toUpperCase()) {
    case 'HIRED':
      return 'Contratado';
    case 'IN_PROCESS':
      return 'En proceso';
    case 'REJECTED':
      return 'Rechazado';
    default:
      return status;
  }
};

const loadProfileData = async () => {
  try {
    isLoading.value = true;
    error.value = null;

    // Get current user from auth
    user.value = auth.getCurrentUser();
    
    // Get referrals
    const referralsData = await profileService.getReferrals();
    referrals.value = referralsData;
  } catch (err) {
    console.error('Error loading profile:', err);
    error.value = err.message;
  } finally {
    isLoading.value = false;
  }
};

const handleLogout = () => {
  auth.logout();
};

onMounted(async () => {
  if (!auth.isAuthenticated()) {
    router.push('/login');
    return;
  }
  await loadProfileData();
});
</script>

<template>
  <main class="profile">
    <div class="container">
      <div v-if="isLoading" class="loading">
        <div class="loading-spinner"></div>
        <p>Cargando perfil...</p>
      </div>

      <div v-else-if="error" class="error-message">
        {{ error }}
      </div>

      <template v-else>
        <div class="profile-header">
          <div class="user-info">
            <div class="avatar">{{ user[1] || 'U' }}</div>
            <div class="user-details">
              <h1>{{ user }}</h1>
            </div>
          </div>
          <button class="logout-btn" @click="handleLogout">Cerrar sesión</button>
        </div>

        <div class="stats-grid">
          <div class="stat-card">
            <h3>Total Referidos</h3>
            <p class="stat-number">{{ referrals.length }}</p>
          </div>
          <div class="stat-card">
            <h3>Contratados</h3>
            <p class="stat-number success">
              {{ referrals.filter(r => r.status.toLowerCase() === 'hired' || r.status.toLowerCase() === 'aceptado').length }}
            </p>
          </div>
          <div class="stat-card">
            <h3>En Proceso</h3>
            <p class="stat-number pending">
              {{ referrals.filter(r => r.status.toLowerCase() === 'in_process' || r.status.toLowerCase() === 'en proceso').length }}
            </p>
          </div>
        </div>

        <div class="referrals-section">
          <h2>Mis Referidos</h2>
          <div class="table-container">
            <table v-if="referrals.length">
              <thead>
                <tr>
                  <th>Candidato</th>
                  <th>Posición</th>
                  <th>Área</th>
                  <th>Estado</th>
                  <th>Fecha</th>
                </tr>
              </thead>
              <tbody>
                <tr v-for="referral in referrals" :key="referral.date">
                  <td>{{ referral.candidateName }}</td>
                  <td>{{ referral.position }}</td>
                  <td>{{ referral.company }}</td>
                  <td>
                    <span :class="['status-badge', getStatusClass(referral.status)]">
                      <span class="status-icon"></span>
                      {{ getStatusText(referral.status) }}
                    </span>
                  </td>
                  <td>{{ new Date(referral.date).toLocaleDateString() }}</td>
                </tr>
              </tbody>
            </table>
            <div v-else class="no-referrals">
              <p>Aún no tienes referidos. ¡Comienza a referir candidatos y gana dinero!</p>
              <button class="search-jobs-btn" @click="router.push('/')">
                Buscar oportunidades
              </button>
            </div>
          </div>
        </div>
      </template>
    </div>
  </main>
</template>

<style scoped>
.profile {
  padding-top: 84px;
  min-height: 100vh;
  background-color: #f8fafc;
}

.container {
  max-width: 1200px;
  margin: 0 auto;
  padding: 2rem;
}

.loading {
  text-align: center;
  padding: 4rem 2rem;
}

.loading-spinner {
  width: 40px;
  height: 40px;
  margin: 0 auto 1rem;
  border: 3px solid #f3f3f3;
  border-top: 3px solid #646cff;
  border-radius: 50%;
  animation: spin 1s linear infinite;
}

@keyframes spin {
  0% { transform: rotate(0deg); }
  100% { transform: rotate(360deg); }
}

.error-message {
  text-align: center;
  padding: 2rem;
  background: #fef2f2;
  color: #dc2626;
  border-radius: 8px;
}

.profile-header {
  display: flex;
  justify-content: space-between;
  align-items: flex-start;
  margin-bottom: 2rem;
}

.user-info {
  display: flex;
  align-items: center;
  gap: 1.5rem;
}

.avatar {
  width: 80px;
  height: 80px;
  background-color: #646cff;
  color: white;
  border-radius: 50%;
  display: flex;
  align-items: center;
  justify-content: center;
  font-size: 2rem;
  font-weight: bold;
}

.user-details h1 {
  font-size: 1.875rem;
  color: #1a1a1a;
  margin: 0 0 0.5rem;
}

.user-details p {
  color: #6b7280;
  margin: 0;
}

.logout-btn {
  background-color: #ef4444;
  color: white;
  border: none;
  padding: 0.75rem 1.5rem;
  border-radius: 8px;
  font-weight: 500;
  cursor: pointer;
  transition: background-color 0.2s;
}

.logout-btn:hover {
  background-color: #dc2626;
}

.stats-grid {
  display: grid;
  grid-template-columns: repeat(auto-fit, minmax(250px, 1fr));
  gap: 1.5rem;
  margin-bottom: 2rem;
}

.stat-card {
  background: white;
  padding: 1.5rem;
  border-radius: 12px;
  box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);
  text-align: center;
}

.stat-card h3 {
  color: #4b5563;
  font-size: 1rem;
  margin-bottom: 0.5rem;
}

.stat-number {
  color: #1a1a1a;
  font-size: 2.5rem;
  font-weight: bold;
}

.stat-number.success {
  color: #059669;
}

.stat-number.pending {
  color: #d97706;
}

.referrals-section {
  background: white;
  padding: 2rem;
  border-radius: 12px;
  box-shadow: 0 1px 3px rgba(0, 0, 0, 0.1);
}

.referrals-section h2 {
  color: #1a1a1a;
  margin-bottom: 1.5rem;
}

.table-container {
  overflow-x: auto;
}

table {
  width: 100%;
  border-collapse: collapse;
}

th, td {
  padding: 1rem;
  text-align: left;
  border-bottom: 1px solid #e5e7eb;
}

th {
  background-color: #f9fafb;
  color: #4b5563;
  font-weight: 500;
}

.status-badge {
  display: inline-flex;
  align-items: center;
  gap: 0.5rem;
  padding: 0.5rem 1rem;
  border-radius: 9999px;
  font-size: 0.875rem;
  font-weight: 500;
  position: relative;
}

.status-icon {
  width: 8px;
  height: 8px;
  border-radius: 50%;
  display: inline-block;
}

.status-success {
  background-color: #ecfdf5;
  color: #059669;
  border: 1px solid #34d399;
}

.status-success .status-icon {
  background-color: #059669;
}

.status-pending {
  background-color: #fffbeb;
  color: #d97706;
  border: 1px solid #fbbf24;
}

.status-pending .status-icon {
  background-color: #d97706;
}

.status-rejected {
  background-color: #fef2f2;
  color: #dc2626;
  border: 1px solid #f87171;
}

.status-rejected .status-icon {
  background-color: #dc2626;
}

.no-referrals {
  text-align: center;
  padding: 3rem 1rem;
  color: #6b7280;
}

.search-jobs-btn {
  margin-top: 1rem;
  padding: 0.75rem 1.5rem;
  background: #646cff;
  color: white;
  border: none;
  border-radius: 8px;
  font-weight: 500;
  cursor: pointer;
  transition: background-color 0.2s;
}

.search-jobs-btn:hover {
  background: #535bf2;
}

@media (max-width: 768px) {
  .container {
    padding: 1rem;
  }

  .profile-header {
    flex-direction: column;
    gap: 1rem;
  }

  .user-info {
    flex-direction: column;
    text-align: center;
  }

  .logout-btn {
    width: 100%;
  }

  .table-container {
    margin: 0 -1rem;
  }

  th, td {
    padding: 0.75rem;
  }
}
</style>