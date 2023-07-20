$('.movie-section').on('click', '.movie-comments-header', function () {
    $('.seasons-container').css('display', 'none');
    $('.comments-container').css('display', '');
});
$('.movie-section').on('click', '.season-list-items1', function () {
    var target = $(this).data('target');
    $('.seasons-container').css('display', 'none');
    $('#' + target).css('display', '');

    var text = $(this).find('div').text();
    $('#season-list-select-btn').text(text);
});

if (isPermitted) {
    $('.movie-section').on('click', '.movie-item', function () {
        var movieId = $(this).data('movieid');
        var episodeId = $(this).data('episodeid');
        console.log(movieId);
        console.log(episodeId);
        $('#play-movie').prop('src', '')
        $.ajax({
            url: 'https://localhost:7038/MovieSource/' + episodeId,
            method: 'GET',
            dataType: 'text',
            headers: { "Authorization": "Bearer " + getAccessToken() },
            contentType: "application/json",
            success: function (response) {
                $('#play-movie').prop('src', 'https://www.youtube.com/embed/' + response);
            },
            error: function (xhr, status, error) {
                // Handle the error here
                console.log('Error:', error);
            }
        });
    });
} else {
    $('.movie-section').on('click', '.movie-item', function () {
        console.log('not permitted');
    });
}