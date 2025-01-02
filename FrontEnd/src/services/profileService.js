import axios from 'axios';
import { auth } from './auth';

const API_URL = 'http://netlb-241207186.sa-east-1.elb.amazonaws.com:5000';

export const profileService = {

  async getReferrals() {
    try {
        const token = auth.getToken();
        if (!token) {
          throw new Error('No se encontró el token de autenticación');
        }
            
        const response = await axios.post(`${API_URL}/Referrals/getReferralsByHunterId`);
        
        if (!response.data) {
            return [];
        }
        
        return response.data.map(referral => ({
            candidateName: referral.completeName,
            position: referral.jobName,
            company: referral.area,
            status: referral.state,
            date: referral.referredAt
        }));
        } catch (error) {
        console.error('Error fetching referrals:', error);
        throw new Error(error.response?.data?.message || 'Error al cargar los referidos');
        }
  }
};