import axios from "axios";
import {
  onGlobalError,
  onGlobalSuccess,
  API_HOST_PREFIX
} from "./serviceHelpers";

const getPaginate = pageIndex => {
  const config = {
    method: "GET",
    withCredentials: true,
    crossdomain: true,
    url: `${API_HOST_PREFIX}/api/surveys/questions?pageIndex=${pageIndex}&pageSize=8`
  };

  return axios(config)
    .then(onGlobalSuccess)
    .catch(onGlobalError);
};

const getGraphInfo = payload => {
  const config = {
    method: "POST",
    withCredentials: true,
    crossdomain: true,
    data: payload,
    header: {
      "Content-Type": "application/json"
    },
    url: `${API_HOST_PREFIX}/api/surveys/answers/graph`
  };

  return axios(config)
    .then(onGlobalSuccess)
    .catch(onGlobalError);
};

const getInstanceByDate = (start, end) => {
  const config = {
    method: "GET",
    withCredentials: true,
    crossdomain: true,
    url: `${API_HOST_PREFIX}/api/surveys/instances/search?start=${start}&end=${end}`
  };

  return axios(config)
    .then(onGlobalSuccess)
    .catch(onGlobalError);
};

export { getPaginate, getGraphInfo, getInstanceByDate };
