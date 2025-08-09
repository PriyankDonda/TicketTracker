import { useState } from "react";
import reactLogo from "./assets/react.svg";
import viteLogo from "/vite.svg";
import "./App.css";

function App() {
  const [count, setCount] = useState(0);

  return (
    <>
      <p className="w-full p-100 flex bg-lime-300 justify-center text-5xl">
        Hello priyank
      </p>
    </>
  );
}

export default App;
