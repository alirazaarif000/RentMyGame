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
                    { title: "Subscription Name", data: "subscription.packageName", "width": "15%" },
                    {
                        title: "Start Date", data: "startDate", "width": "10%", render: function (data, type, row) {
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
                        title: "End Date", data: "endDate", "width": "10%",
                        render: function (data, type, row) {
                            // Check if type is display or filter
                            if (type === 'display' || type === 'filter') {
                                // Format date to AM/PM format
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
                            return data;  // Return data for other types (sort, type, etc.)
                        }
                    },
                    { title: "No of Months", data: "noOfMonths" },
                    { title: "Remaining Months", data: "remainingMonths" },
                    { title: "Price Paid", data: "pricePaid"},
                    { title: "Status", data: "status"},
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
                        "width": "10%"
                    }      
                ]
            });
        },
        error: function (jqXHR, textStatus, errorThrown) {
            console.error("Error fetching data:", textStatus, errorThrown);
        }
    });
}