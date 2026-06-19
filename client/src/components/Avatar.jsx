import avatar_idle from "../assets/avatar/idle.webp";
import avatar_talking from "../assets/avatar/talking.webp";

function Avatar({ isTalking }) {

    const image = isTalking ? avatar_talking : avatar_idle;

    return (
        <div className="avatar-container">
            <img
                src={image}
                alt="Avatar"
                className="avatar"
            />
        </div>
    );
}

export default Avatar;