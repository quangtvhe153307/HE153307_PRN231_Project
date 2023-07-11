// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function getAccessToken() {
    function getCookie(cname) {
        let name = cname + "=";
        let decodedCookie = decodeURIComponent(document.cookie);
        let ca = decodedCookie.split(';');
        for (let i = 0; i < ca.length; i++) {
            let c = ca[i];
            while (c.charAt(0) == ' ') {
                c = c.substring(1);
            }
            if (c.indexOf(name) == 0) {
                return c.substring(name.length, c.length);
            }
        }
        return "";
    }

    return getCookie('accessToken');
}
$('#ranking-select').on('change', function () {
    $('.popular-movies-item').first()
        .html(`<div class="movie-items movie-items-empty">
            </div>            
            <div class="movie-items movie-items-empty">
            </div>            
            <div class="movie-items movie-items-empty">
            </div>            
            <div class="movie-items movie-items-empty">
            </div>            
            <div class="movie-items movie-items-empty">
            </div>            
            <div class="movie-items movie-items-empty">
            </div>            
            <div class="movie-items movie-items-empty">
            </div>            
            <div class="movie-items movie-items-empty">
            </div>            
            <div class="movie-items movie-items-empty">
            </div>`);
    $.ajax({
        url: 'https://localhost:7038/MovieRanking/' + $('#ranking-select').val(),
        method: 'GET',
        dataType: 'json',
        headers: { "Authorization": "Bearer " + getAccessToken() },
        contentType: "application/json",
        success: function (response) {
            appendMovieRanking(response);
        },
        error: function (xhr, status, error) {
            // Handle the error here
            console.log('Error:', error);
        }
    });
});
function appendMovieRanking(data) {
    var a = $('.popular-movies-item').first();
    var result = '';
    for (let index = 0; index < data.length; index++) {
        result += `<div class="movie-items">
                    <div class="img-container">
                        <img src="${data[index].movieImage}" class="rankedMovieImage">
                    </div>

                </div>`;
    }
    a.html(result);
}
function loadDataPreviewMovie(movieId) {
    $.ajax({
        url: `https://localhost:7038/odata/Movies/${movieId}`+`?$expand=MovieSeasons($expand=MovieEpisodes)`,
        method: 'GET',
        dataType: 'json',
        headers: { "Authorization": "Bearer " + getAccessToken() },
        contentType: "application/json",
        success: function (response) {
            appendPreviewData(response);
        },
        error: function (xhr, status, error) {
            // Handle the error here
            console.log('Error:', error);
        }
    });
}
function appendPreviewData(data) {
    var modal = $('#previewModal');
    modal.find('#description').text(data.Description);

    var row = ``;
    var result = '';
    for (var i = 0; i < data.MovieSeasons.length; i++) {
        row += `<li><button class="dropdown-item" type="button">Season ${i + 1}</button></li>`;

        for (var j = 0; j < data.MovieSeasons[i].MovieEpisodes.length; j++) {
            result += `<div class="titleCardList--container episode-item"><div class="titleCard-title_index">${j}</div><div class="titleCard-imageWrapper"><div class="ptrack-content"><img src="${data.MovieSeasons[i].MovieEpisodes[j].episodeImage}"/></div></div><div class="titleCardList--metadataWrapper"><div class="titleCardList-title"><span class="titleCard-title_text">${data.MovieSeasons[i].MovieEpisodes[j].title}</span><span><span class="duration ellipsized">${data.MovieSeasons[i].MovieEpisodes[j].duration}</span></span></div><p class="titleCard-synopsis"><div class="ptrack-content">${data.MovieSeasons[i].MovieEpisodes[j].description}</div></p></div></div> `;
        }
    }
    row += `<li><hr class="dropdown-divider"></li> <li><button class="dropdown-item" type="button">See All Episodes</button></li>`

    var dropdown = modal.find('#dropdown-menu');
    dropdown.empty();
    dropdown.html(row);

    var container = modal.find(`#episodeSelector-container`);
    container.empty();
    container.html(result);
}