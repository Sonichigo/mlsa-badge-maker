import axios from 'axios';
import { apiEndpoint } from '../../config/app.json';
import ApiRequest from "../../shared/ApiRequest";

export async function applyToTeams(imageBlob: Blob, request: ApiRequest) {
  let formData = new FormData();
  formData.append('image', imageBlob);
  formData.append('token', request.token);

  var res = await axios.post(apiEndpoint + '/api/apply/teams', formData);
  return res.status === 200;
}