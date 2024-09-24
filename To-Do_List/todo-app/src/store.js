import {createStore} from 'redux';
import rootReducer from './reducers/todoReducer';

const store = createStore(rootReducer);

export default store;