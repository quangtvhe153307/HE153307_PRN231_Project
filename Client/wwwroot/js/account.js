$('#purchased-section').on('click', '.page-link', function () {
    var id = $(this).data('id');
    var currentPage = $('#purchased-section').data('currentpage');
    var userid = $('#purchased-section').data('userid');
    var totalpage = $('#purchased-section').data('totalpage');
    
    if (id == 'next' || id == 'previous') {
        if (id == 'next') {

            currentPage += 1;
        } else {
            currentPage -= 1;
        }
    } else {
        currentPage = id;
    }
    $('#purchased-section').data('currentpage', currentPage);
    if (currentPage == 1) {
        $('#purchased-section').find('.page-item').removeClass("disabled");
        $('#purchased-section').find('.previous-page').addClass("disabled");
    }
    if (currentPage == totalpage) {
        $('#purchased-section').find('.page-item').removeClass("disabled");
        $('#purchased-section').find('.next-page').addClass("disabled");
    }
    $.ajax({
        url: `https://localhost:7038/odata/PurchasedMovies?$filter=UserId eq ${userid}&expand=Movie&$OrderBy= PurchasedTime desc&$skip=${5 * (currentPage - 1)}`,
        method: 'GET',
        dataType: 'json',
        headers: { "Authorization": "Bearer " + getAccessToken() },
        contentType: "application/json",
        success: function (response) {
            appendPurchasedMovie(response.value);
        },
        error: function (xhr, status, error) {
            console.log(error);
        }
    });
});
$('#transaction-section').on('click', '.page-link', function () {
    var id = $(this).data('id');
    var currentPage = $('#transaction-section').data('currentpage');
    var userid = $('#transaction-section').data('userid');
    var totalpage = $('#transaction-section').data('totalpage');

    if (id == 'next' || id == 'previous') {
        if (id == 'next') {

            currentPage += 1;
        } else {
            currentPage -= 1;
        }
    } else {
        currentPage = id;
    }
    $('#transaction-section').data('currentpage', currentPage);
    if (currentPage == 1) {
        $('#transaction-section').find('.page-item').removeClass("disabled");
        $('#transaction-section').find('.previous-page').addClass("disabled");
    }
    if (currentPage == totalpage) {
        $('#transaction-section').find('.page-item').removeClass("disabled");
        $('#transaction-section').find('.next-page').addClass("disabled");
    }
    $.ajax({
        url: `https://localhost:7038/odata/Transactions?$filter=UserId eq ${userid}&$OrderBy= TransactionDate desc&$skip=${5 * (currentPage - 1)}`,
        method: 'GET',
        dataType: 'json',
        headers: { "Authorization": "Bearer " + getAccessToken() },
        contentType: "application/json",
        success: function (response) {
            //console.log(response);
            appendTransaction(response.value);
        },
        error: function (xhr, status, error) {
            console.log(error);
        }
    });
});
function appendPurchasedMovie(data) {
    var tbody = $('#purchased-section').find('tbody');
    var row = ``;
    for (var i = 0; i < data.length; i++) {
        row += `                                            <tr>
                                                <td>
                                                    <img style="width: 20%;" src="${data[i].Movie.MovieImage}" />
                                                    <span>${data[i].Movie.Title}</span>
                                                </td>
                                                <td><span>${data[i].PurchasedTimeStr}</span></td>
                                            </tr>`;
    }
    tbody.empty();
    tbody.html(row);
}
function appendTransaction(data) {
    var tbody = $('#transaction-section').find('tbody');
    var row = ``;
    for (var i = 0; i < data.length; i++) {
        row += `                                         <tr>
                                            <td>${data[i].TransactionDescription}</td>
                                            <td>${data[i].TransactionDateStr}</td>
                                            <td>${data[i].TransactionType == 0 ? "Add" : "Sub"}</td>
                                        </tr>`;
    }
    tbody.empty();
    tbody.html(row);
}
$('input').on('change', function () {
    if ($(this).val() != undefined && $(this).val() != '') {
        $(this).addClass('hasText');
    } else {
        $(this).removeClass('hasText');
    }
})