import { observer } from "mobx-react-lite";
import { useState } from "react";
import { Accordion, Button, Header, Segment } from "semantic-ui-react";
import UserForm from "../user/UserForm";
import UserImagesForm from "../user/UserImagesForm";
import AccountSettingsForm from "./AccountSettingsForm";

export default observer(function SettingsContainer() {
    const [activeIndex, setActiveIndex] = useState(-1);

    const handleAccordionClick = (index: number) => {
        setActiveIndex(activeIndex === index ? -1 : index);
    };

    return (
        <Segment className='modal-background' padded='very'>
            <Header textAlign='center'>Settings</Header>
            <Accordion fluid>
                <Accordion.Title as={Button} fluid active={activeIndex === 0} onClick={() => handleAccordionClick(0)}>
                    User details
                </Accordion.Title>
                <Accordion.Content active={activeIndex === 0}>
                    <UserForm />
                </Accordion.Content>
                <br />
                <Accordion.Title as={Button} fluid active={activeIndex === 1} onClick={() => handleAccordionClick(1)}>
                    User images settings
                </Accordion.Title>
                <Accordion.Content active={activeIndex === 1}>
                    <UserImagesForm />
                </Accordion.Content>
                <br/>
                <Accordion.Title as={Button} fluid active={activeIndex === 2} onClick={() => handleAccordionClick(2)}>
                    Account settings
                </Accordion.Title>
                <Accordion.Content active={activeIndex === 2}>
                    <AccountSettingsForm />
                </Accordion.Content>
            </Accordion>
        </Segment>
    )
})