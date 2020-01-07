import React from "react";
import PropTypes from "prop-types";

const CardView = props => {
  return (
    <React.Fragment>
      <div className="card custom-card viewCard">
        <div className="card-profile">
          <img src="https://bit.ly/34lPucq" className="rounded-circle" alt="" />
        </div>
        <div className="text-center profile-details">
          <h4>{props.resource.contactName}</h4>
          <h6>{props.resource.phone}</h6>
          <h6>{props.resource.contactEmail}</h6>
        </div>
      </div>
    </React.Fragment>
  );
};

CardView.propTypes = {
  resource: PropTypes.shape({
    id: PropTypes.number,
    name: PropTypes.string,
    contactName: PropTypes.string,
    contactEmail: PropTypes.string,
    email: PropTypes.string,
    phone: PropTypes.string
  })
};

export default CardView;
