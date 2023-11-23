import { observer } from "mobx-react-lite";
import { Divider, Header } from "semantic-ui-react";
import FavoritesProducts from "./FavoritesProducts";
import FavoritesUsers from "./FavoritesUsers";

export default observer(function FavoritesPage() {
    return (
        <>
            <Divider horizontal>
                <Header as='h1'>Favorite users</Header>
            </Divider>
            <FavoritesUsers />
            <Divider horizontal>
                <Header as='h1'>Favorite products</Header>
            </Divider>
            <FavoritesProducts />
        </>
    )
})