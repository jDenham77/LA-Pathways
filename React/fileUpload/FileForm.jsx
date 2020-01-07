import React, { Component } from "react";
import FileImageCard from "./FileImageCard";
import Pagination from "rc-pagination";
import "rc-pagination/assets/index.css";
import * as filesService from "../../services/filesService";
import FileUpload from "./FileUpload";
import swal from "sweetalert";
import PDFReader from "./PdfReader";
import { Typeahead } from "react-bootstrap-typeahead";
import "react-bootstrap-typeahead/css/Typeahead.css";
import "./File.css";
import { toast } from "react-toastify";

class FileForm extends Component {
  constructor() {
    super();
    this.state = {
      formData: {
        urls: []
      },
      showPopover: false,
      pdf: [],
      showPDFView: false,
      filteredFiles: [],
      isFiltering: false,
      files: [],
      options: [],
      fileTypes: [],
      thumbnails: [],
      filterCriteria: [],
      search: false,
      mappedFiles: [],
      currentPage: 0,
      pageSize: 12,
      totalPages: 0,
      fileUrl: [],
      viewImage: [],
      popoverFile: [],
      totalCount: 0
    };
  }

  componentDidMount = () => {
    filesService
      .getPaginated(this.state.currentPage, this.state.pageSize)
      .then(this.onGetSuccess)
      .catch(this.onGetError);
    filesService
      .getFileTypes()
      .then(this.onGetTypesSuccess)
      .catch(this.onGetError);
  };

  onGetTypesSuccess = data => {
    let fileTypes = data.item;
    this.setState(prevState => {
      return {
        ...prevState,
        fileTypes
      };
    });
  };

  onGetSuccess = data => {
    let files = data.item.pagedItems;

    this.setState(prevState => {
      return {
        ...prevState,
        totalCount: data.item.totalCount,
        files,
        mappedFiles: files.map(this.mapFile)
      };
    });
  };

  onGetError = error => {
    toast.error(error.message);
  };

  onUploadFileError = () => {
    swal(
      "Upload Failed",
      "There was an error uploading your file please check valid file type.",
      "info"
    );
  };
  onImageError = e => {
    e.target.onerror = null;
    e.target.src = "https://bit.ly/34ZWVGl";
  };

  mapFile = file => (
    <FileImageCard
      mouseOverToggle={this.mouseOverToggle}
      showPopover={this.state.showPopover}
      onImageClick={this.onImageClick}
      file={file}
      key={file.id}
    />
  );
  mapImage = (image, index) => (
    <div
      className="my-gallery gallery card-body row"
      id="aniimated-thumbnials"
      key={index + 1}
    >
      <figure className="d-flex col-xl-3 col-md-4 col-6 img-hover hover-11">
        <img
          onError={this.onImageError}
          src={image}
          alt=""
          className="img-thumbnail "
        />
      </figure>
    </div>
  );

  onUploadFileSuccess = data => {
    let fileUrl = data.item;
    this.setState(prevState => {
      return {
        ...prevState,
        fileUrl,
        formData: {
          urls: fileUrl
        }
      };
    });
  };
  uploadToDb = () => {
    let fileData = this.state.formData;
    filesService
      .uploadToDb(fileData)
      .then(this.onUploadFileToDbSuccess)
      .catch(this.onUploadFileError);
  };

  onUploadFileToDbSuccess = data => {
    let file = {};
    file.id = data.item;
    file.url = this.state.formData;
    file.fileTypeId = 8;
    let mappedFile = this.mapFile(file);
    this.setState(prevState => {
      return {
        ...prevState,
        files: prevState.files.push(file),
        mappedFiles: [...prevState.mappedFiles, mappedFile]
      };
    });

    swal(`Success!`, `You have uploaded a file`, "success");
    this.reset();
    setTimeout(() => {
      swal.close();
    }, 1000);
  };

  reset = () => {
    this.setState(prevState => {
      return {
        ...prevState,
        formData: {
          urls: []
        }
      };
    });
  };

  onChange = page => {
    this.setState(prevState => {
      return { ...prevState, currentPage: page - 1 };
    });
    filesService
      .getPaginated(page - 1, this.state.pageSize)
      .then(this.onGetSuccess);
  };

  onFileUploadSuccess = res => {
    this.setState(prevState => {
      return {
        ...prevState,
        formData: {
          urls: res
        }
      };
    });
  };

  onImageClick = file => {
    if (file.fileTypeId === 1) {
      this.setState(prevState => {
        return {
          ...prevState,
          pdf: file,
          showPDFView: true
        };
      });
    } else if (
      file.fileTypeId === 2 ||
      file.fileTypeId === 5 ||
      file.fileTypeId === 6 ||
      file.fileTypeId === 7
    ) {
      return;
    } else {
      this.setState(prevState => {
        return {
          ...prevState,
          showModal: true,
          tabIndex: file.id,
          viewImage: file
        };
      });
    }
  };

  toggleLightBox = () => {
    this.setState(prevState => {
      return {
        ...prevState,
        showModal: !prevState.showModal
      };
    });
  };
  goBack = () => {
    this.setState(prevState => {
      return {
        ...prevState,
        showPDFView: false
      };
    });
  };
  mouseOverToggle = file => {
    this.setState(prevState => {
      return {
        ...prevState,
        showPopover: !prevState.showPopover,
        popoverFiles: file
      };
    });
  };

  optionMapper = option => {
    return option.id;
  };

  handleChange = option => {
    this.setState(prevState => {
      return {
        ...prevState,
        filterCriteria: option,
        search: true
      };
    });
  };

  filterFiles = () => {
    let filteredFiles = [];
    let criteria = this.state.filterCriteria;
    let files = this.state.files;
    for (let file of files) {
      for (let item of criteria) {
        if (item.id === file.fileTypeId) {
          filteredFiles.push(file);
        }
      }
    }
    this.setState(prevState => {
      return {
        ...prevState,
        filteredFiles: filteredFiles.map(this.mapFile),
        isFiltering: true
      };
    });
  };

  resetFilter = () => {
    this.setState(prevState => {
      return {
        ...prevState,
        isFiltering: false,
        filterCriteria: []
      };
    });
  };

  render() {
    return (
      <React.Fragment>
        {this.state.showPDFView ? (
          <PDFReader goBack={this.goBack} file={this.state.pdf} />
        ) : (
          <div className="container-fluid">
            <div className="row mb-4">
              <h4>File Manager</h4>
            </div>
            <div className="row mb-4">
              <div className="col">
                <FileUpload onSuccess={this.onFileUploadSuccess} />
              </div>
              <div className="col">
                <button
                  type="button"
                  className="btn btn-pill btn-sm btn-primary ml-2"
                  onClick={this.uploadToDb}
                >
                  Upload File
                </button>
                <button
                  type="button"
                  className="btn btn-pill btn-sm btn-secondary ml-2"
                  onClick={this.reset}
                >
                  Cancel
                </button>
              </div>
            </div>
            <div className="row">
              <div className="col">
                <div className="card">
                  {this.state.formData.urls
                    ? this.state.formData.urls.map(this.mapImage)
                    : ""}
                </div>
              </div>
            </div>
            <br />
            <div className="row mb-4">
              <div className="col">
                <Typeahead
                  id="typeAhead"
                  type="text"
                  name="fileTypeId"
                  multiple={true}
                  placeholder="All Files"
                  onChange={this.handleChange}
                  labelKey={option => `${option.name}`}
                  options={this.state.fileTypes}
                />
              </div>
              <div className="col">
                <button
                  onClick={this.filterFiles}
                  className="btn btn-primary btn-pill btn-sm ml-2"
                >
                  Filter
                </button>
                <button
                  onClick={this.resetFilter}
                  className="btn btn-secondary btn-pill btn-sm ml-2"
                >
                  Reset
                </button>
              </div>
            </div>
            <div className="card">
              <div
                className="my-gallery gallery card-body row"
                id="aniimated-thumbnials"
              >
                {this.state.filteredFiles.length === 0 && this.state.isFiltering
                  ? `There are no files under this file type`
                  : this.state.isFiltering
                  ? this.state.filteredFiles
                  : this.state.mappedFiles}
              </div>
            </div>
            <div className="d-flex justify-content-center">
              {this.state.isFiltering ? (
                ""
              ) : (
                <Pagination
                  className="paginate ant-pagination"
                  pageSize={this.state.pageSize}
                  onChange={this.onChange}
                  current={this.state.current + 1}
                  total={this.state.totalCount}
                  showTitle={false}
                  hideOnSinglePage={true}
                />
              )}
            </div>
          </div>
        )}
      </React.Fragment>
    );
  }
}

export default FileForm;
