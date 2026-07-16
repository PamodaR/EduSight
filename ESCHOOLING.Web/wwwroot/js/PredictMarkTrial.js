// Shared by Student/PredictMark.cshtml and Parent/PredictMark.cshtml — a trial
// prediction form with no Save capability (only Teacher can save a predicted mark).

function ValidatePredictMarkTrialInputs() {
    let mark1 = document.getElementById('Mark1').value;
    let mark2 = document.getElementById('Mark2').value;
    let mark3 = document.getElementById('Mark3').value;
    if (mark1 == "" || mark2 == "" || mark3 == "") {
        Swal.fire('Error!', 'All three previous test marks must be filled out!', 'error');
        return false;
    }

    return true;
}

function PredictMarkTrialBtn_Click(url) {
    if (!ValidatePredictMarkTrialInputs()) {
        return;
    }

    var predictObj = {
        mark1: $('#Mark1').val(),
        mark2: $('#Mark2').val(),
        mark3: $('#Mark3').val()
    };

    $.ajax({
        type: 'POST',
        url: url,
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        data: predictObj,
        success: function (result) {
            if (result.success == true) {
                document.getElementById('PredictedMark').value = result.predictedMark;
            } else {
                Swal.fire('Error!', result.message || 'Prediction failed!', 'error');
            }
        },
        error: function () {
            Swal.fire('Error!', 'Prediction failed!', 'error');
        }
    });
}
