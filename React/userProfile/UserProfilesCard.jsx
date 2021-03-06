import React from "react";
import PropTypes from "prop-types";
import "./UserProfileCss.css";

const UserProfilesCard = props => {
  const handleEditClick = () => {
    props.editUser(props.user);
  };

  const onImageError = e => {
    e.target.onerror = null;
    e.target.src = "https://bit.ly/2QWQhwp";
  };

  return (
    <div className="col-xl-4 col-lg-12" key={props.user.id}>
      <div className="card custom-card default-user-card">
        <div className="card-header">
          <img src="https://bit.ly/2XSehCt" className="img-fluid" alt="" />
        </div>
        <div className="card-profile">
          <img
            src={props.user.avatarUrl}
            onError={onImageError}
            className="rounded-circle"
            width="150"
            alt=""
          />
        </div>
        <ul className="card-social">
          <li>
            <i
              className="fa fa-pencil editPencil fa-lg"
              onClick={handleEditClick}
            ></i>
          </li>
        </ul>
        <div className="text-center profile-details">
          <h4 className="m-b-15 m-t-5">
            {props.user.firstName} {props.user.mi} {props.user.lastName}
          </h4>
        </div>
      </div>
    </div>
  );
};

UserProfilesCard.propTypes = {
  user: PropTypes.shape({
    id: PropTypes.number.isRequired,
    userId: PropTypes.number.isRequired,
    firstName: PropTypes.string.isRequired,
    lastName: PropTypes.string.isRequired,
    mi: PropTypes.string.isRequired,
    avatarUrl: PropTypes.string.isRequired,
    dateCreated: PropTypes.string.isRequired
  }),
  editUser: PropTypes.func
};

export default UserProfilesCard;
