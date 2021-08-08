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
    let res = await applyToTeams(blob, { token: accessToken });
    return 'true';
  }
);

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