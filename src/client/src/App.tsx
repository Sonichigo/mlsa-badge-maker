import React from 'react';
import logo from './logo.svg';
import { Counter } from './features/counter/Counter';
import Editor from './features/editor/Editor';

import './App.css';
import Preview from './features/preview/Preview';

function App() {
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
