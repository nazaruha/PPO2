import "bootstrap/dist/css/bootstrap.min.css";
import "bootstrap/dist/js/bootstrap.min.js";
import "font-awesome/css/font-awesome.min.css";
import React from 'react';
import ReactDOM from 'react-dom/client';
import './index.css';
import App from './App';
import reportWebVitals from './reportWebVitals';
import { HashRouter } from 'react-router-dom';
import { Provider } from "react-redux";
import { store } from "./components/containers/store";
import { IProjectItem, StoreProjectActionType } from "./components/pages/Project/types";

if (localStorage.chosenProjectId) {
  console.log("localStorage project chosen is not empty")
  const project: IProjectItem = {
    id: localStorage.chosenProjectId,
    name: localStorage.chosenProjectName
  };
  store.dispatch({ type: StoreProjectActionType.STORE_CREATE_PROJECT, payload: project })
}

const root = ReactDOM.createRoot(
  document.getElementById('root') as HTMLElement
);

root.render(
  <Provider store={store}>
    <HashRouter>
      <App />
    </HashRouter>
  </Provider>
);

// If you want to start measuring performance in your app, pass a function
// to log results (for example: reportWebVitals(console.log))
// or send to an analytics endpoint. Learn more: https://bit.ly/CRA-vitals
reportWebVitals();
