import "../styles/Lamp.css";
import lamp from "../assets/items/Lamp.webp";


function Lamp() {
    return (
        <div className="lamp-container">
            <img src={lamp} alt="Lamp" className="lamp" />
        </div>
    );
}

export default Lamp;