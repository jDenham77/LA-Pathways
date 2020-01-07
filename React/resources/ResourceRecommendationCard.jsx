import React from "react";
import PropTypes from "prop-types";
import "./resourceCSS.css";

const ResourceRecommendationCard = props => {
  const onImageError = e => {
    e.target.onerror = null;
    e.target.src =
      "https://www.market-research-companies.in//images/default.jpg";
  };
  const handleViewMore = () => {
    props.pushToViewMore(props.recommendation);
  };

  return (
    <div className="col-md-6 col-xs-12">
      <div className="card default-widget-count ">
        <div className="card-body">
          <div className="media">
            {props.recommendation.logo ? (
              <div className="mr-3 left">
                <div className="bg">
                  <img
                    src={props.recommendation.logo}
                    onError={onImageError}
                    className="icon-user image-size"
                    alt=""
                  />
                </div>
              </div>
            ) : (
              <div className="mr-3 left b-primary">
                <div className="bg bg-primary">
                  <i className="icon-user" />
                </div>
              </div>
            )}
            <div className="media-body align-self-center text-center">
              <h4 className="resource-name m-2">{props.recommendation.name}</h4>
              <a href={props.recommendation.siteUrl}>
                <i className="m-2 fa fa-external-link mr-1 txt-primary fa-xs">
                  {"  "}
                  {props.recommendation.name}
                </i>
              </a>
              <div>
                <button
                  onClick={handleViewMore}
                  className="m-2 btn-pill btn-xs btn-light txt-dark btn btn-light"
                >
                  View More Details
                </button>
              </div>
            </div>
          </div>
        </div>
      </div>
    </div>
  );
};

ResourceRecommendationCard.propTypes = {
  pushToViewMore: PropTypes.func,
  recommendation: PropTypes.shape({
    name: PropTypes.string.isRequired,
    description: PropTypes.string.isRequired,
    headline: PropTypes.string.isRequired,
    siteUrl: PropTypes.string.isRequired,
    logo: PropTypes.string.isRequired
  })
};

export default ResourceRecommendationCard;
