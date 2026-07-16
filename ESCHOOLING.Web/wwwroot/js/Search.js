
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

function EditStudentBtn_Click(id) {
    window.location.href = '/Teacher/EditStudent?id=' + id;
}

function DeleteStudentBtn_Click(id, name) {
    Swal.fire({
        title: 'Deactivate ' + name + '?',
        text: 'This will mark the student as not active.',
        icon: 'warning',
        showCancelButton: true,
        confirmButtonText: 'Deactivate',
        cancelButtonText: 'Cancel'
    }).then(function (result) {
        if (result.isConfirmed) {
            var form = document.createElement('form');
            form.method = 'POST';
            form.action = '/Teacher/DeleteStudent';

            var input = document.createElement('input');
            input.type = 'hidden';
            input.name = 'id';
            input.value = id;

            form.appendChild(input);
            document.body.appendChild(form);
            form.submit();
        }
    });
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