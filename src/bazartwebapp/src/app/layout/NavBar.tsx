import { observer } from "mobx-react-lite";
import { Link } from "react-router-dom";
import { HashLink } from "react-router-hash-link";
import { Icon, Menu } from "semantic-ui-react";
import AccountContainer from "../../features/user/AccountContainer";
import { useStore } from "../stores/store";

interface Props {
    className?: string;
}

export default observer(function NavBar({ className }: Props) {
    const { modalStore } = useStore();

    return (
        <Menu borderless compact secondary className={className} icon='labeled' widths='3' >
            <Menu.Item as={HashLink} smooth to='/#about' name='about'>
                <Icon name='info' />
                    About
            </Menu.Item>
            <Menu.Item as='a' onClick={() => modalStore.openModal(<AccountContainer />)} name='login-register'>
                <Icon name='user' />
                    Login / Register
            </Menu.Item>
            <Menu.Item as={Link} to='/' name='search'>
                <Icon name='search' />
                    Search
            </Menu.Item>
        </Menu>
    )
})