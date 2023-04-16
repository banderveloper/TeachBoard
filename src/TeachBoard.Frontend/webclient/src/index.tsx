import React from 'react';
import ReactDOM from 'react-dom/client';
import App from './app/App';
import {BrowserRouter} from "react-router-dom";
import {CssBaseline} from "@mui/material";
import {LocalizationProvider} from "@mui/x-date-pickers";
import {AdapterDateFns} from "@mui/x-date-pickers/AdapterDateFns";
import de from 'date-fns/locale/de';

const root = ReactDOM.createRoot(
    document.getElementById('root') as HTMLElement
);
root.render(
    <BrowserRouter>
        <CssBaseline>
            <LocalizationProvider dateAdapter={AdapterDateFns} adapterLocale={de}>
                <App/>
            </LocalizationProvider>
        </CssBaseline>
    </BrowserRouter>
);