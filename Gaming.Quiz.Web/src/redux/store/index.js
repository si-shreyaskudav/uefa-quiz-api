import { createStore, applyMiddleware } from 'redux';
import promiseMiddleware from 'redux-promise-middleware'
import logger from 'redux-logger'
import rootReducer from './../reducers';

function configureStore(initialState) {
    return createStore(
        rootReducer,
        initialState,
        applyMiddleware(promiseMiddleware, logger, promiseMiddleware), 
    );
}

const store = configureStore();
export default store;