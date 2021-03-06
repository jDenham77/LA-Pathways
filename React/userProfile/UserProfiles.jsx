import React, { Component } from "react";
import UserProfilesCard from "./UserProfilesCard";
import * as UserProfilesServices from "../../services/userProfilesServices";
import Pagination from "rc-pagination";
import "rc-pagination/assets/index.css";
import PropTypes from "prop-types";
import { Input } from "reactstrap";
import UserPanel from "../common/sidebar/UserPanel";
import { toast } from "react-toastify";

export default class UserProfiles extends Component {
  constructor(props) {
    super(props);
    this.state = {
      userProfile: [],
      current: 0,
      totalCount: 0,
      pageSize: 6
    };
  }

  componentDidMount() {
    this.getPaginated(this.state.current, this.state.pageSize);
  }

  getPaginated = (pageIndex, pageSize) => {
    UserProfilesServices.getPaginated(pageIndex, pageSize)
      .then(this.getPaginatedSuccess)
      .catch(this.getPaginatedError);
  };

  getPaginatedSuccess = data => {
    const pageItems = data.item.pagedItems;
    this.setState(prevState => {
      return {
        ...prevState,
        userProfile: pageItems,
        totalCount: data.item.totalCount
      };
    });
  };

  getPaginatedError = error => {
    toast.error(error.message);
  };

  onPageChange = page => {
    this.setState(
      prevState => {
        return {
          ...prevState,
          current: page - 1
        };
      },
      () => {
        this.getPaginated(page - 1, this.state.pageSize);
      }
    );
  };

  editUser = user => {
    this.props.history.push(`/admin/users/profile/${user.id}/edit`, user);
  };

  userProfileMap = user => (
    <UserProfilesCard editUser={this.editUser} key={user.id} user={user} />
  );

  UserPanel = panel => (
    <UserPanel
      key={panel.id}
      panel={panel}
      currentUser={this.state.userProfile}
    />
  );

  changePagesize = e => {
    const pageSize = parseInt(e.target.value);
    this.setState(prevState => {
      return {
        ...prevState,
        pageSize
      };
    });
    this.getPaginated(this.state.current, pageSize);
  };

  render() {
    return (
      <>
        <div className="row">
          <div className="m-3">
            <Input
              type="select"
              onChange={this.changePagesize}
              value={this.state.pageSize}
            >
              <option value={6}>Page Size</option>
              <option value={12}>12</option>
              <option value={18}>18</option>
              <option value={24}>24</option>
            </Input>
          </div>
        </div>

        <div className="row">
          {this.state.userProfile.length > 0
            ? this.state.userProfile.map(this.userProfileMap)
            : "No User Found..."}
        </div>

        <div className="row">
          <Pagination
            className="pb-3 userProfilesPagination"
            pageSize={this.state.pageSize}
            onChange={this.onPageChange}
            current={this.state.current + 1}
            total={this.state.totalCount}
            showTitle={false}
          />
        </div>
      </>
    );
  }
}

UserProfiles.propTypes = {
  history: PropTypes.shape({
    push: PropTypes.func
  })
};
