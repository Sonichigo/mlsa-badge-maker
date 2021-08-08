import { createSlice } from "@reduxjs/toolkit";

export interface EditorState {
  file?: File;
}

const initialState: EditorState = {
  file: undefined
};

export const editorSlice = createSlice({
  name: "editor",
  initialState,
  reducers: {
    setFile: (state, { file }) => ({
      ...state,
      file
    })
  }
});

export const { setFile } = editorSlice.actions;
