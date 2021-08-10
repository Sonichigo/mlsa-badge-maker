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
      <div className="container">
        <div className="my-3">
          <Text variant="xLarge">Student Ambassadors Badge Maker</Text>
        </div>
        <div className="row g-3">
          <div className="col-12 col-lg-6 card">
            <Stack gap={4}>
              <Text variant={'xLarge'}>Edit your Avatar</Text>
              <Editor />
            </Stack>
          </div>
          <div className="col-12 col-lg-6 card">
            <div className="d-flex flex-column h-100">
              <Stack gap={4}>
                <Text variant={'xLarge'}>Preview</Text>
                <Preview />
              </Stack>

              <div className="mt-auto">
                <Use />
              </div>
            </div>
          </div>
        </div>

        <footer className="my-3">
          <div className="row">
            <div className="col text-sm">
              <span>Developed by <a href="https://linktr.ee/ginomessmer">Gino Messmer</a></span> <br/>
              <span>Built with Blazor and Azure Functions. Proudly hosted on Azure Static Web Apps.</span>
            </div>
            <div className="col-auto">
              <a href="https://github.com/ginomessmer/mlsa-badge-maker" target="_blank">Source Code</a> · <a href="https://github.com/ginomessmer/mlsa-badge-maker/discussions/new" target="_blank">Provide Feedback</a>
            </div>
          </div>
        </footer>

      </div>
    </div>
  );
}

export default App;
