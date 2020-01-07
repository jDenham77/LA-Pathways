import * as Yup from "yup";

const loginValidationSchema = Yup.object().shape({
  email: Yup.string()
    .min(2)
    .max(100)
    .required("Please enter a valid email type."),
  password: Yup.string()
    .min(2)
    .max(100)
    .required("Please enter a valid password.")
});

export default loginValidationSchema;
