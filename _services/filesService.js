import axios from "axios";
import * as helper from "./serviceHelpers";

let endpoints = {};
endpoints.fileUpload = helper.API_HOST_PREFIX + `/api/upload`;
endpoints.fileUploadToDb = helper.API_HOST_PREFIX + `/api/upload/db`;
endpoints.files = helper.API_HOST_PREFIX + `/api/files/paginate`;
endpoints.fileTypes = helper.API_HOST_PREFIX + `/api/files/types`;

let getPaginated = (pageIndex, pageSize) => {
  const config = {
    method: "GET",
    url: endpoints.files + `?pageIndex=${pageIndex}&pageSize=${pageSize}`,
    withCredentials: true,
    crossDomain: true,
    header: {
      "Content-Type": "application/json"
    }
  };
  return axios(config)
    .then(helper.onGlobalSuccess)
    .catch(helper.onGlobalError);
};

let uploadFile = data => {
  const config = {
    method: "POST",
    url: endpoints.fileUpload,
    data: data,
    withCredentials: true,
    crossDomain: true,
    header: {
      "Content-Type": "multipart/form-data"
    }
  };
  return axios(config)
    .then(helper.onGlobalSuccess)
    .catch(helper.onGlobalError);
};

let uploadToDb = data => {
  const config = {
    method: "POST",
    url: endpoints.fileUploadToDb,
    data: data,
    withCredentials: true,
    crossDomain: true,
    header: {
      "Content-Type": "application/json"
    }
  };
  return axios(config)
    .then(helper.onGlobalSuccess)
    .catch(helper.onGlobalError);
};

let getFileTypes = () => {
  const config = {
    method: "GET",
    url: endpoints.fileTypes,
    withCredentials: true,
    crossDomain: true,
    header: {
      "Content-Type": "application/json"
    }
  };
  return axios(config)
    .then(helper.onGlobalSuccess)
    .catch(helper.onGlobalError);
};

export { getPaginated, uploadFile, uploadToDb, getFileTypes };
