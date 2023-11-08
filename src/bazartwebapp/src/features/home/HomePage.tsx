import NavBar from "../../app/layout/NavBars/NavBar";
import { Divider, Header, Image } from "semantic-ui-react";
import HomePageCategory from "./HomePageCategory";
import { observer } from "mobx-react-lite";
import ProductCarousel from "../../app/layout/Carousels/Product/ProductCarousel";
import EventCarousel from "../../app/layout/Carousels/Event/EventCarousel";
import HomePageAbout from "./HomePageAbout";
import { useStore } from "../../app/stores/store";
import LoggedUserNavBar from "../../app/layout/NavBars/LoggedUserNavBar";
import { PageTypes } from "../../app/layout/Carousels/pageTypes";

export default observer(function HomePage() {
    const { accountStore } = useStore();

    return (
        <div className='content-style'>
            <div className='sticky-navbar'>
            {accountStore.isLoggedIn
                ? <LoggedUserNavBar />
                : <NavBar />
            }
            </div>
            <Image src="/assets/BazArt_logo_Theme_Light.jpeg" alt="logo" size="large" centered style={{marginBottom:'30px'}} />
            <HomePageCategory />
            <br/><br/>
            <Divider horizontal>
                <Header as='h1'>Latest works</Header>
            </Divider>
            <ProductCarousel page={PageTypes.Home} />
            <br/><br/>
            <Divider horizontal>
                <Header as='h1'>Latest events</Header>
            </Divider>
            <EventCarousel page={PageTypes.Home} />
            <br/><br/>
            <Divider horizontal id='about'>
                <Header as='h1'>About</Header>
            </Divider>
            <HomePageAbout />
        </div>
    )
})