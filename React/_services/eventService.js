import axios from "axios";
import {
  onGlobalError,
  onGlobalSuccess,
  API_HOST_PREFIX
} from "./serviceHelpers";

let create = payload => {
  const config = {
    data: payload,
    method: "POST",
    url: API_HOST_PREFIX + "/api/events/",
    crossdomain: true,
    headers: {
      "content-type": "application/json"
    }
  };

  return axios(config)
    .then(onGlobalSuccess)
    .catch(onGlobalError);
};

let selectById = id => {
  const config = {
    method: "GET",
    url: API_HOST_PREFIX + "/api/events/" + id,
    crossdomain: true,
    headers: {
      "content-type": "application/json"
    }
  };

  return axios(config)
    .then(onGlobalSuccess)
    .catch(onGlobalError);
};

let selectAllPaginated = pageIndex => {
  const config = {
    method: "GET",
    url: API_HOST_PREFIX + `/api/events?pageIndex=${pageIndex}&pageSize=6`,
    crossdomain: true,
    headers: {
      "content-type": "application/json"
    }
  };

  return axios(config)
    .then(onGlobalSuccess)
    .catch(onGlobalError);
};

let updateEvent = payload => {
  const config = {
    method: "PUT",
    url: API_HOST_PREFIX + "/api/events/" + payload.id,
    data: payload,
    crossdomain: true,
    headers: {
      "content-type": "application/json"
    }
  };

  return axios(config)
    .then(onGlobalSuccess)
    .catch(onGlobalError);
};

export { create, selectAllPaginated, updateEvent, selectById };
