import { observer } from "mobx-react-lite";
import { Link } from "react-router-dom";
import { HashLink } from "react-router-hash-link";
import { Icon, Menu } from "semantic-ui-react";

interface Props {
    className?: string;
}

export default observer(function NavBar({ className }: Props) {
    return (
        <Menu borderless compact secondary className={className} icon='labeled' widths='3' >
            <Menu.Item as={HashLink} smooth to='/#about' name='about'>
                <Icon name='info' />
                    About
            </Menu.Item>
            <Menu.Item as='a' to='/' name='login-register'>
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