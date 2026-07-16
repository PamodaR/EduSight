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
