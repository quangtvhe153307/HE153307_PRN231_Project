$('#search-movie-pagination').on('click', '.page-link', function (e) {
    e.preventDefault();
    var id = $(this).data('id');
    var currentPage = $('#search-movie-pagination').data('currentpage');
    var totalpage = $('#search-movie-pagination').data('totalpage');

    if (id == 'next' || id == 'previous') {
        if (id == 'next') {

            currentPage += 1;
        } else {
            currentPage -= 1;
        }
    } else {
        currentPage = id;
    }
    $('#search-movie-pagination').data('currentpage', currentPage);
    if (currentPage == 1) {
        $('#search-movie-pagination').find('.page-item').removeClass("disabled");
        $('#search-movie-pagination').find('.previous-page').addClass("disabled");
    }
    if (currentPage == totalpage) {
        $('#search-movie-pagination').find('.page-item').removeClass("disabled");
        $('#search-movie-pagination').find('.next-page').addClass("disabled");
    }
    search();
});
$('#search-btn').on('click', function () {
    $('#search-movie-pagination').data('currentpage', 1);
    search();
    updateDatePaging(queryString());
})
$('.popular-movies-item').on('click', '.movie-items', function (e) {
    e.preventDefault();
    loadDataPreviewMovie($(this).data('id'));
    $('#preview-modal-btn').click();
});
function queryString() {
    var categories = $('#categories-select').val();
    var type = $('#type-select').val();
    var title = $('#search-key').val();
    var str = "";
    if (categories != null) {
        var relatedMovieQuery = "";
        if (categories != null && categories.length > 0) {
            relatedMovieQuery += `c/CategoryId eq ${categories[0]} `;
        
        for (var i = 1; i < categories.length; i++) {
            relatedMovieQuery += `or c/CategoryId eq ${categories[i]} `;
        }
            str = " and Categories/any(c: " + relatedMovieQuery + ") ";
        }
    }

    var str1 = "";
    if (type != null) {
        var relatedMovieQuery = "";
        if (type.length == 1) {
            var a = type[0] == 1 ? `false` : `true`;
            str1 += `IsSingleEpisode eq ${a} and `;
        }
    }
    var queryStr = `?$filter=${str1}contains(tolower(Title), tolower('${title}'))${str}`;
    return queryStr;
}
function search() {
    var page = $('#search-movie-pagination').data('currentpage');
    var queryStr = queryString();
    searchData(queryStr, page);
    var title = $('#search-key').val();
    if (title != undefined && title != '') {

        title = `Title=` + title;
    }
    var categories = $('#categories-select').val();
    var str = "";
    if (categories != null) {
        var relatedMovieQuery = "";
        if (categories != null && categories.length > 0) {
            for (var i = 0; i < categories.length; i++) {
                relatedMovieQuery += `&Categories=${categories[i]}`;
            }
            str = relatedMovieQuery;
        }
    }
    window.history.pushState({}, '', `/Movie/Search?${title}${str}`);
}
function searchData(query, page) {
    var sort = $('#sort-option').val();
    var order = ``;
    if (sort == 0) {
        order = `UpdatedDate desc`;
    } else {
        order = `ViewCount desc`;
    }
    var queryStr = `https://localhost:7038/`+`odata/Movies${query}&$orderby=${order}&skip=${(page-1) * 8}&$top=8`;
    $.ajax({
        url: queryStr,
        method: 'GET',
        dataType: 'json',
        headers: { "Authorization": "Bearer " + getAccessToken() },
        contentType: "application/json",
        success: function (response) {
            appendSearchedData(response.value);
        },
        error: function (xhr, status, error) {
            console.log(error);
        }
    });
}
function appendSearchedData(data) {
    var row = ``;
    for (var i = 0; i < data.length; i++) {
        row += `<div class="movie-items" data-id="${data[i].MovieId}">
                <div class="img-container">
                    <img src="${data[i].MovieImage}" class="rankedMovieImage">
                    <div class="overlay"></div>
                </div>
                <div class="movie-item-info" style="">
                    <div class="movie-item-info-sub" style="">
                        <div class="movie-item-price-wrapper">
                            <span class="coin">`;
        if (data[i].Price > 0) {
            row += `<svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-coin" viewBox="0 0 16 16">
                                        <path d="M5.5 9.511c.076.954.83 1.697 2.182 1.785V12h.6v-.709c1.4-.098 2.218-.846 2.218-1.932 0-.987-.626-1.496-1.745-1.76l-.473-.112V5.57c.6.068.982.396 1.074.85h1.052c-.076-.919-.864-1.638-2.126-1.716V4h-.6v.719c-1.195.117-2.01.836-2.01 1.853 0 .9.606 1.472 1.613 1.707l.397.098v2.034c-.615-.093-1.022-.43-1.114-.9H5.5zm2.177-2.166c-.59-.137-.91-.416-.91-.836 0-.47.345-.822.915-.925v1.76h-.005zm.692 1.193c.717.166 1.048.435 1.048.91 0 .542-.412.914-1.135.982V8.518l.087.02z"></path>
                                        <path d="M8 15A7 7 0 1 1 8 1a7 7 0 0 1 0 14zm0 1A8 8 0 1 0 8 0a8 8 0 0 0 0 16z"></path>
                                        <path d="M8 13.5a5.5 5.5 0 1 1 0-11 5.5 5.5 0 0 1 0 11zm0 .5A6 6 0 1 0 8 2a6 6 0 0 0 0 12z"></path>
                                    </svg>
                                    <span class="movie-item-price">${data[i].Price}</span>`;
        } else {
            row += `<span class="movie-item-price">Free</span>`;
        }

        row += `</span>
                        </div>
                        <div class="movie-item-view">
                            <svg xmlns="http://www.w3.org/2000/svg" width="16" height="16" fill="currentColor" class="bi bi-eye-fill" viewBox="0 0 16 16">
                                <path d="M10.5 8a2.5 2.5 0 1 1-5 0 2.5 2.5 0 0 1 5 0z"></path>
                                <path d="M0 8s3-5.5 8-5.5S16 8 16 8s-3 5.5-8 5.5S0 8 0 8zm8 3.5a3.5 3.5 0 1 0 0-7 3.5 3.5 0 0 0 0 7z"></path>
                            </svg>
                            <span class="movie-item-view-count"> ${data[i].ViewCount} </span>
                        </div>
                    </div>
                    <div class="movie-item-title-wrapper" style="">
                        <div class="movie-item-title"> ${data[i].Title} </div>
                    </div>
                </div>

            </div>`;
    }
    $('#search-result').empty();
    $('#search-result').html(row);
}
function updateDatePaging(query) {
    var queryStr = `https://localhost:7038/`+`odata/Movies/$count${query}`;

    $.ajax({
        url: queryStr,
        method: 'GET',
        dataType: 'text',
        headers: { "Authorization": "Bearer " + getAccessToken() },
        contentType: "application/json",
        success: function (response) {
            console.log(`new page` + response);
            var number = Math.ceil(response / 8);
            console.log(number);
            refreshPaging(number);
            $('#search-movie-pagination').data('currentpage', 1);
            $('#search-movie-pagination').data('totalpage', number);
        },
        error: function (xhr, status, error) {
            console.log(error);
        }
    });
}
function refreshPaging(data) {
    var pag = $('#search-movie-pagination').find('.pagination');
    var row = ``;


    row += `    <li class="page-item previous-page disabled">
        <a class="page-link" data-id="previous" href="" aria-label="Previous">
            <span aria-hidden="true">&laquo;</span>
        </a>
    </li>`;
    for (var i = 0; i < data; i++) {
        row += `<li class="page-item"><a class="page-link" href="" data-id="${i + 1}">${i + 1}</a></li>`;
    }
    row += `<li class="page-item next-page">
        <a class="page-link" href="" data-id="next" aria-label="Next">
            <span aria-hidden="true">&raquo;</span>
        </a>
    </li>`;

    pag.empty();
    pag.html(row);
}