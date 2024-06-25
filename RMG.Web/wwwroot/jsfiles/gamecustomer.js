$(document).ready(function () {
    function loadGenres() {
        $.ajax({
            url: '/customer/games/getgenres',
            method: 'GET',
            success: function (data) {
                // Assuming the data is an array of genre objects
                // Each genre object has an 'id' and 'name' property
                var genres = data;
                var genreFilterContainer = $('#genre-filter');
                genreFilterContainer.empty(); // Clear any existing content

                // Iterate through the genre data and create checkboxes
                genres.forEach(function (genre) {
                    var checkboxHtml = `
                        <div class="form-check collection-filter-checkbox">
                            <input type="checkbox" class="form-check-input" id="genre-${genre.id}" value="${genre.id}">
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
                            <input type="checkbox" class="form-check-input" id="platform-${platform.id}" value="${platform.id}">
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
        ratingFilterContainer.empty(); // Clear any existing content

        // Iterate through the ratings from 1 to 5 and create radio buttons
        for (var rating = 1; rating <= 5; rating++) {
            var starsHtml = '';
            for (var i = 1; i <= 5; i++) {
                if (i <= rating) {
                    starsHtml += '<i class="fa fa-star" style="color: #ffa200;"></i>'; // Filled star
                } else {
                    starsHtml += '<i class="fa fa-star" style="color: #000;"></i>'; // Unfilled star
                }
            }
            var radioHtml = `
                <div class="form-check collection-filter-checkbox mt-3">
                    <input type="radio" class="form-check-input" name="rating" id="rating-${rating}" value="${rating}">
                    <label class="form-check-label" for="rating-${rating}">${starsHtml}</label>
                </div>
            `;
            ratingFilterContainer.append(radioHtml);
        }
    }

    // Call the function to load genres when the document is ready
    loadGenres();
    loadPlatforms();
    loadRatings();
});