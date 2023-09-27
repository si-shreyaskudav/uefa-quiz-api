import React from 'react';
import ReactDOM from 'react-dom';
import App from './App';
import core from './common/core';
import app from './common/app';
import config from './common/config';
// import $ from 'jquery';
import 'promise-polyfill/src/polyfill';
// import * as serviceWorker from './serviceWorker'; 

core.addClassToElement(document, 'body', `si-${config.CURRENT_LANGUAGE.toLocaleLowerCase()}`, false);

let team = core.getCookie(config.enumBusters.team);

if (!team || isNaN(team)) {
  core.CompCacheBusterCookie(config.enumBusters.team);
}

ReactDOM.render(<App />, document.getElementById('container'));

config.IS_MOBILE = core.isMobile();
config.IS_TABLET = core.isTablet();

// // If you want your app to work offline and load faster, you can change
// // unregister() to register() below. Note this comes with some pitfalls.
// // Learn more about service workers: https://bit.ly/CRA-PWA
// serviceWorker.unregister();
