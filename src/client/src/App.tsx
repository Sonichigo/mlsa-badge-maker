import React, { useEffect } from 'react';
import './App.css';

import { authRequest } from './config/authConfig';
import { useMsal } from '@azure/msal-react';

import {Stack, Text} from '@fluentui/react';

import { useAppDispatch } from './app/hooks';
import { setAuthenticated, setUnauthenticated } from './features/auth/authSlice';

import Editor from './features/editor/Editor';
import Preview from './features/preview/Preview';
import Use from './features/use/Use';

function App() {

  const dispatch = useAppDispatch();

  const msal = useMsal();
  useEffect(() => {
    msal.instance.acquireTokenSilent({ ...authRequest, account: msal.accounts[0] }).then(res => {
      const action = res.account ? setAuthenticated({
        accessToken: res.accessToken,
        username: res.account.username,
      }) : setUnauthenticated();
      
      dispatch(action);
    });
  });

  return (
    <div className="container">
      <Stack horizontal horizontalAlign="center" verticalAlign="center" gap={8}>
        <div className="section">
          <Text variant={'xLarge'}>Edit your Avatar</Text>
          <Editor />
        </div>

        <div className="section">
          <Stack gap={4}>
            <Text variant={'xLarge'}>Preview</Text>
            <Preview />
            <Use />
          </Stack>
        </div>
      </Stack>
    </div>
  );
}

export default App;
