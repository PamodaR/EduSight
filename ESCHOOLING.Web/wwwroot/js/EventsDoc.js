function GetTodayDateString() {
    var d = new Date();
    var mm = String(d.getMonth() + 1).padStart(2, '0');
    var dd = String(d.getDate()).padStart(2, '0');
    return d.getFullYear() + '-' + mm + '-' + dd;
}

function ValidateEventInputs() {
    let eventName = document.getElementById('eventName').value;
    if (eventName == "") {
        Swal.fire('Error!', 'Title must be filled out!', 'error');
        return false;
    }

    let description = document.getElementById('description').value;
    if (description == "") {
        Swal.fire('Error!', 'Description must be filled out!', 'error');
        return false;
    }

    let date = document.getElementById('date').value;
    if (date == "") {
        Swal.fire('Error!', 'Date must be filled out!', 'error');
        return false;
    }

    if (date < GetTodayDateString()) {
        Swal.fire('Error!', 'Date cannot be in the past!', 'error');
        return false;
    }

    let time = document.getElementById('time').value;
    if (time == "") {
        Swal.fire('Error!', 'Time must be filled out!', 'error');
        return false;
    }

    let place = document.getElementById('place').value;
    if (place == "") {
        Swal.fire('Error!', 'Place must be filled out!', 'error');
        return false;
    }

    return true;
}

function SaveEventBtn_Click() {
    if (!ValidateEventInputs()) {
        return;
    }

    var eventObj = {
        eventName: $('#eventName').val(),
        description: $('#description').val(),
        date: $('#date').val(),
        time: $('#time').val(),
        place: $('#place').val(),
        grade: $('#grade').val()
    };

    var rolePrefix = window.location.pathname.split('/')[1];

    $.ajax({
        type: 'POST',
        url: '/' + rolePrefix + '/SaveEvent',
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        data: eventObj,
        success: function (result) {
            if (result.success == true) {
                Swal.fire('Good job!', 'Event added successfully!', 'success');
                ClearEventForm();
            } else {
                Swal.fire('Error!', result.message || 'Failed to add event!', 'error');
            }
        },
        error: function () {
            Swal.fire('Error!', 'Failed to add event!', 'error');
        }
    });
}

function ClearEventForm() {
    document.getElementById('eventName').value = "";
    document.getElementById('description').value = "";
    document.getElementById('date').value = "";
    document.getElementById('time').value = "";
    document.getElementById('place').value = "";
    document.getElementById('grade').value = "";
}

$(document).ready(function () {
    var dateInput = document.getElementById('date');
    dateInput.min = GetTodayDateString();

    function RejectPastDate() {
        if (dateInput.value !== "" && dateInput.value < GetTodayDateString()) {
            Swal.fire('Error!', 'Date cannot be in the past!', 'error');
            dateInput.value = "";
        }
    }

    // Catches a value the browser silently restores on page reload/back-forward
    // navigation, which sets .value directly without firing input/change.
    RejectPastDate();

    dateInput.addEventListener('input', RejectPastDate);
    dateInput.addEventListener('change', RejectPastDate);

    // Re-checks when the page is restored from the back-forward cache (e.g. browser
    // Back button), where DOMContentLoaded does not fire again but a stale value can persist.
    window.addEventListener('pageshow', function () {
        dateInput.min = GetTodayDateString();
        RejectPastDate();
    });
});
