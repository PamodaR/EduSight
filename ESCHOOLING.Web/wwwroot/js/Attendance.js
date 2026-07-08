


function PresentBtn_Click(id) {
    let PeleId = "presentBtn{" + id + "}";
    let AeleId = "absentBtn{" + id + "}";
    let presentBtn = document.getElementById(PeleId);
    let absentBtn = document.getElementById(AeleId);

    var AttendanceObj = {
        StudentId: id,
        IsPresent: true
    };

    $.ajax({
        type: 'Post',
        url: '/Teacher/UpdateAttendance',
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        data: AttendanceObj,
        success: function (result) {
            if (result.success == true) {
                Swal.fire(
                    'Success!',
                    'Attendance Updated.',
                    'success'
                ).then(() => {
                    presentBtn.style.backgroundColor = '#4caf50';
                    absentBtn.style.backgroundColor = 'Red';
                });


            } else {
                Swal.fire(
                    'Error!',
                    'Attendance not Updated.',
                    'error'
                )
            }
        },
        error: function () {
            alert('Failed to receive the Data');
            console.log('Failed ');
        }
    })
}

function AbsentBtn_Click(id) {

    let presentBtn = document.getElementById('presentBtn{' + id + '}');
    let absentBtn = document.getElementById('absentBtn{' + id + '}');

    var AttendanceObj = {
        StudentId:id,
        IsPresent: false
    };

    $.ajax({
        type: 'Post',
        url: '/Teacher/UpdateAttendance',
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        data: AttendanceObj,
        success: function (result) {
            if (result.success == true) {
                Swal.fire(
                    'Success!',
                    'Attendance Updated.',
                    'success'
                ).then(() => {
                    presentBtn.style.backgroundColor = 'Red';
                    absentBtn.style.backgroundColor = '#4caf50';
                });

               
            } else {
                Swal.fire(
                    'Error!',
                    'Attendance not Updated.',
                    'error'
                )
            }
        },
        error: function () {
            alert('Failed to receive the Data');
            console.log('Failed ');
        }
    })
}

function ViewAttendanceBtn_Click(id) {
    var AttendanceObj = {
        StudentId: id,
    };

    $.ajax({
        type: 'Post',
        url: '/Teacher/ViewAttendanceDetails',
        contentType: 'application/x-www-form-urlencoded; charset=UTF-8',
        data: AttendanceObj,
        success: function (result) {
            if (result.success == true) {

            } else {

            }
        },
        error: function () {
            alert('Failed to receive the Data');
            console.log('Failed ');
        }
    });
}