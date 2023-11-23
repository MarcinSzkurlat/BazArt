import { observer } from "mobx-react-lite";
import { Link } from "react-router-dom";
import { Button, Grid, Header, Icon, Popup, Image } from "semantic-ui-react";
import { UserDetails } from "../../app/models/User/userDetails";
import { useStore } from "../../app/stores/store";

interface Props {
    user: UserDetails;
}

export default observer(function FavoriteUserItem({ user }: Props) {
    const { userStore } = useStore();

    const handleRemoveFavoriteButton = () => {
        userStore.deleteFavoriteUser(user.id);
    }

    return (
        <Grid columns={3}>
            <Grid.Column width={5} textAlign='center'>
                <Image circular size='small' as={Link} to={`/user/${user.id}`} src="/assets/user-image-placeholder.png" alt={user.id} />
            </Grid.Column>
            <Grid.Column width={9}>
                <Header as={Link} to={`/user/${user.id}`}>{user.stageName ?? user.email.split('@')[0]}</Header>
                {user.description
                    ? <p><i>"{user.description.length > 200
                        ? user.description.substring(0, 200) + "..."
                        : user.description}"</i></p>
                    : <></>}
            </Grid.Column>
            <Grid.Column width={2} verticalAlign='middle' textAlign='center'>
                <Popup pinned position='top center' trigger={
                    <Button size='large' icon color='red' circular onClick={handleRemoveFavoriteButton}>
                        <Icon name='x' />
                    </Button>}>
                    Remove user from favorite
                </Popup>
            </Grid.Column>
        </Grid>
    )
})