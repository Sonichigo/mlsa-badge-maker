import { Configuration } from "@azure/msal-browser";

export const msalConfig: Configuration = {
  auth: {
    clientId: '97ed429e-c1be-43e8-934c-76f4f96c7189',
    authority: 'https://login.microsoftonline.com/84c31ca0-ac3b-4eae-ad11-519d80233e6f',
    redirectUri: process.env.PUBLIC_URL,
  },
  cache: {
    cacheLocation: 'localStorage',
    secureCookies: false
  },
};

export const authRequest = {
  scopes: ['User.Read', 'User.ReadWrite']
};
