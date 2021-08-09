import React from 'react'
import {PrimaryButton} from "@fluentui/react";

import { useAppDispatch } from '../../app/hooks';
import { applyTeamsAsync, downloadImageAsync } from './useSlice';
import {RootState} from "../../app/store";
import {connect} from "react-redux";

interface UseProps {
  enabled?: boolean;
  downloadAvatarUrl?: string;
}

const Use = (props: UseProps)  => {

  const dispatch = useAppDispatch();

  return (
    <div>
      <PrimaryButton disabled={!props.enabled} onClick={() => dispatch(applyTeamsAsync())}>Apply to Teams</PrimaryButton>
      <PrimaryButton disabled={!props.enabled} onClick={() => dispatch(downloadImageAsync())}>Download Image</PrimaryButton>
      <a id="download-placebo" aria-hidden="true"
         href={props.downloadAvatarUrl} download="mlsa_avatar.png"
         rel="noreferrer" target="_blank" style={{display: 'hidden'}}>Download Image</a>
    </div>
  );
}

const mapStateToProps = (state: RootState, props: UseProps) => ({
  ...props,
  enabled: state.editor.finalAvatarBlobUrl,
  downloadAvatarUrl: state.editor.finalAvatarBlobUrl
});

export default connect(mapStateToProps, null)(Use);