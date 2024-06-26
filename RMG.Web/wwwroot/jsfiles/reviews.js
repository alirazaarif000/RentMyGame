var dataTable;
$(document).ready(function () {
    loadDataTable();
})
function loadDataTable() {
    debugger;
    dataTable = $('#tblData').DataTable({
        ajax: {
            "url": '/admin/review/getall',
            "type": 'GET'
        },
        columns: [
            { data: "applicationUser.userName", "width": "20%" },
            { data: "game.name", "width": "15%" },

            { data: "subject", "width": "5%" },
            { data: "comment", "width": "15%" },
            {
                data: "id",
                "render": function (data) {
                    return `<div class="w-75 " role="group">
                        <a href="/admin/game/upsert?id=${data}"> <i class="fa fa-edit" title="Edit" ></i></a>
                        <a OnClick=Delete("/admin/game/delete/${data}") > <i class="fa fa-trash" title="Delete" ></i></a>
                    </div>`
                },
                "width": "10%"
            },
        ]
    });
}
function getStarRatingHtml(rating) {
    var starHtml = '';
    for (var i = 1; i <= 5; i++) {
        if (i <= rating) {
            starHtml += '<i class="fa fa-star" style="color: #ffa200;"></i>';
        } else {
            starHtml += '<i class="fa fa-star" style="color: #000;"></i>';
        }
    }
    return starHtml;
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