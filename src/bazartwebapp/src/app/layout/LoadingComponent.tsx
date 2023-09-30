import { Loader } from "semantic-ui-react";

interface Props {
    content?: string;
}

export default function LoadingComponent({ content = "Loading..." }: Props) {
    return (
        <Loader active inline='centered'>{content}</Loader>
    )
}