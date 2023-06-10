//Resize header
let resizeTimeout;
function resizeWindowHandler(dataTable) {
    clearTimeout(resizeTimeout);
    resizeTimeout = setTimeout(function () {
        dataTable.columns.adjust();
    }, 300);
}

// Đổi từ giờ UTC sang giờ Local
function ChangeFromUtcTimeToLocalTime(input) {
    let date = new Date($(input).datepicker("getDate"));
    date.setMinutes(date.getMinutes() - date.getTimezoneOffset())
    return date;
}