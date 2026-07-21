function GetTodayDateString() {
    var d = new Date();
    var mm = String(d.getMonth() + 1).padStart(2, '0');
    var dd = String(d.getDate()).padStart(2, '0');
    return d.getFullYear() + '-' + mm + '-' + dd;
}

function ValidateHomeworkInputs() {
    let grade = document.getElementById('grade').value;
    if (grade == "") {
        Swal.fire('Error!', 'Grade must be selected!', 'error');
        return false;
    }

    let subject = document.getElementById('subject').value;
    if (subject == "") {
        Swal.fire('Error!', 'Subject must be filled out!', 'error');
        return false;
    }

    let description = document.getElementById('description').value;
    if (description == "") {
        Swal.fire('Error!', 'Description must be filled out!', 'error');
        return false;
    }

    let dueDate = document.getElementById('dueDate').value;
    if (dueDate == "") {
        Swal.fire('Error!', 'Due date must be filled out!', 'error');
        return false;
    }

    if (dueDate < GetTodayDateString()) {
        Swal.fire('Error!', 'Due date cannot be in the past!', 'error');
        return false;
    }

    return true;
}

function SaveHomeworkBtn_Click() {
    if (!ValidateHomeworkInputs()) {
        return;
    }

    var homeworkObj = {
        grade: $('#grade').val(),
        subject: $('#subject').val(),
        description: $('#description').val(),
        dueDate: $('#dueDate').val()
    };

    $.ajax({
        type: 'POST',
        url: '/Teacher/SaveHomework',
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        data: homeworkObj,
        success: function (result) {
            if (result.success == true) {
                Swal.fire('Good job!', 'Homework posted successfully!', 'success');
                ClearHomeworkForm();
            } else {
                Swal.fire('Error!', result.message || 'Failed to post homework!', 'error');
            }
        },
        error: function () {
            Swal.fire('Error!', 'Failed to post homework!', 'error');
        }
    });
}

function ClearHomeworkForm() {
    document.getElementById('grade').value = "";
    document.getElementById('subject').value = "";
    document.getElementById('description').value = "";
    document.getElementById('dueDate').value = "";
}

$(document).ready(function () {
    var dueDateInput = document.getElementById('dueDate');
    dueDateInput.min = GetTodayDateString();

    function RejectPastDate() {
        if (dueDateInput.value !== "" && dueDateInput.value < GetTodayDateString()) {
            Swal.fire('Error!', 'Due date cannot be in the past!', 'error');
            dueDateInput.value = "";
        }
    }

    // Catches a value the browser silently restores on page reload/back-forward
    // navigation, which sets .value directly without firing input/change.
    RejectPastDate();

    dueDateInput.addEventListener('input', RejectPastDate);
    dueDateInput.addEventListener('change', RejectPastDate);

    // Re-checks when the page is restored from the back-forward cache (e.g. browser
    // Back button), where DOMContentLoaded does not fire again but a stale value can persist.
    window.addEventListener('pageshow', function () {
        dueDateInput.min = GetTodayDateString();
        RejectPastDate();
    });
});
