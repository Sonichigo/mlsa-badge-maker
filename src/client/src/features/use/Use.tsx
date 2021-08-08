import React from 'react'
import { useAppDispatch } from '../../app/hooks';
import { applyTeamsAsync } from './useSlice';

const Use = () => {

  const dispatch = useAppDispatch();

  return <div><button onClick={() => dispatch(applyTeamsAsync())}>Apply to Teams</button></div>
}

export default Use;