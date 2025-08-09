import { createContext, useState } from "react";


const AuthContext = createContext();

const AuthProvider = ({ children }) => {
    const [user,setUser] = useState(null);

    
}

export default AuthContext;