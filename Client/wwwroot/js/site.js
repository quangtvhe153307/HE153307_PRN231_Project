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