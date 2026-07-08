function LoginBtn_Click(){
    let success = ValidationRegisterForm();

    if (success == true)
    {
        var loginObj = {
            Email: document.getElementById("email").value,
            Password: document.getElementById("password").value
        };

        $.ajax({
            type: 'POST',
            url: '/Auth/Login',
            contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
            data: loginObj,
            success: function (result) {
                if (result.success == false) {
                    Swal.fire(
                        'Invalid Credentials!',
                        'User not found, Please Try Again!',
                        'error'
                    )
                }
                else if (result.newUrl != null) {

                    window.location.replace(result.newUrl);
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

    let Email = document.getElementById("email").value;
    if (Email == "") {
        Swal.fire(
            'Error!',
            'Email must be filled out!',
            'error'
        )
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

    return true;
}