import React from "react";
import * as resourcesService from "../../services/resourcesService";
import ResourcesCard from "./ResourcesCard";
import Logger from "sabio-debug";
import Pagination from "rc-pagination";
import { Input } from "reactstrap";
import "rc-pagination/assets/index.css";
import PropTypes from "prop-types";
import swal from "sweetalert";
import "./resourcesAdminCSS.css";

const _Logger = Logger.extend("Resources");

export default class Resources extends React.Component {
  constructor(props) {
    super(props);
    this.state = {
      current: 0,
      pageSize: 12,
      pageIndex: 0,
      totalCount: 0,
      query: ""
    };
  }

  componentDidMount() {
    resourcesService
      .pagination(this.state.current)
      .then(this.onGetSuccess)
      .catch(this.onGetError);
  }

  getResources = pageIndex => {
    resourcesService
      .pagination(pageIndex)
      .then(this.onGetSuccess)
      .catch(this.onGetError);
  };

  onGetSuccess = data => {
    let resources = data.item.pagedItems;
    this.setState(prevState => {
      return {
        ...prevState,
        resources,
        totalCount: data.item.totalCount
      };
    });
  };

  onGetError = data => {
    _Logger(data);
  };

  mapResources = resource => (
    <ResourcesCard
      deleteResource={this.deleteResource}
      routeToForm={this.routeToForm}
      resource={resource}
      key={resource.id}
      viewResource={this.viewResource}
    />
  );

  changePage = page => {
    let currentPage = page - 1;
    this.setState(prevState => {
      return {
        ...prevState,
        current: currentPage
      };
    }, this.getResources(currentPage));
  };

  routeToForm = resource => {
    this.props.history.push(`/admin/resource/form/${resource.id}/edit`, resource);
  };

  createResource = () => {
    this.props.history.push(`/admin/resource/form/create`);
  };

  viewResource = resource => {
    this.props.history.push(`/admin/resource/${resource.id}/details`, resource);
  };

  deleteResource = resource => {
    swal({
      title: "Are you sure?",
      text: "Once deleted, you will not be able to recover this file.",
      icon: "warning",
      buttons: true,
      dangerMode: true
    }).then(willDelete => {
      if (willDelete) {
        let id = resource.id;
        resourcesService
          .deleteResource(id)
          .then(this.delSuccess)
          .catch(this.delError);
        swal("File Deleted", {
          icon: "success"
        });
      } else {
        swal("Resource file has beeen saved.");
      }
    });
  };

  delSuccess = id => {
    this.setState(prevState => {
      let resources = prevState.resources.filter(activeResources => activeResources.id !== id);
      return {
        ...prevState,
        resources,
        mappedResources: resources.map(this.mapResources)
      };
    });
  };

  delError = () => {
    _Logger("Delete Error");
  };

  searchResources = query => {
    resourcesService
      .search(this.state.pageIndex, query)
      .then(this.searchSuccess)
      .catch(this.searchError);
  };

  searchSuccess = data => {
    let resources = data.item.pagedItems;
    this.setState({
      resources,
      totalCount: data.item.totalCount
    });
  };

  searchError = data => {
    _Logger(data);
  };

  onSearch = e => {
    let value = e.target.value;
    this.setState(prevState => {
      return {
        ...prevState,
        query: value
      };
    });
  };

  clearSearch = () => {
    this.setState(prevState => {
      return {
        ...prevState,
        query: ""
      };
    });
    this.getResources(0);
  };

  search = () => {
    if (this.state.query.length > 0 ? this.searchResources(this.state.query) : 0);
  };

  dataTableView = () => {
    this.props.history.push("/admin/resourcestable");
  };

  dataTableView = () => {
    this.props.history.push("/admin/resourcestable");
  };

  render() {
    return (
      <React.Fragment>
        <div className="d-flex justify-content-left">
          <button type="button" className=" btn btn-lg btn-pill btn-primary" id="addResourceBtn" onClick={this.createResource}>
            Add Resource
          </button>
        </div>
        <div>
          <button type="button" className=" btn btn-lg btn-primary btn-pill" id="dataTableView" onClick={this.dataTableView}>
            Table View
          </button>
        </div>
        <div className="d-flex justify-content-center" id="searchBar">
          <Input
            type="text"
            value={this.state.query}
            onChange={this.onSearch}
            className="search-box form-control col-md-3"
            placeholder="Search Resource"
          />
          <button className="btn btn-primary btn-pill ml-2" onClick={this.search}>
            Search
          </button>
          <button className="btn btn-secondary btn-pill ml-2" onClick={this.clearSearch}>
            Clear
          </button>
        </div>
        <div className="col-md-12  card-columns" id="columnCards">
          {this.state.resources ? this.state.resources.map(this.mapResources) : "No Resources"}
        </div>
        <div className="d-flex justify-content-center">
          <Pagination
            id="pagination"
            onChange={this.changePage}
            current={this.state.current + 1}
            total={this.state.totalCount}
            pageSize={this.state.pageSize}
          />
        </div>
      </React.Fragment>
    );
  }
}
Resources.propTypes = {
  history: PropTypes.shape({
    push: PropTypes.func
  })
};
