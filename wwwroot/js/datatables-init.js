$(document).ready(function () {
    $('.datatable').DataTable({
        pageLength: 10,
        order: [[0, 'asc']],
        language: {
            url: "//cdn.datatables.net/plug-ins/1.13.4/i18n/tr.json"
        }
    });
});
