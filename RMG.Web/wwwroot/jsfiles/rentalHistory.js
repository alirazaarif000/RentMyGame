var dataTable;
$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    dataTable = $('#tblData').DataTable({
        ajax: {
            "url": '/customer/rented/getRentalHistory',
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
            {
                data: "rentalDate", width: "15%", render: function (data, type, row) {
                    if (type === 'display' || type === 'filter') {
                        return new Date(data).toLocaleString('en-US', {
                            year: 'numeric',
                            month: 'numeric',
                            day: 'numeric',
                            hour: 'numeric',
                            minute: 'numeric',
                            second: 'numeric',
                            hour12: true
                        });
                    }
                    return data;
                }
            },
            {
                data: "returnDate", width: "15%", render: function (data, type, row) {
                    if (type === 'display' || type === 'filter') {
                        return new Date(data).toLocaleString('en-US', {
                            year: 'numeric',
                            month: 'numeric',
                            day: 'numeric',
                            hour: 'numeric',
                            minute: 'numeric',
                            second: 'numeric',
                            hour12: true
                        });
                    }
                    return data;
                }
            },
            { data: "status", width: "5%" },
            {
                data: null,
                render: function (data, type, row) {
                    var gameId = row.game.id; // Assuming gameId is part of the game object in the JSON data
                    return `<div class="w-75" role="group">
                        <a href="/customer/games/RentGame?&gameId=${gameId}" class="btn btn-danger mx-2 text-white">Rent Again</a>
                    </div>`;
                },
                width: "10%"
            }
        ]
    });
}

