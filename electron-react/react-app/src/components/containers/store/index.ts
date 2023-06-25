import { configureStore } from "@reduxjs/toolkit";
import { applyMiddleware, combineReducers, createStore } from "redux";
import { composeWithDevTools } from "redux-devtools-extension";
import thunk from "redux-thunk";
import { ProjectReducer } from "../../pages/Project/projectReducer";

export const rootReducer = combineReducers({ // імпортуєм наші редюсери
    project: ProjectReducer // підключити наш редюсер, який міститься під полем project
});

export const store = configureStore({ // це новий стор, в якому буде зберігатись все, що в редаксі
    reducer: rootReducer, // набір наших редюсерів, які будуть в редаксі
    devTools: true, // щоб працював дев тулс, щоб в браузері дивитись через F12
    middleware: [thunk] // щоб міг редакс працювати асинхроно
});
