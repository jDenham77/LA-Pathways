import React from "react";
import PropTypes from "prop-types";
import RadioButtons from "./RadioButtons";
import CheckBox from "./CheckBox";
import "./wizardCss.css";
import { isMobile } from "react-device-detect";

const Answer = props => {
  return (
    <div
      className={
        isMobile
          ? `col-${props.lengthValue > 6 ? 6 : 12}`
          : `col-${
              props.lengthValue > 9 ? 2 : Math.floor(12 / props.lengthValue)
            }`
      }
    >
      <label>
        {props.typeId === 1 ? (
          <RadioButtons
            id={props.Id}
            value={props.value}
            text={props.text}
            name={props.name}
            onAnswered={props.onAnswered}
          />
        ) : (
          <CheckBox
            clickedCheckBox={props.clickedCheckBox}
            text={props.text}
            id={props.Id}
            value={props.value}
            name={props.name}
          />
        )}
      </label>
    </div>
  );
};

Answer.propTypes = {
  Id: PropTypes.number,
  text: PropTypes.string,
  name: PropTypes.string,
  value: PropTypes.number,
  typeId: PropTypes.number,
  onAnswered: PropTypes.func,
  clickedCheckBox: PropTypes.func,
  lengthValue: PropTypes.number
};

export default Answer;
