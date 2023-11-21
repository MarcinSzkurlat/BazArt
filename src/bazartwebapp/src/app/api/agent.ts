import axios, { AxiosError, AxiosResponse } from "axios";
import { Category } from "../models/Category/category";
import { Event } from "../models/Event/event";
import { EventDetails } from "../models/Event/eventDetails";
import { Product } from "../models/Product/product";
import { ProductDetails } from "../models/Product/productDetails";
import { User } from "../models/User/user";
import { AccountRegistration } from "../models/Account/accountRegistration";
import { AccountLogin } from "../models/Account/accountLogin";
import { store } from "../stores/store";
import { UserDetails } from "../models/User/userDetails";
import { router } from "../router/Routes";
import { ManipulateProduct } from "../models/Product/manipulateProduct";
import { ManipulateEvent } from "../models/Event/manupulateEvent";
import { AccountChangePassword } from "../models/Account/accountChangePassword";
import { EditUser } from "../models/User/editUser";
import { Searching } from "../models/Search/Searching";
import { PaginatedItems } from "../models/paginatedItems";

axios.defaults.baseURL = 'http://localhost:5000/api';

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
            router.navigate('/authorize');
            break;
        case 403:
            if (data) console.log(data)
            router.navigate(-1);
            if (store.modalStore.modal.open) store.modalStore.closeModal()
            break;
        case 404:
            console.log('not-found')
            break;
        case 500:
            console.log('server error');
            break;
    }
})

const responseBody = <T>(respone: AxiosResponse<T>) => respone.data;

const requests = {
    get: <T>(url: string) => axios.get<T>(url).then(responseBody),
    post: <T>(url: string, body: {}) => axios.post<T>(url, body).then(responseBody),
    put: <T>(url: string, body: {}) => axios.put<T>(url, body).then(responseBody),
    delete: <T>(url: string) => axios.delete<T>(url).then(responseBody)
}

const Events = {
    list: (category: string, pageNumber: number) => requests.get<PaginatedItems<Event>>(`/event?categoryName=${category}&pageNumber=${pageNumber}`),
    details: (id: string) => requests.get<EventDetails>(`/event/${id}`),
    create: (event: ManipulateEvent) => requests.post<EventDetails>('/event', event),
    update: (event: ManipulateEvent, id: string) => requests.put<EventDetails>(`/event/${id}`, event),
    delete: (id: string) => requests.delete<void>(`/event/${id}`),
    latest: () => requests.get<Event[]>('/event/latest'),
    userEvents: (id: string, pageNumber: number) => requests.get<PaginatedItems<Event>>(`/user/${id}/events?pageNumber=${pageNumber}`)
}

const Products = {
    list: (category: string, pageNumber: number) => requests.get<PaginatedItems<Product>>(`/product?categoryName=${category}&pageNumber=${pageNumber}`),
    details: (id: string) => requests.get<ProductDetails>(`/product/${id}`),
    create: (product: ManipulateProduct) => requests.post<ProductDetails>('/product', product),
    update: (product: ManipulateProduct, id: string) => requests.put<ProductDetails>(`/product/${id}`, product),
    delete: (id: string) => requests.delete<void>(`/product/${id}`),
    latest: () => requests.get<Product[]>('/product/latest'),
    userProducts: (id: string, pageNumber: number) => requests.get<PaginatedItems<Product>>(`/user/${id}/products?pageNumber=${pageNumber}`)
}

const Categories = {
    list: () => requests.get<Category[]>('/category'),
    details: (name: string) => requests.get<Category>(`/category/${name}`)
}

const Account = {
    login: (data: AccountLogin) => requests.post<User>('/account/login', data),
    registration: (data: AccountRegistration) => requests.post<User>('/account/registration', data),
    currentUser: () => requests.get<User>('/account'),
    changePassword: (data: AccountChangePassword) => requests.put<User>('/account/password', data)
}

const Users = {
    details: (id: string) => requests.get<UserDetails>(`/user/${id}`),
    editUser: (data: EditUser) => requests.put<UserDetails>('/user', data),
    deleteUser: (id: string) => requests.delete<void>(`/user/${id}`)
}

const Search = {
    search: (searchQuery: string) => requests.get<Searching>(`/search?searchQuery=${searchQuery}`)
}

const agent = {
    Events,
    Products,
    Categories,
    Account,
    Users,
    Search
}

export default agent;