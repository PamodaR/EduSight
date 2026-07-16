function ValidateEnterBehaviourInputs() {
    let studentId = document.getElementById('studentId').value;
    if (studentId == "") {
        Swal.fire('Error!', 'Please select a student!', 'error');
        return false;
    }

    let date = document.getElementById('Date').value;
    if (date == "") {
        Swal.fire('Error!', 'Please select a date!', 'error');
        return false;
    }

    let behaviourType = document.getElementById('BehaviourType').value;
    if (behaviourType == "") {
        Swal.fire('Error!', 'Please select a behaviour type!', 'error');
        return false;
    }

    let description = document.getElementById('Description').value;
    if (description == "") {
        Swal.fire('Error!', 'Description must be filled out!', 'error');
        return false;
    }

    return true;
}

function EnterBehaviourBtn_Click() {
    if (!ValidateEnterBehaviourInputs()) {
        return;
    }

    var entryObj = {
        studentId: $('#studentId').val(),
        date: $('#Date').val(),
        behaviourType: $('#BehaviourType').val(),
        description: $('#Description').val()
    };

    $.ajax({
        type: 'POST',
        url: '/Teacher/EnterBehaviour',
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        data: entryObj,
        success: function (result) {
            if (result.success == true) {
                Swal.fire('Good job!', 'Behaviour note saved successfully!', 'success');
                document.getElementById('Description').value = "";
            } else {
                Swal.fire('Error!', result.message || 'Save failed!', 'error');
            }
        },
        error: function () {
            Swal.fire('Error!', 'Save failed!', 'error');
        }
    });
}
