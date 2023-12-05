import { Link } from "react-router-dom";
import { Button, Header, Icon, Segment } from "semantic-ui-react";

export default function NotFound() {
    return (
        <Segment placeholder basic>
            <Header icon>
                <Icon name='search' />
                Oops - we've looked everywhere but could not find what you are looking for!
            </Header>
            <Segment.Inline>
                <Button as={Link} to='/'>
                    Return to home page
                </Button>
            </Segment.Inline>
        </Segment>
    )
}