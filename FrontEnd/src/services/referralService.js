import { auth } from './auth';

const API_URL = 'http://localhost:5000';

export const referralService = {
  async createReferral(referralData) {
    try {
      const formData = new FormData();
      
      formData.append('JobSearch', referralData.jobId.toString());
      formData.append('FirstName', referralData.firstName);
      formData.append('LastName', referralData.lastName);
      formData.append('Email', referralData.email);
      
      if (referralData.file) {

        formData.append('pdfFileName', referralData.file.name);
        formData.append('pdfContentType', referralData.file.type);
        formData.append('pdfFile', referralData.file);
      }

      const token = auth.getToken();
      if (!token) {
        throw new Error('No se encontró el token de autenticación');
      }

      const response = await fetch(`${API_URL}/Referrals/createReferral`, {
        method: 'POST',
        headers: {
          'Authorization': `Bearer ${token}`
        },
        body: formData
      });

      // Read the response as text first
      const responseText = await response.text();
      let data;
      
      try {
        // Try to parse the response text as JSON if it's not empty
        data = responseText ? JSON.parse(responseText) : {};
      } catch (e) {
        console.error('Error parsing response:', e);
        throw new Error('Error al procesar la respuesta del servidor');
      }

      // Check response status
      if (response.status === 200) {
        if (data.message === 'REFERRAL_CREATED_SUCCESSFULLY' || 
            data.message === 'REFERRAL_UPDATED_SUCCESSFULLY') {
          return { success: true, message: data.message };
        }
      }

      // Handle specific error cases based on status codes
      if (response.status === 404) {
        if (data.message === 'JOB_SEARCH_DONT_EXIST') {
          throw new Error('La oportunidad laboral no existe');
        }
        if (data.message === 'HUNTER_NOT_EXIST') {
          throw new Error('Usuario no encontrado');
        }
      }

      if (response.status === 409 && data.message === 'ALREADY_REFFERED_FOR_THIS_JOB') {
        throw new Error('Ya se ha referido a este candidato para esta oportunidad');
      }

      // If we get here, it's an unknown error
      throw new Error(data.message || 'Error al crear el referido');
    } catch (error) {
      console.error('Referral error:', error);
      throw new Error(error.message || 'Error al crear el referido. Por favor, intenta nuevamente.');
    }
  }
};