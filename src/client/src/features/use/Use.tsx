import React from 'react'
import {MessageBar, MessageBarType, PrimaryButton, Spinner} from "@fluentui/react";
import {Stack} from '@fluentui/react/lib/Stack';

import {connect} from "react-redux";

import {useAppDispatch, useAppSelector} from '../../app/hooks';
import {RootState} from "../../app/store";
import {applyTeamsAsync, downloadImageAsync, selectBusy} from './useSlice';
import Alert from "../../components/Alert";

interface UseProps {
  enabled?: boolean;
  downloadAvatarUrl?: string;
}

const Use = (props: UseProps)  => {

  const dispatch = useAppDispatch();
  const busy = useAppSelector(selectBusy);

  return (
    <div>
      <Stack gap={8}>
        {busy.statusMessage && <Alert status={busy.status} statusMessage={busy.statusMessage} />}

        <Stack horizontal gap={4}>
          <PrimaryButton disabled={!props.enabled || busy.status == 'busy'} onClick={() => dispatch(applyTeamsAsync())}>Apply to Teams</PrimaryButton>
          <PrimaryButton disabled={!props.enabled || busy.status == 'busy'} onClick={() => dispatch(downloadImageAsync())}>Download Image</PrimaryButton>
          <a id="download-placebo" aria-hidden="true"
             href={props.downloadAvatarUrl} download="mlsa_avatar.png"
             rel="noreferrer" target="_blank" style={{display: 'none'}}>Download Image</a>

          {busy.status == 'busy' && <Spinner />}
        </Stack>
      </Stack>
    </div>
  );
}

const mapStateToProps = (state: RootState, props: UseProps) => ({
  ...props,
  enabled: state.editor.finalAvatarBlobUrl,
  downloadAvatarUrl: state.editor.finalAvatarBlobUrl
});

export default connect(mapStateToProps, null)(Use);