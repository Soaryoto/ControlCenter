import { useState, useEffect  } from "react";
import "./App.css";

// Список сервисов
import webSocketService from "./services/WebSocketService";
import messageDispatcher from "./services/MessageDispatcher";

// Визуальные компоненты
import Avatar from "./components/Avatar";
import CupOfCoffee from "./components/CupOfCoffee";
import SteamFromCup from "./components/SteamFromCup";
import EyeComponent from "./components/EyeComponent";
import CapComponent from "./components/CapComponent";
import PistolWithSilencer from "./components/PistolWithSilencer";


function App() {

    const [isTalking, setIsTalking] = useState(false);
    
    useEffect(() => {
      messageDispatcher.register("Avatar", "isTalking", (payload) => {
          setIsTalking(payload.isTalking);
      });

      webSocketService.connect((json) => {
        messageDispatcher.dispatch(json);
      });

      return () => {
        webSocketService.disconnect();
      };
    }, []);
    
    return (
      <div className="app-container">
        <Avatar isTalking={isTalking} />
        <EyeComponent />
        {/* <PistolWithSilencer /> */}
        <CupOfCoffee />
        <SteamFromCup />
      </div>
    );
}

export default App;