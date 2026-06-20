import "../styles/CapComponent.css";
import miner_cap from "../assets/items/miner_cap.webp";


function CapComponent() {
    return (
        <div className="cap-container">
            <img src={miner_cap} alt="MinerCap" className="miner-cap" />
        </div>
    );
}

export default CapComponent;