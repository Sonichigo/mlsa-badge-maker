import axios from 'axios';
import { apiEndpoint } from '../../app/config.json';

export async function generateImage(imageBlob: Blob) {
  let formData = new FormData();
  formData.append('image', imageBlob);

  var res = await axios.post(apiEndpoint + '/api/badge', formData, {
    responseType: 'blob'
  });

  let blob = res.data as Blob;
  return URL.createObjectURL(blob);
}