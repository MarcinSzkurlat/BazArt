import { observer } from "mobx-react-lite";
import { useState } from "react";
import { Button, Divider, Grid, Header, Icon, Segment } from "semantic-ui-react";
import LoginForm from "./LoginForm";
import RegistrationForm from "./RegistrationForm";

export default observer(function AccountContainer() {
    const [isLoginForm, setIsLoginForm] = useState(true);
    const [isArtist, setIsArtist] = useState(false);

    const handleArtistTypeButton = () => {
        setIsArtist(true);
    }

    const handleClientTypeButton = () => {
        setIsArtist(false);
    }

    const handleLoginButton = () => {
        setIsLoginForm(true);
    }

    const handleRegistrationButton = () => {
        setIsLoginForm(false);
    }

    return (
        <Segment.Group>
            <Segment className='modal-background' padded='very'>
                <Grid columns={2}>
                    <Divider vertical>Or</Divider>
                    <Grid.Row verticalAlign='middle'>
                        <Grid.Column textAlign='center'>
                            <Icon name='user' size='massive'/>
                            <Header>
                                <i>Do you already have an account with us?</i>
                            </Header>
                            <Button content='Login' onClick={handleLoginButton} active={isLoginForm} />
                        </Grid.Column>
                        <Grid.Column textAlign='center'>
                            <Icon name='users' size='massive'/>
                            <Header>
                                <i>You're an artist? Do you create art? Or maybe you went for a little shopping?</i>
                            </Header>
                            <Button content='Register' onClick={handleRegistrationButton} active={!isLoginForm} />
                        </Grid.Column>
                    </Grid.Row>
                </Grid>
            </Segment>
            {isLoginForm
                ? <Segment className='modal-background' padded='very'>
                    <LoginForm />
                </Segment>
                : <Segment className='modal-background' padded='very'>
                    <Grid centered columns={2}>
                        <Grid.Column textAlign='center'>
                            <Button onClick={handleClientTypeButton} size='medium' active={!isArtist}>
                                <Header content='Client' />
                                <p>If you want to pop in for some shopping</p>
                            </Button>
                        </Grid.Column>
                        <Grid.Column textAlign='center'>
                            <Button onClick={handleArtistTypeButton} size='medium' active={isArtist}>
                                <Header content='Artist' />
                                <p>If you want to exhibit and sell your artworks</p>
                            </Button>
                        </Grid.Column>
                    </Grid>
                    <RegistrationForm isArtist={isArtist} />
                </Segment>}
        </Segment.Group>
    )
})