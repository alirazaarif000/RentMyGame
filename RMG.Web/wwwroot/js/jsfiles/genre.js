var dataTable;
$(document).ready(function () {
    loadDataTable();
})
function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        ajax: {
            "url": '/admin/company/getall',
            "type": 'GET'
        },
        columns: [
            { data: "name", "width": "15%" },
            { data: "city", "width": "15%" },
            { data: "state", "width": "15%" },
            { data: "postalCode", "width": "15%" },
            { data: "phoneNumber", "width": "15%" },
            {
                data: "id",
                "render": function (data) {
                    return `<div class="w-75 btn-group" role="group">
                        <a href="/admin/company/upsert?id=${data}" class="btn btn-primary mx-2"> <i class="fa fa-edit"></i> Edit</a>
                        <a OnClick=Delete("/admin/company/delete/${data}") class="btn btn-danger mx-2"> <i class="fa fa-trash"></i> Delete</a>
                    </div>`
                }
            },
        ]
    });
}