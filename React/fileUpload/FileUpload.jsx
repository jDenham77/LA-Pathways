import React from "react";
import PropTypes from "prop-types";
import * as filesService from "../../services/filesService";

const FileUpload = props => {
  const handleChange = e => {
    let fileList = e.target.files;
    const formData = new FormData();
    if (fileList) {
      for (let file of fileList) {
        formData.append("files", file);
      }
      filesService
        .uploadFile(formData)
        .then(onUploadFileSuccess)
        .catch(onFileUploadError);
    }
  };

  const onUploadFileSuccess = data => {
    props.onSuccess(data.item);
  };

  const onFileUploadError = () => {
    props.onError();
  };

  return (
    <div className="col">
      <input
        type="file"
        name="file"
        className="form-control btn btn-pill"
        onChange={handleChange}
      />
    </div>
  );
};

FileUpload.propTypes = {
  onSuccess: PropTypes.func.isRequired,
  onError: PropTypes.func
};

export default FileUpload;
