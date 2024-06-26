$(document).ready(function () {
    function getGames() {
        debugger;
        $.ajax({
            url: '/customer/games/getall', // URL to fetch game data
            method: 'GET',
            success: function (data) {
                console.log(data);
                var gameListContainer = $('#game-list');
                gameListContainer.empty(); // Clear the existing game list
                data.forEach(function (game) {
                    var gameHtml = renderGameItem(game);
                    gameListContainer.append(gameHtml);
                });
            }
        });
    }
    function loadGenres() {
        $.ajax({
            url: '/customer/games/getgenres',
            method: 'GET',
            success: function (data) {
                var genres = data;
                var genreFilterContainer = $('#genre-filter');
                genreFilterContainer.empty(); // Clear any existing content

                // Iterate through the genre data and create checkboxes
                genres.forEach(function (genre) {
                    var checkboxHtml = `
                        <div class="form-check collection-filter-checkbox">
                            <input type="checkbox" class="form-check-input genre-checkbox" id="genre-${genre.id}" value="${genre.id}">
                            <label class="form-check-label" for="genre-${genre.id}">${genre.genreName}</label>
                        </div>
                    `;
                    genreFilterContainer.append(checkboxHtml);
                });
            },
            error: function (xhr, status, error) {
                console.error('Failed to fetch genres:', error);
            }
        });
    }

    function loadPlatforms() {
        $.ajax({
            url: '/customer/games/getplatforms',
            method: 'GET',
            success: function (data) {
                var platforms = data;
                var platformFilterContainer = $('#platform-filter');
                platformFilterContainer.empty();

                platforms.forEach(function (platform) {
                    var checkboxHtml = `
                        <div class="form-check collection-filter-checkbox">
                            <input type="checkbox" class="form-check-input platform-checkbox" id="platform-${platform.id}" value="${platform.id}">
                            <label class="form-check-label" for="platform-${platform.id}">${platform.platformName}</label>
                        </div>
                    `;
                    platformFilterContainer.append(checkboxHtml);
                });
            },
            error: function (xhr, status, error) {
                console.error('Failed to fetch platforms:', error);
            }
        });
    }

    function loadRatings() {
        var ratingFilterContainer = $('#rating-filter');
        ratingFilterContainer.empty();

        // Iterate through the ratings from 1 to 5 and create radio buttons
        for (var rating = 1; rating <= 5; rating++) {
            var starsHtml = '';
            for (var i = 1; i <= 5; i++) {
                if (i <= rating) {
                    starsHtml += '<i class="fa fa-star" style="color: #ffa200;"></i>';
                } else {
                    starsHtml += '<i class="fa fa-star" style="color: #000;"></i>';
                }
            }
            var radioHtml = `
                <div class="form-check collection-filter-checkbox mt-3">
                    <input type="radio" class="form-check-input rating-radio" name="rating" id="rating-${rating}" value="${rating}">
                    <label class="form-check-label" for="rating-${rating}">${starsHtml} <span> &nbsp &nbsp & Up</span></label>
                </div>
            `;
            ratingFilterContainer.append(radioHtml);
        }
    }

    function getFilteredGames() {
        var selectedGenres = [];
        var selectedPlatforms = [];
        var selectedRating = $('input[name="rating"]:checked').val();
        var fromDate = $('#FromDate').val();
        var toDate = $('#ToDate').val();

        $('.genre-checkbox:checked').each(function () {
            selectedGenres.push($(this).val());
        });

        $('.platform-checkbox:checked').each(function () {
            selectedPlatforms.push($(this).val());
        });

        var queryParams = $.param({
            genres: selectedGenres.join(','),
            platforms: selectedPlatforms.join(','),
            rating: selectedRating,
            fromDate: fromDate,
            toDate: toDate
        });

        $.ajax({
            url: '/customer/games/GetFilteredGames?' + queryParams,
            method: 'GET',
            success: function (data) {
                var gameListContainer = $('#game-list');
                gameListContainer.empty();
                data.forEach(function (game) {
                    var gameHtml = renderGameItem(game)
                    gameListContainer.append(gameHtml);
                });
            },
            error: function (xhr, status, error) {
                console.error('Failed to fetch games:', error);
            }
        });
    }

    
    function renderGameItem(game) {
        return `
        <div class="col-xl-3 col-6 col-grid-box">
            <div class="product-box">
                <div class="img-wrapper">
                    <div class="front">
                        <a href="customer/games/detail/${game.id}">
                            <img src="${game.imageUrl}" class="img-fluid blur-up lazyload" alt="">
                        </a>
                    </div>
                    <div class="back">
                        <a href="customer/games/detail/${game.id}">
                            <img src="${game.imageUrl}" class="img-fluid blur-up lazyload" alt="">
                        </a>
                    </div>
                </div>
                <div class="product-detail">
                    <div>
                        <div class="rating">
                            ${getStarRatingHtml(game.ratings)}
                        </div>
                        <a href="customer/games/detail/${game.id}">
                            <h6>${game.name}</h6>
                        </a>
                        <h7> Release Date: ${game.releaseDate}</h7>
                        <h5>(Stock: ${game.stock})</h5>
                    </div>
                </div>
            </div>
        </div>
    `;
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

    $(document).on('change', '.genre-checkbox, .platform-checkbox, .rating-radio, #ToDate', function () {
        console.log('Filter changed, fetching filtered games...');
        getFilteredGames();
    });

    getGames();
    loadGenres();
    loadPlatforms();
    loadRatings();

});
