import React from 'react'
import { useAppSelector } from '../../app/hooks';
import { selectFinalAvatarBlobUrl } from '../editor/editorSlice';

const Preview = () => {
  const avatarBlobUrl = useAppSelector(selectFinalAvatarBlobUrl);

  return (
    <div>
      <h1>Preview</h1>
      {avatarBlobUrl && <img src={avatarBlobUrl} />}
    </div>
  )
}

export default Preview;