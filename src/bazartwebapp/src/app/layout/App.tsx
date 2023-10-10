import { observer } from "mobx-react-lite";
import { Link, Outlet, useLocation } from "react-router-dom";
import { Grid, Image } from "semantic-ui-react";
import HomePage from "../../features/home/HomePage";
import Footer from "./Footer";
import NavBar from "./NavBar";

function App() {
    const location = useLocation();

    return (
        <div className="app">
            {location.pathname === '/'
                ? <HomePage />
                : <>
                    <Grid>
                        <Grid.Row>
                            <Grid.Column width={6} textAlign='center'>
                                <Image as={Link} to='/' src="/assets/BazArt_logo_Theme_Light.jpeg" alt="logo" size="medium" verticalAlign='middle' />
                            </Grid.Column>
                            <Grid.Column width={10}>
                                <NavBar className="navbar" />
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
