import { createAsyncThunk, createSlice } from "@reduxjs/toolkit";
import { RootState } from "../../app/store";
import { applyToTeams } from "./useAPI";

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
  initialState: {},
  reducers: {},
  extraReducers: (builder) => {
    builder
      .addCase(applyTeamsAsync.pending, (state) => ({
        ...state
      }))
      .addCase(applyTeamsAsync.rejected, (state) => ({
        ...state
      }))
      .addCase(applyTeamsAsync.fulfilled, (state) => ({
        ...state
      }))
  }
});

// Exports
export default useSlice.reducer;