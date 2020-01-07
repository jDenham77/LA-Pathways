import axios from "axios";
import {
  onGlobalError,
  onGlobalSuccess,
  API_HOST_PREFIX
} from "./serviceHelpers";

const endpoint = `${API_HOST_PREFIX}/api/userprofiles`;

let getPaginated = (pageIndex, pageSize) => {
  const config = {
    method: "GET",
    url: `${endpoint}?pageIndex=${pageIndex}&pageSize=${pageSize}`,
    withCredentials: true
  };

  return axios(config)
    .then(onGlobalSuccess)
    .catch(onGlobalError);
};

let getById = id => {
  const config = {
    method: "GET",
    url: `${endpoint}/${id}`,
    withCredentials: true
  };

  return axios(config)
    .then(onGlobalSuccess)
    .catch(onGlobalError);
};

let add = payload => {
  const config = {
    method: "POST",
    url: `${endpoint}`,
    data: payload,
    withCredentials: true,
    crossdomain: true,
    headers: {
      "Content-Type": "application/json"
    }
  };

  return axios(config)
    .then(onGlobalSuccess)
    .catch(onGlobalError);
};

let edit = payload => {
  const config = {
    method: "PUT",
    url: `${endpoint}/${payload.id}`,
    data: payload,
    withCredentials: true,
    crossdomain: true,
    headers: {
      "Content-Type": "application/json"
    }
  };

  return axios(config)
    .then(onGlobalSuccess)
    .catch(onGlobalError);
};

let verifyProfile = userId => {
  const config = {
    method: "GET",
    url: endpoint + "/verify/" + userId,
    data: userId,
    withCredentials: true,
    crossdomain: true,
    headers: {
      "Content-Type": "application/json"
    }
  };

  return axios(config)
    .then(onGlobalSuccess)
    .catch(onGlobalError);
};

export { getPaginated, add, edit, verifyProfile, getById };
