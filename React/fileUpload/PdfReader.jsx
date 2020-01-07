import React from "react";
import PDFViewer from "mgr-pdf-viewer-react";
import PropTypes from "prop-types";

const PDFReader = props => {
  const onGoBack = () => {
    props.goBack();
  };
  return (
    <React.Fragment>
      <div className="container-fluid mt-10">
        <div className="row">
          <button onClick={onGoBack} className="btn btn-pill btn-primary">
            Go Back
          </button>
        </div>
      </div>
      <PDFViewer
        document={{
          url: props.file.url
        }}
      />
    </React.Fragment>
  );
};
PDFReader.propTypes = {
  goBack: PropTypes.func,
  page: PropTypes.number,
  loader: PropTypes.node,
  document: PropTypes.shape({
    file: PropTypes.any, // File object,
    url: PropTypes.string, // URL to fetch the pdf
    connection: PropTypes.object, // connection parameters to fetch the PDF, see PDF.js docs
    base64: PropTypes.string // PDF file encoded in base64
    // binary: PropTypes.UInt8Array
  }),
  css: PropTypes.string,
  file: PropTypes.shape({
    id: PropTypes.number.isRequired,
    url: PropTypes.string.isRequired,
    fileTypeId: PropTypes.number.isRequired
  }),
  navigation: PropTypes.oneOfType([
    // Can be an object with css classes or react elements to be rendered
    PropTypes.shape({
      css: PropTypes.shape({
        previousPageBtn: PropTypes.string, // CSS Class for the previous page button
        nextPageBtn: PropTypes.string, // CSS Class for the next page button
        pages: PropTypes.string, // CSS Class for the pages indicator
        wrapper: PropTypes.string // CSS Class for the navigation wrapper
      }),
      elements: PropTypes.shape({
        previousPageBtn: PropTypes.any, // previous page button React element
        nextPageBtn: PropTypes.any, // next page button React element
        pages: PropTypes.any // pages indicator React Element
      })
    }),
    // Or a full navigation component
    PropTypes.any // Full navigation React element
  ])
};

export default PDFReader;
