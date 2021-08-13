import { Label, PrimaryButton, Spinner, Stack, Text } from '@fluentui/react';
import React, { useState } from 'react';
import { Cropper } from 'react-cropper';
import { useDropzone } from 'react-dropzone';

import { useAppDispatch, useAppSelector } from '../../app/hooks';
import Alert from "../../components/Alert";
import { generateAsync, selectFileBlobUrl, selectStatus, setCroppedFileBlobUrl, setFileBlobUrl, setStatus } from './editorSlice';

const Editor = () => {
  // App State
  const fileBlobUrl = useAppSelector(selectFileBlobUrl);
  const status = useAppSelector(selectStatus);

  const dispatch = useAppDispatch();

  // Scoped State
  const [cropper, setCropper] = useState<Cropper>();
  const [fileName, setFileName] = useState<string>('');

  // File upload
  const { getRootProps, getInputProps } = useDropzone({
    accept: 'image/png,image/jpeg',
    maxFiles: 1,
    // max size should be 3 MB
    maxSize: 3 * 1024 * 1024,
    onDrop: (files: File[]) => onFileChange(files),
    onDropRejected: rejections => {
      const errorMessage = rejections[0].errors.map(error => error.message).join('\n');
      dispatch(setStatus({ status: 'failed', statusMessage: errorMessage }));
    },
    onDropAccepted: () => dispatch(setStatus({ status: 'idle', statusMessage: '' }))
  });

  // read browser file from input
  const onFileChange = (files: FileList | File[]) => {
    if (!files)
      return;

    let file = files[0];
    if (!file)
      return;

    setFileName(file.name);

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
    <Stack gap={8}>
      <Alert status={status.status} statusMessage={status.statusMessage} />

      <Stack gap={4}>
        {fileBlobUrl &&
          <Cropper src={fileBlobUrl}
                   responsive={true}
                   guides={true}
                   aspectRatio={1}
                   viewMode={3}
                   onInitialized={(instance) => setCropper(instance)} />}
      </Stack>

      <Stack gap={4}>
        <Label>Upload an image</Label>

        <div {...getRootProps({className: 'dropzone'})}>
          <input {...getInputProps()} />

          <Text>
            Drag an image or click here to upload <br/>
            <small>{fileName}</small>
          </Text>
        </div>

        <small className="text-muted">PNG/JPG, 3 MB max.</small>
      </Stack>

      <Stack horizontal gap={4}>
        <PrimaryButton onClick={handleCrop} className="w-100"
                       disabled={!fileBlobUrl || status.status === 'busy'}>
          {status.status === 'busy' ? <Spinner className="ml-5" /> : "Generate"}
        </PrimaryButton>
      </Stack>
    </Stack>
  );
};

export default Editor;