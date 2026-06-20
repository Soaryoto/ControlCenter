import { useState, useEffect  } from "react";
import "./App.css";
import Avatar from "./components/Avatar";
import CupOfCoffee from "./components/CupOfCoffee";
import SteamFromCup from "./components/SteamFromCup";
import EyeComponent from "./components/EyeComponent";
import CapComponent from "./components/CapComponent";


function App() {

    const [isTalking, setIsTalking] = useState(false);
    
    useEffect(() => {
      const socket = new WebSocket("ws://localhost:8765");
      socket.onmessage = (event) => {
        console.log("event.data: ", event.data);
        const data = JSON.parse(event.data);
        if (data.Payload.isTalking != undefined) {
          setIsTalking(data.Payload.isTalking);
        }
      };

      return () => {
        socket.close();
      };
    }, []);
    
    return (
      <div className="app-container">
        <Avatar isTalking={isTalking} />
        <EyeComponent />
        <CupOfCoffee />
        <SteamFromCup />
        <CapComponent />
      </div>
    );
}

export default App;