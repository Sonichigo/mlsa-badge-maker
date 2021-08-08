import axios from 'axios';
import { apiEndpoint } from '../../config/app.json';

export interface ApiRequest {
  token: string;
}

export async function generateImage(imageBlob: Blob, request: ApiRequest) {
  let formData = new FormData();
  formData.append('image', imageBlob);
  formData.append('token', request.token);

  var res = await axios.post(apiEndpoint + '/api/badge', formData, {
    responseType: 'blob'
  });

  let blob = res.data as Blob;
  return URL.createObjectURL(blob);
}