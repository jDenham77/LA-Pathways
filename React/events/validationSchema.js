import * as Yup from "yup";

const ValidationSchema = Yup.object().shape({
  name: Yup.string()
    .min(2, "Please enter a name with at least 2 characters.")
    .max(255, "You have reached max capacity")
    .required("Required"),
  summary: Yup.string()
    .min(2, "Please enter a name with at least 2 characters.")
    .max(255, "You have reached max capacity")
    .required("Required"),
  shortDescription: Yup.string()
    .min(2, "Please enter a name with at least 2 characters.")
    .max(4000, "You have reached max capacity")
    .required("Required"),
  externalSiteUrl: Yup.string()
    .min(2, "Please enter a name with at least 2 characters.")
    .max(400, "You have reached max capacity")
    .required("Required"),
  eventTypeId: Yup.number()
    .min(1, "Please enter a number greater than 1.")
    .max(100, "Please enter a value from 2-100.")
    .required("Required"),
  venueId: Yup.number()
    .min(1, "Please enter a number greater than 1.")
    .max(100, "Please enter a value from 2-100.")
    .required("Required"),
  eventStatusId: Yup.number()
    .min(1, "Please enter a number greater than 1.")
    .max(100, "Please enter a value from 2-100.")
    .required("Required"),
  isFree: Yup.bool()
    .nullable()
    .required("Required"),
  dateStart: Yup.date().required("Required"),
  dateEnd: Yup.date().required("Required")
});

export default ValidationSchema;
