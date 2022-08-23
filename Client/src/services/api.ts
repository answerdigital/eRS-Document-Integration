import axios from "axios";

const customAxios = axios.create({
    baseURL: `${process.env.REACT_APP_API_Address}/api`
});

export default customAxios;