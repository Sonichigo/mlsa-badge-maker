import { useMsal } from '@azure/msal-react';
import React, { useEffect } from 'react';
import './App.css';
import { useAppDispatch } from './app/hooks';
import { setAuthenticated, setUnauthenticated } from './features/auth/authSlice';

import { Counter } from './features/counter/Counter';
import Editor from './features/editor/Editor';
import Preview from './features/preview/Preview';

function App() {

  const dispatch = useAppDispatch();

  const msal = useMsal();
  useEffect(() => {
    const account = msal.accounts[0];
    const action = account ? setAuthenticated(account.username) : setUnauthenticated();
    dispatch(action);
  }, []);

  return (
    <div className="container">
      <div className="section">
        <Editor />
      </div>

      <div className="section">
        <Counter />
      </div>

      <div className="section">
        <Preview />
      </div>
    </div>
  );
}

export default App;
