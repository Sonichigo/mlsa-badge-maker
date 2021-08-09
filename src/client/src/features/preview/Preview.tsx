import {Image, ImageFit, Shimmer, ShimmerElementType} from '@fluentui/react';
import React from 'react'
import {useAppSelector} from '../../app/hooks';
import {selectFinalAvatarBlobUrl} from '../editor/editorSlice';

const Preview = () => {
  const avatarBlobUrl = useAppSelector(selectFinalAvatarBlobUrl);

  return (
    <div>
      {avatarBlobUrl
        ? <Image src={avatarBlobUrl} imageFit={ImageFit.cover} alt="Preview of your avatar" width={400} height={400} />
        : <Shimmer shimmerElements={[{type: ShimmerElementType.circle, width: 400, height: 400}]} />}
    </div>
  )
}

export default Preview;