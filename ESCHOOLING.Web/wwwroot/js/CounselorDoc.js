function ValidateCounselorInputs() {
    var validEmailRegex = /^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$/;

    let name = document.getElementById('counselorName').value;
    if (name == "") {
        Swal.fire('Error!', 'Name must be filled out!', 'error');
        return false;
    }

    let email = document.getElementById('counselorEmail').value;
    if (email == "") {
        Swal.fire('Error!', 'Email must be filled out!', 'error');
        return false;
    } else if (!email.match(validEmailRegex)) {
        Swal.fire('Error!', 'Invalid email address.', 'error');
        return false;
    }

    let mobileNo = document.getElementById('counselorMobileNo').value;
    if (mobileNo == "") {
        Swal.fire('Error!', 'MobileNumber must be filled out!', 'error');
        return false;
    }

    let address = document.getElementById('counselorAddress').value;
    if (address == "") {
        Swal.fire('Error!', 'Address must be filled out!', 'error');
        return false;
    }

    return true;
}

function SaveCounselorBtn_Click() {
    if (!ValidateCounselorInputs()) {
        return;
    }

    var counselorObj = {
        Name: $('#counselorName').val(),
        Email: $('#counselorEmail').val(),
        MobileNo: $('#counselorMobileNo').val(),
        Address: $('#counselorAddress').val(),
        Specialization: $('#counselorSpecialization').val()
    };

    $.ajax({
        type: 'POST',
        url: '/Counselor/Create',
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        data: counselorObj,
        success: function (result) {
            if (result.success == true) {
                Swal.fire('Good job!', 'Counsellor added successfully!', 'success');
                ClearCounselorForm();
            } else {
                Swal.fire('Error!', 'Failed to add counselor!', 'error');
            }
        },
        error: function () {
            Swal.fire('Error!', 'Failed to add counselor!', 'error');
        }
    });
}

function ClearCounselorForm() {
    document.getElementById('counselorName').value = "";
    document.getElementById('counselorEmail').value = "";
    document.getElementById('counselorMobileNo').value = "";
    document.getElementById('counselorAddress').value = "";
    document.getElementById('counselorSpecialization').value = "";
}
