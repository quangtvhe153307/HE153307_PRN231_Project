$('#trans-pagination-section').on('click', '.page-link', function (e) {
    e.preventDefault();
    var id = $(this).data('id');
    var currentPage = $('#trans-pagination-section').data('currentpage');
    var totalpage = $('#trans-pagination-section').data('totalpage');

    if (id == 'next' || id == 'previous') {
        if (id == 'next') {

            currentPage += 1;
        } else {
            currentPage -= 1;
        }
    } else {
        currentPage = id;
    }
    $('#trans-pagination-section').data('currentpage', currentPage);
    if (currentPage == 1) {
        $('#trans-pagination-section').find('.page-item').removeClass("disabled");
        $('#trans-pagination-section').find('.previous-page').addClass("disabled");
    }
    if (currentPage == totalpage) {
        $('#trans-pagination-section').find('.page-item').removeClass("disabled");
        $('#trans-pagination-section').find('.next-page').addClass("disabled");
    }
    search();
});
$(document).ready(function () {
    $('.header-search').on('keydown', function (event) {
        if (event.keyCode === 13) {
            // Enter key pressed
            event.preventDefault(); // Prevent the default form submission
            $('#trans-pagination-section').data('currentpage', 1);
            search();
            renderPagination();
        }
    });
});
//$('#trans-table').on('click', '.btn-delete', function () {
//    var tr = $(this).closest('tr');
//    var id = tr.data('id');
//    var movieid = tr.data('movieid');
//    var commenteddate = tr.data('commenteddate');

//    $('#modal-content').text(`Do you want to delete this comment`);
//    $('#modal-content').data('userid', userId);
//    $('#modal-content').data('movieid', movieid);
//    $('#modal-content').data('commenteddate', commenteddate);
//    $('#btn-do-delete').click();
//});
function getQuery() {
    var transactionid = $('#transaction-id').val();
    var userEmail = $('#userEmail').val();
    var startdate = $('#start-date').val();
    var enddate = $('#end-date').val();

    var strQuery = `?$filter=1 eq 1 `;
    if (transactionid != '') {
        strQuery += ` and TransactionId eq ${transactionid}`;
    }
    if (startdate != '') {
        strQuery += ` and TransactionDate ge ${startdate}`;
    }
    if (enddate != '') {
        strQuery += ` and TransactionDate le ${enddate}`;
    }
    strQuery += ` and contains(tolower(User/Email), tolower('${userEmail}'))`;
    return strQuery;
}
function search() {
    var currentPage = $('#trans-pagination-section').data('currentpage');
    var query = getQuery();
    var strQuery = `${query}&$expand=User&$Orderby= TransactionId desc&$skip=${(currentPage - 1) * 10}&$top=10`;
    $.ajax({
        url: 'https://localhost:7038/odata/Transactions' + strQuery,
        method: 'GET',
        dataType: 'json',
        headers: { "Authorization": "Bearer " + getAccessToken() },
        success: function (response) {
            appendSearched(response.value);
            console.log(response);
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
    var tbody = $('#trans-table');
    var row = ``;
    for (var i = 0; i < data.length; i++) {
        row += `<tr data-userid="${data[i].UserId}" data-id="${data[i].TransactionId}" data-transactiondate="${data[i].TransactionDate}">
                    <td>${data[i].TransactionId}</td>
                    <td>${data[i].User.Email}</td>
                    <td>${data[i].TransactionDescription}</td>
                    <td>${data[i].TransactionDateStr}</td>
                    <td>`
        if (data[i].TransactionType == 0) {
            row += 'Add';
        } else {
            row += 'Subtract';
        }
            row += `</tr>`;
    }
    tbody.empty();
    tbody.html(row);
}
function renderPagination() {
    var query = getQuery();
    $.ajax({
        url: 'https://localhost:7038/odata/Transactions/$count' + query,
        method: 'GET',
        dataType: 'text',
        headers: { "Authorization": "Bearer " + getAccessToken() },
        success: function (response) {
            var number = Math.ceil(response / 10);
            $('#trans-pagination-section').data('totalpage', number);
            render(number);
        },
        error: function (xhr, status, error) {
            // Handle the error here
            console.log('Error:', error);
            appendAlert('Error', 'danger');
        }
    });
}
//$('#delete-confirmation').on('click', function () {
//    var userId = $('#modal-content').data('userid');
//    var movieid = $('#modal-content').data('movieid');
//    var commenteddate = $('#modal-content').data('commenteddate');
//    var a = {
//        userId: userId,
//        movieId: movieid,
//        commentedDate: commenteddate
//    };
//    $.ajax({
//        url: 'https://localhost:7038/DeleteComment',
//        method: 'DELETE',
//        data: JSON.stringify(a),
//        dataType: 'text',
//        contentType: 'application/json',
//        headers: { "Authorization": "Bearer " + getAccessToken() },
//        success: function (response) {
//            appendAlert("delete success", 'success');
//        },
//        error: function (xhr, status, error) {
//            // Handle the error here
//            console.log('Error:', error);
//            appendAlert('Error', 'danger');
//        }
//    });
//});