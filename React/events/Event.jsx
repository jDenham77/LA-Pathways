import React, { Component } from "react";
import Pagination from "rc-pagination";
import localeInfo from "rc-pagination/lib/locale/en_US";
import "rc-pagination/assets/index.css";
import EventCard from "./EventCard";
import PropTypes from "prop-types";
import * as eventServices from "../../services/eventService";
import "./Event.css";
import { toast } from "react-toastify";
import "react-toastify/dist/ReactToastify.css";

class Event extends Component {
  state = {
    mappedCard: [],
    mappedObject: [],
    totalPages: 0,
    currentPage: 0,
    event: [],
    events: [],
    formData: {
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
    eventServices
      .selectAllPaginated(this.state.currentPage)
      .then(this.onSuccess)
      .catch(this.onerror);
  };

  onChange = page => {
    this.setState(prevState => {
      return { ...prevState, currentPage: page - 1 };
    });
    eventServices.selectAllPaginated(page - 1).then(this.onSuccess);
  };

  handleClick = () => {
    let update = this.state.formData;
    eventServices
      .updateEvent(update)
      .then(this.onSuccess)
      .catch(this.onError);
  };

  onSelectedCard = event => {
    this.setState(prevState => {
      return {
        ...prevState,
        formData: event,
        event: true
      };
    });
  };

  editEvent = event => {
    this.props.history.push(`/admin/Event/${event.id}/edit`, event);
  };

  createEvent = () => {
    this.props.history.push("/admin/Event/create");
  };

  mapFunction = event => (
    <EventCard
      key={event.id}
      event={event}
      editEvent={this.editEvent}
      onViewMore={this.onViewMore}
    />
  );

  onSuccess = data => {
    let events = data.item.pagedItems;
    this.setState(prevState => {
      return {
        ...prevState,
        events,
        totalPages: data.item.totalCount,
        mappedObject: data.item.pagedItems.map(this.mapFunction),
        mappedCard: events[0]
      };
    });
  };

  onError = () => {
    toast.error("There is a problem with the servers...", {
      postition: toast.POSITION.TOP_LEFT
    });
  };

  handleViewClick = () => {
    this.setState(prevState => {
      return {
        ...prevState,
        mappedCard: prevState.event
      };
    });
  };

  onViewMore = event => {
    this.props.history.push(`/admin/Event/${event.id}/Details`, event);
  };

  render() {
    return (
      <React.Fragment>
        <div className="row addEventBtn">
          <button
            type="button"
            className="btn btn-pill
             btn-primary"
            onClick={this.createEvent}
          >
            Add Event
          </button>
        </div>
        <br />
        <div className="row">
          {this.state.mappedObject && this.state.mappedObject.length > 0 ? (
            this.state.mappedObject
          ) : (
            <strong>No Events Available.</strong>
          )}
        </div>
        <div className="row">
          <Pagination
            className="paginate"
            onChange={this.onChange}
            defaultCurrent={this.state.currentPage}
            total={this.state.totalPages}
            locale={localeInfo}
          />
        </div>
      </React.Fragment>
    );
  }
}

Event.propTypes = {
  history: PropTypes.shape({
    push: PropTypes.func
  })
};

export default Event;
