import _ from "lodash";

//test strings
// let string = "TA1 OR TA2 OR TA3 OR ST1 OR ST14 OR CAP1 OR COM1 OR COM2 OR COM3 OR COM4 OR COM5 OR COM6 OR COM7";
// let simple ="TA1"
// let andString = "TA1 AND LOC3 AND IND7";
// let andOrString = "LOC3 AND TA2 OR TA3 OR ST1 OR ST14 OR CAP1 OR COM1 OR COM2 OR COM3 OR COM4 OR COM5 OR COM6 OR COM7";
// //for complex strings break up into this fashion START WITH ANDS?
// let complexString = "DEMO3 AND CAP2 AND CAP4 OR CAP5 OR CAP6 / LOC6 OR COM3 OR ST4 OR ST5 / LOC2 AND TA1 AND IND8"
let prefix =
  " AND EXISTS(SELECT 1 FROM @AnswerList AS A WHERE A.AnswerValues = ";
let orClause = " OR A.AnswerValues =";
let closingBracket = ")";

let sqlConditionFunction = string => {
  let finalSQL = [];
  let andString = [];
  let orString = [];
  let andOrString = [];
  let splitString;

  if (_.includes(string, "/")) {
    splitString = string.split(" / ");

    _.forEach(splitString, item => {
      if (_.includes(item, "AND")) {
        if (_.includes(item, "OR")) {
          andOrString.push(sqlConditionCreatorWithAndOr(item));
        } else {
          andString.push(sqlConditionCreatorWithAnd(item));
        }
      } else if (_.includes(item, "OR")) {
        orString.push(sqlConditionCreatorWithOrForComplex(item));
      }
    });
  } else {
    if (_.includes(string, "AND")) {
      if (_.includes(string, "OR")) {
        andOrString.push(sqlConditionCreatorWithAndOr(string));
      } else {
        andString.push(sqlConditionCreatorWithAnd(string));
      }
    } else if (_.includes(string, "OR")) {
      orString.push(sqlConditionCreatorWithOrForSimple(string));
    } else {
      andString.push(sqlConditionCreatorWithAnd(string));
    }
  }

  finalSQL = _.concat(andString, andOrString, orString).join(" ");

  return finalSQL;
};

///////////////////////////////////////////////////OR
let sqlConditionCreatorWithOrForSimple = string => {
  let newStr = string.split(" ");

  let removedOr = _.remove(newStr, item => {
    if (item !== "OR") {
      return item;
    }
  });
  let finalSQL = mapOrSql(removedOr);

  return finalSQL.join(" ");
};

let sqlConditionCreatorWithOrForComplex = string => {
  let newStr = string.split(" ");

  let removedOr = _.remove(newStr, item => {
    if (item !== "OR") {
      return item;
    }
  });
  let finalSQL = mapAndOrSql(removedOr);

  return finalSQL.join(" ");
};

///////////////////////////////////////////////////AND/OR
let sqlConditionCreatorWithAndOr = string => {
  let newStr = string.split(" ");

  let orCollection = newStr.splice(_.indexOf(newStr, "OR"), newStr.length - 1);

  orCollection = orCollection.filter(item => item !== "OR");

  let newArr = [];

  _.remove(newStr, item => {
    if (item !== "AND") {
      newArr.push(item);
    }
  });
  let finalSQL = mapAndSql(newArr);

  let orString = mapAndOrSqlSimple(orCollection).join(" ");

  let item = finalSQL[finalSQL.length - 1];

  item = item.replace(closingBracket, orString);

  finalSQL[finalSQL.length - 1] = item;

  return finalSQL.join(" ");
};

///////////////////////////////////////////////////AND
let sqlConditionCreatorWithAnd = string => {
  let newStr = string.split(" ");

  let newArr = [];

  _.forEach(newStr, item => {
    if (item !== "AND") {
      newArr.push(item);
    }
  });
  let finalSQL = mapAndSql(newArr);

  return finalSQL.join(" ");
};

let mapOrSql = arr => {
  return arr.map(code => {
    if (arr.indexOf(code) === 0) {
      if (arr.indexOf(code) === arr.length - 1) {
        return orClause + `'${code}'` + closingBracket;
      }
      return prefix + `'${code}'`;
    } else if (arr.indexOf(code) === arr.length - 1) {
      return orClause + `'${code}'` + closingBracket;
    } else {
      return orClause + `'${code}'`;
    }
  });
};
let mapAndOrSql = arr => {
  return arr.map(code => {
    if (arr.indexOf(code) === 0) {
      if (arr.indexOf(code) === arr.length - 1) {
        return prefix + `'${code}'` + closingBracket;
      }

      return prefix + `'${code}'`;
    } else if (arr.indexOf(code) === arr.length - 1) {
      return orClause + `'${code}'` + closingBracket;
    } else {
      return orClause + `'${code}'`;
    }
  });
};
let mapAndOrSqlSimple = arr => {
  return arr.map(code => {
    if (arr.indexOf(code) === 0) {
      if (arr.indexOf(code) === arr.length - 1) {
        return orClause + `'${code}'` + closingBracket;
      }

      return orClause + `'${code}'`;
    } else if (arr.indexOf(code) === arr.length - 1) {
      return orClause + `'${code}'` + closingBracket;
    } else {
      return orClause + `'${code}'`;
    }
  });
};
let mapAndSql = arr => {
  return arr.map(code => {
    return prefix + `'${code}'` + closingBracket;
  });
};

export default sqlConditionFunction;
