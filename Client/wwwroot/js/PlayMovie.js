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
$('.comment-input').on('click', '#comment-add', function () {
    addComment();
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
$('.movie-section').on('click', '#more-comment-btn', function () {
    var nextLink = $(this).data('link');

    $.ajax({
        url: nextLink,
        method: 'GET',
        dataType: 'json',
        headers: { "Authorization": "Bearer " + getAccessToken() },
        contentType: "application/json",
        success: function (response) {
            appendComment(response);
        },
        error: function (xhr, status, error) {
            // Handle the error here
            console.log('Error:', error);
        }
    });
});
$('#player').on('click', '#btn-purchase', function () {
    var id = $(this).data('id');
    var name = $(this).data('name');
    var price = $(this).data('price');
    var img = $(this).data('img');

    $('#modal-content').find('#purchase-img-movie').prop('src', img);
    $('#modal-content').find('#purchase-name').html(name);
    $('#modal-content').find('#purchase-price').html(price);
    $('#modal-content').data('id', id);
    $('#btn-do-purchcase').click();
});
$('#purchaseModal').on('click', '#purchase-confirmation', function () {
    var a = {
        movieId: $('#modal-content').data('id')
    };
    $.ajax({
        url: 'https://localhost:7038/odata/PurchasedMovies',
        method: 'POST',
        dataType: 'json',
        data: JSON.stringify(a),
        headers: { "Authorization": "Bearer " + getAccessToken() },
        contentType: "application/json",
        success: function (response) {
            appendAlert('Purchased Successfully', 'success');
            setTimeout(function () {
                location.reload();
            }, 1000);
        },
        error: function (xhr, status, error) {
            console.log(xhr.responseJSON.message);
            if (xhr.responseJSON.message == 'Not enough balance') {
                appendAlert('Not enough balance', 'danger');
                setTimeout(function () {
                    window.location.href = `https://localhost:7180/Balance/Add`;
                }, 1000);
            }
        }
    });
});
function appendComment(data) {
    var a = $('#comment-list-container');
    var result = '';
    for (let index = 0; index < data.value.length; index++) {
        result += `<div class="comment-item">
                                            <div class="author-avatar">
                                                <img class="" src="https://th.bing.com/th/id/OIP.Cl56H6WgxJ8npVqyhefTdQHaHa?pid=ImgDet&rs=1" />
                                            </div>
                                            <div style="flex: 1;"></div>
                                            <div class="comment-item-body w-full">
                                                <div class="author-name items-center">
                                                    ${data.value[index].User.Email}
                                                </div>
                                                <div class="comment-content font-light break-words clear-both">
                                                    ${data.value[index].Content}
                                                </div>
                                                <div class="comment-action">
                                                    <span class="comment-time">
                                                        <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor" class="w-3 h-3 inline-block" style="width: 16px;">
                                                            <path stroke-linecap="round" stroke-linejoin="round" d="M12 6v6h4.5m4.5 0a9 9 0 11-18 0 9 9 0 0118 0z"></path>
                                                        </svg> ${data.value[index].CommentedTimeInterval} ago
                                                    </span>
                                                </div>
                                            </div>
                                        </div>`;
    }
    a.append(result);

    if (data["@odata.nextLink"] == undefined) {
        $('.more-comment').remove();
    }
}
function addComment() {
    var content = $('#comment-input').val();
    if (content != undefined && content != '') {
        var a = {
            movieId: movieId,
            content: content
        };

        $.ajax({
            url: 'https://localhost:7038/odata/Comments?$expand=User',
            method: 'POST',
            dataType: 'json',
            data: JSON.stringify(a),
            headers: { "Authorization": "Bearer " + getAccessToken() },
            contentType: "application/json",
            success: function (response) {
                var result = ``;
                result += `<div class="comment-item">
                                            <div class="author-avatar">
                                                <img class="" src="https://th.bing.com/th/id/OIP.Cl56H6WgxJ8npVqyhefTdQHaHa?pid=ImgDet&rs=1" />
                                            </div>
                                            <div style="flex: 1;"></div>
                                            <div class="comment-item-body w-full">
                                                <div class="author-name items-center">
                                                    ${response.User.Email}
                                                </div>
                                                <div class="comment-content font-light break-words clear-both">
                                                    ${response.Content}
                                                </div>
                                                <div class="comment-action">
                                                    <span class="comment-time">
                                                        <svg xmlns="http://www.w3.org/2000/svg" fill="none" viewBox="0 0 24 24" stroke-width="1.5" stroke="currentColor" class="w-3 h-3 inline-block" style="width: 16px;">
                                                            <path stroke-linecap="round" stroke-linejoin="round" d="M12 6v6h4.5m4.5 0a9 9 0 11-18 0 9 9 0 0118 0z"></path>
                                                        </svg> ${response.CommentedTimeInterval} ago
                                                    </span>
                                                </div>
                                            </div>
                                        </div>`;

                var a = $('#comment-list-container').prepend(result);
            },
            error: function (xhr, status, error) {
                $('#comment-error').html(`You don't have permission to comment`);
            }
        });
    }
}