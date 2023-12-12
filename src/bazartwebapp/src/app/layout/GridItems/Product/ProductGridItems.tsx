import { observer } from "mobx-react-lite";
import { useEffect, useState } from "react";
import { Card, Icon, Pagination, PaginationProps } from "semantic-ui-react";
import ProductItem from "../../../../features/product/ProductItem";
import { Product } from "../../../models/Product/product";
import { useStore } from "../../../stores/store";
import { PageTypes } from "../../Carousels/pageTypes";

interface Props {
    page: PageTypes;
    categoryName?: string;
    userId?: string;
}

export default observer(function ProductGridItems({ page, categoryName, userId }: Props) {
    const { productStore, accountStore } = useStore();
    const { loadProducts, productsRegistry, pageNumber, totalPages, loadUserProducts } = productStore;

    const [productItems, setProductItems] = useState<Product[]>();
    const [activePage, setActivePage] = useState(pageNumber + 1);

    const onChanged = (_event: React.MouseEvent<HTMLAnchorElement, MouseEvent>, pageInfo: PaginationProps) => {
        setActivePage(pageInfo.activePage as number);
    }

    useEffect(() => {
        switch (page) {
            case PageTypes.Category:
                loadProducts(categoryName!, activePage).then(() => {
                    setProductItems(Array.from(productsRegistry.values()));
                })
                break;
            case PageTypes.User:
                if (accountStore.isLoggedIn) {
                    loadUserProducts(userId!, activePage).then(() => {
                        setProductItems(Array.from(productsRegistry.values()));
                    })
                }
                break;
        }
    }, [activePage])

    return (
        <div style={{ width: '80%', marginLeft: '10%' }}>
            <Card.Group centered itemsPerRow={4} stackable>
                {productItems?.map((product: Product) => (
                    <ProductItem key={product.id} product={product} />
                ))}
            </Card.Group>
            <div style={{ marginTop: '30px', textAlign: 'center' }}>
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
        </div>
    )
})