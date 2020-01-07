const formatTime = date => {
  let timeOption = {
    month: "long",
    day: "numeric",
    year: "numeric",
    hour12: true,
    hour: "2-digit",
    minute: "2-digit",
    timeZoneName: "short"
  };
  return new Date(date).toLocaleTimeString(undefined, timeOption);
};

export { formatTime };
