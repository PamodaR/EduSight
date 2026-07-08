function UserRegistrationBtn_Click(id) {

    let success = ValidationRegisterForm();

    if (success == true)
    {
        var regObj = {
            Username: $('#userName').val(),
            Email: $('#email').val(),
            Password: $('#password').val(),
            Address: $('#address').val(),
            MobileNo: $('#mobileNo').val(),
            UserType: id,
            IsActive: true
        };

        $.ajax({
            type: 'POST',
            url: '/Auth/Register',
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



