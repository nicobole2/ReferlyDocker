import { createApp } from 'vue'
import { createRouter, createWebHistory } from 'vue-router'
import './style.css'
import App from './App.vue'
import Home from './components/Home.vue'
import Login from './components/Login.vue'
import Register from './components/Register.vue'
import JobDetail from './components/JobDetail.vue'
import About from './components/About.vue'
import Contact from './components/Contact.vue'
import Profile from './components/Profile.vue'
import SearchResults from './components/SearchResult.vue'

const router = createRouter({
  history: createWebHistory(),
  routes: [
    { path: '/', component: Home },
    { path: '/login', component: Login },
    { path: '/register', component: Register },
    { path: '/job/:id', component: JobDetail },
    { path: '/about', component: About },
    { path: '/contact', component: Contact },
    { path: '/profile', component: Profile },
    { path: '/search', component: SearchResults }
  ]
})

const app = createApp(App)
app.use(router)
app.mount('#app')