import React from "react";
import PropTypes from "prop-types";
import { Button } from "reactstrap";
import { formatTime } from "../../services/dateTimeService";
import { toast } from "react-toastify";
import * as eventServices from "../../services/eventService";
import "./Event.css";

class EventDetails extends React.Component {
  state = {
    event: {
      eventTypeId: 0,
      name: "",
      summary: "",
      shortDescription: "",
      venueId: 0,
      eventStatusId: 0,
      imageUrl: "",
      externalSiteUrl: "",
      isFree: "false",
      dateStart: new Date(),
      dateEnd: new Date(),
      dateCreated: "",
      dateModified: ""
    }
  };

  componentDidMount = () => {
    const { id } = this.props.match.params;
    if (id) {
      let event = this.props.location.state;
      if (event) {
        this.setState({
          event
        });
      } else {
        this.selectById(id);
      }
    }
  };

  editEvent = () => {
    this.props.history.push(
      `/admin/Event/${this.state.event.id}/edit`,
      this.state.event
    );
  };

  goBack = () => {
    this.props.history.push("/admin/Event");
  };

  selectById = id => {
    eventServices
      .selectById(id)
      .then(this.onSuccess)
      .catch(this.onError);
  };

  onSuccess = data => {
    let event = data.item;
    this.setState(prevState => {
      return {
        ...prevState,
        event
      };
    });
  };

  onError = () => {
    toast.error("Sorry... Something happened to the servers.", {
      position: toast.POSITION.TOP_LEFT
    });
  };

  render() {
    return (
      <React.Fragment key={this.state.event.id}>
        <div className="row col-md-6 offset-3 col-md-2" id="bigCard">
          <div className="card card-absolute">
            <div className="ribbon ribbon-secondary ribbon-clip">
              <h5>{this.state.event.name}</h5>
            </div>
            <div className="card-body mt-5">
              <img
                src={
                  this.state.event.imageUrl === null ||
                  this.state.event.imageUrl === undefined
                    ? "No Image."
                    : this.state.event.imageUrl
                }
                width="300"
                alt=""
              />
              <h5 className="mt-3">{this.state.event.summary}</h5>
              <h6>{this.state.event.shortDescription}</h6>
              <p>
                Is this event free?{" "}
                <strong>
                  {this.state.event.isFree === false ? "No" : "Yes"}
                </strong>
              </p>
              <p>
                <strong>
                  {this.state.event.venueId === 0
                    ? "Confrence Hall"
                    : "Stadium"}
                </strong>
              </p>
              <p>
                <strong>
                  {this.state.event.eventStatusId === 2
                    ? "Inactive "
                    : "Active "}
                </strong>
                {this.state.event.eventStatusId === 2 ? (
                  <i className="fa fa-circle text-danger" />
                ) : (
                  <i className="fa fa-circle text-success" />
                )}
              </p>
              <p>
                <strong>
                  {this.state.event.eventTypeId === 0
                    ? "Charity"
                    : "Fundraiser"}
                </strong>
              </p>
              <p>
                Event starts from <br />
                <strong>{formatTime(this.state.event.dateStart)}</strong>
                {" ~~To~~ "}
                <strong>{formatTime(this.state.event.dateEnd)}</strong>
              </p>
            </div>
            <div className="card-footer">
              <Button
                type="button"
                className="btn-pill btn btn-primary"
                onClick={this.editEvent}
              >
                Update
              </Button>
              <Button
                type="button"
                className="btn btn-pill btn-danger ml-2"
                onClick={this.goBack}
              >
                Go Back
              </Button>
            </div>
          </div>
        </div>
      </React.Fragment>
    );
  }
}

export default EventDetails;

EventDetails.propTypes = {
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
    dateStart: PropTypes.instanceOf(Date).isRequired,
    dateEnd: PropTypes.instanceOf(Date).isRequired,
    dateCreated: PropTypes.toString(Date).isRequired,
    dateModified: PropTypes.toString(Date).isRequired
  }),
  history: PropTypes.shape({
    push: PropTypes.func
  }),
  match: PropTypes.shape({
    params: PropTypes.shape({
      id: PropTypes.toString("").isRequired
    })
  }),
  location: PropTypes.shape({
    state: PropTypes.shape({
      id: PropTypes.number.isRequired,
      eventTypeId: PropTypes.number.isRequired,
      name: PropTypes.string.isRequired,
      summary: PropTypes.string.isRequired,
      shortDescription: PropTypes.string.isRequired,
      venueId: PropTypes.number.isRequired,
      eventStatusId: PropTypes.number.isRequired,
      imageUrl: PropTypes.string.isRequired,
      externalSiteUrl: PropTypes.string.isRequired,
      isFree: PropTypes.bool.toString("").isRequired,
      dateStart: PropTypes.toString(Date).isRequired,
      dateEnd: PropTypes.toString(Date).isRequired,
      dateCreated: PropTypes.toString(Date).isRequired,
      dateModified: PropTypes.toString(Date).isRequired
    })
  }),
  editEvent: PropTypes.func
};
