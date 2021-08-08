import { createSlice, PayloadAction } from "@reduxjs/toolkit";

export interface EditorState {
  file: File | null;
  status: 'idle' | 'busy' | 'failed';
  statusMessage: string;
}

const initialState: EditorState = {
  file: null,
  status: 'idle',
  statusMessage: '',
};

export const editorSlice = createSlice({
  name: 'editor',
  initialState,
  reducers: {
    setFile: (state, action: PayloadAction<File | null>) => ({
      ...state,
      file: action.payload
    })
  }
});

export const { setFile } = editorSlice.actions;

export default editorSlice.reducer;