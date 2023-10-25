import { observer } from "mobx-react-lite";
import { useState } from "react";
import { Link } from "react-router-dom";
import { Icon, Menu, Sidebar } from "semantic-ui-react";
import { useStore } from "../../stores/store";

interface Props {
    className?: string;
}

export default observer(function LoggedUserNavBar({ className }: Props) {
    const { accountStore: { user, logout } } = useStore();
    const [visible, setVisible] = useState(false);

    const handleMenuButton = () => {
        setVisible(!visible);
    }

    return (
        <>
            <Menu borderless compact secondary className={className} icon='labeled' widths='4' >
                <Menu.Item as={Link} to='/' name='search'>
                    <Icon name='search' />
                    Search
                </Menu.Item>
                <Menu.Item as={Link} to='/' name='favorites'>
                    <Icon name='heart' />
                    Favorites
                </Menu.Item>
                <Menu.Item as={Link} to='/' name='cart'>
                    <Icon name='cart' />
                    Cart
                </Menu.Item>
                <Menu.Item as='a' onClick={handleMenuButton}>
                    <Icon name='bars' />
                    Menu
                </Menu.Item>
            </Menu>
            <Sidebar as={Menu}
                onHide={() => setVisible(false)}
                animation='push'
                direction='right'
                vertical
                icon='labeled'
                compact
                width='thin'
                visible={visible}
                className='background-color-gold'>
                <Menu.Item as={Link} to={`/user/${user?.id}`}>
                    <Icon name='user' />
                    My profile
                </Menu.Item>
                <Menu.Item as={Link} to={'/'}>
                    <Icon name='money bill alternate outline' />
                    Sell work
                </Menu.Item>
                <Menu.Item as={Link} to={'/'}>
                    <Icon name='calendar plus outline' />
                    Create event
                </Menu.Item>
                <Menu.Item as={Link} to={'/'}>
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