import { useState, useEffect, useRef } from "react";
import "./App.css";

function App() {
  const [isTalking, setIsTalking] = useState(false);

  return (
    <div className="overlay">
      <p>Статус: {isTalking ? "Говорю" : "Молчу"}</p>
    </div>
  );
}

export default App;