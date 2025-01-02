import axios from 'axios';

const API_URL = 'http://netlb-241207186.sa-east-1.elb.amazonaws.com:5000';

const handleApiError = (error) => {
  console.error('API Error:', {
    message: error.message,
    response: error.response?.data,
    status: error.response?.status,
    config: error.config
  });
  throw error;
};



export const jobService = {

  async getJobDetail(jobId) {
    try {
      console.log('Fetching job with params:', { jobId });
      
      const response = await axios.post(`${API_URL}/JobOffer/searchDetailedJobOffer`, {
        jobId
      }, {
        headers: {
          'Content-Type': 'application/json'
        }
      });
      
      console.log('API Response:', {
        status: response.status,
        data: response.data,
        headers: response.headers
      });
      
      if (!response.data) {
        throw new Error('No data received from API');
      }
      
      return {
        jobs: response.data
      };
    } catch (error) {
      handleApiError(error);
    }
  },

  async getJobs(page = 1, pageSize = 10, keyword = '') {
    try {
      console.log('Fetching jobs with params:', { page, pageSize, keyword });
      
      const response = await axios.post(`${API_URL}/JobOffer/searchJobOffers`, {
        keyword,
        page,
        pageSize
      }, {
        headers: {
          'Content-Type': 'application/json'
        }
      });
      
      console.log('API Response:', {
        status: response.status,
        data: response.data,
        headers: response.headers
      });
      
      if (!response.data) {
        throw new Error('No data received from API');
      }
      
      return {
        jobs: response.data.items || [],
        total: response.data.total || 0,
        page: response.data.page || 1,
        pageSize: response.data.pageSize || 10
      };
    } catch (error) {
      handleApiError(error);
    }
  },
};