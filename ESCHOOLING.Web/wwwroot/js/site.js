// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.

// Toggles a password input between hidden (dots) and visible (plain text).
function TogglePasswordVisibility(inputId, iconEl) {
    var input = document.getElementById(inputId);
    if (!input) {
        return;
    }

    if (input.type === 'password') {
        input.type = 'text';
        iconEl.classList.add('password-toggle-icon--visible');
        iconEl.setAttribute('title', 'Hide password');
    } else {
        input.type = 'password';
        iconEl.classList.remove('password-toggle-icon--visible');
        iconEl.setAttribute('title', 'Show password');
    }
}

// Uploads the selected file from #profilePictureInput to the given controller URL,
// updating #profilePicturePreview on success. Used by each role's My Profile page.
function UploadProfilePictureBtn_Click(url) {
    var fileInput = document.getElementById('profilePictureInput');
    if (!fileInput.files || fileInput.files.length === 0) {
        Swal.fire('Error!', 'Please choose a file first.', 'error');
        return;
    }

    var formData = new FormData();
    formData.append('profilePicture', fileInput.files[0]);

    $.ajax({
        type: 'POST',
        url: url,
        data: formData,
        processData: false,
        contentType: false,
        success: function (result) {
            if (result.success) {
                document.getElementById('profilePicturePreview').src = result.imageUrl + '?t=' + Date.now();
                Swal.fire('Good job!', 'Profile picture updated!', 'success');
            } else {
                Swal.fire('Error!', result.errorMessage || 'Upload failed.', 'error');
            }
        },
        error: function () {
            Swal.fire('Error!', 'Upload failed.', 'error');
        }
    });
}

// Prompts for a new password via SweetAlert2, then POSTs it to the given Admin reset-password
// action for the given user. Used by the Reset Password button on Edit Teacher/Parent/Student.
function ResetPasswordBtn_Click(userId, url) {
    Swal.fire({
        title: 'Reset Password',
        icon: 'warning',
        input: 'password',
        inputLabel: 'New password',
        inputPlaceholder: 'Enter a new password',
        showCancelButton: true,
        confirmButtonText: 'Reset',
        confirmButtonColor: '#e0a30d',
        inputValidator: function (value) {
            if (!value) {
                return 'Password cannot be empty';
            }
        }
    }).then(function (result) {
        if (!result.isConfirmed) {
            return;
        }

        $.ajax({
            type: 'POST',
            url: url,
            contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
            data: { id: userId, newPassword: result.value },
            success: function (response) {
                if (response.success) {
                    Swal.fire('Done!', 'Password has been reset.', 'success');
                    var passwordField = document.getElementById('Password');
                    if (passwordField) {
                        passwordField.value = result.value;
                    }
                } else {
                    Swal.fire('Error!', 'Failed to reset password.', 'error');
                }
            },
            error: function () {
                Swal.fire('Error!', 'Failed to reset password.', 'error');
            }
        });
    });
}
