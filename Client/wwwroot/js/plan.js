$('#upgrade-plan-container').on('click', '#upgrade-btn', function () {
    $.ajax({
        url: 'https://localhost:7038/Upgrade',
        method: 'POST',
        dataType: 'json',
        headers: { "Authorization": "Bearer " + getAccessToken() },
        contentType: "application/json",
        success: function (response) {
            appendAlert('Upgrade Successfully', 'success');
            setTimeout(function () {
                window.location.href = `https://localhost:7180`;
            }, 1000);
        },
        error: function (xhr, status, error) {
            if (xhr.responseJSON.message == 'Balance not enough') {
                appendAlert('Balance not enough', 'danger');
                setTimeout(function () {
                    window.location.href = `https://localhost:7180/Balance/Add`;
                }, 1000);
            }
        }
    });
});