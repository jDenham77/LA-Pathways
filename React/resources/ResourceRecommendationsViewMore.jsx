import React from "react";
import PropTypes from "prop-types";
import "./resourceCSS.css";
import * as resourceService from "../../services/resourceRecommendationService";

class ResourceRecommendationsViewMore extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      resource: [],
      resourceCategories: [],
      instanceId: 0
    };
  }

  componentDidMount = () => {
    const { id } = this.props.match.params;
    let instanceId = this.props.match.params.instanceId;
    if (id) {
      let resource = this.props.history.location.state;
      if (resource) {
        this.setState(prevState => {
          return {
            ...prevState,
            resource,
            instanceId: instanceId,
            resourceCategories: resource.resourceCategories.map(
              this.mapCategories
            )
          };
        });
      } else {
        resourceService
          .getResourceById(id)
          .then(this.onGetResourceSuccess)
          .catch(this.onGetError);
      }
    }
  };

  onGetResourceSuccess = data => {
    let resource = data.item;
    this.setState(prevState => {
      return {
        ...prevState,
        resource,
        resourceCategories: resource.resourceCategories.map(this.mapCategories)
      };
    });
  };

  removeLinkPrefix = link => {
    let result = "";
    result = link.replace(/(^\w+:|^)\/\//, "");

    return result;
  };

  mapCategories = category => (
    <li key={category.categoryType}>
      <i className="fa fa-angle-double-right txt-primary m-r-5"></i>
      {category.categoryType}
    </li>
  );

  goBack = () => {
    this.props.history.push(
      `/resources/recommendations/${this.state.instanceId}`,
      this.state.instanceId
    );
  };

  onImageError = e => {
    e.target.onerror = null;
    e.target.src =
      "https://www.market-research-companies.in//images/default.jpg";
  };
  render() {
    return (
      <div className="user-profile">
        <div className="row">
          <div className="col-sm-12">
            <div className="card hovercard text-center">
              <div className="cardheader background-image" />
              <div className="user-image">
                <div className="avatar">
                  <img
                    alt=""
                    onError={this.onImageError}
                    src={this.state.resource.logo}
                  />
                </div>
              </div>
              <div className="info-padding ">
                <div className="row">
                  <div className="col-sm-6 col-xl-4 order-sm-1 order-xl-0">
                    <div className="row">
                      <div className="col-md-6">
                        <div className="ttl-info text-left">
                          <h6>
                            <i className="icofont icofont-id-card" />
                            &nbsp;&nbsp;&nbsp;Contact
                          </h6>
                          <span>{this.state.resource.contactName}</span>
                        </div>
                      </div>
                      <div className="col-md-6">
                        <div className="ttl-info text-left">
                          <h6>
                            <i className="fa fa-envelope" />
                            &nbsp;&nbsp;&nbsp;Email
                          </h6>
                          <a
                            href={`mailto:${this.state.resource.contactEmail}`}
                          >
                            {this.state.resource.contactEmail}
                          </a>
                        </div>
                      </div>
                    </div>
                  </div>
                  <div className="col-sm-12 col-xl-4 order-sm-0 order-xl-1">
                    <div className="user-designation">
                      <div className="title text-large">
                        <a href={this.state.resource.siteUrl}>
                          {this.state.resource.name}
                        </a>
                      </div>
                      <div className="title text-large">
                        <button
                          onClick={this.goBack}
                          className="btn-pill btn-light mt-2 txt-dark btn btn-light btn-sm"
                        >
                          Go Back <i className="fa fa-angle-double-right" />
                        </button>
                      </div>
                    </div>
                  </div>
                  <div className="col-sm-6  col-xl-4 order-sm-2 order-xl-2">
                    <div className="row">
                      <div className="col-md-6">
                        <div className="ttl-info text-left">
                          <h6>
                            <i className="fa fa-external-link" />
                            &nbsp;&nbsp;&nbsp;Website
                          </h6>
                          <a href={this.state.resource.siteUrl}>
                            {this.state.resource.siteUrl}
                          </a>
                        </div>
                      </div>
                      <div className="col-md-6">
                        <div className="ttl-info text-left">
                          <h6>
                            <i className="fa fa-phone" />
                            &nbsp;&nbsp;&nbsp;Phone
                          </h6>
                          <span>{this.state.resource.phone}</span>
                        </div>
                      </div>
                    </div>
                  </div>
                </div>
                <hr id="middleHr" />
                <div className="follow">
                  <div className="row">
                    <div className="col-6 text-md-right border-right">
                      <div>
                        <span className="text-medium txt-dark">
                          Resource Overview
                        </span>
                      </div>
                      <span>{this.state.resource.description}</span>
                    </div>
                    <div className="col-6 text-md-left">
                      <div>
                        <span className="text-medium txt-dark">
                          Resource Categories
                        </span>
                      </div>
                      <div className="col-md-6">
                        {this.state.resourceCategories.length > 4 ? (
                          <div className="row">
                            <div className="col-md-6 ">
                              <ul>
                                {this.state.resourceCategories.slice(0, 4)}
                              </ul>
                            </div>
                            <div className="col-md-6">
                              <ul>
                                {this.state.resourceCategories.slice(4, 8)}
                              </ul>
                            </div>
                          </div>
                        ) : (
                          <ul>{this.state.resourceCategories}</ul>
                        )}
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            </div>
          </div>
        </div>
      </div>
    );
  }
}

ResourceRecommendationsViewMore.propTypes = {
  recommendation: PropTypes.shape({
    name: PropTypes.string.isRequired,
    description: PropTypes.string.isRequired,
    headline: PropTypes.string.isRequired,
    siteUrl: PropTypes.string.isRequired,
    logo: PropTypes.string.isRequired
  }),
  match: PropTypes.shape({
    params: PropTypes.shape({
      id: PropTypes.string.isRequired,
      instanceId: PropTypes.string.isRequired
    })
  }),
  history: PropTypes.shape({
    push: PropTypes.func,
    location: PropTypes.shape({
      state: PropTypes.shape({
        name: PropTypes.string.isRequired,
        description: PropTypes.string.isRequired,
        headline: PropTypes.string.isRequired,
        siteUrl: PropTypes.string.isRequired,
        logo: PropTypes.string.isRequired,
        resourceCategories: PropTypes.arrayOf(
          PropTypes.shape({ categoryType: PropTypes.string.isRequired })
        )
      })
    })
  })
};

export default ResourceRecommendationsViewMore;
