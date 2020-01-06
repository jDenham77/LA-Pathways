import React, { Component } from "react";
import * as graphServices from "../../../services/graphServices";
import * as instanceServices from "../../../services/surveyInstanceService";
import Pie from "./PieChart";
import Bar from "./BarChart";
import SideWidget from "./SideWidget";
import DatePicker from "react-datepicker";
import "react-datepicker/dist/react-datepicker.css";
import * as questionServices from "../../../services/surveyAnswerService";
import Select from "react-select";
import * as resourceServices from "../../../services/resourcesService";
import "./Dash.css";
import * as userProfileServices from "../../../services/userProfilesServices"; //
import * as userServices from "../../../services/userServices"; //
import PropTypes from "prop-types";
import { toast } from "react-toastify";

export default class AdminDash extends Component {
  constructor(props) {
    super(props);
    this.state = {
      questionIds: { questionId: [16, 10, 13, 9, 15, 11, 14, 12] },
      barChartOptions: {
        scales: {
          xAxes: [
            {
              stacked: true
            }
          ],
          yAxes: [
            {
              stacked: true
            }
          ]
        }
      },
      backgroundColor: [
        "rgb(89, 166, 254)", //teal
        "#9575cd", //Pastel Purple
        "rgb(0, 194, 146)", //Pastel Green
        "rgb(51, 106, 255)", //Blue
        "#FEEF8B",
        "Red",
        "#4caf50", //Green
        "#ff5722", //Tangerine
        "#8A8A8A", //gray
        "Purple",
        "#ffeb3b", //Yellow
        "Pink",
        "#e91e63", //Hot Pink
        "#FE8E1D", //Orange
        "#1DFED9" //Teal
      ],
      pieChartOptions: {
        legend: {
          position: "right"
        },
        cutoutPercentage: 50,
        animation: {
          responsive: true,
          maintainAspectRatio: true,
          animateScale: true
        }
      },
      chartType: "Pie",
      startDate: new Date(),
      endDate: new Date()
    };
  }

  componentDidMount() {
    instanceServices
      .getAll(0, 10)
      .then(this.onGetInsatncesSuccess)
      .catch(this.axiosFail);
    graphServices
      .getGraphInfo(this.state.questionIds)
      .then(this.onGetGraphInfoSuccess)
      .catch(this.axiosFail);
    questionServices
      .getQuestionInfo()
      .then(this.onQuestionInfoSuccess)
      .catch(this.axiosFail);
    resourceServices
      .pagination(1)
      .then(this.onGetResourcesSuccess)
      .catch(this.getResourcesfail);
    userServices.getCurrent().then(this.checkProfile);
  }

  checkProfile = response => {
    let id = response.item.id;
    userProfileServices
      .verifyProfile(response.item.id)
      .then()
      .catch(() => this.onProfileVerifyFailure(id));
  };
  onProfileVerifyFailure = id => {
    this.props.history.push(`/admin/users/profile/${id}/edit`);
    toast.info("Please Complete Your Profile!");
  };

  getResourcesfail = () => {
    this.setState(prevState => {
      return { ...prevState, totalResources: 0 };
    });
  };

  onGetResourcesSuccess = data => {
    this.setState(prevState => {
      return { ...prevState, totalResources: data.item.totalCount };
    });
  };

  onQuestionInfoSuccess = data => {
    let cleanedData = data.item.map(this.questionDataCleaner);
    let questionOptionsArr = cleanedData.map(this.questionMapper);
    questionOptionsArr = questionOptionsArr.filter(this.questionOptionsFilter);
    let defaultValues = [
      questionOptionsArr[7],
      questionOptionsArr[0],
      questionOptionsArr[1],
      questionOptionsArr[4]
    ];
    this.setState(prevState => {
      return {
        ...prevState,
        questions: questionOptionsArr,
        selectedChartsArr: defaultValues,
        defaultValues
      };
    });
  };

  questionOptionsFilter = question => {
    if (
      question.key === 16 ||
      question.key === 10 ||
      question.key === 13 ||
      question.key === 9 ||
      question.key === 15 ||
      question.key === 11 ||
      question.key === 12 ||
      question.key === 14
    ) {
      return question;
    }
  };

  questionDataCleaner = idx => {
    let obj = {
      questionText: idx.questionText,
      id: idx.id
    };
    return obj;
  };

  eNum = obj => {
    switch (obj.id) {
      case 16:
        obj.questionText = "Demographics";
        break;
      case 10:
        obj.questionText = "Locations";
        break;
      case 13:
        obj.questionText = "Contracting";
        break;
      case 9:
        obj.questionText = "Consulting";
        break;
      case 15:
        obj.questionText = "Industry";
        break;
      case 11:
        obj.questionText = "Special Topics";
        break;
      case 12:
        obj.questionText = "Capital";
        break;
      case 14:
        obj.questionText = "Compliance";
        break;
      default:
    }
  };

  questionMapper = question => {
    this.eNum(question);
    return {
      key: question.id,
      value: question.id,
      label: question.questionText
    };
  };

  onGetInsatncesSuccess = data => {
    this.setState(prevState => {
      return {
        ...prevState,
        totalInstances: data.item.totalCount,
        startDate: new Date(),
        endDate: new Date()
      };
    });
  };

  graphInfoDataCleaner = question => {
    let obj = {
      Id: question.questionId,
      number: question.number,
      answerText: question.answerText,
      questionText: question.questionText
    };
    return obj;
  };

  onGetGraphInfoSuccess = data => {
    let Demographics = [];
    let Location = [];
    let Contracting = [];
    let Consulting = [];
    let Industry = [];
    let SpecialTopics = [];
    let Capital = [];
    let Compliance = [];

    let cleanData = data.item.map(this.graphInfoDataCleaner);

    cleanData.forEach(question => {
      switch (question.Id) {
        case 16:
          Demographics.push(question);
          break;
        case 10:
          Location.push(question);
          break;
        case 13:
          Contracting.push(question);
          break;
        case 9:
          Consulting.push(question);
          break;
        case 15:
          Industry.push(question);
          break;
        case 11:
          SpecialTopics.push(question);
          break;
        case 12:
          Capital.push(question);
          break;
        case 14:
          Compliance.push(question);
          break;
        default:
      }
    });

    this.setState(prevState => {
      return {
        ...prevState,
        Demographics,
        Contracting,
        Location,
        Consulting,
        Industry,
        SpecialTopics,
        Capital,
        Compliance
      };
    }, this.createChartsData);
  };

  chartDataOrganizer = idx => {
    return idx.number;
  };

  chartLabelOrganizer = idx => {
    return idx.answerText;
  };

  createChartsData = () => {
    //------------------------Making data arrays--------------
    let Demographics = this.state.Demographics.map(this.chartDataOrganizer);
    let Consulting = this.state.Consulting.map(this.chartDataOrganizer);
    let Location = this.state.Location.map(this.chartDataOrganizer);
    let Contracting = this.state.Contracting.map(this.chartDataOrganizer);
    let IndustryNum = this.state.Industry.map(this.chartDataOrganizer);
    let SpecialTopicsNum = this.state.SpecialTopics.map(
      this.chartDataOrganizer
    );
    let CapitalNum = this.state.Capital.map(this.chartDataOrganizer);
    let ComplianceNum = this.state.Compliance.map(this.chartDataOrganizer);

    //----------Creating labels array---------------
    let DemographicsLabels = this.state.Demographics.map(
      this.chartLabelOrganizer
    );
    let ConsultingLabels = this.state.Consulting.map(this.chartLabelOrganizer);
    let LocationLabels = this.state.Location.map(this.chartLabelOrganizer);
    let ContractingLabels = this.state.Contracting.map(
      this.chartLabelOrganizer
    );
    let IndustryLabels = this.state.Industry.map(this.chartLabelOrganizer);
    let SpecialTopicsLabels = this.state.SpecialTopics.map(
      this.chartLabelOrganizer
    );
    let CapitalLabels = this.state.Capital.map(this.chartLabelOrganizer);
    let ComplianceLabels = this.state.Compliance.map(this.chartLabelOrganizer);

    //----------------------Objects for mapping-------------

    let Demo = {
      labels: DemographicsLabels,
      data: Demographics,
      chartName: "Demographics"
    };
    let Consult = {
      labels: ConsultingLabels,
      data: Consulting,
      chartName: "Consulting"
    };
    let Loc = {
      labels: LocationLabels,
      data: Location,
      chartName: "Locations"
    };
    let Contract = {
      labels: ContractingLabels,
      data: Contracting,
      chartName: "Contracting"
    };
    let Industry = {
      labels: IndustryLabels,
      data: IndustryNum,
      chartName: "Industry"
    };
    let SpecialTopics = {
      labels: SpecialTopicsLabels,
      data: SpecialTopicsNum,
      chartName: "Special Topics"
    };
    let Capital = {
      labels: CapitalLabels,
      data: CapitalNum,
      chartName: "Capital"
    };
    let Compliance = {
      labels: ComplianceLabels,
      data: ComplianceNum,
      chartName: "Compliance"
    };

    let chartArr = [
      Demo,
      Consult,
      Loc,
      Contract,
      Industry,
      SpecialTopics,
      Capital,
      Compliance
    ];

    let barChartData = chartArr.map(this.barGraphDataMapper);
    let pieChartData = chartArr.map(this.pieChartDataMapper);

    this.setState(prevState => {
      return {
        ...prevState,
        barCharts: barChartData.map(this.barMapper),
        pieCharts: pieChartData.map(this.pieMapper)
      };
    }, this.setInitialCharts);
  };

  setInitialCharts = () => {
    let arr = this.state.pieCharts;
    let selected = [arr[0], arr[1], arr[2], arr[3]];
    this.setState(prevState => {
      return {
        ...prevState,
        currentView: selected,
        defaultSelected: selected,
        defaultView: selected
      };
    });
  };

  axiosFail = error => {
    toast.error(error.message);
  };

  barGraphDataMapper = idx => ({
    datasets: [
      {
        data: idx.data,
        barPercentage: 0.5,
        barThickness: "flex",
        minBarLength: 2,
        label: idx.chartName,
        backgroundColor: this.state.backgroundColor
      }
    ],
    labels: idx.labels
  });

  pieChartDataMapper = idx => ({
    data: {
      datasets: [
        {
          backgroundColor: this.state.backgroundColor,
          data: idx.data
        }
      ],
      labels: idx.labels,
      position: "bottom"
    },
    chartName: idx.chartName
  });

  pieMapper = (idx, index) => (
    <div key={index} className="col-xl-5 col-lg-12 col-md-12 card graphCard">
      <h6 className="chartName">{idx.chartName}</h6>
      <Pie
        chartData={idx.data}
        chartOptions={this.state.pieChartOptions}
        className="mr-2"
      />
    </div>
  );

  barMapper = (idx, index) => (
    <div key={index} className="col-xl-5 col-lg-12 col-md-12 card graphCard">
      <Bar
        chartName={idx.datasets[0].label}
        chartData={idx}
        chartOptions={this.state.barChartOptions}
      />
    </div>
  );

  onChartSelect = valueArr => {
    this.setState(prevState => {
      return {
        ...prevState,
        selectedChartsArr: valueArr
      };
    }, this.showSelectedCharts);
  };

  multiGetMapper = idx => {
    return idx.value;
  };

  chartFilter = chart => {
    let selectedCharts = this.state.selectedChartsArr;
    let returnedChart = null;
    let searchTerms = null;

    if (this.state.chartType === "Pie") {
      searchTerms = chart.props.children[0].props.children;
    } else if (this.state.chartType === "Bar") {
      searchTerms = chart.props.children.props.chartName;
    }

    selectedCharts.forEach(obj => {
      if (obj.label === searchTerms) {
        returnedChart = chart;
      }
    });
    return returnedChart;
  };

  showSelectedCharts = () => {
    if (
      this.state.selectedChartsArr !== undefined &&
      this.state.selectedChartsArr !== null &&
      this.state.selectedChartsArr.length !== 0
    ) {
      if (this.state.selectedChartsArr.length >= 1) {
        if (this.state.chartType === "Pie") {
          let chartsToShow = this.state.pieCharts.filter(this.chartFilter);
          this.setState(prevState => {
            return { ...prevState, currentView: chartsToShow };
          });
        } else if (this.state.chartType === "Bar") {
          let chartsToShow = this.state.barCharts.filter(this.chartFilter);
          this.setState(prevState => {
            return { ...prevState, currentView: chartsToShow };
          });
        }
      }
    } else {
      this.setState(prevState => {
        return {
          ...prevState,
          currentView: null,
          selectedChartsArr: this.state.defaultValues,
          chartType: "Pie"
        };
      });
    }
  };

  onChartTypeSelect = e => {
    let value = e.target.value;
    this.setState(prevState => {
      return {
        ...prevState,
        chartType: value
      };
    }, this.showSelectedCharts);
  };

  handleStartDateChange = date => {
    this.setState(prevState => {
      return { ...prevState, startDate: date };
    });
  };

  handleEndDateChange = date => {
    this.setState(prevState => {
      return { ...prevState, endDate: date };
    });
  };

  updateInstanceCount = data => {
    this.setState(prevState => {
      return { ...prevState, totalInstances: data.item.totalCount };
    });
  };

  searchByDate = () => {
    let startDate = `${this.state.startDate.getFullYear()}-${this.state.startDate.getMonth() +
      1}-${this.state.startDate.getDate()}`;

    let endDate = `${this.state.endDate.getFullYear()}-${this.state.endDate.getMonth() +
      1}-${this.state.endDate.getDate() + 1}`;

    instanceServices
      .dateRange(startDate, endDate, 0, 1)
      .then(this.updateInstanceCount)
      .catch(this.instanceFail);
  };

  instanceFail = () => {
    this.setState(prevState => {
      return { ...prevState, totalInstances: 0 };
    });
  };

  clearDateSearch = () => {
    instanceServices
      .getAll(0, 10)
      .then(this.onGetInsatncesSuccess)
      .catch(this.axiosFail);
  };

  render() {
    return (
      <div className="col-12">
        <div className="row d-flex justify-content-center">
          <SideWidget
            totalInstances={this.state.totalResources}
            resource={true}
            title={"Resources"}
          />
          <SideWidget
            totalInstances={this.state.totalInstances}
            title={"Surveys Taken"}
          />
          <div className="col-md-4">
            <div className="card default-widget-count statCard noPadding cardHeight">
              <div className="card-body form-group center height noPadding">
                <div className="media-body align-self-center pt-4">
                  <h5>Surveys by Date</h5>
                </div>
                <div className="row">
                  <div className="m-auto">
                    <DatePicker
                      name="startDate"
                      className="col-lg-10 mb-1"
                      selected={this.state.startDate}
                      onChange={this.handleStartDateChange}
                    />

                    <DatePicker
                      name="endDate"
                      className="col-lg-10"
                      selected={this.state.endDate}
                      onChange={this.handleEndDateChange}
                    />
                  </div>
                </div>
                <div className="m-auto d-flex justify col-10">
                  <button
                    type="button"
                    className="btn btn-pill btn-primary mt-2"
                    onClick={this.searchByDate}
                  >
                    Filter
                  </button>
                  <button
                    type="button"
                    className="btn btn-pill btn-secondary mt-2"
                    onClick={this.clearDateSearch}
                  >
                    Clear
                  </button>
                </div>
              </div>
            </div>
          </div>
        </div>
        <div className="row mt-4">
          <div className="center">
            <div className="row justify-content-center">
              <div className="form-group">
                <Select
                  isMulti
                  name="questions"
                  classNamePrefix="select"
                  className="basic-multi-select"
                  value={this.state.selectedChartsArr}
                  onChange={this.onChartSelect}
                  options={this.state.questions}
                />
              </div>
              <div>
                <select
                  className="form-control ml-2"
                  value={this.state.chartType}
                  onChange={this.onChartTypeSelect}
                >
                  <option value="Pie">Pie</option>
                  <option value="Bar">Bar</option>
                </select>
              </div>
            </div>
          </div>
        </div>
        <div className="center mt-2">
          {this.state.currentView ? (
            <div id="graphs" className=" mt-2 row">
              {this.state.currentView}
            </div>
          ) : (
            <div id="graphs" className="mt-2 row">
              {this.state.defaultView}
            </div>
          )}
        </div>
      </div>
    );
  }
}
AdminDash.propTypes = {
  history: PropTypes.shape({
    push: PropTypes.func
  }),
  currentUser: PropTypes.shape({
    id: PropTypes.number
  })
};
