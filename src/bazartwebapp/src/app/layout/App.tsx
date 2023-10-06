import { observer } from "mobx-react-lite";
import { Outlet, useLocation } from "react-router-dom";
import HomePage from "../../features/home/HomePage";
import Footer from "./Footer";

function App() {
    const location = useLocation();

    return (
        <div className="app">
            <div className='content'>
                {location.pathname === '/'
                    ? <HomePage />
                    : <Outlet />
                }
            </div>
            <Footer />
        </div>
    );
}

export default observer(App);
