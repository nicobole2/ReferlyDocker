<script setup>
import { ref, onMounted, watch } from 'vue';
import { useRouter } from 'vue-router';
import { auth } from '../services/auth';

const searchQuery = ref('');
const router = useRouter();
const isUserAuthenticated = ref(false);
const currentUser = ref(null);

const checkAuthStatus = () => {
  isUserAuthenticated.value = auth.isAuthenticated();
  currentUser.value = auth.getCurrentUser();
};

const handleSearch = () => {
  if (searchQuery.value.trim()) {
    router.push({
      path: '/search',
      query: { q: searchQuery.value }
    });
  }
};

const navigate = (path) => {
  router.push(path);
};

const handleLogout = () => {
  auth.logout();
  checkAuthStatus();
};

// Check auth status on mount and whenever route changes
onMounted(checkAuthStatus);
watch(() => router.currentRoute.value, checkAuthStatus);
</script>

<template>
  <nav class="navbar">
    <div class="nav-brand" @click="navigate('/')">
      <h1>Referly</h1>
    </div>
    
    <div class="search-container">
      <div class="search-input">
        <span class="search-icon">🔍</span>
        <input 
          type="text" 
          v-model="searchQuery"
          placeholder="Buscar empleos..."
          @keyup.enter="handleSearch"
        />
        <button class="search-button" @click="handleSearch">
          Buscar
        </button>
      </div>
    </div>

    <div class="nav-links">
      <a href="#" @click.prevent="navigate('/about')">Sobre nosotros</a>
      <a href="#" @click.prevent="navigate('/contact')">Contacto</a>
      <template v-if="isUserAuthenticated">
        <div class="user-menu">
          <button class="profile-btn" @click="navigate('/profile')">
            <span class="user-name">{{'Mi Perfil' }}</span>
          </button>
          <button class="logout-btn" @click="handleLogout">
            Cerrar sesión
          </button>
        </div>
      </template>
      <template v-else>
        <button class="sign-in-btn" @click="navigate('/login')">Sign In</button>
      </template>
    </div>
  </nav>
</template>

<style scoped>
.navbar {
  background: white;
  padding: 1rem 2rem;
  display: flex;
  justify-content: space-between;
  align-items: center;
  box-shadow: 0 2px 4px rgba(0, 0, 0, 0.1);
  position: fixed;
  top: 0;
  left: 0;
  right: 0;
  z-index: 100;
}

.nav-brand {
  cursor: pointer;
}

.nav-brand h1 {
  color: #646cff;
  margin: 0;
  font-size: 1.5rem;
}

.search-container {
  flex: 1;
  max-width: 500px;
  margin: 0 2rem;
}

.search-input {
  position: relative;
  width: 100%;
  display: flex;
  gap: 0.5rem;
}

.search-icon {
  position: absolute;
  left: 1rem;
  top: 50%;
  transform: translateY(-50%);
  color: #6b7280;
}

.search-input input {
  flex: 1;
  padding: 0.5rem 1rem 0.5rem 2.5rem;
  border: 1px solid #e5e7eb;
  border-radius: 6px;
  font-size: 0.875rem;
  transition: all 0.2s;
  background: white;
  color: #111827;
}

.search-input input::placeholder {
  color: #9ca3af;
}

.search-input input:focus {
  outline: none;
  border-color: #646cff;
  box-shadow: 0 0 0 3px rgba(100, 108, 255, 0.1);
}

.search-button {
  padding: 0.5rem 1rem;
  background: #646cff;
  color: white;
  border: none;
  border-radius: 6px;
  font-size: 0.875rem;
  cursor: pointer;
  transition: background-color 0.2s;
}

.search-button:hover {
  background: #535bf2;
}

.nav-links {
  display: flex;
  gap: 2rem;
  align-items: center;
}

.nav-links a {
  color: #374151;
  text-decoration: none;
  font-weight: 500;
  transition: color 0.2s;
}

.nav-links a:hover {
  color: #646cff;
}

.user-menu {
  display: flex;
  gap: 1rem;
  align-items: center;
}

.sign-in-btn, .profile-btn {
  background: #646cff;
  color: white;
  border: none;
  padding: 0.5rem 1.5rem;
  border-radius: 6px;
  font-weight: 500;
  cursor: pointer;
  transition: background-color 0.2s;
}

.sign-in-btn:hover, .profile-btn:hover {
  background: #535bf2;
}

.logout-btn {
  background: #ef4444;
  color: white;
  border: none;
  padding: 0.5rem 1.5rem;
  border-radius: 6px;
  font-weight: 500;
  cursor: pointer;
  transition: background-color 0.2s;
}

.logout-btn:hover {
  background: #dc2626;
}

.user-name {
  font-size: 0.875rem;
}

@media (max-width: 1024px) {
  .search-container {
    max-width: 300px;
    margin: 0 1rem;
  }
}

@media (max-width: 768px) {
  .navbar {
    padding: 1rem;
    flex-direction: column;
    gap: 1rem;
  }

  .nav-links {
    gap: 1rem;
    flex-wrap: wrap;
    justify-content: center;
  }

  .search-container {
    order: 3;
    max-width: 100%;
    margin: 1rem 0 0;
    width: 100%;
  }

  .user-menu {
    flex-direction: column;
    width: 100%;
  }

  .profile-btn, .logout-btn {
    width: 100%;
  }
}
</style>