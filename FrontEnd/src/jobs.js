import { ref } from 'vue'

// Generate 50 mock jobs
const generateMockJobs = () => {
  const jobs = []
  const companies = [
    'TechCorp Solutions', 'Global Innovations', 'Digital Dynamics', 
    'Future Systems', 'Smart Solutions', 'Data Experts',
    'Cloud Technologies', 'AI Solutions', 'Web Masters',
    'Mobile Experts'
  ]
  
  const locations = [
    'Buenos Aires', 'Córdoba', 'Rosario', 'Mendoza', 
    'La Plata', 'Mar del Plata', 'Salta', 'Santa Fe'
  ]
  
  const positions = [
    'Frontend Developer', 'Backend Developer', 'Full Stack Developer',
    'UX/UI Designer', 'Product Manager', 'Data Scientist',
    'DevOps Engineer', 'QA Engineer', 'Scrum Master',
    'Project Manager', 'Business Analyst', 'Marketing Specialist',
    'Sales Representative', 'HR Manager', 'Financial Analyst'
  ]
  
  const workModes = ['Remoto', 'Presencial', 'Híbrido']
  
  for (let i = 1; i <= 50; i++) {
    const daysAgo = Math.floor(Math.random() * 30)
    jobs.push({
      id: i,
      title: positions[Math.floor(Math.random() * positions.length)],
      company: companies[Math.floor(Math.random() * companies.length)],
      location: `${locations[Math.floor(Math.random() * locations.length)]}, Argentina`,
      workMode: workModes[Math.floor(Math.random() * workModes.length)],
      description: `Estamos buscando un profesional con experiencia para unirse a nuestro equipo. El candidato ideal debe tener al menos 3 años de experiencia en el rubro y excelentes habilidades de comunicación...`,
      publishedAt: `Publicado hace ${daysAgo} día${daysAgo !== 1 ? 's' : ''}`,
      salary: `USD ${Math.floor(Math.random() * (8000 - 3000) + 3000)}`,
      requirements: [
        'Experiencia mínima de 3 años',
        'Inglés avanzado',
        'Trabajo en equipo',
        'Capacidad de resolución de problemas'
      ],
      benefits: [
        'Seguro médico',
        'Capacitación continua',
        'Horario flexible',
        'Home office'
      ]
    })
  }
  
  return jobs
}

export const useJobsStore = () => {
  const jobs = ref(generateMockJobs())
  const featuredJobs = ref(jobs.value.slice(0, 10))
  
  const searchJobs = (query, location) => {
    if (!query && !location) return jobs.value
    
    return jobs.value.filter(job => {
      const matchQuery = !query || 
        job.title.toLowerCase().includes(query.toLowerCase()) ||
        job.company.toLowerCase().includes(query.toLowerCase()) ||
        job.description.toLowerCase().includes(query.toLowerCase())
        
      const matchLocation = !location || 
        location === 'Todo el país' ||
        job.location.toLowerCase().includes(location.toLowerCase())
        
      return matchQuery && matchLocation
    })
  }
  
  const getJobById = (id) => {
    return jobs.value.find(job => job.id === Number(id))
  }
  
  return {
    jobs,
    featuredJobs,
    searchJobs,
    getJobById
  }
}