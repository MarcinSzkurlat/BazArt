import { observer } from "mobx-react-lite";
import HomePage from "../../features/home/HomePage";
import Footer from "./Footer";

function App() {
    return (
        <div className="App">
            <HomePage />
            <Footer />
        </div>
    );
}

export default observer(App);
