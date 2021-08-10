import { configureStore, ThunkAction, Action } from '@reduxjs/toolkit';

import authReducer from '../features/auth/authSlice';
import counterReducer from '../features/counter/counterSlice';
import editorReducer from '../features/editor/editorSlice';
import useReducer from '../features/use/useSlice';


export const store = configureStore({
  reducer: {
    auth: authReducer,
    counter: counterReducer,
    editor: editorReducer,
    use: useReducer
  },
});

export type AppDispatch = typeof store.dispatch;
export type RootState = ReturnType<typeof store.getState>;
export type AppThunk<ReturnType = void> = ThunkAction<
  ReturnType,
  RootState,
  unknown,
  Action<string>
>;
