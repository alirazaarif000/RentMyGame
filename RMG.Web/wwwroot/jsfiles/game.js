var dataTable;
$(document).ready(function () {
    loadDataTable();
})
function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        ajax: {
            "url": '/admin/game/getall',
            "type": 'GET'
        },
        columns: [
            {
                data: 'imageUrl', // Column for image URL
                render: function (data, type, full, meta) {
                    return '<img src="' + data + '" data-field="image">';
                },
                width:"20%"
            },
            { data: "name", "width": "20%" },
            { data: "genre.genreName", "width": "15%" },
            { data: "platform.platformName", "width": "15%" },
            { data: "stock", "width": "5%" },
            { data: "available", "width": "15%" },
            {
                data: "id",
                "render": function (data) {
                    return `<div class="w-75 " role="group">
                        <a href="/admin/game/upsert?id=${data}"> <i class="fa fa-edit" title="Edit" ></i></a>
                        <a OnClick=Delete("/admin/game/delete/${data}") > <i class="fa fa-trash" title="Delete" ></i></a>
                    </div>`
                },
                "width":"10%"
            },
        ]
    });
}

function Delete(url) {
    Swal.fire({
        title: "Are you sure?",
        text: "You won't be able to revert this!",
        icon: "warning",
        showCancelButton: true,
        confirmButtonColor: "#3085d6",
        cancelButtonColor: "#d33",
        confirmButtonText: "Yes, delete it!"
    }).then((result) => {
        if (result.isConfirmed) {
            $.ajax({
                url: url,
                type: 'DELETE',
                success: function(data) {
                    dataTable.ajax.reload();
                    toastr.success(data.message);
                }
            })
        }
    });
}