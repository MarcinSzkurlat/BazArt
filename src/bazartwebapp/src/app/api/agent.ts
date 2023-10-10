import axios, { AxiosResponse } from "axios";
import { Category } from "../models/Category/category";
import { CreateEvent } from "../models/Event/createEvent";
import { Event } from "../models/Event/event";
import { EventDetails } from "../models/Event/eventDetails";
import { CreateProduct } from "../models/Product/createProduct";
import { Product } from "../models/Product/product";
import { ProductDetails } from "../models/Product/productDetails";

axios.defaults.baseURL = 'https:localhost:5050/api';

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
    details: (name: string) => requests.get <Category>(`/category/${name}`)
}

const agent = {
    Events,
    Products,
    Categories
}

export default agent;