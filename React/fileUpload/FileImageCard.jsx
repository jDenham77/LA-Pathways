import React from "react";
import PropTypes from "prop-types";

const FileImageCard = props => {
  const onImageError = e => {
    e.target.onerror = null;
    e.target.src = "https://bit.ly/34ZWVGl";
  };
  const clickImage = () => {
    props.onImageClick(props.file);
  };
  const popover = () => {
    props.mouseOverToggle(props.file);
  };
  return (
    <React.Fragment key={props.file.id}>
      {/* {props.file.fileTypeId} */}
      <figure
        id={`Tooltip-${props.file.id}`}
        className="d-flex col-xl-3 col-md-4 col-6 img-hover hover-11"
        onMouseOver={popover}
      >
        <img
          onClick={clickImage}
          onError={onImageError}
          src={
            props.file.fileTypeId === 1
              ? "https://bit.ly/34i2MpG"
              : props.file.url
          }
          alt=""
          className="img-thumbnail "
        />
        {/* <a className="m-2" href={`/downloads/${props.file.url}`} download>
          <i className="icofont icofont-download-alt"></i>
        </a> */}
      </figure>
    </React.Fragment>
  );
};

FileImageCard.propTypes = {
  showPopover: PropTypes.bool.isRequired,
  mouseOverToggle: PropTypes.func.isRequired,
  onImageClick: PropTypes.func.isRequired,
  file: PropTypes.shape({
    id: PropTypes.number.isRequired,
    url: PropTypes.string.isRequired,
    fileTypeId: PropTypes.number.isRequired
  })
};

export default FileImageCard;
