function ValidateEnterMarksInputs() {
    let studentId = document.getElementById('studentId').value;
    if (studentId == "") {
        Swal.fire('Error!', 'Please select a student!', 'error');
        return false;
    }

    let term = document.getElementById('Term').value;
    if (term == "") {
        Swal.fire('Error!', 'Please select a term!', 'error');
        return false;
    }

    let subject = document.getElementById('Subject').value;
    if (subject == "") {
        Swal.fire('Error!', 'Subject must be filled out!', 'error');
        return false;
    }

    let marks = document.getElementById('Marks').value;
    if (marks == "") {
        Swal.fire('Error!', 'Marks must be filled out!', 'error');
        return false;
    }

    return true;
}

function EnterMarksBtn_Click() {
    if (!ValidateEnterMarksInputs()) {
        return;
    }

    var entryObj = {
        studentId: $('#studentId').val(),
        term: $('#Term').val(),
        subject: $('#Subject').val(),
        marks: $('#Marks').val()
    };

    $.ajax({
        type: 'POST',
        url: '/Teacher/EnterMarks',
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        data: entryObj,
        success: function (result) {
            if (result.success == true) {
                Swal.fire('Good job!', 'Mark saved successfully!', 'success');
                document.getElementById('Subject').value = "";
                document.getElementById('Marks').value = "";
            } else {
                Swal.fire('Error!', result.message || 'Save failed!', 'error');
            }
        },
        error: function () {
            Swal.fire('Error!', 'Save failed!', 'error');
        }
    });
}
