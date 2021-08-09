import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import { RootState } from "../../app/store";

export interface AuthState {
  isAuthenticated: boolean;
  username: string;
  accessToken: string;
}

const initialState: AuthState = {
  isAuthenticated: false,
  username: '',
  accessToken: ''
};

// Reducer
export const authSlice = createSlice({
  name: 'auth',
  initialState,
  reducers: {
    setAuthenticated: (state, action: PayloadAction<{ username: string; accessToken: string }>) => ({
      ...state,
      isAuthenticated: true,
      username: action.payload.username,
      accessToken: action.payload.accessToken
    }),
    setUnauthenticated: (state) => ({
      ...state,
      isAuthenticated: false,
      username: '',
    }),
  }
});

// Actions
export const { setAuthenticated, setUnauthenticated } = authSlice.actions;

// Selectors
export const isAuthenticated = (state: RootState) => state.auth.isAuthenticated;
export const selectUsername = (state: RootState) => state.auth.username;

// Exports
export default authSlice.reducer;
  