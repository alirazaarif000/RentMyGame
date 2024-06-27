var dataTable;
$(document).ready(function () {
    loadDataTable();
})
function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        ajax: {
            "url": '/admin/users/getall',
            "type": 'GET'
        },
        columns: [
            { title:"Email", data: "email", "width": "25%" },
            { title:"Package", data: "subscription.packageName", "width": "25%" },
            { title:"Role", data: "role", "width": "25%" },
            {
                title:"Action",
                data: "id",
                "render": function (data) {
                    return `<div class="w-75 " role="group">
                        <a OnClick=Delete("/admin/users/delete/${data}") class="btn btn-primary mx-2"> <i class="fa fa-trash"></i> Delete</a>
                    </div>`
                },
                "width": "25%"
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
                success: function (data) {
                    dataTable.ajax.reload();
                    toastr.success(data.message);
                }
            })
        }
    });
}