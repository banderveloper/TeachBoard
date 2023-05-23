import {LocalizationProvider} from '@mui/x-date-pickers';
import React from 'react';
import {Navbar, Routing} from "../widgets";
import {AdapterDayjs} from "@mui/x-date-pickers/AdapterDayjs";

function App() {

    return (
        <div>
            <Navbar/>
            <Routing/>
        </div>
    );
}

export default App;
