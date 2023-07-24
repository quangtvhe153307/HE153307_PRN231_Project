$('#user-pagination-section').on('click', '.page-link', function (e) {
    e.preventDefault();
    var id = $(this).data('id');
    var currentPage = $('#user-pagination-section').data('currentpage');
    var totalpage = $('#user-pagination-section').data('totalpage');

    if (id == 'next' || id == 'previous') {
        if (id == 'next') {

            currentPage += 1;
        } else {
            currentPage -= 1;
        }
    } else {
        currentPage = id;
    }
    $('#user-pagination-section').data('currentpage', currentPage);
    if (currentPage == 1) {
        $('#user-pagination-section').find('.page-item').removeClass("disabled");
        $('#user-pagination-section').find('.previous-page').addClass("disabled");
    }
    if (currentPage == totalpage) {
        $('#user-pagination-section').find('.page-item').removeClass("disabled");
        $('#user-pagination-section').find('.next-page').addClass("disabled");
    }
    search();
});
$(document).ready(function () {
    $('.header-search').on('keydown', function (event) {
        if (event.keyCode === 13) {
            // Enter key pressed
            event.preventDefault(); // Prevent the default form submission
            $('#user-pagination-section').data('currentpage', 1);
            search();
            renderPagination();
        }
    });
});
$('#user-table').on('click', '.btn-delete', function () {
    var tr = $(this).closest('tr');
    var userId = tr.data('id');


    $('#modal-content').text(`Do you want to delete user with id = ${userId}`);
    $('#modal-content').data('id', userId);
    $('#btn-do-delete').click();
});
function getQuery() {
    var userid = $('#user-id').val();
    var email = $('#email').val();
    var userrole = $('#user-role').val();
    var firstname = $('#first-name').val();
    var lastname = $('#last-name').val();

    var strQuery = `?$filter=1 eq 1 `;
    if (userid != '') {
        strQuery += ` and UserId eq ${userid}`;
    }
    strQuery += ` and contains(tolower(Email), tolower('${email}')) and contains(tolower(FirstName), tolower('${firstname}')) and contains(tolower(LastName), tolower('${lastname}'))  and contains(tolower(Role/RoleName), tolower('${userrole}'))`;
    return strQuery;
}
function search() {
    var currentPage = $('#user-pagination-section').data('currentpage');
    var query = getQuery();
    var strQuery = `${query}&$expand=Role&$skip=${(currentPage - 1) * 10}&$top=10`;
    $.ajax({
        url: 'https://localhost:7038/odata/Users' + strQuery,
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
    var tbody = $('#user-table');
    var row = ``;
    for (var i = 0; i < data.length; i++) {
        row += `<tr data-id="${data[i].UserId}" data-email="${data[i].Email}" data-role="${data[i].RoleId}" data-emailconfirmed="${data[i].EmailConfirmed}" data-firstname="${data[i].FirstName}" data-lastname="${data[i].LastName}" data-balance="${data[i].Balance}" data-expirationdate="${data[i].ExpirationDateEmail}">
                    <td>${data[i].UserId}</td>
                    <td>${data[i].Email}</td>
                    <td>${data[i].Role.RoleName}</td>
                    <td>${data[i].EmailConfirmed}</td>
                    <td>${data[i].FirstName}</td>
                    <td>${data[i].LastName}</td>
                    <td>${data[i].Balance}</td>
                    <td>${data[i].ExpirationDate}</td>
                    <td style="text-align: center;"><button class="item-action btn-primary btn-edit" data-id="${data[i].UserId}">Edit</button>|<button class="item-action btn-danger btn-delete" data-id="${data[i].UserId}">Delete</button></td>
                </tr>`;
    }
    tbody.empty();
    tbody.html(row);
}
function renderPagination() {
    var query = getQuery();
    $.ajax({
        url: 'https://localhost:7038/odata/Users/$count' + query,
        method: 'GET',
        dataType: 'text',
        headers: { "Authorization": "Bearer " + getAccessToken() },
        success: function (response) {
            var number = Math.ceil(response / 10);
            $('#user-pagination-section').data('totalpage', number);
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
        url: 'https://localhost:7038/DeleteComment',
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
$('#add-btn').on('click', function () {
    $('#btn-do-add').click();
});
$('#add-confirmation').on('click', function (e) {
    e.preventDefault();
    var email = $('#user-add-name').val();
    var roleId = $('#user-add-role').val();
    var firstName = $('#user-add-first-name').val();
    var lastName = $('#user-add-last-name').val();
    var emailConfirmed = $('#email-confirm').val() == 0 ? false : true;
    var balance = $('#user-add-balance').val();
    var expirationDate = $('#user-add-expiration').val();
    var author = {
        email: email,
        roleId: roleId,
        firstName: firstName,
        lastName: lastName,
        emailConfirmed: emailConfirmed,
        balance: balance,
        expirationDate: expirationDate
    };
    $.ajax({
        url: 'https://localhost:7038/AddUser',
        data: JSON.stringify(author),
        method: 'POST',
        headers: { "Authorization": "Bearer " + getAccessToken() },
        /*        dataType: 'text',*/
        contentType: "application/json",
        success: function (response) {
            appendAlert('Add successfully', 'success');
            $('.header-search').val('');
            search();
        },
        error: function (xhr, status, error) {
            // Handle the error here
            console.log('Error:', error);
            appendAlert('Add error', 'success');
        }
    });

    $('#add-done').click();
});