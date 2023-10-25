import { createBrowserRouter, RouteObject } from "react-router-dom";
import AccountContainer from "../../features/account/AccountContainer";
import CategoryPage from "../../features/category/CategoryPage";
import EventPage from "../../features/event/EventPage";
import ProductPage from "../../features/product/ProductPage";
import UserPage from "../../features/user/UserPage";
import App from "../layout/App";

export const routes: RouteObject[] = [
    {
        path: '/',
        element: <App />,
        children: [
            { path: 'category/:categoryName', element: <CategoryPage /> },
            { path: 'event/:id', element: <EventPage /> },
            { path: 'product/:id', element: <ProductPage /> },
            { path: 'user/:id', element: <UserPage /> },
            { path: 'authorize', element: <AccountContainer /> }
        ]
    }
]

export const router = createBrowserRouter(routes);