
$(document).ready(function () {

    function loadDashboardStats() {
        $.ajax({
            url: '/admin/home/getstats',
            method: 'GET',
            success: function (data) {
                $('#earning').text(data.totalEarning);
                $('#newGames').text(data.gameCount);
                $('#rentedGames').text(data.rentalCount);
                $('#newUsers').text(data.userCount);
            },
            error: function (xhr, status, error) {
                console.error('Failed to fetch genres:', error);
            }
        });
    }
    function getLastestProducts() {
        $.ajax({
            url: '/admin/home/GetLatestProduct',
            method: 'GET',
            success: function (data) {
                var products = data;
                var tbody = $('#latestProducts');

                products.forEach(function (product) {
                    var html = `
                    <tr>
                        <td>${product.id}</td>
                        <td class="digits">${product.name}</td>
                        <td class="digits">${product.genre.genreName}</td>
                        <td class="digits">${product.platform.platformName}</td>
                        <td class="digits">${product.releaseDate}</td>
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
    getLastestProducts();
    loadDashboardStats();
});