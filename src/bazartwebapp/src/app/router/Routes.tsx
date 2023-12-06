import { createBrowserRouter, Navigate, RouteObject } from "react-router-dom";
import AccountContainer from "../../features/account/AccountContainer";
import CartPage from "../../features/cart/CartPage";
import CategoryPage from "../../features/category/CategoryPage";
import NotFound from "../../features/errors/NotFound";
import EventPage from "../../features/event/EventPage";
import FavoritesPage from "../../features/favorites/FavoritesPage";
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
            { path: 'authorize', element: <AccountContainer /> },
            { path: 'favorites', element: <FavoritesPage /> },
            { path: 'cart', element: <CartPage /> },
            { path: 'not-found', element: <NotFound /> },
            { path: '*', element: <Navigate replace to='/not-found' /> }
        ]
    }
]

export const router = createBrowserRouter(routes);