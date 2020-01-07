import React from "react";
import PropTypes from "prop-types";
import * as resourcesService from "../../services/resourcesService";
import "./resourcesAdminCSS.css";
import { toast } from "react-toastify";
import ResourceCategories from "./ResourceCategories";

class ResourceDetails extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      resource: []
    };
  }

  componentDidMount() {
    const { id } = this.props.match.params;
    if (id) {
      let resource = this.props.history.location.state;
      this.state.resource.push(resource);
      if (resource) {
        this.setState(prevState => {
          return {
            ...prevState,
            resource,
            baseCategoryType: resource.baseCategoryType.map(this.mapCategories)
          };
        });
      } else {
        this.getById(id);
      }
    }
  }

  sortCategories = () => {
    this.state.resource.baseCategoryType.map();
  };

  editResource = () => {
    this.props.history.push(`/admin/resource/${this.props.match.params.id}/edit`, this.state.resource);
  };

  goBack = () => {
    return this.props.history.push("/admin/resources");
  };

  getById = id => {
    resourcesService
      .getById(id)
      .then(this.onSuccess)
      .catch(this.onError);
  };

  onSuccess = data => {
    let baseCategoryType = data.item;
    this.setState(prevState => {
      return {
        ...prevState,
        baseCategoryType: baseCategoryType.map(this.mapCategories),
        resource: { ...prevState.resource, baseCategoryType }
      };
    });
  };

  onError = error => {
    toast.error(error.message);
  };

  goBack = () => {
    this.props.history.push(`/admin/resources`);
  };

  mapCategories = (category, index) => <ResourceCategories key={index} cats={category} />;

  render() {
    return (
      <React.Fragment>
        <div className="card  cardTop">
          <div className="container">
            <div className="row">
              <div className="title">
                <h3>{this.state.resource ? this.state.resource.name : ""}</h3>
              </div>
            </div>
            <hr />
            <div className="row">
              <img
                src={this.state.resource ? "https://bit.ly/364rnAc" : this.state.resource.logo}
                alt="Source Not Provided"
                id="resourceImg"
              />
              <div className="row m-auto">
                <div className="column">
                  <p className="description">{this.state.resource ? this.state.resource.description : ""}</p>
                  <p>{this.state.resource ? this.state.resource.contactName : ""}</p>
                  <p>{this.state.resource ? this.state.resource.contactEmail : ""}</p>
                  <p>{this.state.resource ? this.state.resource.phone : ""}</p>
                  <a href={this.state.resource ? this.state.resource.siteUrl : ""}>Go To Website</a>
                </div>
              </div>
            </div>
            <hr />
          </div>
          <div className="resourceCats">
            <h3 className="m-auto">Provided Resources</h3>
            <div className="d-flex justify-content-around mt-3">{this.state.baseCategoryType}</div>
          </div>
          <div>
            <button className="btn btn-secondary btn-pill float-right mr-2 mb-2 mt-5" onClick={this.goBack}>
              Go Back
            </button>
          </div>
        </div>
      </React.Fragment>
    );
  }
}
ResourceDetails.propTypes = {
  history: PropTypes.shape({
    location: PropTypes.shape({
      state: PropTypes.shape({
        name: PropTypes.string,
        headline: PropTypes.string,
        description: PropTypes.string,
        logo: PropTypes.string,
        locationId: PropTypes.number,
        contactName: PropTypes.string,
        contactEmail: PropTypes.string,
        phone: PropTypes.string,
        siteUrl: PropTypes.string,
        baseCategoryType: PropTypes.arrayOf(
          PropTypes.shape({
            name: PropTypes.string,
            code: PropTypes.string,
            categoryType: PropTypes.string
          })
        )
      })
    }),
    push: PropTypes.func,
    resource: PropTypes.shape({
      id: PropTypes.number.isRequired,
      name: PropTypes.string.isRequired,
      logo: PropTypes.string.isRequired,
      description: PropTypes.string.isRequired,
      contactName: PropTypes.string.isRequired,
      contactEmail: PropTypes.string.isRequired,
      phone: PropTypes.string.isRequired,
      siteUrl: PropTypes.string.isRequired
    })
  }),
  match: PropTypes.shape({
    params: PropTypes.shape({
      id: PropTypes.string
    })
  })
};

export default ResourceDetails;
