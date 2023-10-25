import { useState } from "react";
import { Button, Grid, Header, Icon, Menu, Message } from "semantic-ui-react";

export default function Footer() {
    const [activeIndex, setActiveIndex] = useState(-1);

    const handleClick = (index: number) => {
        setActiveIndex(activeIndex === index ? -1 : index);
    };

    return (
        <Grid columns={3} centered className='footer' >
            <Grid.Row stretched>
                <Grid.Column textAlign='center'>
                    <Header as='h3'>Help</Header>
                    <Grid>
                        <Grid.Column width={4}>
                            <Menu fluid tabular borderless>
                                <Menu.Item index={0} active={activeIndex === 0} onClick={() => { handleClick(0) }}>How to buy?</Menu.Item>
                            </Menu>
                            <Menu fluid tabular borderless>
                                <Menu.Item index={1} active={activeIndex === 1} onClick={() => { handleClick(1) }}>How to sell?</Menu.Item>
                            </Menu>
                        </Grid.Column>
                        <Grid.Column width={12}>
                            <Message hidden={activeIndex !== 0}>
                                <Message.Header>How to buy?</Message.Header>
                                <p>Example text about how to buy</p> {/*TODO fill text*/}
                            </Message>
                            <Message hidden={activeIndex !== 1}>
                                <Message.Header>How to sell?</Message.Header>
                                <p>Example text about how to sell</p> {/*TODO fill text*/}
                            </Message>
                        </Grid.Column>
                    </Grid>
                </Grid.Column>
                <Grid.Column textAlign='center'>
                    <Header as='h3'>Contact</Header>
                    <Grid columns={3} stackable textAlign='center' verticalAlign='middle'>
                        <Grid.Row>
                            <Grid.Column>
                                <Button color='grey' href='mailto:szkurlat.martin@gmail.com'>
                                    <Icon size='huge' fitted name='mail' />
                                </Button>
                            </Grid.Column>
                            <Grid.Column>
                                <Button href='https://linkedin.com/in/marcin-szkurlat' color='linkedin' >
                                    <Icon size='huge' fitted name='linkedin' />
                                </Button>
                            </Grid.Column>
                            <Grid.Column>
                                <Button href='https://github.com/MarcinSzkurlat' color='grey' >
                                    <Icon size='huge' fitted name='github' />
                                </Button>
                            </Grid.Column>
                        </Grid.Row>
                    </Grid>
                </Grid.Column>
                <Grid.Column textAlign='center'>
                    <Header as='h3'>Find us</Header>
                    <Grid columns={3} stackable textAlign='center' verticalAlign='middle'>
                        <Grid.Row>
                            <Grid.Column>
                                <Button color='facebook' href='https://facebook.com'>
                                    <Icon size='huge' fitted name='facebook square' />
                                </Button>
                            </Grid.Column>
                            <Grid.Column>
                                <Button color='instagram' href='https://instagram.com' >
                                    <Icon size='huge' fitted name='instagram' />
                                </Button>
                            </Grid.Column>
                        </Grid.Row>
                    </Grid>
                </Grid.Column>
            </Grid.Row>
        </Grid>
    )
}