import { observer } from "mobx-react-lite";
import { useEffect, useState } from "react";
import { Grid, Header, Icon, Pagination, PaginationProps } from "semantic-ui-react";
import LoadingComponent from "../../app/layout/LoadingComponent";
import { UserDetails } from "../../app/models/User/userDetails";
import { useStore } from "../../app/stores/store";
import FavoriteUserItem from "./FavoriteUserItem";

export default observer(function FavoritesUsers() {
    const { userStore } = useStore();
    const { loadUserFavoriteUsers, usersRegistry, pageNumber, totalPages, loadingInitial } = userStore;

    const [userItems, setUserItems] = useState<UserDetails[]>();
    const [activePage, setActivePage] = useState(pageNumber);

    const onChanged = (event: React.MouseEvent<HTMLAnchorElement, MouseEvent>, pageInfo: PaginationProps) => {
        setActivePage(pageInfo.activePage as number);
    }

    useEffect(() => {
        loadUserFavoriteUsers(activePage).then(() => {
            setUserItems(Array.from(usersRegistry.values()));
        })
    }, [activePage, usersRegistry.size])

    if (loadingInitial) return <LoadingComponent />

    if (userItems?.length === 0) return <Header textAlign='center'>You currently have no favorite users</Header>

    return (
        <>
            <Grid columns={2} style={{ width: '80%', marginLeft: '10%' }}>
                {userItems?.map((user: UserDetails) => (
                    <Grid.Column key={user.id}>
                        <FavoriteUserItem user={user} />
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