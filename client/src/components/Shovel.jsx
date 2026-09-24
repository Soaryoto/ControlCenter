import "../styles/Shovel.css";
import shovel from "../assets/items/Shovel.webp";


function Shovel() {
    return (
        <div className="shovel-container">
            <img src={shovel} alt="Shovel" className="shovel" />
        </div>
    );
}

export default Shovel;