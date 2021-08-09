import React, { useState } from 'react';

import { useAppDispatch, useAppSelector } from '../../app/hooks';
import {
  setCroppedFileBlobUrl,
  setFileBlobUrl,
  generateAsync,
  selectFileBlobUrl,
  selectStatus
} from './editorSlice';

import { Label, PrimaryButton, Stack, Spinner } from '@fluentui/react';
import { Cropper } from 'react-cropper';
import Alert from "../../components/Alert";

const Editor = () => {
  // App State
  const fileBlobUrl = useAppSelector(selectFileBlobUrl);
  const status = useAppSelector(selectStatus);

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
      <Stack>
        {status.statusMessage && <Alert status={status.status} statusMessage={status.statusMessage} />}

        <Stack gap={4}>
          <Label>Upload an image</Label>
          <input
            name="imageFile"
            type="file"
            accept="image/*"
            onChange={onFileChange}
          />
        </Stack>

        <Stack gap={4}>
          {fileBlobUrl && <Cropper src={fileBlobUrl}
                                   responsive={true}
                                   guides={true}
                                   style={{ height: 400, width: "100%" }}
                                   initialAspectRatio={1}
                                   aspectRatio={1}
                                   onInitialized={(instance) => setCropper(instance)} />}

          <Stack horizontal gap={4}>
            <PrimaryButton onClick={handleCrop}
                           disabled={!fileBlobUrl || status.status == 'busy'}>
              Generate
            </PrimaryButton>

            {status.status == 'busy' && <Spinner />}
          </Stack>
        </Stack>
      </Stack>
    </div>
  );
};

export default Editor;