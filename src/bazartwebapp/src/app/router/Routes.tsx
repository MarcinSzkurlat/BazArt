import { createBrowserRouter, RouteObject } from "react-router-dom";
import CategoryPage from "../../features/category/CategoryPage";
import App from "../layout/App";

export const routes: RouteObject[] = [
    {
        path: '/',
        element: <App />,
        children: [
            { path: 'category/:categoryName', element: <CategoryPage />}
        ]
    }
]

export const router = createBrowserRouter(routes);