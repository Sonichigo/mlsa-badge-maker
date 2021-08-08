import { Text } from '@fluentui/react';
import React from 'react'
import { useAppSelector } from '../../app/hooks';
import { selectFinalAvatarBlobUrl } from '../editor/editorSlice';

const Preview = () => {
  const avatarBlobUrl = useAppSelector(selectFinalAvatarBlobUrl);

  return (
    <div>
      <div>
        <Text variant={'xLarge'}>Preview</Text>
      </div>
      {avatarBlobUrl && <img src={avatarBlobUrl} />}
    </div>
  )
}

export default Preview;