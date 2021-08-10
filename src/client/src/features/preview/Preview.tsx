import React from 'react'
import {Image, ImageFit, Text} from '@fluentui/react';
import {useAppSelector} from '../../app/hooks';
import {selectFinalAvatarBlobUrl} from '../editor/editorSlice';

const Preview = () => {
  const avatarBlobUrl = useAppSelector(selectFinalAvatarBlobUrl);

  return (
    <div>
      {avatarBlobUrl
        ? <Image src={avatarBlobUrl} imageFit={ImageFit.cover} alt="Preview of your avatar" />
        : <Text variant={'small'}>Generate your avatar using the editor above first.</Text>}
    </div>
  )
}

export default Preview;