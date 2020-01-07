import React from "react";
import { Form, FormGroup, Label } from "reactstrap";
import { Formik, Field } from "formik";
import resourceValidationSchema from "./ResourceValidationSchema";
import { Typeahead } from "react-bootstrap-typeahead";
import "react-bootstrap-typeahead/css/Typeahead.css";
import { toast } from "react-toastify";
import * as resourcesService from "../../services/resourcesService";
import PropTypes from "prop-types";
import "./resourcesAdminCSS.css";
import "pretty-checkbox";

class ResourceForm extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      isEditing: false,
      formData: {
        name: "",
        headline: "",
        description: "",
        logo: "",
        locationId: "",
        contactName: "",
        contactEmail: "",
        phone: "",
        siteUrl: "",
        specialTopicsTypes: [],
        industryTypes: [],
        demographicTypes: [],
        contractingTypes: 0,
        consultingTypes: 0,
        locationZoneTypes: 0,
        capitalTypes: [],
        complianceTypes: []
      }
    };
  }

  componentDidMount() {
    resourcesService
      .getResourceTypes()
      .then(this.successGetTypes)
      .catch(this.errorGetTypes);
    resourcesService
      .getLocationOptions()
      .then(this.onLocationOptionSuccess)
      .catch(this.axiosFail);
    const { id } = this.props.match.params;
    if (id) {
      let formData = this.props.location.state;
      if (formData) {
        this.setFormData(formData);
      } else {
        this.getById(id);
      }
    }
  }

  successGetTypes = data => {
    let capitalTypes = data.item.capitalTypes;
    let locationZoneTypes = data.item.locationTypes;
    let complianceTypes = data.item.complianceTypes;
    let consultingTypes = data.item.consultingTypes;
    let contractingTypes = data.item.contractingTypes;
    let demographicTypes = data.item.demographicTypes;
    let industryTypes = data.item.industryTypes;
    let specialTopicsTypes = data.item.specialTopicsTypes;

    this.setState(prevState => {
      return {
        ...prevState,
        categoriesTypes: {
          specialTopicsTypes,
          industryTypes,
          demographicTypes,
          contractingTypes,
          consultingTypes,
          locationZoneTypes,
          capitalTypes,
          complianceTypes
        }
      };
    });
  };

  errorGetTypes = error => {
    toast.error(error.message);
  };

  getById = id => {
    resourcesService
      .getById(id)
      .then(this.onGetByIdSuccess)
      .catch(this.onGetByIdError);
  };

  onGetByIdSuccess = result => {
    this.setFormData(result.item);
  };

  setFormData = data => {
    let formData = {
      id: data.id,
      name: data.name,
      headline: data.headline,
      description: data.description,
      logo: data.logo,
      locationId: data.locationId,
      contactName: data.contactName,
      contactEmail: data.contactEmail,
      phone: data.phone,
      siteUrl: data.siteUrl
    };
    this.setState(prevState => {
      return {
        ...prevState,
        formData,
        isEditing: true
      };
    });
  };

  getCats = (key, array) => {
    return array.reduce((objectsByKeyValue, obj) => {
      const value = obj[key];
      objectsByKeyValue[value] = (objectsByKeyValue[value] || []).concat(obj);
      delete obj[key];
      return objectsByKeyValue;
    }, {});
  };
  onGetByIdError = error => {
    toast.error(error.message);
  };

  handleSubmit = values => {
    values.locationId = this.state.formData.locationId;
    this.state.isEditing
      ? resourcesService
          .update(values)
          .then(this.onSuccessEdit)
          .catch(this.onError)
      : resourcesService
          .add(values)
          .then(this.OnSuccessRouteToResources)
          .catch(this.onError);
  };

  onError = error => {
    toast.error(error.message);
  };

  onSuccessEdit = () => {
    this.props.history.push(`/admin/resources`);
  };

  onSuccessAdd = () => {
    this.props.history.push(`/admin/resources`);
  };

  OnSuccessRouteToResources = () => {
    this.props.history.push(`/admin/resources`);
  };

  getOptions = option => {
    return option.length > 0 ? option.map(item => item.id) : [];
  };

  axiosFail = error => {
    toast.error(error.message);
  };

  onLocationOptionSuccess = data => {
    this.setState(prevState => {
      return {
        ...prevState,
        locationOptions: data.item.map(this.optionMapper)
      };
    });
  };

  optionMapper = location => {
    let obj = {
      value: location.id,
      id: location.id,
      label: location.name
    };
    return obj;
  };

  locationMapper = location => {
    return {
      key: location.id,
      value: location.id,
      label: location.locationText
    };
  };

  onLocationIdSelect = optionArr => {
    if (optionArr[0] !== undefined) {
      this.setState(prevState => {
        return {
          ...prevState,
          formData: {
            ...prevState.formData,
            locationId: optionArr[0].value
          }
        };
      });
    } else {
      this.setState(prevState => {
        return {
          ...prevState,
          formData: {
            ...prevState.formData,
            locationId: false
          }
        };
      });
    }
  };

  goToResources = () => {
    this.props.history.push(`/admin/resources`);
  };

  render() {
    return (
      <React.Fragment>
        {(this.state.categoriesTypes && this.state.locationOptions && !this.state.isEditing) ||
        (this.state.categoriesTypes && this.state.locationOptions) ? (
          <Formik
            enableReinitialize={true}
            validationSchema={resourceValidationSchema}
            initialValues={this.state.formData}
            onSubmit={this.handleSubmit}
            isInitialValid={this.state.isEditing}
          >
            {props => {
              const { values, touched, errors, handleSubmit, isValid, setFieldValue } = props;
              return (
                <Form onSubmit={handleSubmit}>
                  <div></div>
                  <div className="card" id="formCard">
                    <h2 id="formTitle">{this.state.isEditing ? "Edit Resource" : "Add Resource"}</h2>
                    <div className="row">
                      <div className="column col-md-6" id="leftCard">
                        <FormGroup>
                          <Label>
                            <strong>Name</strong>
                          </Label>
                          <Field
                            name="name"
                            type="text"
                            values={this.state.formData.name}
                            placeholder="Name"
                            autoComplete="off"
                            className={!errors.name && touched.name ? "form-control" : "form-control"}
                          />
                          {errors.name && touched.name && <span className="input-feedback">{errors.name}</span>}
                        </FormGroup>
                        <FormGroup>
                          <Label>
                            <strong>Headline</strong>
                          </Label>
                          <Field
                            name="headline"
                            type="text"
                            values={this.state.formData.headline}
                            placeholder="Headline"
                            autoComplete="off"
                            className={!errors.headline && touched.headline ? "form-control" : "form-control"}
                          />
                          {errors.headline && touched.headline && <span className="input-feedback">{errors.headline}</span>}
                        </FormGroup>
                        <FormGroup>
                          <Label>
                            <strong>Description</strong>
                          </Label>
                          <Field
                            name="description"
                            type="text"
                            values={this.state.formData.description}
                            placeholder="Description"
                            autoComplete="off"
                            className={!errors.description && touched.description ? "form-control" : "form-control"}
                          />
                          {errors.description && touched.description && <span className="input-feedback">{errors.description}</span>}
                        </FormGroup>
                        <FormGroup>
                          <Label>
                            <strong>Logo</strong>
                          </Label>
                          <Field
                            name="logo"
                            type="text"
                            values={this.state.formData.logo}
                            placeholder="Logo"
                            autoComplete="off"
                            className={!errors.logo && touched.logo ? "form-control" : "form-control"}
                          />
                          {errors.logo && touched.logo && <span className="input-feedback">{errors.logo}</span>}
                        </FormGroup>
                        <FormGroup>
                          <Label>
                            <strong>Location</strong>
                          </Label>
                          <Typeahead
                            name="locationId"
                            type="text"
                            id="a9"
                            options={this.state.locationOptions}
                            onChange={this.onLocationIdSelect}
                            values={this.state.formData.locationId}
                            placeholder="Location..."
                            maxHeight={
                              this.state.isEditing && this.state.formData.locationId !== undefined
                                ? this.state.locationOptions.filter(location => location.id === this.state.formData.locationId)
                                : []
                            }
                          />
                          {this.state.formData.locationId === false ? (
                            <span className="input-feedback">Please enter a location.</span>
                          ) : (
                            <span></span>
                          )}
                        </FormGroup>
                        <FormGroup>
                          <Label>
                            <strong>Contact Name</strong>
                          </Label>
                          <Field
                            name="contactName"
                            type="text"
                            values={this.state.formData.contactName}
                            placeholder="Contact Name"
                            autoComplete="off"
                            className={!errors.contactName && touched.contactName ? "form-control" : "form-control"}
                          />
                          {errors.contactName && touched.contactName && <span className="input-feedback">{errors.contactName}</span>}
                        </FormGroup>
                        <FormGroup>
                          <Label>
                            <strong>Contact Email</strong>
                          </Label>
                          <Field
                            name="contactEmail"
                            type="text"
                            values={this.state.formData.contactEmail}
                            placeholder="Contact Email"
                            autoComplete="off"
                            className={!errors.contactEmail && touched.contactEmail ? "form-control" : "form-control"}
                          />
                          {errors.contactEmail && touched.contactEmail && <span className="input-feedback">{errors.contactEmail}</span>}
                        </FormGroup>
                        <FormGroup>
                          <Label>
                            <strong>Phone Number</strong>
                          </Label>
                          <Field
                            name="phone"
                            type="text"
                            values={this.state.formData.phone}
                            placeholder="Phone Number"
                            autoComplete="off"
                            className={!errors.phone && touched.phone ? "form-control" : "form-control"}
                          />
                          {errors.phone && touched.phone && <span className="input-feedback">{errors.phone}</span>}
                        </FormGroup>
                        <FormGroup>
                          <Label>
                            <strong>Website</strong>
                          </Label>
                          <Field
                            name="siteUrl"
                            type="text"
                            values={this.state.formData.siteUrl}
                            placeholder="Enter Website"
                            autoComplete="off"
                            className={!errors.siteUrl && touched.siteUrl ? "form-control" : "form-control"}
                          />
                          {errors.siteUrl && touched.siteUrl && <span className="input-feedback">{errors.siteUrl}</span>}
                        </FormGroup>
                      </div>
                      <div className="column col-md-6" id="rightCard">
                        <div className="card-columns" id="centerContent">
                          <FormGroup>
                            <div className="card" id="sptAdjust">
                              <Label>
                                <strong>Special Topics</strong>
                              </Label>
                              <div>
                                <Typeahead
                                  name="specialTopicsTypes"
                                  type="text"
                                  multiple
                                  clearButton
                                  id="a1"
                                  labelKey={option => `${option.name}`}
                                  options={this.state.categoriesTypes.specialTopicsTypes}
                                  onChange={option => setFieldValue("specialTopicsTypes", this.getOptions(option))}
                                  values={values.specialTopicsTypes}
                                  placeholder="Special Topics Types..."
                                />
                              </div>
                            </div>
                          </FormGroup>
                          <FormGroup>
                            <div className="card" id="consulting">
                              <Label>
                                <strong>Consulting</strong>
                              </Label>
                              <div>
                                <Typeahead
                                  name="ConsultingTypes"
                                  type="text"
                                  id="a2"
                                  labelKey={option => `${option.name}`}
                                  options={this.state.categoriesTypes.consultingTypes}
                                  values={values.consultingTypes}
                                  placeholder="Consulting Topics Types..."
                                  onChange={option => setFieldValue("ConsultingTypes", option.length > 0 ? option[0].id : 0)}
                                />
                              </div>
                            </div>
                          </FormGroup>
                          <FormGroup>
                            <div className="card" id="location">
                              <Label>
                                <strong>Location</strong>
                              </Label>
                              <div>
                                <Typeahead
                                  name="locationZoneTypes"
                                  type="text"
                                  id="a3"
                                  clearButton
                                  onChange={option => setFieldValue("locationZoneTypes", option.length > 0 ? option[0].id : 0)}
                                  labelKey={option => `${option.name}`}
                                  options={this.state.categoriesTypes.locationZoneTypes}
                                  values={values.locationZoneTypes}
                                  placeholder="Location Zone Types..."
                                />
                              </div>
                            </div>
                          </FormGroup>
                          <FormGroup id="capital">
                            <div className="card" id="capital">
                              <Label>
                                <strong>Capital</strong>
                              </Label>
                              <div>
                                <Typeahead
                                  name="capitalTypes"
                                  type="text"
                                  clearButton
                                  id="a4"
                                  multiple
                                  labelKey={option => `${option.name}`}
                                  options={this.state.categoriesTypes.capitalTypes}
                                  values={values.capitalTypes}
                                  placeholder="Capital Types..."
                                  onChange={option => setFieldValue("capitalTypes", this.getOptions(option))}
                                />
                              </div>
                            </div>
                          </FormGroup>
                          <FormGroup>
                            <div className="card" id="compliance">
                              <Label>
                                <strong>Compliance</strong>
                              </Label>
                              <div>
                                <Typeahead
                                  name="complianceTypes"
                                  type="text"
                                  id="a5"
                                  clearButton
                                  multiple
                                  labelKey={option => `${option.name}`}
                                  options={this.state.categoriesTypes.complianceTypes}
                                  values={values.complianceTypes}
                                  placeholder="Compliance Types..."
                                  onChange={option => setFieldValue("complianceTypes", this.getOptions(option))}
                                />
                              </div>
                            </div>
                          </FormGroup>
                          <FormGroup id="industrySpecific">
                            <div className="card">
                              <Label>
                                <strong>Industry Specific</strong>
                              </Label>
                              <div>
                                <Typeahead
                                  name="industryTypes"
                                  type="text"
                                  clearButton
                                  id="a6"
                                  multiple
                                  labelKey={option => `${option.name}`}
                                  options={this.state.categoriesTypes.industryTypes}
                                  values={values.industryTypes}
                                  placeholder="Industry Types..."
                                  onChange={option => setFieldValue("industryTypes", this.getOptions(option))}
                                />
                              </div>
                            </div>
                          </FormGroup>
                          <FormGroup id="demoCats">
                            <div className="card">
                              <Label>
                                <strong>Demo Categories </strong>
                              </Label>
                              <div>
                                <Typeahead
                                  name="demographicTypes"
                                  type="text"
                                  clearButton
                                  id="a7"
                                  multiple
                                  labelKey={option => `${option.name}`}
                                  options={this.state.categoriesTypes.demographicTypes}
                                  onChange={option => setFieldValue("demographicTypes", this.getOptions(option))}
                                  values={values.demographicTypes}
                                  placeholder="Demographic Types..."
                                />
                              </div>
                            </div>
                          </FormGroup>
                          <FormGroup>
                            <div className="card" id="contracting">
                              <Label>
                                <strong>Contracting</strong>
                              </Label>
                              <div>
                                <Typeahead
                                  name="ContractingTypes"
                                  type="text"
                                  clearButton
                                  onChange={option => setFieldValue("ContractingTypes", option.length > 0 ? option[0].id : 0)}
                                  id="a8"
                                  labelKey={option => `${option.name}`}
                                  options={this.state.categoriesTypes.contractingTypes}
                                  values={values.contractingTypes}
                                  placeholder="Contracting Types..."
                                />
                              </div>
                            </div>
                          </FormGroup>
                        </div>
                        <button type="button" disabled={!isValid} className="btn btn-primary btn-pill" onClick={handleSubmit}>
                          {this.state.isEditing ? "Update" : "Submit"}
                        </button>
                        <button type="button" className="btn btn-secondary btn-pill ml-2" onClick={this.goToResources}>
                          Go Back
                        </button>
                      </div>
                    </div>
                  </div>
                </Form>
              );
            }}
          </Formik>
        ) : (
          ""
        )}
      </React.Fragment>
    );
  }
}
ResourceForm.propTypes = {
  match: PropTypes.shape({
    params: PropTypes.object
  }),
  location: PropTypes.shape({
    state: PropTypes.object
  }),
  history: PropTypes.shape({
    push: PropTypes.func
  })
};
export default ResourceForm;
