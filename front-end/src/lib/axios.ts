import type { AxiosInstance } from 'axios'
import axios from 'axios'

/**
 * Base instance for Axios API calls.
 * Configured with a base URL, a timeout, and default headers.
 */
export const baseApiInstance: AxiosInstance = axios.create({
  baseURL: process.env.NEXT_PUBLIC_API_URL,
  timeout: 30000,
  headers: {
    'Content-Type': 'application/json',
  },
})
