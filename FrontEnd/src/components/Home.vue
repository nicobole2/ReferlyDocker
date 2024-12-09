<script setup>
import JobCard from '../components/JobCard.vue'
import { ref, reactive, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import { jobService } from '../services/jobService'

const router = useRouter()
const jobsStore = reactive({
  jobs: [],
  total: 0
})
const searchQuery = ref('')
const selectedLocation = ref('Todo el país')

const fetchJobs = async () => {
  try {
    const data = await jobService.getJobs()
    jobsStore.jobs = data.jobs
    console.log("jobResponse" + jobsStore.jobs)
    jobsStore.total = data.total
  } catch (error) {
    console.error("Error al obtener trabajos:", error)
  }
}

const handleSearch = () => {
  router.push({
    path: '/search',
    query: {
      q: searchQuery.value,
      location: selectedLocation.value
    }
  })
}

onMounted(fetchJobs)
</script>

<template>
  <main class="home">
    <section class="hero">
      <div class="hero-content">
        <div class="hero-text">
          <h1>Gana dinero con Referly</h1>
          <p>Conecta talento con oportunidades y recibe recompensas</p>
        </div>
        
        <div class="hero-stats">
          <h2>
            Hay <span class="job-count">{{ jobsStore.total }}</span> trabajos
          </h2>
          <p>esperándote en Argentina</p>
        </div>

        <div class="hero-search">
          <form class="search-form" @submit.prevent="handleSearch">
            <div class="search-input">
              <span class="search-icon">🔍</span>
              <input 
                type="text" 
                v-model="searchQuery"
                placeholder="Puesto, empresa o palabra clave"
              />
            </div>

            <div class="location-input">
              <span class="location-icon">📍</span>
              <select v-model="selectedLocation">
                <option>Todo el país</option>
                <option>Buenos Aires</option>
                <option>Córdoba</option>
                <option>Rosario</option>
                <option>Mendoza</option>
              </select>
            </div>

            <button type="submit" class="search-button">
              Buscar trabajo
            </button>
          </form>
        </div>
      </div>
    </section>

    <section class="featured-jobs">
      <div class="featured-header">
        <h2>Oportunidades Destacadas</h2>
        <p>Encuentra las mejores oportunidades laborales y gana dinero refiriendo talento</p>
      </div>
      <div class="jobs-list">
        <JobCard
          v-for="job in jobsStore.jobs"
          :key="job.referenceId"
          v-bind="job"
        />
      </div>
    </section>
  </main>
</template>

<style scoped>
.home {
  min-height: 100vh;
  padding-top: 64px;
}

.hero {
  background: linear-gradient(135deg, #f9f0ff 0%, #e4f2ff 100%);
  padding: 4rem 2rem;
  text-align: center;
}

.hero-content {
  max-width: 1200px;
  margin: 0 auto;
}

.hero-text {
  margin-bottom: 2rem;
}

.hero-text h1 {
  font-size: 3rem;
  color: #1a1a1a;
  margin-bottom: 1rem;
}

.hero-text p {
  font-size: 1.25rem;
  color: #4b5563;
}

.hero-stats {
  margin-bottom: 2rem;
}

.hero-stats h2 {
  font-size: 2rem;
  color: #1a1a1a;
  margin-bottom: 0.5rem;
}

.hero-stats p {
  color: #4b5563;
  font-size: 1.25rem;
}

.job-count {
  color: #646cff;
}

.hero-search {
  max-width: 800px;
  margin: 0 auto;
}

.search-form {
  display: flex;
  gap: 1rem;
  background: white;
  padding: 1rem;
  border-radius: 12px;
  box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
}

.search-input,
.location-input {
  position: relative;
  flex: 1;
}

.search-icon,
.location-icon {
  position: absolute;
  left: 1rem;
  top: 50%;
  transform: translateY(-50%);
  color: #6b7280;
}

.search-input input,
.location-input select {
  width: 100%;
  padding: 0.75rem 1rem 0.75rem 2.5rem;
  border: 1px solid #e5e7eb;
  border-radius: 8px;
  font-size: 1rem;
  color: #1a1a1a;
  background-color: #f9fafb;
}

.search-input input:focus,
.location-input select:focus {
  outline: none;
  border-color: #646cff;
  box-shadow: 0 0 0 3px rgba(100, 108, 255, 0.1);
}

.search-button {
  padding: 0.75rem 2rem;
  background: #646cff;
  color: white;
  border: none;
  border-radius: 8px;
  font-size: 1rem;
  font-weight: 500;
  cursor: pointer;
  transition: background-color 0.2s;
  white-space: nowrap;
}

.search-button:hover {
  background: #535bf2;
}

.featured-jobs {
  padding: 4rem 2rem;
  max-width: 1200px;
  margin: 0 auto;
}

.featured-header {
  text-align: center;
  margin-bottom: 3rem;
}

.featured-header h2 {
  font-size: 2rem;
  color: #1a1a1a;
  margin-bottom: 1rem;
}

.featured-header p {
  color: #4b5563;
  font-size: 1.1rem;
}

.jobs-list {
  display: flex;
  flex-direction: column;
  gap: 1rem;
}

@media (max-width: 768px) {
  .hero {
    padding: 3rem 1rem;
  }

  .hero-text h1 {
    font-size: 2rem;
  }

  .hero-stats h2 {
    font-size: 1.5rem;
  }

  .search-form {
    flex-direction: column;
  }

  .search-button {
    width: 100%;
  }

  .featured-jobs {
    padding: 2rem 1rem;
  }
}
</style>