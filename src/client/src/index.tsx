import React from 'react';
import ReactDOM from 'react-dom';
import './index.css';
import App from './App';
import { store } from './app/store';
import { Provider } from 'react-redux';
import * as serviceWorker from './serviceWorker';

import "cropperjs/dist/cropper.css";

// MSAL
import { InteractionType, PublicClientApplication } from '@azure/msal-browser';
import { MsalAuthenticationTemplate, MsalProvider } from "@azure/msal-react";
import { authRequest, msalConfig } from './config/authConfig';
import { ThemeProvider } from '@fluentui/react';

const msalInstance = new PublicClientApplication(msalConfig);

const themes = {
  dark: {
    themePrimary: '#058bf2',
    themeLighterAlt: '#00060a',
    themeLighter: '#011627',
    themeLight: '#012a49',
    themeTertiary: '#035491',
    themeSecondary: '#047bd5',
    themeDarkAlt: '#1d96f4',
    themeDark: '#3fa6f5',
    themeDarker: '#70bdf8',
    neutralLighterAlt: '#1a1a1a',
    neutralLighter: '#232323',
    neutralLight: '#323232',
    neutralQuaternaryAlt: '#3b3b3b',
    neutralQuaternary: '#434343',
    neutralTertiaryAlt: '#636363',
    neutralTertiary: '#c8c8c8',
    neutralSecondary: '#d0d0d0',
    neutralPrimaryAlt: '#dadada',
    neutralPrimary: '#ffffff',
    neutralDark: '#f4f4f4',
    black: '#f8f8f8',
    white: '#0f0f0f',
  }
};

ReactDOM.render(
  <React.StrictMode>
    <Provider store={store}>
      <MsalProvider instance={msalInstance}>
        <MsalAuthenticationTemplate
          interactionType={InteractionType.Redirect}
          authenticationRequest={authRequest}>
          <ThemeProvider theme={{
            // palette: themes.dark
          }}>
            <App />
          </ThemeProvider>
        </MsalAuthenticationTemplate>
      </MsalProvider>
    </Provider>
  </React.StrictMode>,
  document.getElementById('root')
);

// If you want your app to work offline and load faster, you can change
// unregister() to register() below. Note this comes with some pitfalls.
// Learn more about service workers: https://bit.ly/CRA-PWA
serviceWorker.unregister();
