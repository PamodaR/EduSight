document.addEventListener('DOMContentLoaded', function () {
    document.querySelectorAll('[data-table-search-target]').forEach(function (searchInput) {
        var table = document.getElementById(searchInput.dataset.tableSearchTarget);
        if (!table || !table.tBodies[0]) {
            return;
        }
        var rows = table.tBodies[0].rows;

        searchInput.addEventListener('input', function () {
            var term = searchInput.value.trim().toLowerCase();
            Array.prototype.forEach.call(rows, function (row) {
                row.style.display = row.textContent.toLowerCase().indexOf(term) === -1 ? 'none' : '';
            });
        });
    });
});
