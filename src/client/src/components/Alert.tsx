import React from "react";
import {MessageBar, MessageBarType} from "@fluentui/react";

const Alert = (props: ({ status: 'idle' | 'busy' | 'success' | 'failed', statusMessage: string })) => <MessageBar messageBarType={(() => {
  switch(props.status) {
    case 'busy': return MessageBarType.info;
    case 'failed': return MessageBarType.error;
    case 'success': return MessageBarType.success;
  }
})()}>{props.statusMessage}</MessageBar>

export default Alert;