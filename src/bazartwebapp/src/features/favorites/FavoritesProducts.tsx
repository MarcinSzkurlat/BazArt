import { observer } from "mobx-react-lite";
import { useEffect, useState } from "react";
import { Grid, Header, Icon, Pagination, PaginationProps } from "semantic-ui-react";
import LoadingComponent from "../../app/layout/LoadingComponent";
import { Product } from "../../app/models/Product/product";
import { useStore } from "../../app/stores/store";
import FavoriteProductItem from "./FavoriteProductItem";

export default observer(function FavoritesProducts() {
    const { productStore } = useStore();
    const { loadUserFavoriteProducts, productsRegistry, pageNumber, totalPages, loadingInitial } = productStore;

    const [productItems, setProductItems] = useState<Product[]>();
    const [activePage, setActivePage] = useState(pageNumber);

    const onChanged = (event: React.MouseEvent<HTMLAnchorElement, MouseEvent>, pageInfo: PaginationProps) => {
        setActivePage(pageInfo.activePage as number);
    }

    useEffect(() => {
        loadUserFavoriteProducts(activePage).then(() => {
            setProductItems(Array.from(productsRegistry.values()));
        })
    }, [activePage, productsRegistry.size])

    if (loadingInitial) return <LoadingComponent />

    if (productItems?.length === 0) return <Header textAlign='center'>You currently have no favorite products</Header>

    return (
        <>
            <Grid columns={2} style={{ width: '80%', marginLeft: '10%' }}>
                {productItems?.map((product: Product) => (
                    <Grid.Column key={product.id}>
                        <FavoriteProductItem product={product} />
                    </Grid.Column>
                ))}
            </Grid>
            {totalPages > 1
                ? <div style={{ marginTop: '30px', textAlign: 'center' }}>
                    <Pagination
                        activePage={pageNumber}
                        ellipsisItem={{ content: <Icon name="ellipsis horizontal" />, icon: true }}
                        firstItem={{ content: <Icon name="angle double left" />, icon: true }}
                        lastItem={{ content: <Icon name="angle double right" />, icon: true }}
                        prevItem={{ content: <Icon name="angle left" />, icon: true }}
                        nextItem={{ content: <Icon name="angle right" />, icon: true }}
                        totalPages={totalPages}
                        onPageChange={onChanged}
                        siblingRange={1}
                    />
                </div>
                : <></>}
        </>
    )
})