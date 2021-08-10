interface StatusState {
  status: 'idle' | 'busy' | 'success' | 'failed';
  statusMessage: string;
}

export default StatusState;