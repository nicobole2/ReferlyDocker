<script setup>
import { ref, computed, onMounted, watch } from 'vue';
import { useRoute } from 'vue-router';
import JobCard from '../components/JobCard.vue';
import { formatDate } from '../utils/dateFormatter';
import { jobService } from '../services/jobService';

const route = useRoute();
const currentPage = ref(1);
const pageSize = ref(10);
const searchQuery = ref('');
const sortBy = ref('relevantes');
const isLoading = ref(false);
const error = ref(null);
const jobs = ref([]);
const total = ref(0);

const handleSearch = async () => {
  try {
    isLoading.value = true;
    error.value = null;
    
    const response = await jobService.getJobs(
      currentPage.value, 
      pageSize.value, 
      searchQuery.value
    );
    
    jobs.value = response.jobs;
    total.value = response.total;
  } catch (err) {
    error.value = 'Error al cargar los empleos';
    console.error('Search error:', err);
  } finally {
    isLoading.value = false;
  }
};

const handlePageChange = async (page) => {
  currentPage.value = page;
  await handleSearch();
  window.scrollTo({ top: 0, behavior: 'smooth' });
};

const totalPages = computed(() => 
  Math.ceil(total.value / pageSize.value)
);

watch(() => route.query.q, (newQuery) => {
  searchQuery.value = newQuery || '';
  currentPage.value = 1;
  handleSearch();
});

onMounted(async () => {
  searchQuery.value = route.query.q || '';
  await handleSearch();
});
</script>

<template>
  <main class="search-results">
    <div class="container">
      <div class="results-header">
        <h1>Bolsa de empleo</h1>
        <p class="results-count">
          <span class="count">{{ total }}</span> ofertas de empleo en Argentina
        </p>
      </div>

      <div class="results-controls">
        <div class="sort-controls">
          <span>Ordenar por:</span>
          <button 
            :class="['sort-btn', { active: sortBy === 'relevantes' }]"
            @click="sortBy = 'relevantes'"
          >
            Relevantes
          </button>
          <button 
            :class="['sort-btn', { active: sortBy === 'recientes' }]"
            @click="sortBy = 'recientes'"
          >
            Recientes
          </button>
        </div>
      </div>

      <div v-if="isLoading" class="loading">
        <div class="loading-spinner"></div>
        <p>Cargando empleos...</p>
      </div>

      <div v-else-if="error" class="error">
        {{ error }}
      </div>

      <div v-else-if="jobs.length === 0" class="no-results">
        No se encontraron empleos que coincidan con tu búsqueda
      </div>
      

      <div v-else class="jobs-grid">
        <JobCard
          v-for="job in jobs"
          :key="job.referenceId"
          :referenceId="job.referenceId"
          :positionName="job.positionName"
          :category="job.category"
          :location="job.location"
          :workMode="job.workMode"
          :description="job.description"
          :published="job.published"
          :share="job.share || 500"
        />
      </div>

      <div v-if="totalPages > 1" class="pagination">
        <button 
          class="page-btn"
          :disabled="currentPage === 1"
          @click="handlePageChange(currentPage - 1)"
        >
          Anterior
        </button>
        
        <button 
          v-for="page in totalPages"
          :key="page"
          :class="['page-btn', { active: currentPage === page }]"
          @click="handlePageChange(page)"
        >
          {{ page }}
        </button>
        
        <button 
          class="page-btn"
          :disabled="currentPage === totalPages"
          @click="handlePageChange(currentPage + 1)"
        >
          Siguiente
        </button>
      </div>
    </div>
  </main>
</template>

<style scoped>
.search-results {
  padding-top: 84px;
  min-height: 100vh;
  background-color: #f8fafc;
}

.container {
  max-width: 1200px;
  margin: 0 auto;
  padding: 2rem;
}

.results-header {
  margin-bottom: 2rem;
}

.results-header h1 {
  font-size: 2rem;
  color: #1a1a1a;
  margin-bottom: 0.5rem;
}

.results-count {
  color: #4b5563;
  font-size: 1.1rem;
}

.count {
  color: #646cff;
  font-weight: bold;
}

.results-controls {
  display: flex;
  justify-content: flex-end;
  margin-bottom: 2rem;
}

.sort-controls {
  display: flex;
  align-items: center;
  gap: 1rem;
}

.sort-controls span {
  color: #4b5563;
}

.sort-btn {
  padding: 0.5rem 1rem;
  border: 1px solid #e5e7eb;
  background: white;
  border-radius: 6px;
  color: #4b5563;
  cursor: pointer;
  transition: all 0.2s;
}

.sort-btn:hover {
  border-color: #646cff;
  color: #646cff;
}

.sort-btn.active {
  background: #646cff;
  color: white;
  border-color: #646cff;
}

.loading {
  text-align: center;
  padding: 2rem;
  color: #4b5563;
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

.error {
  text-align: center;
  padding: 2rem;
  color: #dc2626;
  background: #fef2f2;
  border-radius: 8px;
}

.no-results {
  text-align: center;
  padding: 2rem;
  color: #4b5563;
  background: #f9fafb;
  border-radius: 8px;
}

.jobs-grid {
  display: flex;
  flex-direction: column;
  gap: 1rem;
  margin-bottom: 2rem;
}

.pagination {
  display: flex;
  justify-content: center;
  gap: 0.5rem;
  margin-top: 2rem;
}

.page-btn {
  padding: 0.5rem 1rem;
  border: 1px solid #e5e7eb;
  background: white;
  border-radius: 6px;
  color: #4b5563;
  cursor: pointer;
  transition: all 0.2s;
}

.page-btn:hover:not(:disabled) {
  border-color: #646cff;
  color: #646cff;
}

.page-btn.active {
  background: #646cff;
  color: white;
  border-color: #646cff;
}

.page-btn:disabled {
  opacity: 0.5;
  cursor: not-allowed;
}

@media (max-width: 768px) {
  .container {
    padding: 1rem;
  }

  .results-controls {
    flex-direction: column;
    gap: 1rem;
  }

  .sort-controls {
    justify-content: center;
  }

  .pagination {
    flex-wrap: wrap;
  }
}
</style>