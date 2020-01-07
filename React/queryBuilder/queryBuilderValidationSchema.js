import * as Yup from "yup";

const recommendationsValidationSchema = Yup.object().shape({
  resourceId: Yup.string()
    .min(3)
    .max(30)
    .required("Please choose a resource."),
  condition: Yup.string()
    .min(4)
    .max(10000)
    .required("Please enter a valid condition with at least 4 characters."),
  query: Yup.string()
    .min(4)
    .max(10000)
    .required("Please enter a valid condition with at least 4 characters.")
});

export default { recommendationsValidationSchema };
