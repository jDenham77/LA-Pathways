import React, { Component } from "react";
import * as resourceService from "../../services/resourceRecommendationService";
import ResourceRecommendationCard from "./ResourceRecommendationCard";
import Pagination from "rc-pagination";
import "rc-pagination/assets/index.css";
import swal from "sweetalert";
import PropTypes from "prop-types";

import * as resourcePdfCreator from "./resourcePdfCreator";

class ResourceRecommendations extends Component {
  constructor() {
    super();
    this.state = {
      activeTab: "1",
      setActiveTab: "1",
      active: "1",
      instanceId: [],
      pageSize: 4,
      current: 0,
      totalCount: 0,
      resources: [],
      categories: {},
      mappedResources: [],
      allResources: [],
      email: ""
    };
  }

  componentDidMount = () => {
    let { id } = this.props.match.params;
    this.setState(prevState => {
      return {
        ...prevState,
        instanceId: id
      };
    });
    if (id) {
      resourceService
        .getResourcesByInstanceId(id, this.state.current, this.state.pageSize)
        .then(this.onGetResourcesSuccess)
        .catch(this.onGetError);
    }
    resourceService
      .getResourcesAllByInstanceId(id)
      .then(this.onGetAllResourceSuccess)
      .catch(this.onGetError);
  };

  onGetAllResourceSuccess = data => {
    let allResources = data.item;

    this.setState(prevState => {
      return {
        ...prevState,
        allResources
      };
    });
  };

  onGetResourcesSuccess = data => {
    let resources = data.item.pagedItems;

    this.setState(prevState => {
      return {
        ...prevState,
        resources,

        mappedResources: resources.map(this.mapRecommendation),
        totalCount: data.item.totalCount
      };
    });
  };

  onChange = page => {
    this.setState(prevState => {
      return {
        ...prevState,
        current: page - 1
      };
    });

    resourceService
      .getResourcesByInstanceId(
        this.state.instanceId,
        page - 1,
        this.state.pageSize
      )
      .then(this.onGetResourcesSuccess)
      .catch(this.onGetError);
  };

  mapRecommendation = recommendation => (
    <ResourceRecommendationCard
      pushToViewMore={this.pushToViewMore}
      recommendation={recommendation}
      key={recommendation.id}
    />
  );

  pushToViewMore = recommendation => {
    this.props.history.push(
      `/resources/recommendations/viewmore/${this.state.instanceId}/${recommendation.id}`,
      recommendation,
      this.state.instanceId
    );
  };

  pushToContactsPage = () => {
    this.props.history.push(`/contactUs`);
  };

  handleEmailResults = () => {
    swal("Please enter the email you would like to send to:", {
      content: "input",
      button: {
        text: "Submit",
        closeModal: false
      }
    }).then(value => {
      if (this.validateEmail(value) === false) {
        swal("Please enter a valid email");
      } else {
        this.setState(prevState => {
          return {
            ...prevState,
            email: value
          };
        });
        resourcePdfCreator.exportPDF(this.state.allResources, value, true);
        swal(
          "Email Sent!",
          "Please check your email for your personalized resources",
          "success"
        );
      }
    });
  };
  validateEmail = mail => {
    var re = /^[a-zA-Z0-9]+@[a-zA-Z0-9]+\.[A-Za-z]+$/;
    if (re.test(mail)) {
      return true;
    }

    return false;
  };

  render() {
    return (
      <div className="m-10">
        <div className="row m-2 text-center">
          <div className="col-md-12">
            <h3>Resource Recommendations</h3>
          </div>
        </div>
        <div className="row m-1 text-center">
          <div className="col-md-12">
            <h5>
              Thank you for completing our survey! Here are your resources that
              will help your company grow.
            </h5>
          </div>
        </div>
        <div className="row">{this.state.mappedResources}</div>

        <div className="row text-center mb-3">
          <div className="offset-md-2 col-8">
            <button
              onClick={this.handleEmailResults}
              type="button"
              className="btn-pill btn-air-secondary btn btn-secondary btn-small btn-air-secondary ml-3 mr-3 "
            >
              Email Results{" "}
              <i className=" icon icon-email text-center mr-2"></i>
            </button>

            <button
              onClick={this.pushToContactsPage}
              type="button"
              className="btn-pill btn-air-light btn btn-light btn-small btn-air-light text-center ml-3 mr-3"
            >
              Contact Us! <i className="fa fa-group"></i>
            </button>

            <button
              className="btn-pill btn-air-primary btn btn-primary btn btn-primary ml-3 mr-3"
              onClick={() =>
                resourcePdfCreator.exportPDF(
                  this.state.allResources,
                  "null",
                  false
                )
              }
            >
              Download Results{" "}
              <i className="icofont icofont-download-alt text-center"></i>
            </button>
          </div>
        </div>
        {this.state.allResources.length > this.state.pageSize ? (
          <div className="row">
            <Pagination
              className="pb-3 m-auto ant-pagination pagination "
              pageSize={this.state.pageSize}
              onChange={this.onChange}
              current={this.state.current + 1}
              total={this.state.totalCount}
              showTitle={false}
            />
          </div>
        ) : (
          ""
        )}
      </div>
    );
  }
}

ResourceRecommendations.propTypes = {
  match: PropTypes.shape({
    params: PropTypes.shape({
      id: PropTypes.string
    })
  }),
  history: PropTypes.shape({
    push: PropTypes.func.isRequired
  })
};

export default ResourceRecommendations;
