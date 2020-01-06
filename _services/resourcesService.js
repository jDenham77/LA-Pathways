import axios from "axios";
import * as helpers from "./serviceHelpers";

let pagination = pageIndex => {
  const config = {
    method: "GET",
    url: `${helpers.API_HOST_PREFIX}/api/resources/paginate?pageIndex=${pageIndex}&pageSize=12`,
    withCredentials: true,
    crossdomain: true,
    headers: { "Content-Type": "application/json" }
  };
  return axios(config)
    .then(helpers.onGlobalSuccess)
    .catch(helpers.onGlobalError);
};

let getById = id => {
  const config = {
    method: "GET",
    url: `${helpers.API_HOST_PREFIX}/api/resources/${id}`,
    withCredentials: true,
    crossdomain: true,
    headers: { "Content-Type": "application/json" }
  };
  return axios(config)
    .then(helpers.onGlobalSuccess)
    .catch(helpers.onGlobalError);
};

let getResourceTypes = () => {
  const config = {
    method: "GET",
    url: `${helpers.API_HOST_PREFIX}/api/resources/types`,
    withCredentials: true,
    crossdomain: true,
    headers: { "Content-Type": "application/json" }
  };
  return axios(config)
    .then(helpers.onGlobalSuccess)
    .catch(helpers.onGlobalError);
};

let add = payload => {
  const config = {
    method: "POST",
    url: `${helpers.API_HOST_PREFIX}/api/resources`,
    withCredentials: true,
    data: payload,
    crossdomain: true,
    headers: { "Content-Type": "application/json" }
  };
  return axios(config)
    .then(helpers.onGlobalSuccess)
    .catch(helpers.onGlobalError);
};

let update = payload => {
  const config = {
    method: "PUT",
    url: `${helpers.API_HOST_PREFIX}/api/resources/${payload.id}`,
    withCredentials: true,
    data: payload,
    crossdomain: true,
    headers: { "Content-Type": "application/json" }
  };
  return axios(config)
    .then(helpers.onGlobalSuccess)
    .catch(helpers.onGlobalError);
};

let deleteResource = id => {
  const config = {
    method: "DELETE",
    url: `${helpers.API_HOST_PREFIX}/api/resources/${id}`,
    withCredentials: true,
    crossdomain: true,
    headers: { "Content-Type": "application/json" }
  };
  return axios(config)
    .then(helpers.onGlobalSuccess)
    .then(() => id)
    .catch(helpers.onGlobalError);
};

let search = (pageIndex, query) => {
  const config = {
    method: "GET",
    url: `${helpers.API_HOST_PREFIX}/api/resources/search?pageIndex=${pageIndex}&pageSize=12&q=${query}`,
    withCredentials: true,
    headers: { "Content-Type": "application/json" }
  };
  return axios(config)
    .then(helpers.onGlobalSuccess)
    .catch(helpers.onGlobalError);
};

let activeResourceDetails = id => {
  const config = {
    method: "GET",
    url: `${helpers.API_HOST_PREFIX}/api/resources/${id}`,
    withCredentials: true,
    crossdomain: true,
    headers: { "Content-Type": "application/json" }
  };
  return axios(config)
    .then(helpers.onGlobalSuccess)
    .catch(helpers.onGlobalError);
};

let getLocationOptions = () => {
  const config = {
    method: "GET",
    url: `${helpers.API_HOST_PREFIX}/api/locations/options`,
    withCredentials: true,
    crossdomain: true,
    headers: { "Content-Type": "application/json" }
  };
  return axios(config)
    .then(helpers.onGlobalSuccess)
    .catch(helpers.onGlobalError);
};

export { getLocationOptions, pagination, getById, add, update, deleteResource, search, getResourceTypes, activeResourceDetails };
