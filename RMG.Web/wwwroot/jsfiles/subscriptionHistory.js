var dataTable;
var GlobalVar = {SubscriptionList: []}
$(document).ready(function () {
    loadDataTable();
});

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
                            if (row.status == 'Active') {
                                return `<div class="w-75" role="group">
                                            <a href="/customer/BuySubscription/" class="btn btn-danger mx-2 text-white">Renew Subscription</a>
                                        </div>`;
                            } else {
                                return '';
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