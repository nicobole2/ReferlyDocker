import { ref } from 'vue'
import axios from 'axios';
import { jwtDecode } from 'jwt-decode'

const API_URL = 'http://netlb-241207186.sa-east-1.elb.amazonaws.com:5000'
const currentUser = ref(null)
const TOKEN_KEY = 'auth_token'
const USER_KEY = 'user'


axios.interceptors.request.use(
  (config) => {
    //const token = localStorage.getItem("token")
    const token = localStorage.getItem(TOKEN_KEY)

    if (token) {
      console.log("token: ",token)
      config.headers.Authorization = `Bearer ${token}`
      console.log("interceptors: ",config.headers.Authorization)
    }
    return config
  },
  (error) => {
    return Promise.reject(error)
  }
)

export const auth = {
  async register(userData) {
    try {
      const response = await axios.post(`${API_URL}/Auth/Register`, userData)
      const token = response.data.token
      const user = userData.email
      
      this.setSession(user, token)
      return user
    } catch (error) {
      console.error('Registration error:', error)
      throw new Error('Error al registrar usuario')
    }
  },

  async login(email, password) {
    try {
      const response = await axios.post(`${API_URL}/Auth/Login`, {
        email,
        password
      })

      if (!response.data) {
        throw new Error('No data received from API')
      }

      const user = email
      this.setSession(user, response.data.token)
      return user
    } catch (error) {
      console.error('Login error:', error)
      throw new Error(error.response?.data?.message || 'Error al iniciar sesión')
    }
  },

  logout() {
    this.clearSession()
    window.location.href = '/'
  },

  setSession(user, token) {
    localStorage.setItem(TOKEN_KEY, token)
    localStorage.setItem(USER_KEY, user)
    currentUser.value = user
    axios.defaults.headers.common.Authorization = `Bearer ${token}`
  },

  clearSession() {
    localStorage.removeItem(TOKEN_KEY)
    localStorage.removeItem(USER_KEY)
    currentUser.value = null
    delete axios.defaults.headers.common.Authorization
  },

  getCurrentUser() {
    if (currentUser.value) return currentUser.value
    
    const user = localStorage.getItem(USER_KEY)
    console.log("user: ", user)
    if (user) {
      currentUser.value = user
      return currentUser.value
    }
    return null
  },

  getToken() {
    return localStorage.getItem(TOKEN_KEY)
  },

  isTokenExpired(token) {
    try {
      const decoded = jwtDecode(token);
      if (!decoded.exp) return true;
      
      const expirationDate = decoded.exp * 1000;
      const currentDate = Date.now();
      
      const bufferTime = 5 * 60 * 1000;
      return expirationDate - bufferTime <= currentDate;
    } catch (error) {
      console.error('Error decoding token:', error);
      return true;
    }
  },

  isAuthenticated() {
    const token = this.getToken()
    if (!token) return false
    console.log("Token Expiration: ", this.isTokenExpired(token))
    return true
  }
}