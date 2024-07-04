var dataTable;
var GlobalVar = {SubscriptionList: []}
$(document).ready(function () {
    loadDataTable();
    //BindSubscriptionList();
});

//function loadDataTable() {
//    debugger
//    dataTable = $('#tblData').DataTable({
//        ajax: {
//            "url": '/customer/subscriptionHistory/GetSubscriptionHistory',
//            "type": 'GET',
//            "dataSrc": function (json) {
//                console.log("JSON Response:", json); // Log the JSON response to the console
//                return json; // Directly return the JSON array
//            }
//             data: data
//        },
//        columns: [
//            { title: "Subscription Name", data: "subscription.packageName", width: "20%" },
//            { title:"Start Date", data: "startDate", width: "15%" },
//            { title:"End Date", data: "endDate", width: "15%" },
//            { title: "Status", data: "status", width: "5%" }

//        ]
//    });
//}
function loadDataTable() {
    $.ajax({
        url: '/customer/subscriptionHistory/GetSubscriptionHistory',
        type: 'GET',
        success: function (data) {
            console.log("JSON Response:", data); // Log the JSON response to the console

            $('#tblData').DataTable({
                data: data, // Directly use the data array from the response
                columns: [
                    { title: "Subscription Name", data: "subscription.packageName", width: "20%" },
                    { title: "Start Date", data: "startDate", width: "15%" },
                    { title: "End Date", data: "endDate", width: "15%" },
                    { title: "Status", data: "status", width: "5%" },
                    {
                        title:"Action",
                        render: function (data, type, row) {
                            var id = row.id;
                            var gameid = row.subscription.id; // assuming gameid is part of the game object in the json data
                            if (data.status == 'Active') {
                                return `<div class="w-75" role="group">
                                            <a href="/customer/games/removerent?" class="btn btn-danger mx-2 text-white">Renew Subscription</a>
                                        </div>`;
                            }
                        },
                        width: "10%"
                    }      
                ]
            });
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.error("Error fetching data:", textStatus, errorThrown);
        }
    });
}
function BindSubscriptionList() {
    $.ajax({
        url: '/customer/subscriptionHistory/GetSubscriptionHistory',
        method: 'GET',
        success: function (data) {
            GlobalVar.SubscriptionList = data;
            var tbody = $('#subscriptionList');
            console.log(GlobalVar.SubscriptionList);
            GlobalVar.SubscriptionList.forEach(function (sub) {
                var html = `
                    <tr>
                        <td class="digits">${sub.subscription.packageName}</td>
                        <td class="digits">${sub.startDate}</td>
                        <td class="digits">${sub.endDate}</td>
                        <td class="digits">${sub.status}</td>
                        </tr>
                    `;
                tbody.append(html);
            })
        },
        error: function (xhr, status, error) {
            console.error('Failed to fetch games:', error);
        }
    });
}