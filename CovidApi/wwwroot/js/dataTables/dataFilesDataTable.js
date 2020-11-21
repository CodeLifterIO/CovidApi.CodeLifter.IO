$(document).ready(function () {
    $('#dataFilesDataTable').dataTable({
        "order": [[1, "desc"]],
        "responsive": true,
        "processing": true,
        "serverSide": true,
        "filter": true,
        "ajax": {
            "url": "/data-files/datatable",
            "type": "POST",
            "datatype": "json"
        },
        "columnDefs": [
            {
                "targets": [0],
                "visible": false,
                "searchable": false
            },
        ],
        "columns": [
            { "data": "id", "name": "Id", "autoWidth": true },
            { "data": "fileName", "name": "File Name", "autoWidth": true },
            { "data": "recordsProcessed", "name": "Records Processed", "autoWidth": true },
            { "data": "completed", "name": "Completed", "autoWidth": true },
            { "data": "completedAt", "name": "Completed At", "autoWidth": true },
            {
                "render": function (data, row) { return "<a href='#' class='btn btn-danger' onclick=ProcessFile('" + row.fileName + "'); >Delete</a>"; }
            },
        ]
    });
});