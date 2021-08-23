import './index.css';
import React from 'react';
import ReactDOM from 'react-dom';
import reportWebVitals from './reportWebVitals';
import RelayEnvironment from 'RelayEnvironment';
import RouterRenderer from 'infraestructure/routing/RouteRenderer';

import { RelayEnvironmentProvider } from 'react-relay/hooks';
import routes from 'routes';
import Router from 'infraestructure/routing/Router';
import RouterContext from 'infraestructure/routing/RouterContext';
// Uses the custom router setup to define a router instance that we can pass through context
const router = new Router(routes);

ReactDOM.render(
  <React.StrictMode>
    <RelayEnvironmentProvider environment={RelayEnvironment}>
      <RouterContext.Provider value={router}>
        {/* Render the active route */}
        <RouterRenderer />
      </RouterContext.Provider>
    </RelayEnvironmentProvider>
    ,
  </React.StrictMode>,
  document.getElementById('root'),
);

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
