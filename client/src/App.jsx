// Copyright (C) 2026 Soaryoto
//
// This file is part of StreamingController.
//
// Licensed under the GNU General Public License v3.0 or later.
// See LICENSE for the full license text.
//
// SPDX-License-Identifier: GPL-3.0-or-later

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
import PistolWithSilencer from "./components/PistolWithSilencer";
import Lamp from "./components/Lamp";
import Shovel from "./components/Shovel";


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
        <Lamp />
        <Shovel /> 
        {/* <CupOfCoffee /> */}
        {/* <SteamFromCup /> */}
      </div>
    );
}

export default App;