var dataTable;
$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        ajax: {
            "url": '/customer/rented/getall',
            "type": 'GET',
            "dataSrc": function (json) {
                console.log("JSON Response:", json); // Log the JSON response to the console
                return json; // Directly return the JSON array
            }
        },
        columns: [
            {
                data: 'game.imageUrl', // Column for image URL
                render: function (data, type, full, meta) {
                    return '<img src="' + data + '" data-field="image" style="width: 100px; height: auto;">';
                },
                width: "20%"
            },
            { data: "game.name", width: "20%" },
            { data: "rentalDate", width: "15%" },
            { data: "returnDate", width: "15%" },
            { data: "status", width: "5%" },
            {
                data: null,
                render: function (data, type, row) {
                    var id = row.id;
                    var gameId = row.game.id; // Assuming gameId is part of the game object in the JSON data
                    return `<div class="w-75" role="group">
                        <a href="/customer/games/RemoveRent?id=${id}&gameId=${gameId}" class="btn btn-danger mx-2 text-white">Return Game</a>
                    </div>`;
                },
                width: "10%"
            }
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
            });
        }
    });
}
