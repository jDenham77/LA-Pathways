import React from "react";
import { CardImg, CardBody, CardTitle, CardText, CardFooter } from "reactstrap";
import "./resourcesAdminCSS.css";
import PropTypes from "prop-types";

const ResourceCard = props => {
  const handleDelete = () => {
    props.deleteResource(props.resource);
  };

  const handleEditClick = () => {
    props.routeToForm(props.resource);
  };

  const handleResourceDetails = () => {
    props.viewResource(props.resource);
  };

  const onImageError = e => {
    e.target.onerror = null;
    e.target.src = "https://bit.ly/364rnAc";
  };

  return (
    <div className="conatiner">
      <div className="row">
        <div className="col">
          <div className="card">
            <CardTitle id="ctrTitle">
              <strong id="ctrTitle">
                {props.resource.name.length > 35 ? props.resource.name.slice(0, 35) + "..." : props.resource.name}
              </strong>
            </CardTitle>
            <CardImg src={props.resource.logo} onError={onImageError} alt="Provide Resource Image" id="imgAdjustment" />
            <CardBody>
              <CardText>
                {props.resource.contactName.length > 35 ? props.resource.contactName.slice(0, 35) + "..." : props.resource.contactName}
              </CardText>
              <a href={`mailto:${props.resource.contactEmail}`}>{props.resource.contactEmail}</a>
              <CardText id="phone">
                {props.resource.phone.length > 35 ? props.resource.phone.slice(0, 35) + "..." : props.resource.phone}
              </CardText>
              <a href={props.resource.siteUrl}>Visit Website</a>
            </CardBody>
            <CardFooter>
              <div className="text-center">
                <button className="btn btn-primary btn-pill" id="viewMoreBtn" onClick={handleResourceDetails}>
                  View More
                </button>
                <button className="btn btn-danger btn-pill" id="deleteBtn" onClick={handleDelete}>
                  Delete
                </button>
                <button className="btn btn-secondary btn-pill" id="editBtn" onClick={handleEditClick}>
                  Edit
                </button>
              </div>
            </CardFooter>
          </div>
        </div>
      </div>
    </div>
  );
};
ResourceCard.propTypes = {
  deleteResource: PropTypes.func,
  routeToForm: PropTypes.func,
  viewResource: PropTypes.func,
  resourceTypes: PropTypes.shape({
    consulting: PropTypes.string.isRequired,
    resources: PropTypes.string.isRequired,
    specialTopics: PropTypes.string.isRequired,
    capital: PropTypes.string.isRequired,
    contracting: PropTypes.string.isRequired,
    compliance: PropTypes.string.isRequired,
    industrySpecific: PropTypes.string.isRequired,
    demoCategories: PropTypes.string.isRequired
  }),
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
};

export default ResourceCard;
