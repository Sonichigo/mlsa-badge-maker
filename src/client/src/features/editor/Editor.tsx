import React, { useState } from 'react';

import { useAppDispatch, useAppSelector } from '../../app/hooks';
import {
  setCroppedFileBlobUrl,
  setFileBlobUrl,
  generateAsync,
  selectFileBlobUrl
} from './editorSlice';

import { Label, PrimaryButton } from '@fluentui/react';
import { Cropper } from 'react-cropper';

const Editor = () => {
  // App State
  const fileBlobUrl = useAppSelector(selectFileBlobUrl);
  const dispatch = useAppDispatch();

  // Scoped State
  const [cropper, setCropper] = useState<Cropper>();

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

  const handleCrop = () => {
    if (!cropper)
      return;
    
    cropper.getCroppedCanvas().toBlob(x => {
      if (!x)
        return;

      let blob = new Blob([x], { type: x.type });
      let blobUrl = URL.createObjectURL(blob);
      dispatch(setCroppedFileBlobUrl(blobUrl));
      dispatch(generateAsync(blobUrl));
    });
  }

  return (
    <div>
      <Label>Upload an image</Label>
      <input
        name="imageFile"
        type="file"
        accept="image/*"
        onChange={onFileChange}
      />
      <div className="editor-container">
        {fileBlobUrl && <Cropper src={fileBlobUrl}
          responsive={true}
          guides={true}
          style={{ height: 400, width: "100%" }}
          initialAspectRatio={1}
          aspectRatio={1}
          onInitialized={(instance) => setCropper(instance)} />}
        
        <PrimaryButton onClick={handleCrop}
          disabled={!fileBlobUrl}>Generate</PrimaryButton>
      </div>
    </div>
  );
};

export default Editor;