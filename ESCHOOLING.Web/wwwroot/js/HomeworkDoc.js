function ValidateHomeworkInputs() {
    let grade = document.getElementById('grade').value;
    if (grade == "") {
        Swal.fire('Error!', 'Grade must be selected!', 'error');
        return false;
    }

    let subject = document.getElementById('subject').value;
    if (subject == "") {
        Swal.fire('Error!', 'Subject must be filled out!', 'error');
        return false;
    }

    let description = document.getElementById('description').value;
    if (description == "") {
        Swal.fire('Error!', 'Description must be filled out!', 'error');
        return false;
    }

    let dueDate = document.getElementById('dueDate').value;
    if (dueDate == "") {
        Swal.fire('Error!', 'Due date must be filled out!', 'error');
        return false;
    }

    return true;
}

function SaveHomeworkBtn_Click() {
    if (!ValidateHomeworkInputs()) {
        return;
    }

    var homeworkObj = {
        grade: $('#grade').val(),
        subject: $('#subject').val(),
        description: $('#description').val(),
        dueDate: $('#dueDate').val()
    };

    $.ajax({
        type: 'POST',
        url: '/Teacher/SaveHomework',
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        data: homeworkObj,
        success: function (result) {
            if (result.success == true) {
                Swal.fire('Good job!', 'Homework posted successfully!', 'success');
                ClearHomeworkForm();
            } else {
                Swal.fire('Error!', 'Failed to post homework!', 'error');
            }
        },
        error: function () {
            Swal.fire('Error!', 'Failed to post homework!', 'error');
        }
    });
}

function ClearHomeworkForm() {
    document.getElementById('grade').value = "";
    document.getElementById('subject').value = "";
    document.getElementById('description').value = "";
    document.getElementById('dueDate').value = "";
}
