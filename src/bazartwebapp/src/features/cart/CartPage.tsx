import { observer } from "mobx-react-lite";
import { Divider, Header } from "semantic-ui-react";
import CartProducts from "./CartProducts";

export default observer(function CartPage() {
    return (
        <>
            <Divider horizontal>
                <Header as='h1'>Your cart</Header>
            </Divider>
            <CartProducts />
        </>
    )
})