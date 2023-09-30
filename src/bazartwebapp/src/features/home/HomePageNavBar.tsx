import { observer } from "mobx-react-lite";
import { Icon, Menu } from "semantic-ui-react";

interface Props {
    scrollToAbout: Function;
}

export default observer(function HomePageNavBar({ scrollToAbout}: Props) {
    return (
        <Menu borderless compact fixed='top' icon='labeled' widths='3' >
            <Menu.Item as='a' onClick={() => { scrollToAbout() }} name='about'>
                <Icon name='info' />
                    About
            </Menu.Item>
            <Menu.Item as='a' to='/' name='login-register'>
                <Icon name='user' />
                    Login / Register
            </Menu.Item>
            <Menu.Item as='a' to='/' name='search'>
                <Icon name='search' />
                    Search
            </Menu.Item>
        </Menu>
    )
})