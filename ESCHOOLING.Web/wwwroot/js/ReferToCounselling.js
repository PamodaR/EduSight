function ValidateReferToCounsellingInputs() {
    let studentId = document.getElementById('studentId').value;
    if (studentId == "") {
        Swal.fire('Error!', 'Please select a student!', 'error');
        return false;
    }

    let counselorId = document.getElementById('counselorId').value;
    if (counselorId == "") {
        Swal.fire('Error!', 'Please select a counsellor!', 'error');
        return false;
    }

    let reason = document.getElementById('Reason').value;
    if (reason == "") {
        Swal.fire('Error!', 'Reason must be filled out!', 'error');
        return false;
    }

    return true;
}

function ReferToCounsellingBtn_Click() {
    if (!ValidateReferToCounsellingInputs()) {
        return;
    }

    var referralObj = {
        studentId: $('#studentId').val(),
        counselorId: $('#counselorId').val(),
        reason: $('#Reason').val()
    };

    $.ajax({
        type: 'POST',
        url: '/Teacher/ReferToCounselling',
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        data: referralObj,
        success: function (result) {
            if (result.success == true) {
                if (result.emailSent == false) {
                    Swal.fire('Referral Saved', result.message || 'Referral saved, but the counsellor email could not be sent.', 'warning');
                } else {
                    Swal.fire('Good job!', 'Referral submitted and the counsellor has been notified by email!', 'success');
                }
                document.getElementById('Reason').value = "";
            } else {
                Swal.fire('Error!', result.message || 'Save failed!', 'error');
            }
        },
        error: function () {
            Swal.fire('Error!', 'Save failed!', 'error');
        }
    });
}
