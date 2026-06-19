import eye_right from "../assets/avatar/eye_right.webp";
import eye_left from "../assets/avatar/eye_left.webp";
import "../styles/EyeComponent.css";

function EyeComponent() {
    return (
        <div className="eye-component-container">
            <div className="eye eye-right-container">
                <img src={eye_right} alt="Right Eye" />
            </div>
            <div className="eye eye-left-container">
                <img src={eye_left} alt="Left Eye" />
            </div>
        </div>
    );
}

export default EyeComponent;