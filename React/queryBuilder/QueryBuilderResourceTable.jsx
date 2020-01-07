import { Datatable } from "@o2xp/react-datatable";
import React, { Component } from "react";
import PropTypes from "prop-types";
import * as adminServices from "../../services/adminServices";
import "./recommendations.css";

class ResourcesTable extends Component {
  constructor(props) {
    super(props);
    this.state = {
      resources: [],
      options: {
        title: "Resource Queries",
        keyColumn: "resourceName",
        font: "Arial",
        data: {
          columns: [
            {
              id: "resourceName",
              label: "Resource Name",
              colSize: "75px",
              editable: false,
              dataType: "text",
              inputType: "input"
            },
            {
              id: "query",
              label: "Query",
              colSize: "75px",
              editable: false
            },
            {
              id: "edit",
              label: "Edit",
              colSize: "25px",
              editable: false
            }
          ],
          rows: []
        },
        features: {
          canDelete: false,
          canPrint: true,
          canDownload: true,
          canSearch: true,
          canRefreshRows: false,
          canOrderColumns: true,
          canSelectRow: true,
          canSaveUserConfiguration: false,
          userConfiguration: {
            columnsOrder: ["resourceName", "query", "edit"],
            copyToClipboard: false
          },
          rowsPerPage: {
            available: [10, 25, 50, 100],
            selected: 50
          },
          additionalIcons: [
            {
              title: "Create new query",
              icon: <i className="fas fa-plus-square fa"></i>,
              onClick: this.createQuery
            }
          ]
        }
      }
    };
  }

  componentDidMount() {
    adminServices
      .getAllRecources()
      .then(this.getResourcesSucc)
      .catch(this.getResourcesErr);
  }
  getResourcesSucc = data => {
    let values = data.items.map(this.mapResource);
    this.setState(prevState => {
      return {
        ...prevState,
        resources: data.items,
        options: {
          ...prevState.options,
          data: {
            ...prevState.options.data,
            rows: values
          }
        }
      };
    });
  };

  mapResource = (item, index) => {
    let editButton = (
      <a className="txt-primary pointer">
        <i
          key={item.id}
          className="fa fa-pencil fa-lg "
          id={index}
          onClick={this.onEditClick}
        ></i>
      </a>
    );
    return {
      resourceId: item.id,
      resourceName: item.name,
      query: item.query,
      edit: editButton
    };
  };

  onEditClick = e => {
    let resource = this.state.resources[e.target.id];
    this.props.history.push(
      `/admin/queryBuilder/edit/${resource.id}`,
      resource
    );
  };

  createQuery = () => {
    this.props.history.push(`/admin/queryBuilder`);
  };

  render() {
    return (
      <div className="container-fluid">
        <div className="fixedView row">
          <Datatable options={this.state.options} />
        </div>
      </div>
    );
  }
}

ResourcesTable.propTypes = {
  history: PropTypes.shape({
    push: PropTypes.func
  }),
  resources: PropTypes.shape({
    id: PropTypes.number,
    name: PropTypes.string,
    email: PropTypes.string,
    phone: PropTypes.number
  })
};

export default ResourcesTable;
