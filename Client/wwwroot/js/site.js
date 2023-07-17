$(document).ready(function () {
    $('.popular-movies').on('click', '.movie-items', function (e) {
        e.preventDefault();
        loadDataPreviewMovie($(this).data('id'));
        $('#preview-modal-btn').click();
    });
    $('#previewModal').on('click', '.season-list-items', function (e) {
        e.preventDefault();
        var targetId = $(this).data('target');
        if (targetId != 'all') {
            $('#episodeSelector-container').find('.seasons-container').css('display', 'none');
            $('#episodeSelector-container').find('#' + targetId).css('display', '');
            $('#episodeSelector-container').find('.season-title').css('display', 'none');
            $('.episodeSelector-header').find('.episodeSelector-dropdown').find('button').text($(this).find('div').text());
        } else {
            $('#episodeSelector-container').find('.seasons-container').css('display', '');
            $('#episodeSelector-container').find('.season-title').css('display', '');
            $('.episodeSelector-header').find('.episodeSelector-dropdown').find('button').text('All Episodes');
        }
    })
});
$('.episodeSelector-container').on('click', '.episode-item', function (e) {
    e.preventDefault();
    var movieId = $(this).data('movieid');
    var episodeId = $(this).data('episodeid');
    window.location.href = `https://localhost:7180/Movie/Index/${movieId}/${episodeId}`;
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
        url: `https://localhost:7038/odata/Movies/${movieId}`+`?$expand=Categories,MovieSeasons($expand=MovieEpisodes)`,
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
    trailerContainer.html(`<iframe width="100%" style="aspect-ratio: 16/9;" src="https://www.youtube.com/embed/${data.TrailerUrl}" frameborder="0" allowfullscreen></iframe>`);
    var row = ``;
    var result = '';
    var trailer = 'previewModal--player_container'
    for (var i = 0; i < data.MovieSeasons.length; i++) {
        row += `<li data-target="season-container-${data.MovieSeasons[i].MovieSeasonId}" class="season-list-items"><div class="" type="button">Season ${i + 1}</div></li>`;

        result += `<div class="seasons-container" id="season-container-${data.MovieSeasons[i].MovieSeasonId}" ` + (i != 0 ? `style="display:none;"`: ``)+`>`;
        result += `<div class="season-title" style="display:none;"><div class="ltr-1bdvpws"><span class="allEpisodeSelector-season-label ltr-78kobz">Season ${i + 1}</span></div></div>`;
        for (var j = 0; j < data.MovieSeasons[i].MovieEpisodes.length; j++) {
            result += `<div class="titleCardList--container episode-item" data-movieid="${data.MovieId}" data-episodeid="${data.MovieSeasons[i].MovieEpisodes[j].EpisodeId}"><div class="titleCard-title_index">${j + 1}</div><div class="titleCard-imageWrapper"><div class="ptrack-content"><img src="${data.MovieSeasons[i].MovieEpisodes[j].EpisodeImage}"/></div><div class="titleCard-playIcon"><svg width="24" height="24" viewBox="0 0 24 24" fill="none" xmlns="http://www.w3.org/2000/svg" class="titleCard-playSVG ltr-0 e1mhci4z1" data-name="Play" aria-hidden="true"><path d="M5 2.69127C5 1.93067 5.81547 1.44851 6.48192 1.81506L23.4069 11.1238C24.0977 11.5037 24.0977 12.4963 23.4069 12.8762L6.48192 22.1849C5.81546 22.5515 5 22.0693 5 21.3087V2.69127Z" fill="currentColor"></path></svg></div></div><div class="titleCardList--metadataWrapper"><div class="titleCardList-title"><span class="titleCard-title_text">${data.MovieSeasons[i].MovieEpisodes[j].Title}</span><span><span class="duration ellipsized">${data.MovieSeasons[i].MovieEpisodes[j].Duration}</span></span></div><div class="titleCard-synopsis"><div class="ptrack-content">${data.MovieSeasons[i].MovieEpisodes[j].Description}</div></div></div></div> `;
        }
        result += `</div>`;

    }
    if (data.MovieSeasons.length > 1) {
        row += `<div><hr class="dropdown-divider"></div> <li data-target="all"  class="season-list-items"><div class="" type="button">See All Episodes</div></li>`
    }

    //categories
    var categoriesContainer = modal.find('#previewModal--tags');
    categoriesContainer.find('.tag-item').remove();

    var categoriesRow = ``;
    for (var i = 0; i < data.Categories.length; i++) {
        categoriesRow += `<span class="tag-item">${data.Categories[i].CategoryName}` + ((i < data.Categories.length - 1) ? `,` : ``)+ `</span>`;
    }
    categoriesContainer.append(categoriesRow);

    $('.episodeSelector-header').find('.episodeSelector-dropdown').find('button').text('Season 1');

    modal.find('#released-date-tag-item').html(data.ReleaseDate.split('T')[0]);
    var dropdown = modal.find('#dropdown-menu');
    dropdown.empty();
    dropdown.html(row);

    var container = modal.find(`#episodeSelector-container`);
    container.empty();
    container.html(result);
}