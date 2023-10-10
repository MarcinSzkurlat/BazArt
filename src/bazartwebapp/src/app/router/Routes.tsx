import { createBrowserRouter, RouteObject } from "react-router-dom";
import CategoryPage from "../../features/category/CategoryPage";
import EventPage from "../../features/event/EventPage";
import ProductPage from "../../features/product/ProductPage";
import App from "../layout/App";

export const routes: RouteObject[] = [
    {
        path: '/',
        element: <App />,
        children: [
            { path: 'category/:categoryName', element: <CategoryPage /> },
            { path: 'event/:id', element: <EventPage /> },
            { path: 'product/:id', element: <ProductPage /> }
        ]
    }
]

export const router = createBrowserRouter(routes);