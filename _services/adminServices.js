import axios from "axios";
import * as helper from "./serviceHelpers";

let endpoints = {};
endpoints.resources = helper.API_HOST_PREFIX + "/api/resources/all";

let getAllRecources = () => {
  const config = {
    method: "GET",
    url: endpoints.resources,
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

export { getAllRecources };
