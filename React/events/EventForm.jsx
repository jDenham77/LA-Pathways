import React from "react";
import DatePicker from "react-datepicker";
import { Form, FormGroup, Label } from "reactstrap";
import { Formik, Field } from "formik";
import PropTypes from "prop-types";
import * as eventService from "../../services/eventService";
import ValidationSchema from "./validationSchema";
import "react-datepicker/dist/react-datepicker.css";
import "./Event.css";
import { toast } from "react-toastify";
import FileUpload from "../files/FileUpload";
import "react-toastify/dist/ReactToastify.css";

class EventForm extends React.Component {
  state = {
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
      dateModified: "",
      fileId: ""
    },
    isEditing: false
  };

  componentDidMount = () => {
    const { id } = this.props.match.params;
    if (id) {
      let formData = this.props.location.state;
      if (formData) {
        this.setFormData(formData);
      } else {
        this.selectById(id);
      }
    }
  };

  selectById = id => {
    eventService
      .selectById(id)
      .then(this.onSuccess)
      .catch(this.onError);
  };

  goBack = () => {
    this.props.history.push("/admin/Event");
  };

  create = values => {
    eventService
      .create(values)
      .then(this.onSuccess)
      .catch(this.onError);
  };

  handleSubmit = values => {
    this.state.isEditing
      ? eventService
          .updateEvent(values)
          .then(this.onSuccessCreateUpdate)
          .catch(this.onError)
      : eventService
          .create(values)
          .then(this.onSuccessCreateUpdate)
          .catch(this.onError);
  };

  setFormData = data => {
    let formData = {
      id: data.id,
      eventTypeId: data.eventTypeId,
      name: data.name,
      summary: data.summary,
      shortDescription: data.shortDescription,
      venueId: data.venueId,
      eventStatusId: data.eventStatusId,
      imageUrl: data.imageUrl,
      externalSiteUrl: data.externalSiteUrl,
      isFree: data.isFree.toString(""),
      dateStart: new Date(data.dateStart),
      dateEnd: new Date(data.dateEnd),
      dateCreated: data.dateCreated,
      dateModified: data.dateModified
    };
    this.setState(prevState => {
      return {
        ...prevState,
        formData,
        isEditing: true
      };
    });
  };

  onSuccessCreateUpdate = () => {
    this.props.history.push("/admin/Event");
  };

  onSuccess = res => {
    this.setFormData(res.item);
  };

  onError = () => {
    toast.error("There is a problem with our servers...", {
      postition: toast.POSITION.TOP_LEFT
    });
  };

  render() {
    return (
      <React.Fragment>
        <Formik
          enableReinitialize={true}
          validationSchema={ValidationSchema}
          initialValues={this.state.formData}
          onSubmit={this.handleSubmit}
          isInitialValid={this.state.isEditing}
        >
          {props => {
            const {
              values,
              touched,
              errors,
              handleSubmit,
              isValid,
              isSubmitting,
              isEditing,
              setFieldValue
            } = props;
            return (
              <Form
                onSubmit={handleSubmit}
                className={"col-md-6 offset-3 pt-4"}
              >
                <div>
                  <h5>{this.state.isEditing ? "Edit Event" : "Add Event"}</h5>
                </div>
                <FormGroup>
                  <Label>Name</Label>
                  <Field
                    name="name"
                    type="text"
                    values={values.name}
                    placeholder="Name"
                    autoComplete="off"
                    className="form-control"
                  />
                  {errors.name && touched.name && (
                    <span className="input-feedback">{errors.name}</span>
                  )}
                </FormGroup>
                <FormGroup>
                  <Label>Summary</Label>
                  <Field
                    name="summary"
                    type="text"
                    values={values.summary}
                    placeholder="Summary"
                    autoComplete="off"
                    className="form-control"
                  />
                  {errors.summary && touched.summary && (
                    <span className="input-feedback">{errors.summary}</span>
                  )}
                </FormGroup>
                <FormGroup>
                  <Label>Short Description</Label>
                  <Field
                    name="shortDescription"
                    type="text"
                    values={values.shortDescription}
                    placeholder="Short Description"
                    autoComplete="off"
                    className="form-control"
                  />
                  {errors.shortDescription && touched.shortDescription && (
                    <span className="input-feedback">
                      {errors.shortDescription}
                    </span>
                  )}
                </FormGroup>
                <FormGroup>
                  <Label>External Site Url</Label>
                  <Field
                    name="externalSiteUrl"
                    type="text"
                    values={values.externalSiteUrl}
                    placeholder="External Site Url"
                    autoComplete="off"
                    className="form-control"
                  />
                  {errors.externalSiteUrl && touched.externalSiteUrl && (
                    <span className="input-feedback">
                      {errors.externalSiteUrl}
                    </span>
                  )}
                </FormGroup>
                <FormGroup>
                  <Label>Event Type</Label>
                  <Field
                    name="eventTypeId"
                    component="select"
                    values={values.eventTypeId}
                    label="Event Type"
                    className="form-control"
                    as="select"
                  >
                    <option value="">Select Status</option>
                    <option value="1">Workshop</option>
                    <option value="2">Meetup</option>
                    <option value="3">Career Fair</option>
                    <option value="4">Panel Discussion</option>
                  </Field>
                  {errors.eventTypeId && touched.eventTypeId && (
                    <span className="input-feedback">{errors.eventTypeId}</span>
                  )}
                </FormGroup>
                <FormGroup>
                  <Label>Venue</Label>
                  <Field
                    name="venueId"
                    component="select"
                    values={values.venueId}
                    label="Venue"
                    className="form-control"
                    as="select"
                  >
                    <option value="">Select Status</option>
                    <option value="1">Export-Import Bank of the US</option>
                    <option value="2">Los Angeles Chamber of Commerce</option>
                    <option value="3">
                      Los Angeles Regional Export Council
                    </option>
                    <option value="4">Los Angeles Law Library</option>
                    <option value="5">Boyle Heights Chamber Of Commerce</option>
                    <option value="6">
                      The Valley International Trade Association
                    </option>
                    <option value="7">El Camino College</option>
                  </Field>
                  {errors.venueId && touched.venueId && (
                    <span className="input-feedback">{errors.venueId}</span>
                  )}
                </FormGroup>
                <FormGroup>
                  <Label>Event Status</Label>
                  <Field
                    name="eventStatusId"
                    component="select"
                    values={values.eventStatusId}
                    label="Event Status"
                    className="form-control"
                    as="select"
                  >
                    <option value="">Select Status</option>
                    <option value="1">Active</option>
                    <option value="2">InActive</option>
                  </Field>
                  {errors.eventStatusId && touched.eventStatusId && (
                    <span className="input-feedback">
                      {errors.eventStatusId}
                    </span>
                  )}
                </FormGroup>
                <FormGroup>
                  <Label>Is this event free?</Label>
                  <Field
                    name="isFree"
                    component="select"
                    values={values.isFree}
                    label="Is Free"
                    className="form-control"
                    as="select"
                  >
                    <option>Select Status</option>
                    <option value="false">No</option>
                    <option value="true">Yes</option>
                  </Field>
                  {errors.isFree && touched.isFree && (
                    <span className="input-feedback">{errors.isFree}</span>
                  )}
                </FormGroup>
                <FormGroup>
                  <Label>Date Start:</Label>
                  <DatePicker
                    name="dateStart"
                    selected={values.dateStart}
                    showTimeSelect
                    timeFormat="hh:mm aa"
                    timeCaption="time"
                    value={values.dateStart}
                    dateFormat="MMMM d, yyyy hh:mm aa"
                    onChange={Date => setFieldValue("dateStart", Date)}
                    className="form-control"
                  />
                  {errors.dateStart && touched.dateStart && (
                    <span className="input-feedback">{errors.dateStart}</span>
                  )}
                </FormGroup>
                <FormGroup>
                  <Label>Date End:</Label>
                  <DatePicker
                    name="dateEnd"
                    selected={values.dateEnd}
                    showTimeSelect
                    timeFormat="hh:mm aa"
                    timeCaption="time"
                    value={values.dateEnd}
                    dateFormat="MMMM d, yyyy hh:mm aa"
                    onChange={Date => setFieldValue("dateEnd", Date)}
                    className="form-control"
                  />
                  {errors.dateEnd && touched.dateEnd && (
                    <span className="input-feedback">{errors.dateEnd}</span>
                  )}
                </FormGroup>
                <FormGroup>
                  <FileUpload
                    onSuccess={res => {
                      setFieldValue("imageUrl", res[0]);
                    }}
                  />
                </FormGroup>
                <button
                  type="submit"
                  disabled={!isValid || isSubmitting || isEditing}
                  className="btn btn-pill btn-secondary"
                >
                  {this.state.isEditing ? "Update" : "Submit"}
                </button>
                {"  "}
                <button
                  type="button"
                  className="btn btn-pill btn-danger"
                  onClick={this.goBack}
                >
                  Go Back
                </button>
              </Form>
            );
          }}
        </Formik>
      </React.Fragment>
    );
  }
}
EventForm.propTypes = {
  event: PropTypes.shape({
    id: PropTypes.number.isRequired,
    eventTypeId: PropTypes.number.isRequired,
    name: PropTypes.string.isRequired,
    summary: PropTypes.string.isRequired,
    shortDescription: PropTypes.string.isRequired,
    venueId: PropTypes.number.isRequired,
    eventStatusId: PropTypes.number.isRequired,
    eventImage: PropTypes.shape({
      fileId: PropTypes.number
    }),
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
      eventImage: PropTypes.shape({
        fileId: PropTypes.number
      }),
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

export default EventForm;
