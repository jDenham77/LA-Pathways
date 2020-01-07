import * as Yup from "yup";

const userProfilesvalidationSchema = Yup.object().shape({
  firstName: Yup.string()
    .min(1, "Must be more than 5 characters.")
    .max(50, "Must be below 50 characters.")
    .required("Required"),
  lastName: Yup.string()
    .min(1, "Must be more than 1 characters.")
    .max(50, "Must be below 50 characters.")
    .required("Required"),
  mi: Yup.string().max(1, "Middle initial must be one character")
});

export default userProfilesvalidationSchema;
