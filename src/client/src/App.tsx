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
    <div className="app">
      <div className="col-md-6 col-lg-4 mx-auto">
        <div className="my-3 text-center">
          <Text variant="xLarge">Student Ambassadors Badge Maker</Text>
        </div>

        <div className="d-flex flex-column">
          <div className="fill card">
            <Stack gap={4}>
              <Text variant={'xLarge'}>Edit your Avatar</Text>
              <Editor />
            </Stack>
          </div>

          <div className="fill card mt-3">
            <Stack gap={4}>
              <Text variant={'xLarge'}>Preview</Text>
              <Preview />
            </Stack>

            <div className="mt-2">
              <Use />
            </div>
          </div>
        </div>

        <footer className="py-3 text-center">
          <p>
            <span>Developed by <a href="https://linktr.ee/ginomessmer">Gino Messmer</a></span> <br/>
            <span>Proudly hosted on Azure Static Web Apps</span> <br/>
          </p>
          <a href="https://github.com/ginomessmer/mlsa-badge-maker" target="_blank" rel="noreferrer">Source Code</a> · <a href="https://github.com/ginomessmer/mlsa-badge-maker/discussions/new" target="_blank" rel="noreferrer">Provide Feedback</a>
        </footer>
      </div>
    </div>
  );
}

export default App;
