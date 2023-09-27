import React, { Component } from 'react';
import { Provider } from 'react-redux';
import store from './redux/store';
import Wrapper from './components/wrapper';
import config from './common/config';
import app from './common/app';
import core from './common/core';
class App extends Component {
  render() {
    return (
      <div className="App">
        <Provider store={store}>
          <Wrapper />
        </Provider>
      </div>
    );
  }
}

export default App;
