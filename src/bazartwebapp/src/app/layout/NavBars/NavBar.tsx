import { observer } from "mobx-react-lite";
import { useState } from "react";
import { HashLink } from "react-router-hash-link";
import { Icon, Menu } from "semantic-ui-react";
import AccountContainer from "../../../features/account/AccountContainer";
import { useStore } from "../../stores/store";
import SearchBar from "../SearchBar";

export default observer(function NavBar() {
    const { modalStore } = useStore();

    const [visibleSearch, setVisibleSearch] = useState(false);

    const handleSearchButton = () => {
        setVisibleSearch(!visibleSearch);
    }

    return (
        <>
            <Menu borderless compact secondary icon='labeled' widths='3' >
                <Menu.Item as={HashLink} smooth to='/#about' name='about'>
                    <Icon name='info' />
                    About
                </Menu.Item>
                <Menu.Item as='a' onClick={() => modalStore.openModal(<AccountContainer />)} name='login-register'>
                    <Icon name='user' />
                    Login / Register
                </Menu.Item>
                <Menu.Item as='a' onClick={handleSearchButton}>
                    <Icon name='search' />
                    Search
                </Menu.Item>
            </Menu>
            <div hidden={!visibleSearch}>
                <SearchBar />
            </div>
        </>
    )
})