import React from "react";
import PropTypes from "prop-types";

const ResourceCategories = props => {
  return (
    <ul>
      <li>
        {props.cats.code}-{props.cats.name}
      </li>
    </ul>
  );
};

ResourceCategories.propTypes = {
  cats: PropTypes.shape({
    name: PropTypes.string,
    headline: PropTypes.string,
    description: PropTypes.string,
    logo: PropTypes.string,
    locationId: PropTypes.number,
    contactName: PropTypes.string,
    contactEmail: PropTypes.string,
    phone: PropTypes.string,
    siteUrl: PropTypes.string,
    categoryType: PropTypes.string,
    code: PropTypes.string,
    baseCategoryType: PropTypes.arrayOf(
      PropTypes.shape({
        name: PropTypes.string,
        code: PropTypes.string,
        categoryType: PropTypes.string
      })
    )
  })
};
export default ResourceCategories;
