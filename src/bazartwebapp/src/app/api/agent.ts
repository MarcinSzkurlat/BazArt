import axios, { AxiosError, AxiosResponse } from "axios";
import { Category } from "../models/Category/category";
import { CreateEvent } from "../models/Event/createEvent";
import { Event } from "../models/Event/event";
import { EventDetails } from "../models/Event/eventDetails";
import { CreateProduct } from "../models/Product/createProduct";
import { Product } from "../models/Product/product";
import { ProductDetails } from "../models/Product/productDetails";
import { User } from "../models/User/user";
import { AccountRegistration } from "../models/Account/accountRegistration";
import { AccountLogin } from "../models/Account/accountLogin";
import { store } from "../stores/store";

axios.defaults.baseURL = 'https:localhost:5050/api';

axios.interceptors.request.use(config => {
    const token = store.accountStore.token;

    if (token) config.headers.Authorization = `Bearer ${token}`;

    return config;
})

axios.interceptors.response.use(async respone => {
    return respone;
}, (error: AxiosError) => {
    const { data, status } = error.response as AxiosResponse;

    switch (status) {
        case 400:
            if (data.errors) {
                const errors = new Map<string, string[]>();
                for (const key in data.errors) {
                    if (data.errors[key]) {
                        errors.set(key, data.errors[key]);
                    }
                }

                throw errors;
            }
            break;
        case 401:
            console.log('unathorized');
            break;
        case 403:
            console.log('forbidden');
            break;
        case 404:
            console.log('not-found')
            break;
        case 500:
            console.log('server error');
            break;
    }
})

const responeBody = <T>(respone: AxiosResponse<T>) => respone.data;

const requests = {
    get: <T>(url: string) => axios.get<T>(url).then(responeBody),
    post: <T>(url: string, body: {}) => axios.post<T>(url, body).then(responeBody),
    put: <T>(url: string, body: {}) => axios.put<T>(url, body).then(responeBody),
    delete: <T>(url: string) => axios.delete<T>(url).then(responeBody)
}

const Events = {
    list: (category: string) => requests.get<Event[]>(`/event?categoryName=${category}`),
    details: (id: string) => requests.get<EventDetails>(`/event/${id}`),
    create: (event: CreateEvent) => requests.post<void>('/event', event),
    update: (event: EventDetails, id: string) => requests.put<EventDetails>(`/event/${id}`, event),
    delete: (id: string) => requests.delete<void>(`/event/${id}`),
    latest: () => requests.get<Event[]>('/event/latest')
}

const Products = {
    list: (category: string) => requests.get<Product[]>(`/product?categoryName=${category}`),
    details: (id: string) => requests.get<ProductDetails>(`/product/${id}`),
    create: (product: CreateProduct) => requests.post<void>('/product', product),
    update: (product: ProductDetails, id: string) => requests.put<ProductDetails>(`/product/${id}`, product),
    delete: (id: string) => requests.delete<void>(`/product/${id}`),
    latest: () => requests.get<Product[]>('/product/latest')
}

const Categories = {
    list: () => requests.get<Category[]>('/category'),
    details: (name: string) => requests.get<Category>(`/category/${name}`)
}

const Account = {
    login: (data: AccountLogin) => requests.post<User>('/account/login', data),
    registration: (data: AccountRegistration) => requests.post<User>('/account/registration', data),
    currentUser: () => requests.get<User>('/account')
}

const agent = {
    Events,
    Products,
    Categories,
    Account
}

export default agent;