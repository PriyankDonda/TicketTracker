import axios from "axios";
import config from "../config/config";

//create reusable axios instance
const api = axios.create({
  baseURL: config.baseURL,
});

//request intercepter to attach auth token
api.interceptors.request.use(
  (config) => {
    const token = localStorage.getItem("authToken"); //we can use cookie/storage

    if (token) config.headers.Authorization = `Bearer ${token}`;

    return config;
  },
  (error) => Promise.reject(error)
);

//response intercepter to handle global error/logging
api.interceptors.response.use(
  (response) => {
    const customRes = response.data;

    if (customRes.isSuccess === false) {
      return Promise.reject({
        message: customRes.message,
        data: customRes.data,
      });
    }

    return customRes.data;
  },
  (error) => {
    //console.log("BE Error: " + error.response.data);

    if (error.response?.status === 401) {
      console.warn("Unauthorized - redirecting to login");
      localStorage.removeItem("authToken");
      //navigate to login
    }

    if (error.response?.status === 500) {
      console.error("Server Error: ", error.response.data);
    }

    return Promise.reject(error);
  }
);

export default api;
