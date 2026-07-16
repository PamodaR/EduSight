function ValidateSendNoteInputs() {
    let noteText = document.getElementById('NoteText').value;
    if (noteText == "") {
        Swal.fire('Error!', 'Please write a note before sending!', 'error');
        return false;
    }

    return true;
}

function SendNoteBtn_Click() {
    if (!ValidateSendNoteInputs()) {
        return;
    }

    var noteObj = {
        noteText: $('#NoteText').val()
    };

    $.ajax({
        type: 'POST',
        url: '/Parent/SendNote',
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        data: noteObj,
        success: function (result) {
            if (result.success == true) {
                Swal.fire('Good job!', 'Note sent to the teacher successfully!', 'success');
                document.getElementById('NoteText').value = "";
            } else {
                Swal.fire('Error!', result.message || 'Save failed!', 'error');
            }
        },
        error: function () {
            Swal.fire('Error!', 'Save failed!', 'error');
        }
    });
}
