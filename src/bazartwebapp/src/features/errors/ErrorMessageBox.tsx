import { Message } from "semantic-ui-react";

interface Props {
    errors: Map<string, string[]>;
}

export default function ErrorMessageBox({ errors }: Props) {
    return (
        <Message error visible>
            {Array.from(errors).map(([key, value]) => (
                <div key={key}>
                    <Message.Header>{key}</Message.Header>
                    <Message.List>
                        {value.map((error, i) => (
                            <Message.Item key={i}>{error}</Message.Item>
                        ))}
                    </Message.List>
                </div>
            ))}
        </Message>
    )
}