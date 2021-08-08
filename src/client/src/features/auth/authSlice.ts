import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import { RootState } from "../../app/store";

export interface AuthState {
  isAuthenticated: boolean;
  username: string;
}

const initialState: AuthState = {
  isAuthenticated: false,
  username: '',
};

// Reducer
export const authSlice = createSlice({
  name: 'auth',
  initialState,
  reducers: {
    setAuthenticated: (state, action: PayloadAction<string>) => ({
      ...state,
      isAuthenticated: true,
      username: action.payload,
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

// Exports
export default authSlice.reducer;
  