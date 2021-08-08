import { createSlice, PayloadAction } from "@reduxjs/toolkit";
import { RootState } from "../../app/store";

export interface EditorState {
  fileBlobUrl?: string;
  status: 'idle' | 'busy' | 'failed';
  statusMessage: string;
}

const initialState: EditorState = {
  fileBlobUrl: undefined,
  status: 'idle',
  statusMessage: '',
};

export const editorSlice = createSlice({
  name: 'editor',
  initialState,
  reducers: {
    setFileBlobUrl: (state, action: PayloadAction<string>) => ({
      ...state,
      fileBlobUrl: action.payload,
    })
  }
});

export const { setFileBlobUrl } = editorSlice.actions;

export const selectFileBlobUrl = (state: RootState) => state.editor.fileBlobUrl;

export default editorSlice.reducer;