import "../styles/PistolWithSilencer.css";
import pistol_with_silencer from "../assets/items/PistolWithSilencer.webp";


function PistolWithSilencer() {
    return (
        <div className="pistol-with-silencer-container">
            <img src={pistol_with_silencer} alt="PistolWithSilencer" className="pistol-with-silencer" />
        </div>
    );
}

export default PistolWithSilencer;