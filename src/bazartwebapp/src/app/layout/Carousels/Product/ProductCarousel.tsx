import { Product } from "../../../models/Product/product";
import ReactSimplyCarousel from 'react-simply-carousel';
import { useEffect, useState } from "react";
import ProductItem from "../../../../features/product/ProductItem";
import { Card, Header, Icon } from "semantic-ui-react";
import { useStore } from "../../../stores/store";
import LoadingComponent from "../../LoadingComponent";
import { observer } from "mobx-react-lite";

interface Props {
    page: string;
    categoryName?: string;
}

export default observer(function ProductCarousel({ page, categoryName }: Props) {
    const { productStore } = useStore();
    const { loadingInitial, latestProductRegistry, loadLatestProducts, loadProducts, productsRegistry } = productStore;

    const [activeSliderIndex, setActiveSliderIndex] = useState(0);
    const [productItems, setProductItems] = useState<Product[]>();

    useEffect(() => {
        switch (page) {
            case 'home':
                loadLatestProducts().then(() => {
                    setProductItems(Array.from(latestProductRegistry.values()));
                })
                break;
            case 'category':
                loadProducts(categoryName!).then(() => {
                    setProductItems(Array.from(productsRegistry.values()));
                })
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