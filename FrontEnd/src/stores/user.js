import { ref } from 'vue'

const referrals = ref([])
const notifications = ref([])

export const useUserStore = () => {
  const addReferral = (jobId, referralData) => {
    const newReferral = {
      id: Date.now(),
      jobId,
      candidateName: `${referralData.firstName} ${referralData.lastName}`,
      email: referralData.email,
      status: 'En proceso',
      date: new Date().toISOString(),
      ...referralData
    }
    
    referrals.value.unshift(newReferral)
    
    // Add notification
    notifications.value.unshift({
      id: Date.now(),
      type: 'success',
      message: `Has referido exitosamente a ${newReferral.candidateName}`,
      date: new Date().toISOString()
    })
    
    return newReferral
  }
  
  const clearUserData = () => {
    referrals.value = []
    notifications.value = []
  }
  
  const getReferrals = () => referrals.value
  const getNotifications = () => notifications.value
  
  return {
    referrals,
    notifications,
    addReferral,
    clearUserData,
    getReferrals,
    getNotifications
  }
}