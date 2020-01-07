import React from "react";
import PropTypes from "prop-types";
import { Button } from "reactstrap";

import "./Event.css";

const EventCard = props => {
  const handleClick = () => {
    props.editEvent(props.event);
  };

  const handleCardView = () => {
    props.onViewMore(props.event);
  };

  return (
    <React.Fragment key={props.event.id}>
      <div className="col-sm-6 col-md-4" id="smallCard">
        <div className="card card-absolute">
          <div className="ribbon-header ribbon ribbon-clip ribbon-primary">
            <h5>{props.event.name}</h5>
          </div>
          <div className="card-body pt-5">
            <img
              id="cardImg"
              src={
                props.event.imageUrl === null ||
                props.event.imageUrl === undefined
                  ? "No Image."
                  : props.event.imageUrl
              }
              width="300"
              height="180"
              alt=""
            />
            <p>
              <strong>
                {props.event.eventStatusId === 2 ? "Inactive " : "Active "}
              </strong>
              {props.event.eventStatusId === 2 ? (
                <i className="fa fa-circle text-danger" />
              ) : (
                <i className="fa fa-circle text-success" />
              )}
            </p>
            <p>{props.event.shortDescription}</p>
            <p>
              Event Date Details:
              <br />
              {props.event.dateStart.slice(0, 10)}
              {" To "}
              {props.event.dateEnd.slice(0, 10)}
            </p>
          </div>
          <div className="card-footer">
            <Button
              type="button"
              className="btn btn-pill btn-primary"
              onClick={handleClick}
            >
              Edit
            </Button>{" "}
            <Button
              type="button"
              className="btn btn-pill btn-danger"
              onClick={handleCardView}
            >
              View More Info
            </Button>
          </div>
        </div>
      </div>
    </React.Fragment>
  );
};

export default EventCard;

EventCard.propTypes = {
  event: PropTypes.shape({
    id: PropTypes.number.isRequired,
    eventTypeId: PropTypes.number.isRequired,
    name: PropTypes.string.isRequired,
    summary: PropTypes.string.isRequired,
    shortDescription: PropTypes.string.isRequired,
    venueId: PropTypes.number.isRequired,
    eventStatusId: PropTypes.number.isRequired,
    imageUrl: PropTypes.string.isRequired,
    externalSiteUrl: PropTypes.string.isRequired,
    isFree: PropTypes.bool.isRequired,
    dateStart: PropTypes.toString(Date).isRequired,
    dateEnd: PropTypes.toString(Date).isRequired,
    dateCreated: PropTypes.toString(Date).isRequired,
    dateModified: PropTypes.toString(Date).isRequired
  }),
  editEvent: PropTypes.func.isRequired,
  onViewMore: PropTypes.func.isRequired
};
