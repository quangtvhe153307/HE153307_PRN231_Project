$('#comment-pagination-section').on('click', '.page-link', function (e) {
    e.preventDefault();
    var id = $(this).data('id');
    var currentPage = $('#comment-pagination-section').data('currentpage');
    var totalpage = $('#comment-pagination-section').data('totalpage');

    if (id == 'next' || id == 'previous') {
        if (id == 'next') {

            currentPage += 1;
        } else {
            currentPage -= 1;
        }
    } else {
        currentPage = id;
    }
    $('#comment-pagination-section').data('currentpage', currentPage);
    if (currentPage == 1) {
        $('#comment-pagination-section').find('.page-item').removeClass("disabled");
        $('#comment-pagination-section').find('.previous-page').addClass("disabled");
    }
    if (currentPage == totalpage) {
        $('#comment-pagination-section').find('.page-item').removeClass("disabled");
        $('#comment-pagination-section').find('.next-page').addClass("disabled");
    }
    search();
});
$(document).ready(function () {
    $('.header-search').on('keydown', function (event) {
        if (event.keyCode === 13) {
            // Enter key pressed
            event.preventDefault(); // Prevent the default form submission
            $('#comment-pagination-section').data('currentpage', 1);
            search();
            renderPagination();
        }
    });
});
$('#comment-table').on('click', '.btn-delete', function () {
    var tr = $(this).closest('tr');
    var userId = tr.data('userid');
    var movieid = tr.data('movieid');
    var commenteddate = tr.data('commenteddate');

    $('#modal-content').text(`Do you want to delete this comment`);
    $('#modal-content').data('userid', userId);
    $('#modal-content').data('movieid', movieid);
    $('#modal-content').data('commenteddate', commenteddate);
    $('#btn-do-delete').click();
});
function getQuery() {
    var userEmail = $('#userEmail').val();
    var moviename = $('#movie-name').val();
    var startdate = $('#start-date').val();
    var enddate = $('#end-date').val();

    var strQuery = `?$filter=1 eq 1 `;
    if (startdate != '') {
        strQuery += ` and CommentedDate ge ${startdate}`;
    }
    if (enddate != '') {
        strQuery += ` and CommentedDate le ${enddate}`;
    }
    strQuery += ` and contains(tolower(User/Email), tolower('${userEmail}')) and contains(tolower(Movie/Title), tolower('${moviename}'))`;
    return strQuery;
}
function search() {
    var currentPage = $('#comment-pagination-section').data('currentpage');
    var query = getQuery();
    var strQuery = `${query}&$expand=User,Movie&$skip=${(currentPage - 1)*10}&$top=10`;
    $.ajax({
        url: 'https://localhost:7038/odata/Comments' + strQuery,
        method: 'GET',
        dataType: 'json',
        headers: { "Authorization": "Bearer " + getAccessToken() },
        success: function (response) {
            appendSearched(response.value);
            appendAlert('Data loaded successfully', 'success');
        },
        error: function (xhr, status, error) {
            // Handle the error here
            console.log('Error:', error);
            appendAlert('Error', 'danger');
        }
    });
}
function appendSearched(data) {
    var tbody = $('#comment-table');
    var row = ``;
    for (var i = 0; i < data.length; i++) {
        row += `<tr data-userid="${data[i].User.UserId}" data-movieid="${data[i].Movie.MovieId}" data-commenteddate="${data[i].CommentedDate}" data-content="${data[i].Content}">
                    <td>${data[i].User.Email}</td>
                    <td>${data[i].Movie.Title}</td>
                    <td>${data[i].CommentedDateStr}</td>
                    <td>${data[i].Content}</td>
                    <td style="text-align: center;"><button class="item-action btn-danger btn-delete">Delete</button></td>
                </tr>`;
    }
    tbody.empty();
    tbody.html(row);
}
function renderPagination() {
    var query = getQuery();
    $.ajax({
        url: 'https://localhost:7038/odata/Comments/$count' + query,
        method: 'GET',
        dataType: 'text',
        headers: { "Authorization": "Bearer " + getAccessToken() },
        success: function (response) {
            var number = Math.ceil(response / 10);
            $('#comment-pagination-section').data('totalpage', number);
            render(number);
        },
        error: function (xhr, status, error) {
            // Handle the error here
            console.log('Error:', error);
            appendAlert('Error', 'danger');
        }
    });
}
$('#delete-confirmation').on('click', function () {
    var userId = $('#modal-content').data('userid');
    var movieid = $('#modal-content').data('movieid');
    var commenteddate = $('#modal-content').data('commenteddate');
    var a = {
        userId: userId,
        movieId: movieid,
        commentedDate: commenteddate
    };
    $.ajax({
        url: 'https://localhost:7038/DeleteComment' ,
        method: 'DELETE',
        data: JSON.stringify(a),
        dataType: 'text',
        contentType: 'application/json',
        headers: { "Authorization": "Bearer " + getAccessToken() },
        success: function (response) {
            appendAlert("delete success", 'success');
        },
        error: function (xhr, status, error) {
            // Handle the error here
            console.log('Error:', error);
            appendAlert('Error', 'danger');
        }
    });
});