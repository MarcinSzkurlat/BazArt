import { observer } from "mobx-react-lite";
import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import { Icon, Label, Menu, Sidebar } from "semantic-ui-react";
import SettingsContainer from "../../../features/account/SettingsContainer";
import EventForm from "../../../features/event/EventForm";
import ProductForm from "../../../features/product/ProductForm";
import { ActionTypes } from "../../models/actionTypes";
import { useStore } from "../../stores/store";
import SearchBar from "../SearchBar";

export default observer(function LoggedUserNavBar() {
    const { accountStore: { user, logout }, modalStore, searchStore, productStore } = useStore();
    const { visibleSearchBar, setVisibleSearchBar } = searchStore;

    const [visibleMenu, setVisibleMenu] = useState(false);

    const handleMenuButton = () => {
        setVisibleMenu(!visibleMenu);
    }

    const handleSearchButton = () => {
        setVisibleSearchBar(!visibleSearchBar);
    }

    useEffect(() => {
        productStore.loadUserCartProducts();
    }, [])

    return (
        <>
            <Menu borderless compact secondary icon='labeled' widths='4' >
                <Menu.Item as='a' onClick={handleSearchButton}>
                    <Icon name='search' />
                    Search
                </Menu.Item>
                <Menu.Item as={Link} to='favorites' name='favorites'>
                    <Icon name='heart' />
                    Favorites
                </Menu.Item>
                <Menu.Item as={Link} to='cart' name='cart'>
                    <div style={{ marginBottom:'7px' }}>
                        <Icon size='large' name='cart' />
                        {productStore.userCartQuantity > 0
                            ? <Label circular color='black' size='tiny'>{productStore.userCartQuantity}</Label>
                            : <></>}
                    </div>
                    Cart
                </Menu.Item>
                <Menu.Item as='a' onClick={handleMenuButton}>
                    <Icon name='bars' />
                    Menu
                </Menu.Item>
            </Menu>
            <div hidden={!visibleSearchBar}>
                <SearchBar />
            </div>
            <Sidebar as={Menu}
                onHide={() => setVisibleMenu(false)}
                animation='push'
                direction='right'
                vertical
                icon='labeled'
                compact
                width='thin'
                visible={visibleMenu}
                className='background-color-gold'>
                <Menu.Item as={Link} to={`/user/${user?.id}`}>
                    <Icon name='user' />
                    My profile
                </Menu.Item>
                <Menu.Item as='a' onClick={() => modalStore.openModal(<ProductForm action={ActionTypes.Create} />)}>
                    <Icon name='money bill alternate outline' />
                    Sell work
                </Menu.Item>
                <Menu.Item as='a' onClick={() => modalStore.openModal(<EventForm action={ActionTypes.Create} />)}>
                    <Icon name='calendar plus outline' />
                    Create event
                </Menu.Item>
                <Menu.Item as='a' onClick={() => modalStore.openModal(<SettingsContainer />)}>
                    <Icon name='settings' />
                    Settings
                </Menu.Item>
                <Menu.Item onClick={logout}>
                    <Icon name='power' />
                    Logout
                </Menu.Item>
            </Sidebar>
        </>
    )
})