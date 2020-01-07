import React from "react";
import PropTypes from "prop-types";
import "./SideWidget.css";

const SideWidget = props => {
  return (
    <div className="col-md-4">
      <div className="card default-widget-count statCard cardHeight">
        <div className="card-body">
          <div className="media height">
            <div className="media-body align-self-center">
              <h4 className="m-auto counter textAlign">
                {props.totalInstances}
              </h4>
              <div className="row">
                <span className="m-auto text-align">{props.title}</span>
              </div>
            </div>
            {props.resource ? (
              <i className="pl-2 fa fa-4x fa-database txt-secondary"></i>
            ) : (
              <i className="pl-2 fa fa-5x fa-pencil-square-o txt-secondary"></i>
            )}
          </div>
        </div>
      </div>
    </div>
  );
};

SideWidget.propTypes = {
  totalInstances: PropTypes.number,
  title: PropTypes.string,
  resource: PropTypes.bool
};

export default SideWidget;
