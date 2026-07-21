function ShowNoteDetailsBtn_Click(row) {
    var container = document.createElement('div');
    container.style.textAlign = 'left';

    function addField(label, value) {
        var p = document.createElement('p');
        var strong = document.createElement('strong');
        strong.textContent = label + ': ';
        p.appendChild(strong);
        p.appendChild(document.createTextNode(value || ''));
        container.appendChild(p);
    }

    addField('Student', row.dataset.student);
    addField('Parent', row.dataset.parent);
    addField('Date', row.dataset.date);

    container.appendChild(document.createElement('hr'));

    var noteP = document.createElement('p');
    noteP.style.whiteSpace = 'pre-wrap';
    noteP.textContent = row.dataset.note || '';
    container.appendChild(noteP);

    Swal.fire({
        title: 'Parent Note',
        html: container,
        confirmButtonText: 'Close'
    });
}

$(document).ready(function () {
    $('.parent-note-row').on('click', function () {
        ShowNoteDetailsBtn_Click(this);
    });
});
