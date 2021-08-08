import React, { useState } from 'react';
import { Cropper } from 'react-cropper';
import { useAppDispatch, useAppSelector } from '../../app/hooks';
import { selectFileBlobUrl, setFileBlobUrl } from './editorSlice';

const Editor = () => {
  const fileBlobUrl = useAppSelector(selectFileBlobUrl);
  const dispatch = useAppDispatch();

  // read browser file from input
  const onFileChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    let files = e.currentTarget.files;
    if (!files)
      return;

    let file = files.item(0);
    if (!file)
      return;
    
    let blob = new Blob([file], { type: file.type });
    let blobUrl = URL.createObjectURL(blob);
    dispatch(setFileBlobUrl(blobUrl));
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
      <Cropper
        src=""/>
      <div className="editor-container">
        {fileBlobUrl && (
          <img src={fileBlobUrl} />
        )}
      </div>
    </div>
  );
};

export default Editor;