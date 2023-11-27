import { observer } from "mobx-react-lite";
import { useEffect, useState } from "react";
import { Button, Divider, Header } from "semantic-ui-react";
import { Product } from "../../app/models/Product/product";
import { useStore } from "../../app/stores/store";
import CartProductItem from "./CartProductItem";

export default observer(function CartProducts() {
    const { productStore } = useStore();
    const { loadUserCartProducts, userCartProductsRegistry, cartTotalPrice } = productStore;

    const [productItems, setProductItems] = useState<Product[]>();

    useEffect(() => {
        loadUserCartProducts().then(() => {
            setProductItems(Array.from(userCartProductsRegistry.values()));
        })
    }, [userCartProductsRegistry.size])

    if (productItems?.length === 0) return <Header textAlign='center'>You currently have no products in your cart</Header>

    return (
        <div style={{ width: '60%', marginLeft: '20%' }}>
            {productItems?.map((product: Product) => (
                <CartProductItem key={product.id} product={product} />
            ))}
            <Divider />
            <div>
                <Header as='h2' floated='right' >Total: $ {cartTotalPrice.toFixed(2)}</Header>
                <br/><br/><br/>
                <Button floated='right' size='large' disabled={cartTotalPrice === 0}>Buy</Button>
            </div>
        </div>
    )
})