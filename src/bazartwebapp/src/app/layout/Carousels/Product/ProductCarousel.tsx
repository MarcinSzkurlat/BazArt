import { Product } from "../../../models/Product/product";
import ReactSimplyCarousel from 'react-simply-carousel';
import { useEffect, useState } from "react";
import ProductItem from "../../../../features/product/ProductItem";
import { Card, Header, Icon } from "semantic-ui-react";
import { useStore } from "../../../stores/store";
import LoadingComponent from "../../LoadingComponent";
import { observer } from "mobx-react-lite";
import { PageTypes } from "../pageTypes";

interface Props {
    page: PageTypes;
    categoryName?: string;
    userId?: string;
}

export default observer(function ProductCarousel({ page, categoryName, userId }: Props) {
    const { productStore, accountStore } = useStore();
    const { loadingInitial, latestProductRegistry, loadLatestProducts, loadProducts, productsRegistry, loadUserProducts } = productStore;

    const [activeSliderIndex, setActiveSliderIndex] = useState(0);
    const [productItems, setProductItems] = useState<Product[]>();

    useEffect(() => {
        switch (page) {
            case PageTypes.Home:
                loadLatestProducts().then(() => {
                    setProductItems(Array.from(latestProductRegistry.values()));
                })
                break;
            case PageTypes.Category:
                loadProducts(categoryName!).then(() => {
                    setProductItems(Array.from(productsRegistry.values()));
                })
                break;
            case PageTypes.User:
                if (accountStore.isLoggedIn) {
                    loadUserProducts(userId!).then(() => {
                        setProductItems(Array.from(productsRegistry.values()));
                    })
                }
                break;
        }
    }, [])

    if (loadingInitial) return <LoadingComponent />

    if (productItems?.length === 0) return <Header textAlign='center'>There are currently no available products</Header>

    return (
        <>
            {productItems?.length! > 4
                ? <ReactSimplyCarousel
                    activeSlideIndex={activeSliderIndex}
                    onRequestChange={setActiveSliderIndex}
                    itemsToShow={4}
                    itemsToScroll={1}
                    forwardBtnProps={{
                        style: {
                            alignSelf: 'center',
                            background: 'transparent',
                            border: 'none',
                            cursor: 'pointer',
                            fontSize: '40px'
                        },
                        children: <span><Icon name="chevron right" /></span>,
                    }}
                    backwardBtnProps={{
                        style: {
                            alignSelf: 'center',
                            background: 'transparent',
                            border: 'none',
                            cursor: 'pointer',
                            fontSize: '40px'
                        },
                        children: <span><Icon name="chevron left" /></span>,
                    }}
                    responsiveProps={[
                        {
                            itemsToShow: 4,
                            itemsToScroll: 1,
                            autoplay: true,
                            autoplayDirection: 'forward',
                            autoplayDelay: 2000
                        }
                    ]}
                    speed={1200}
                    easing='linear'
                >
                    {productItems?.map((product: Product) => (
                        <ProductItem key={product.id} product={product} />
                    ))}
                </ReactSimplyCarousel>
                : <Card.Group centered>
                    {productItems?.map((product: Product) => (
                        <ProductItem key={product.id} product={product} />
                    ))}</Card.Group>}
        </>
    )
})