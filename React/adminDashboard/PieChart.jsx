import React from "react";
import DoughnutChart from "react-chartjs-2";
import PropTypes from "prop-types";
import "./Graph.css";

const PieChart = props => {
  const { chartData, chartOptions } = props;
  return <DoughnutChart data={chartData} options={chartOptions} />;
};
PieChart.propTypes = {
  chartData: PropTypes.shape({
    datasets: PropTypes.arrayOf(
      PropTypes.shape({
        data: PropTypes.arrayOf(PropTypes.number),
        backgroundColor: PropTypes.arrayOf(PropTypes.string),
        borderAlign: PropTypes.string,
        hoverBackgroundColor: PropTypes.string
      })
    )
  }),
  chartOptions: PropTypes.shape({
    cutoutPercentage: PropTypes.number,
    animation: PropTypes.shape({
      animateScale: PropTypes.bool
    })
  }),
  chartName: PropTypes.string
};
export default PieChart;
