$('.movie-section').on('click', '.movie-comments-header', function () {
    $('.seasons-container').css('display', 'none');
    $('.comments-container').css('display', '');
});
$('.movie-section').on('click', '.season-list-items1', function () {
    var target = $(this).data('target');
    $('.seasons-container').css('display', 'none');
    $('#' + target).css('display', '');
});