import React from "react";
import PropTypes from "prop-types";
import "./wizardCss.css";

const Question = props => {
  return (
    <>
      <h6 className="pb-4" name="questionId" value={props.value}>
        {props.question}
      </h6>

      <div className="wizardAnswers row">
        <div className="col-12 answerOptions">{props.answers}</div>
      </div>
    </>
  );
};

Question.propTypes = {
  question: PropTypes.string,
  answers: PropTypes.arrayOf(PropTypes.element),
  name: PropTypes.string,
  value: PropTypes.number
};

export default Question;
