import * as Yup from "yup";

const registrationValidationSchema = Yup.object().shape({
  email: Yup.string()
    .min(1)
    .max(100)
    .required("Please enter a valid email type."),
  password: Yup.string()
    .min(
      8,
      "Please enter a password with at least 8 characters, one upper case and 1 special character"
    )
    .max(100)
    .required("Please enter a valid password."),
  confirmPassword: Yup.string()
    .min(8, "Please repeat password")
    .max(100)
    .required("Please re-enter password")
    .test("passwords-match", "Passwords must match", function(value) {
      return this.parent.password === value;
    })
});

export default registrationValidationSchema;
