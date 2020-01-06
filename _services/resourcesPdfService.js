import axios from "axios";
import * as helper from "./serviceHelpers";

let endpoints = {};
endpoints.fileUpload = helper.API_HOST_PREFIX + `/api/files/pdf/`;

let uploadPdf = (data, email) => {
  const config = {
    method: "POST",
    url: endpoints.fileUpload + `?email=${email}`,
    data: data,
    withCredentials: true,
    crossDomain: true,
    processData: false,
    header: {
      "Content-Type": false
    }
  };
  return axios(config)
    .then(helper.onGlobalSuccess)
    .catch(helper.onGlobalError);
};
export { uploadPdf };
