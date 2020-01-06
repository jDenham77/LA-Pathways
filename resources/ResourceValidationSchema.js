import * as Yup from "yup";

const resourceValidationSchema = Yup.object().shape({
  name: Yup.string()
    .min(2)
    .max(200)
    .required("Please enter a valid name."),
  headline: Yup.string()
    .min(2)
    .max(2000)
    .required("Please enter a valid headline."),
  description: Yup.string()
    .min(2)
    .max(8000)
    .required("Please enter a valid description."),
  logo: Yup.string()
    .min(2)
    .max(1000)
    .required("Please enter a valid logo."),
  locationId: Yup.string()
    .min(1)
    .max(2)
    .required("Please enter a valid location Id."),
  contactName: Yup.string()
    .min(2)
    .max(100)
    .required("Please enter a valid contact name."),
  contactEmail: Yup.string()
    .min(2)
    .max(200)
    .required("Please enter a valid contact email."),
  phone: Yup.string()
    .min(7)
    .max(15)
    .required("Please enter a valid phone number."),
  siteUrl: Yup.string()
    .min(2)
    .max(200)
    .required("Please enter a valid website.")
});

export default resourceValidationSchema;
