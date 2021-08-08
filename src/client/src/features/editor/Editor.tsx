import React, { useState } from 'react';
import { useAppDispatch } from '../../app/hooks';
import { setFile } from './editorSlice';

const Editor = () => {
  const dispatch = useAppDispatch();

  // read browser file from input
  const onFileChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    let files = e.currentTarget.files;
    if (!files)
      return;

    let file = files.item(0);
    dispatch(setFile(file));
  };

  return (
    <div className="editor">
      <label>Use a custom image</label>
      <input
        name="imageFile"
        type="file"
        accept="image/*"
        onChange={onFileChange}
      />
      <div className="editor-container">
        {/* {file && (
          <img src={file} />
        )} */}
      </div>
    </div>
  );
};

export default Editor;