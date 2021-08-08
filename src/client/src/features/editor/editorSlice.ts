import { createSlice } from "@reduxjs/toolkit";

export interface EditorState {
  file?: File;
  status: 'idle' | 'busy' | 'failed';
  statusMessage: string;
}

const initialState: EditorState = {
  file: undefined,
  status: 'idle',
  statusMessage: '',
};

export const editorSlice = createSlice({
  name: "editor",
  initialState,
  reducers: {
    setFile: (state, { payload: { file } }) => ({
      ...state,
      file
    })
  }
});

export const { setFile } = editorSlice.actions;

export default editorSlice.reducer;