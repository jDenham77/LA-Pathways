import axios from "axios";
import * as helper from "./serviceHelpers";

let endpoint = helper.API_HOST_PREFIX + "/api/user";

let register = payload => {
  const config = {
    method: "POST",
    url: endpoint + "/register",
    withCredentials: true,
    data: payload,
    crossdomain: true,
    headers: { "Content-Type": "application/json" }
  };
  return axios(config)
    .then(helper.onGlobalSuccess)
    .catch(helper.onGlobalError);
};
let getCurrent = () => {
  const config = {
    method: "GET",
    url: endpoint + "/current",

    withCredentials: true,
    crossdomain: true
  };
  return axios(config)
    .then(helper.onGlobalSuccess)
    .catch(helper.onGlobalError);
};

let login = payload => {
  const config = {
    method: "POST",
    url: endpoint + "/login",
    withCredentials: true,
    data: payload,
    crossdomain: true,
    headers: { "Content-Type": "application/json" }
  };
  return axios(config)
    .then(helper.onGlobalSuccess)
    .catch(helper.onGlobalError);
};

let logOut = () => {
  const config = {
    method: "GET",
    url: helper.API_HOST_PREFIX + "/api/user/logout",
    withCredentials: true,
    crossdomain: true,
    headers: {
      "Content-Type": "application/json"
    }
  }
  return axios(config)
    .then(helper.onGlobalSuccess)
    .catch(helper.onGlobalError)
}

export { getCurrent, login, register, logOut };
