$(document).ready(function () {
    $('.popular-movies').on('click', '.movie-items', function (e) {
        loadDataPreviewMovie($(this).data('id'));
        $('#preview-modal-btn').click();
    });
});
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
    var trailerContainer = modal.find('#previewModal--player_container');
    trailerContainer.html(`<iframe width="100%" style="aspect-ratio: 16/9;" src="https://www.youtube.com/embed/7jxB9-9aLl0" frameborder="0" allowfullscreen></iframe>`);
    var row = ``;
    var result = '';
    var trailer = 'previewModal--player_container'
    for (var i = 0; i < data.MovieSeasons.length; i++) {
        row += `<li><button class="dropdown-item" type="button" data-target="season-container-${data.MovieSeasons[i].MovieSeasonId}">Season ${i + 1}</button></li>`;

        result += `<div class="seasons-container" id="season-container-${data.MovieSeasons[i].MovieSeasonId}">`;
        for (var j = 0; j < data.MovieSeasons[i].MovieEpisodes.length; j++) {
            result += `<div class="titleCardList--container episode-item"><div class="titleCard-title_index">${j}</div><div class="titleCard-imageWrapper"><div class="ptrack-content"><img src="${data.MovieSeasons[i].MovieEpisodes[j].EpisodeImage}"/></div></div><div class="titleCardList--metadataWrapper"><div class="titleCardList-title"><span class="titleCard-title_text">${data.MovieSeasons[i].MovieEpisodes[j].Title}</span><span><span class="duration ellipsized">${data.MovieSeasons[i].MovieEpisodes[j].Duration}</span></span></div><div class="titleCard-synopsis"><div class="ptrack-content">${data.MovieSeasons[i].MovieEpisodes[j].Description}</div></div></div></div> `;
        }
        result += `</div>`;

    }
    row += `<li><hr class="dropdown-divider"></li> <li><button class="dropdown-item" type="button">See All Episodes</button></li>`

    var dropdown = modal.find('#dropdown-menu');
    dropdown.empty();
    dropdown.html(row);

    var container = modal.find(`#episodeSelector-container`);
    container.empty();
    container.html(result);
}