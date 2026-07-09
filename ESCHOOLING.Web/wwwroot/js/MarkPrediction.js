function ValidatePredictionInputs() {
    let studentId = document.getElementById('studentId').value;
    if (studentId == "") {
        Swal.fire('Error!', 'Student Id must be filled out!', 'error');
        return false;
    }

    let subject = document.getElementById('Subject').value;
    if (subject == "") {
        Swal.fire('Error!', 'Subject must be filled out!', 'error');
        return false;
    }

    let mark1 = document.getElementById('Mark1').value;
    let mark2 = document.getElementById('Mark2').value;
    let mark3 = document.getElementById('Mark3').value;
    if (mark1 == "" || mark2 == "" || mark3 == "") {
        Swal.fire('Error!', 'All three previous test marks must be filled out!', 'error');
        return false;
    }

    return true;
}

function PredictMarkBtn_Click() {
    if (!ValidatePredictionInputs()) {
        return;
    }

    var predictObj = {
        studentId: $('#studentId').val(),
        subject: $('#Subject').val(),
        mark1: $('#Mark1').val(),
        mark2: $('#Mark2').val(),
        mark3: $('#Mark3').val()
    };

    $.ajax({
        type: 'POST',
        url: '/Teacher/PredictMark',
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        data: predictObj,
        success: function (result) {
            if (result.success == true) {
                document.getElementById('PredictedMark').value = result.predictedMark;
            } else {
                Swal.fire('Error!', 'Prediction failed!', 'error');
            }
        },
        error: function () {
            Swal.fire('Error!', 'Prediction failed!', 'error');
        }
    });
}

function SaveMarkBtn_Click() {
    if (!ValidatePredictionInputs()) {
        return;
    }

    let predictedMark = document.getElementById('PredictedMark').value;
    if (predictedMark == "") {
        Swal.fire('Error!', 'Predict a mark before saving!', 'error');
        return;
    }

    var saveObj = {
        studentId: $('#studentId').val(),
        subject: $('#Subject').val(),
        predictedMark: predictedMark
    };

    $.ajax({
        type: 'POST',
        url: '/Teacher/SaveMark',
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        data: saveObj,
        success: function (result) {
            if (result.success == true) {
                Swal.fire('Good job!', 'Mark saved successfully!', 'success');
            } else {
                Swal.fire('Error!', 'Save failed!', 'error');
            }
        },
        error: function () {
            Swal.fire('Error!', 'Save failed!', 'error');
        }
    });
}
