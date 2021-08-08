import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import { RootState } from "../../app/store";

// State
export interface EditorState {
  fileBlobUrl?: string;
  croppedFileBlobUrl?: string;
  status: 'idle' | 'busy' | 'failed';
  statusMessage: string;
}

const initialState: EditorState = {
  status: 'idle',
  statusMessage: '',
};

// Reducer
export const editorSlice = createSlice({
  name: 'editor',
  initialState,
  reducers: {
    setFileBlobUrl: (state, action: PayloadAction<string>) => ({
      ...state,
      fileBlobUrl: action.payload,
    }),
    setCroppedFileBlobUrl: (state, action: PayloadAction<string>) => ({
      ...state,
      croppedFileBlobUrl: action.payload,
    }),
  }
});

// Actions
export const { setFileBlobUrl, setCroppedFileBlobUrl } = editorSlice.actions;

// Selectors
export const selectFileBlobUrl = (state: RootState) => state.editor.fileBlobUrl;
export const selectCroppedFileBlobUrl = (state: RootState) => state.editor.croppedFileBlobUrl;

// Exports
export default editorSlice.reducer;
