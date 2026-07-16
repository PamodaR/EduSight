function StudentRegistrationBtn_Click(id) {

    let success = ValidationRegisterForm();

    if (success == true) {
        var regObj = {
            Username: $('#userName').val(),
            Email: $('#email').val(),
            Password: $('#password').val(),
            Address: $('#address').val(),
            MobileNo: $('#mobileNo').val(),
            Grade: $('#grade').val(),
            UserType: id,
            IsActive: true
        };

        $.ajax({
            type: 'POST',
            url: '/Teacher/SaveStudent',
            contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
            data: regObj,
            success: function (result) {
                if (result.success == true) {
                    Swal.fire(
                        'Good job!',
                        'Registration Successfull Completed!',
                        'success'
                    )
                    Clear();
                } else {
                    Swal.fire(
                        'Error!',
                        'Registration Failed!',
                        'error'
                    )
                    Clear();
                }
            },
            error: function () {
                alert('Failed to receive the Data');
                console.log('Failed ');
            }
        })
    }
}

function ValidationRegisterForm() {

    var validEmailRegex = /^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$/;

    let Username = document.getElementById("userName").value;
    if (Username == "") {
        Swal.fire(
            'Error!',
            'Name must be filled out!',
            'error'
        )
        return false;
    }

    let Email = document.getElementById("email").value;
    if (Email == "") {
        Swal.fire(
            'Error!',
            'Email must be filled out!',
            'error'
        )
        return false;
    } else if (!Email.match(validEmailRegex)) {
        alert("Invalid email address.");
        return false;
    }

    let Password = document.getElementById("password").value;
    if (Password == "") {
        Swal.fire(
            'Error!',
            'Password must be filled out!',
            'error'
        )
        return false;
    }

    let MobileNumber = document.getElementById("mobileNo").value;
    if (MobileNumber == "") {
        Swal.fire(
            'Error!',
            'MobileNumber must be filled out!',
            'error'
        )
        return false;
    }

    let Address = document.getElementById("address").value;
    if (Address == "") {
        Swal.fire(
            'Error!',
            'Address must be filled out!',
            'error'
        )
        return false;
    }

    return true;
}

function Clear() {
    document.getElementById('userName').value = "";
    document.getElementById('email').value = "";
    document.getElementById('password').value = "";
    document.getElementById('address').value = "";
    document.getElementById('mobileNo').value = "";
    document.getElementById('grade').value = "";
}

// ---- Admin "Manage Users" tabs page: each pane (Teacher/Parent/Student) has its
// ---- own field id prefix so the three forms can coexist on one page.

function ValidateRoleForm(prefix, includeGrade) {

    var validEmailRegex = /^[a-zA-Z0-9.!#$%&'*+/=?^_`{|}~-]+@[a-zA-Z0-9-]+(?:\.[a-zA-Z0-9-]+)*$/;

    let Username = document.getElementById(prefix + "UserName").value;
    if (Username == "") {
        Swal.fire('Error!', 'Name must be filled out!', 'error');
        return false;
    }

    let Email = document.getElementById(prefix + "Email").value;
    if (Email == "") {
        Swal.fire('Error!', 'Email must be filled out!', 'error');
        return false;
    } else if (!Email.match(validEmailRegex)) {
        Swal.fire('Error!', 'Invalid email address.', 'error');
        return false;
    }

    let Password = document.getElementById(prefix + "Password").value;
    if (Password == "") {
        Swal.fire('Error!', 'Password must be filled out!', 'error');
        return false;
    }

    let MobileNumber = document.getElementById(prefix + "MobileNo").value;
    if (MobileNumber == "") {
        Swal.fire('Error!', 'MobileNumber must be filled out!', 'error');
        return false;
    }

    let Address = document.getElementById(prefix + "Address").value;
    if (Address == "") {
        Swal.fire('Error!', 'Address must be filled out!', 'error');
        return false;
    }

    if (includeGrade) {
        let Grade = document.getElementById(prefix + "Grade").value;
        if (Grade == "") {
            Swal.fire('Error!', 'Grade must be filled out!', 'error');
            return false;
        }
    }

    return true;
}

function ClearRoleForm(prefix, includeGrade) {
    document.getElementById(prefix + 'UserName').value = "";
    document.getElementById(prefix + 'Email').value = "";
    document.getElementById(prefix + 'Password').value = "";
    document.getElementById(prefix + 'Address').value = "";
    document.getElementById(prefix + 'MobileNo').value = "";
    if (includeGrade) {
        document.getElementById(prefix + 'Grade').value = "";
    }
    var childStudentIdField = document.getElementById(prefix + 'ChildStudentId');
    if (childStudentIdField) {
        childStudentIdField.value = "";
    }
}

function SubmitRoleRegistration(prefix, url, includeGrade, includeChildStudentId) {

    let success = ValidateRoleForm(prefix, includeGrade);
    if (success != true) {
        return;
    }

    var regObj = {
        Username: $('#' + prefix + 'UserName').val(),
        Email: $('#' + prefix + 'Email').val(),
        Password: $('#' + prefix + 'Password').val(),
        Address: $('#' + prefix + 'Address').val(),
        MobileNo: $('#' + prefix + 'MobileNo').val()
    };

    if (includeGrade) {
        regObj.Grade = $('#' + prefix + 'Grade').val();
    }

    if (includeChildStudentId) {
        regObj.ChildStudentId = $('#' + prefix + 'ChildStudentId').val();
    }

    $.ajax({
        type: 'POST',
        url: url,
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        data: regObj,
        success: function (result) {
            if (result.success == true) {
                Swal.fire('Good job!', 'Registration Successfully Completed!', 'success');
                ClearRoleForm(prefix, includeGrade);
            } else {
                Swal.fire('Error!', 'Registration Failed!', 'error');
            }
        },
        error: function () {
            Swal.fire('Error!', 'Registration Failed!', 'error');
        }
    });
}

function RegisterTeacherBtn_Click() {
    SubmitRoleRegistration('teacher', '/Admin/RegisterTeacher', false, false);
}

function RegisterParentBtn_Click() {
    SubmitRoleRegistration('parent', '/Admin/RegisterParent', false, true);
}

function RegisterStudentBtn_Click() {
    SubmitRoleRegistration('student', '/Admin/RegisterStudent', true, false);
}

