import React from "react";
import { Form, Label, Button, FormGroup } from "reactstrap"; //
import { Formik, Field } from "formik"; //
import PropTypes from "prop-types";
import loginValidationSchema from "./loginValidation";
import * as userServices from "../../services/userServices";
import swal from "sweetalert";
import "./Login.css";
import Auth from "./../../assets/images/auth-layer.png";

class LoginForm extends React.Component {
  formikRef = React.createRef();
  state = {
    formData: {
      email: "",
      password: ""
    }
  };

  handleSubmit = values => {
    userServices
      .login(values)
      .then(userServices.getCurrent)
      .then(this.onCurrentUserSuccess)
      .catch(this.onError);
  };

  onCurrentUserSuccess = response => {
    this.props.history.push("/admin/dashboard", {
      type: "login",
      user: response.item
    });
  };

  onError = () => {
    swal(
      "Error has occured.",
      "Either your name or password were entered incorrectly.",
      "error"
    );
    this.formikRef.current.setSubmitting(false);
  };
  onClickResetPassword = () => {
    this.props.history.push("/forgotpassword");
  };

  render() {
    return (
      <React.Fragment>
        <Formik
          ref={this.formikRef}
          enableReinitialize={true}
          validationSchema={loginValidationSchema}
          onSubmit={this.handleSubmit}
        >
          {props => {
            const {
              values,
              touched,
              errors,
              handleSubmit,
              isValid,
              isSubmitting
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
                          <div className="col-md-5 p-0">
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
                          <div className="col-md-7 p-0">
                            <div className="auth-innerright">
                              <div className="authentication-box">
                                <h4>LOGIN</h4>
                                <h6>
                                  Enter your Username and Password to Login
                                </h6>
                                <div className="card mt-4 p-4 mb-0">
                                  <Form onSubmit={handleSubmit}>
                                    <FormGroup className="form-group">
                                      <Label className="col-form-Label">
                                        Email
                                      </Label>
                                      <div className="form-group">
                                        <Field
                                          required
                                          type="email"
                                          name="email"
                                          autoComplete="on"
                                          values={values.email}
                                          placeholder="Enter Email"
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
                                        <p className="help-block"></p>
                                      </div>
                                    </FormGroup>
                                    <FormGroup>
                                      <Label className="col-form-label">
                                        Password
                                      </Label>
                                      <div className="form-group">
                                        <Field
                                          autoComplete="on"
                                          type="password"
                                          name="password"
                                          values={values.password}
                                          required
                                          placeholder="**********"
                                          className={
                                            errors.password && touched.password
                                              ? "form-control errorMessage"
                                              : "form-control"
                                          }
                                        />
                                        {errors.password &&
                                          touched.password && (
                                            <span className="input-feedback">
                                              {errors.password}
                                            </span>
                                          )}
                                      </div>
                                    </FormGroup>
                                    <div className="row">
                                      <div className="col-md-6">
                                        <Button
                                          type="submit"
                                          disabled={!isValid || isSubmitting}
                                          className="btn btn-pill btn-secondary"
                                        >
                                          LOGIN
                                        </Button>
                                      </div>
                                      <div className="col-md-6">
                                        <button
                                          className="btn btn-pill btn-primary"
                                          type="button"
                                          onClick={this.onClickResetPassword}
                                        >
                                          Forgot Password
                                        </button>
                                      </div>
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

LoginForm.propTypes = {
  getUser: PropTypes.func,
  state: PropTypes.shape({
    email: PropTypes.string,
    password: PropTypes.string
  }),
  history: PropTypes.shape({
    push: PropTypes.func
  })
};

export default LoginForm;
