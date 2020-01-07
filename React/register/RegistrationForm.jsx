import React from "react";
import { Form, Label, Button, FormGroup, InputGroup } from "reactstrap";
import { Formik, Field } from "formik";
import PropTypes from "prop-types";
import registrationValidationSchema from "./registrationValidation";
import * as userServices from "../../services/userServices";
import swal from "sweetalert";
import Auth from "./../../assets/images/auth-layer.png";

class RegistrationForm extends React.Component {
  formikRef = React.createRef();
  state = {
    formData: {
      email: "",
      password: "",
      confirmPassword: ""
    }
  };

  handleSubmit = values => {
    userServices
      .register(values)
      .then(this.onSuccess)
      .catch(this.onError);
  };

  onSuccess = () => {
    swal("Success!", "You have successfully registered.", "success");
      this.props.history.push("/admin/users/profiles");
  };
  onError = () => {
    swal("That's unfortunate...", "Something went wrong.", "error");
   
    this.formikRef.current.setSubmitting(false);
  };

  render() {
    return (
      <React.Fragment>
        <Formik
          ref={this.formikRef}
          enableReinitialize={true}
          validationSchema={registrationValidationSchema}
          initialValues={this.state.formData}
          onSubmit={this.handleSubmit}
        >
          {props => {
            const {
              values,
              touched,
              errors,
              handleSubmit,
              isValid,
              isSubmitting,
              setFieldValue
            } = props;
            return (
              <div id="root">
                <div>
                  <div className="loader-wrapper" style={{ display: "none" }}>
                    <div className="loader bg-white">
                      <div className="line" />
                      <div className="line" />
                      <div className="line" />
                      <div className="line" />
                      <h4>
                        Have a great day at work today <span>☺</span>
                      </h4>
                    </div>
                  </div>
                  <div className="page-wrapper">
                    <div className="container-fluid">
                      <div className="authentication-main">
                        <div className="row">
                          <div className="col-md-4 p-0">
                            <div
                              className="auth-innerleft"
                              style={{
                                backgroundImage: "url(" + Auth + ")"
                              }}
                            >
                              <div className="text-center">
                                <h1>LA PATHWAYS</h1>
                                <hr />
                              </div>
                            </div>
                          </div>
                          <div className="col-md-8 p-0">
                            <div className="auth-innerright">
                              <div className="authentication-box">
                                <h4>REGISTER</h4>
                                <h6>Enter your Information</h6>
                                <div className="card mt-4 p-4 mb-0">
                                  <Form onSubmit={handleSubmit}>
                                    <FormGroup>
                                      <Label className="col-form-label">
                                        Email
                                      </Label>
                                      <Field
                                        required
                                        name="email"
                                        type="email"
                                        values={values.email}
                                        placeholder="Enter Email Address"
                                        className={
                                          errors.email && touched.email
                                            ? "form-control errorMessage"
                                            : "form-control"
                                        }
                                      />
                                      {errors.email && touched.email && (
                                        <span className="input-feedback">
                                          {errors.email}
                                        </span>
                                      )}
                                    </FormGroup>
                                    <FormGroup>
                                      <Label className="col-form-label">
                                        Password
                                      </Label>
                                      <Field
                                        required
                                        name="password"
                                        type="password"
                                        values={values.password}
                                        placeholder="Enter Password"
                                        autoComplete="off"
                                        className={
                                          errors.password && touched.password
                                            ? "form-control errorMessage"
                                            : "form-control"
                                        }
                                      />
                                      {errors.password && touched.password && (
                                        <span className="input-feedback">
                                          {errors.password}
                                        </span>
                                      )}
                                    </FormGroup>
                                    <FormGroup>
                                      <Label className="col-form-label">
                                        Confirm Password
                                      </Label>
                                      <Field
                                        required
                                        name="confirmPassword"
                                        type="password"
                                        values={values.confirmPassword}
                                        placeholder="Confirm Password"
                                        autoComplete="off"
                                        className={
                                          errors.confirmPassword &&
                                          touched.confirmPassword
                                            ? "form-control errorMessage"
                                            : "form-control"
                                        }
                                      />
                                      {errors.confirmPassword &&
                                        touched.confirmPassword && (
                                          <span className="input-feedback">
                                            {errors.confirmPassword}
                                          </span>
                                        )}
                                    </FormGroup>
                                    <InputGroup className="mt-4">
                                      <Label>Admin</Label>
                                      <input
                                        selected
                                        name="roleId"
                                        className="ml-2"
                                        type="radio"
                                        value="1"
                                        onChange={e =>
                                          setFieldValue(
                                            "roleId",
                                            e.target.value
                                          )
                                        }
                                      />
                                      <Label className="ml-3">Mentor</Label>
                                      <input
                                        name="roleId"
                                        className="ml-2"
                                        type="radio"
                                        value="3"
                                        onChange={e =>
                                          setFieldValue(
                                            "roleId",
                                            e.target.value
                                          )
                                        }
                                      />
                                      <Label className="ml-3">Mentee</Label>
                                      <input
                                        name="roleId"
                                        className="ml-2"
                                        type="radio"
                                        value="4"
                                        onChange={e =>
                                          setFieldValue(
                                            "roleId",
                                            e.target.value
                                          )
                                        }
                                      />
                                    </InputGroup>
                                    <div className="col-md-3 mt-3">
                                      <Button
                                        type="submit"
                                        disabled={!isValid || isSubmitting}
                                        className="btn btn-secondary"
                                      >
                                        Submit
                                      </Button>
                                    </div>
                                  </Form>
                                </div>
                              </div>
                            </div>
                          </div>
                        </div>
                      </div>
                    </div>
                  </div>
                </div>
              </div>
            );
          }}
        </Formik>
      </React.Fragment>
    );
  }
}

RegistrationForm.propTypes = {
  state: PropTypes.shape({
    email: PropTypes.string,
    password: PropTypes.string,
    confirmPassword: PropTypes.string
  }),
  history: PropTypes.shape({
    push: PropTypes.func
  })
};

export default RegistrationForm;
