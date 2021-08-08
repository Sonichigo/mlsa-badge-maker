import { createAsyncThunk, createSlice, PayloadAction } from "@reduxjs/toolkit";
import { RootState } from "../../app/store";
import { generateImage } from "./editorAPI";

// State
export interface EditorState {
  fileBlobUrl?: string;
  croppedFileBlobUrl?: string;
  finalAvatarBlobUrl?: string;
  status: 'idle' | 'busy' | 'failed';
  statusMessage: string;
}

const initialState: EditorState = {
  status: 'idle',
  statusMessage: '',
};

// Thunks
export const generateAsync = createAsyncThunk('editor/generateImage',
  async (fileBlobUrl: string, thunkAPI) => {
    const internalResponse = await fetch(fileBlobUrl);
    const internalBlob = await internalResponse.blob();

    const state = thunkAPI.getState() as RootState;

    const generatedImageBlobUrl = await generateImage(internalBlob, {token: state.auth.accessToken});
    return generatedImageBlobUrl;
  }
);

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
    setFinalAvatarBlobUrl: (state, action: PayloadAction<string>) => ({
      ...state,
      finalAvatarBlobUrl: action.payload,
    }),
  },
  extraReducers: (builder) => {
    builder
      .addCase(generateAsync.pending, (state) => ({
        ...state,
        status: 'busy',
        statusMessage: 'Generating image...',
      }))
      .addCase(generateAsync.rejected, (state) => ({
        ...state,
        status: 'failed',
        statusMessage: 'Failed to generate image',
      }))
      .addCase(generateAsync.fulfilled, (state, action) => ({
        ...state,
        status: 'idle',
        statusMessage: '',
        finalAvatarBlobUrl: action.payload
      }));
  }
});

// Actions
export const { setFileBlobUrl, setCroppedFileBlobUrl, setFinalAvatarBlobUrl } = editorSlice.actions;

// Selectors
export const selectFileBlobUrl = (state: RootState) => state.editor.fileBlobUrl;
export const selectCroppedFileBlobUrl = (state: RootState) => state.editor.croppedFileBlobUrl;
export const selectFinalAvatarBlobUrl = (state: RootState) => state.editor.finalAvatarBlobUrl;

// Exports
export default editorSlice.reducer;
