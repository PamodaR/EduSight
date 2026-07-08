
function SearchForAttendanceBtn_Click() {
    var searchUrl = 'SearchForAttendance';
    // Get the selected grade value from the dropdown
    var selectedGrade = document.getElementById("Grade").value;

    // Construct the URL manually using the JavaScript variable
    var url = searchUrl + '?grade=' + selectedGrade;

    // Redirect to the constructed URL
    window.location.href = url;
}

function SearchForStudentsBtn_Click() {
    var searchUrl = 'SearchForStudents';
    // Get the selected grade value from the dropdown
    var selectedGrade = document.getElementById("Grade").value;

    // Construct the URL manually using the JavaScript variable
    var url = searchUrl + '?grade=' + selectedGrade;

    // Redirect to the constructed URL
    window.location.href = url;
}

function SearchByStudentsId_Click() {

    var searchUrl = 'SearchForStudentsId';
    // Get the selected grade value from the dropdown
    var selectedId = document.getElementById("searchById").value;

    // Construct the URL manually using the JavaScript variable
    var url = searchUrl + '?id=' + selectedId;

    // Redirect to the constructed URL
    window.location.href = url;
}

function SearchByMonth_Click() {
    var regexPattern = /\/(\d+)$/;
    var previousUrl = document.referrer;
    var match = previousUrl.match(regexPattern);
    if (match !== null) {
        var id = match[1];
    } else {
        console.log("No matching value found in the previous URL.");
        // Handle the case where there's no match
    }

    // Get the selected grade value from the dropdown
    let selectedMonth = document.getElementById("searchByAttendance").value;

    let url =  '?id=' + id + '&date=' + selectedMonth;

    // Redirect to the constructed URL
    window.location.href = url;
}