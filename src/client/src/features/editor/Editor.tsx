import React, { useState } from 'react';
import { Cropper } from 'react-cropper';
import { useAppDispatch, useAppSelector } from '../../app/hooks';
import { selectFileBlobUrl, setCroppedFileBlobUrl, setFileBlobUrl } from './editorSlice';

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
    });
  }

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
        {fileBlobUrl && <Cropper src={fileBlobUrl}
          responsive={true}
          guides={true}
          style={{ height: 400, width: "100%" }}
          initialAspectRatio={1}
          aspectRatio={1}
          onInitialized={(instance) => setCropper(instance)} />}
        
        <button onClick={handleCrop}>Crop</button>
      </div>
    </div>
  );
};

export default Editor;