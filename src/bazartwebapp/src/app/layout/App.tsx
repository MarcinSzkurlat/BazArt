import { observer } from "mobx-react-lite";
import { useEffect } from "react";
import { Link, Outlet, useLocation } from "react-router-dom";
import { Grid, Image } from "semantic-ui-react";
import HomePage from "../../features/home/HomePage";
import LoggedUserNavBar from "../../features/user/LoggedUserNavBar";
import { useStore } from "../stores/store";
import Footer from "./Footer";
import ModalContainer from "./Modals/ModalContainer";
import NavBar from "./NavBar";

function App() {
    const { accountStore } = useStore();
    const location = useLocation();

    useEffect(() => {
        if (accountStore.user === null && accountStore.token) {
            accountStore.getCurrentUser();
        }
    }, [accountStore.user, accountStore.token, accountStore.isLoggedIn])

    return (
        <div className="app">
            <ModalContainer />
            {location.pathname === '/'
                ? <HomePage />
                : <>
                    <Grid>
                        <Grid.Row>
                            <Grid.Column width={6} textAlign='center'>
                                <Image as={Link} to='/' src="/assets/BazArt_logo_Theme_Light.jpeg" alt="logo" size="medium" verticalAlign='middle' />
                            </Grid.Column>
                            <Grid.Column width={10}>
                                {accountStore.isLoggedIn
                                    ? <LoggedUserNavBar className='navbar' />
                                    : <NavBar className="navbar" />
                                }
                            </Grid.Column>
                        </Grid.Row>
                    </Grid>
                    <div className='content'>
                        <Outlet />
                    </div>
                </>
            }
            <Footer />
        </div>
    );
}

export default observer(App);
