var GlobalVar = { subscriptions: [], UserSubscription: null }
$(document).ready(function () {
    function loadSubscriptionData() {
        $.ajax({
            url: '/customer/BuySubscription/GetSubscriptionsData',
            method: 'GET',
            success: function (data) {
                GlobalVar.subscriptions = data;
                var body = $('#subscription');
                body.empty();
                GlobalVar.subscriptions.forEach(function (sub) {
                    debugger
                    var html = `
                    <div class="subscription-card shadow p-3 mb-5 bg-white rounded">
                        <div class="compare-part">
                            <div class="img-secton">
                                <div></div>
                                <a href="#">
                                    <h2 class="text-primary-emphasis">${sub.packageName}</h2>
                                </a>
                            </div>
                            <div class="detail-part">
                                <div class="title-detail">
                                    <h6>Total Rent Game</h6>
                                </div>
                                <div class="inner-detail">
                                    <h5>${sub.rentCount}</h5>
                                </div>
                            </div>
                            <div class="detail-part">
                                <div class="title-detail">
                                    <h6>Rent New Released Games</h6>
                                </div>
                                <div class="inner-detail">
                                    <h5>${sub.newGameCount}</h5>
                                </div>
                            </div>
                            <div class="detail-part">
                                <div class="title-detail">
                                    <h6>Replace Games</h6>
                                </div>
                                <div class="inner-detail">
                                    <h5>${sub.replaceCount}</h5>
                                </div>
                            </div>
                            <div class="detail-part">
                                <div class="title-detail">
                                    <h6>Enter No of Month</h6>
                                </div>
                                <div class="inner-detail">
                                    <input class="form-control month-input" type="number" name="month" data-id="${sub.id}" data-price="${sub.price}" value="1" min="1" />
                                </div>
                            </div>
                            <div class="detail-part">
                                <div class="title-detail">
                                    <h6>Price</h6>
                                </div>
                                <div class="inner-detail">
                                    <h5 class="total-price" data-id="${sub.id}">${calculateTotalPrice(sub.price, 1, sub.id)}</h5>
                                </div>
                            </div>
                            <div class="btn-part">
                                <button class="btn btn-solid buy-now" data-id="${sub.id}" data-price="${sub.price}">Buy Now</button>
                            </div>
                        </div>
                    </div>
                    `;
                    body.append(html);
                });

                // Attach event listeners after the elements are added to the DOM
                $('.month-input').on('input', function () {
                    var month = $(this).val();
                    if (month < 1 || month === '') {
                        month = 1;
                        $(this).val(1);
                    }

                    var price = $(this).data('price');
                    var subscriptionId = $(this).data('id');
                    var totalPrice = calculateTotalPrice(price, month, subscriptionId);
                    $(this).closest('.subscription-card').find('.total-price').text(totalPrice);
                });

                $('.buy-now').on('click', function () {
                    var subscriptionId = $(this).data('id');
                    var month = $(this).closest('.subscription-card').find('.month-input').val();
                    console.log(subscriptionId, month);
                    var totalPrice = $(this).closest('.subscription-card').find('.total-price').text();

                    // Make the API call to buy the subscription
                    $.ajax({
                        url: '/customer/BuySubscription/Buy',
                        method: 'POST',
                        data: {
                            SubcriptionId: subscriptionId,
                            NoOfMonths: month,
                            PricePaid: totalPrice
                        },
                        success: function (data) {
                            //setTimeout(function () {
                            //    window.location.href = '/';
                            //}, 2000);

                            if (data.status) {
                                toastr.success(data.message);
                                GetUserSubscription();
                            }
                            else {
                                toastr.error(data.message);
                            }

                            // Handle success response

                        },
                        error: function (xhr, status, error) {
                            console.error('Failed to purchase subscription:', error);
                            // Handle error response
                        }

                    });
                });
                updateSubscriptionButtons();
            },
            error: function (xhr, status, error) {
                console.error('Failed to fetch subscriptions:', error);
            }
        });
    }


    function GetUserSubscription() {
        $.ajax({
            url: '/customer/BuySubscription/GetUserSubscription',
            method: 'GET',
            success: function (data) {
                GlobalVar.UserSubscription = data;
                loadSubscriptionData();
            }

        })
    }
    function updateSubscriptionButtons() {
        debugger
        if (GlobalVar.UserSubscription) {
            var button = $(`.buy-now[data-id='${GlobalVar.UserSubscription.subscriptionId}']`);
            if (button.length) {
                button.text('Update');
            }
        }
    }
    function BuySubscription() {
        $.ajax({
            url: '/customer/BuySubscription/Buy',
            method: 'POST',
            data: {
                SubcriptionId: subscriptionId,
                NoOfMonths: month,
                PricePaid: totalPrice
            },
            success: function (response) {
                setTimeout(function () {
                    window.location.href = '/';
                }, 2000);
                toastr.success('Subscription purchased successfully!');
                // Handle success response
            },
            error: function (xhr, status, error) {
                console.error('Failed to purchase subscription:', error);
                // Handle error response
            }
        });
    }
    function calculateTotalPrice(price, months, subscribedId) {
        debugger;
        if (GlobalVar.UserSubscription) {
            if (GlobalVar.UserSubscription.subscriptionId == subscribedId) {
                if (months > GlobalVar.UserSubscription.remainingMonths) {
                    return price * (months-GlobalVar.UserSubscription.remainingMonths);
                }
                else {
                    var remainingMonths = GlobalVar.UserSubscription.remainingMonths;
                    return (price * months) - (GlobalVar.UserSubscription.subscription.price * remainingMonths);
                }
            }
            else
            {
                var remainingMonths = GlobalVar.UserSubscription.remainingMonths - 1;
                return (price * months) - (GlobalVar.UserSubscription.subscription.price * remainingMonths);

            }
            
        }
        return price * months;
    }
    GetUserSubscription();
});
