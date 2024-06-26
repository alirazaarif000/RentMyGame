$(document).ready(function () {
    loadDataTable();
});

function loadDataTable() {
    $.ajax({
        url: '/admin/review/getall',
        type: 'GET',
        success: function (data) {
            console.log("JSON Response:", data); // Log the JSON response to the console

            $('#tblData').DataTable({
                data: data, // Directly use the data array from the response
                columns: [
                    { "title": "User Email", "data": "applicationUser.userName", "defaultContent": "" },
                    { "title": "Game Name", "data": "game.name", "defaultContent": "" },
                    {
                        "title": "Rating",
                        "data": 'rating', // Column for image URL
                        render: function (data, type, full, meta) {
                            return getStarRatingHtml(data);
                        },
                        width: "20%"
                    },
                    { "title": "Subject", "data": "subject", "defaultContent": "" },
                    { "title": "Comments", "data": "comment", "defaultContent": "" },
                    {
                        "title":"Action",
                        "data": "id",
                        "render": function (data) {
                            return `<div class="w-75 " role="group">
                        <a href="/admin/review/upsert?id=${data}"> Approve</a>
                        <a OnClick=Delete("/admin/game/delete/${data}") > Reject</a>
                    </div>`
                        },
                        "width": "10%"
                    },
                    // Add more columns as needed
                ]
            });
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.error("Error fetching data:", textStatus, errorThrown);
        }
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