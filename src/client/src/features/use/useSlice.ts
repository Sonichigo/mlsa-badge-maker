import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import { RootState } from "../../app/store";
import { applyToTeams } from "./useAPI";
import StatusState from "../../shared/StatusState";

// State
export interface UseState extends StatusState {

}

const initialState: UseState = {
  status: 'idle',
  statusMessage: ''
}

// Thunks
export const applyTeamsAsync = createAsyncThunk('use/applyToTeams',
  async (_, thunkAPI) => {
    const state = thunkAPI.getState() as RootState;
    const { finalAvatarBlobUrl } = state.editor;
    const { accessToken } = state.auth;

    if (!finalAvatarBlobUrl)
      throw new Error("No avatar was generated");

    const blob = await fetch(finalAvatarBlobUrl).then(r => r.blob());
    await applyToTeams(blob, { token: accessToken });
    return 'true';
  }
);

export const downloadImageAsync = createAsyncThunk('use/downloadImage',
  async (_, thunkAPI) => {
    const state = thunkAPI.getState() as RootState;
    const { finalAvatarBlobUrl } = state.editor;

    if (!finalAvatarBlobUrl)
      return;

    // This is JavaScript for you baby
    const a = document.getElementById('download-placebo') as HTMLLinkElement;
    a.click();
  });

// Reducer
export const useSlice = createSlice({
  name: 'use',
  initialState,
  reducers: {},
  extraReducers: (builder) => {
    builder
      .addCase(applyTeamsAsync.pending, (state) => ({
        ...state,
        status: 'busy',
        statusMessage: ''
      }))
      .addCase(applyTeamsAsync.rejected, (state) => ({
        ...state,
        status: 'failed',
        statusMessage: 'An unknown error occurred while applying your avatar to Microsoft Teams. Please try again and ' +
          'contact the site admins in case this issue continues to persist.'
      }))
      .addCase(applyTeamsAsync.fulfilled, (state) => ({
        ...state,
        status: 'success',
        statusMessage: 'Your new avatar was applied to Microsoft Teams. ' +
          'Please note that it may take up to 24-48 hours until it shows up for all users.'
      }))
}});

// Selectors
export const selectBusy = (state: RootState) => ({ status: state.use.status, statusMessage: state.use.statusMessage });

// Exports
export default useSlice.reducer;