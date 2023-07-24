$('#add-btn').on('click', function () {
    $('#btn-do-add').click();
});
//call edit to server
$('#add-confirmation').on('click', function (e) {
    e.preventDefault();
    var name = $('#category-add-name').val();
    var author = {
        categoryName: name
    };
    $.ajax({
        url: 'https://localhost:7038/odata/Categories',
        data: JSON.stringify(author),
        method: 'POST',
        headers: { "Authorization": "Bearer " + getAccessToken() },
        /*        dataType: 'text',*/
        contentType: "application/json",
        success: function (response) {
            appendAlert('Add successfully', 'success');
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
$('#delete-confirmation').on('click', function () {
    var id = $('#modal-content').data('id');
    console.log(id);
    deleteItem(id);
});
$('#cate-table').on('click', '.btn-edit', function () {
    var row = $(this).closest('tr');
    var id = row.data('id');
    var name = row.data('name');

    //clear modal
    clearEditModal();

    //update modal content
    getEditModalBody('CategoryId', 'Name');

    //readonly for id
    $("#CategoryId-input").prop('readonly', true);

    //assign value to modal
    $("#CategoryId-input").val(id);
    $("#Name-input").val(name);
    $('#edit-modal').data('id', id);

    $('#btn-do-edit').click();
});
//call edit to server
$('#edit-confirmation').on('click', function () {
    var id = $('#edit-modal').data('id');

    //call ajax to edit
    EditRowItem(id);

    console.log(id);

    $('#edit-done').click();
});
$('#cate-table').on('click', '.btn-delete', function () {
    var id = $(this).data('id');
    console.log(id);
    $('#modal-content').text(`Do you want to delete author with id = ${id}`);
    $('#modal-content').data('id', id);
    $('#btn-do-delete').click();
});
function EditRowItem(id) {
    var CategoryId = $("#CategoryId-input").val();
    var Name = $("#Name-input").val();

    var editData = {
        categoryId: CategoryId,
        categoryName: Name
    };
    $.ajax({
        url: 'https://localhost:7038/odata/Categories/' + CategoryId,
        data: JSON.stringify(editData),
        method: 'PUT',
        headers: { "Authorization": "Bearer " + getAccessToken() },
        dataType: 'json',
        contentType: "application/json",
        success: function (response) {
            clearEditModal();

            search();

            //Alert
            appendAlert('Edit Successfully', 'success');
        },
        error: function (xhr, status, error) {
            // Handle the error here
            console.log('Error:', error);
        }
    });
}
function appendRow(data) {
    var id = data.catego;
    var row = $(`#row-data-${id}`)

    var result = '';
    result += `<tr id="row-data-${data.authorId}"  data-id="${data.authorId}" data-firstname="${data.firstName}" data-lastname="${data.lastName}" data-city="${data.city}">
                    <td>${data.authorId}</td>
                    <td>${data.firstName}</td>
                    <td>${data.lastName}</td>
                    <td>${data.city}</td>
                    <td style="text-align: center;"><button class="item-action  btn-primary  btn-edit"  data-id="${data.authorId}">Edit</button>|<button class="item-action  btn-danger btn-delete" data-id="${data.authorId}">Delete</button></td>
                </tr>`;

    row.replaceWith(result);
}
function deleteItem(id) {
    $.ajax({
        url: 'https://localhost:7038/odata/Categories/' + id,
        method: 'DELETE',
        dataType: 'text',
        headers: { "Authorization": "Bearer " + getAccessToken() },
        success: function (response) {
            //clearEditModal();

            clearRow(id);
            //Alert
            appendAlert(`Book ${id} deleted`, 'success');

            clearDeleteModal();
            clearForm();
            search();
        },
        error: function (xhr, status, error) {
            // Handle the error here
            console.log('Error:', error);
            appendAlert('Error', 'danger');
        }
    });
} function clearEditModal() {
    $('#edit-modal').empty();
}
function clearRow(id) {
    $(`#row-data-${id}`).remove();
}
function clearDeleteModal() {
    $('#modal-content').text(``);
    $('#modal-content').data('id', 0);
    $('#delete-done').click();
}
function getEditModalBody() {
    var $parentDiv = $("<div>", { "class": "container-fluid" });

    for (var i = 0; i < arguments.length; i++) {
        var $rowDiv = $("<div>", { "class": "row" });
        var $fieldNameDiv = $("<div>", { "class": "col-md-4" });
        var $fieldNameSpan = $("<span>", { "class": "edit-fields" });
        $fieldNameSpan.text(arguments[i]);
        $fieldNameDiv.append($fieldNameSpan);

        var $fieldInputDiv = $("<div>", { "class": "col-md-8" });
        var $fieldNameInput = $("<input>", { id: arguments[i] + '-input', "class": "edit-input-fields", "type": "text", "placeholder": arguments[i] });
        $fieldInputDiv.append($fieldNameInput);

        $rowDiv.append($fieldNameDiv);
        $rowDiv.append($fieldInputDiv);

        $parentDiv.append($rowDiv);
    }
    $('#edit-modal').append($parentDiv);
}
function search() {
    $.ajax({
        url: 'https://localhost:7038/odata/Categories',
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
    var tbody = $('#cate-table');
    var row = ``;
    for (var i = 0; i < data.length; i++) {
        row += `<tr data-id="${data[i].CategoryId}" data-name="${data[i].CategoryName}">
                    <td>${data[i].CategoryId}</td>
                    <td>${data[i].CategoryName}</td>
                    <td style="text-align: center;"><button class="item-action btn-primary btn-edit" data-id="${data[i].CategoryId}">Edit</button>|<button class="item-action btn-danger btn-delete" data-id="${data[i].CategoryId}">Delete</button></td>
                </tr>`;
    }
    tbody.empty();
    tbody.html(row);
}