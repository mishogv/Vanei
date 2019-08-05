$('#warehouse-delete').on('click',
    function () {
        $('.modal-title').text('Do you want to delete this warehouse?');
        let wareHouseName = $(this).attr('value');
        let hrefValue = $(this).attr('href');
        $('.modal-body').text(wareHouseName);
        $('.continue-delete').attr('href', hrefValue);
    });

$('#company-delete').on('click',
    function () {
        $('.modal-title').text('Do you want to delete this company?');
        let companyName = $(this).attr('value');
        let hrefValue = $(this).attr('href');
        $('.modal-body').text(companyName);
        $('.continue-delete').attr('href', hrefValue);
    });

$('#report-delete').on('click',
    function () {
        $('.modal-title').text('Do you want to delete this report?');
        let reportName = $(this).attr('value');
        let hrefValue = $(this).attr('href');
        $('.modal-body').text(reportName);
        $('.continue-delete').attr('href', hrefValue);
    });

$(document).on('click',
    '.product-modal',
    function () {
        $('.modal-title').text('Do you want to delete this product?');
        let productName = $(this).attr('value');
        let hrefValue = $(this).attr('href');
        $('.modal-body').text(productName);
        $('.continue-delete').attr('href', hrefValue);
    });

$(document).on('click',
    '.category-modal',
    function () {
        $('.modal-title').text('Do you want to delete this category?');
        let categoryName = $(this).attr('value');
        let hrefValue = $(this).attr('href');
        $('.modal-body').text(categoryName);
        $('.continue-delete').attr('href', hrefValue);
    });

$(document).on('click',
    '.employee-modal',
    function () {
        $('.modal-title').text('Do you want to remove this employee from your company?');
        let employeeName = $(this).attr('value');
        let hrefValue = $(this).attr('href');
        $('.modal-body').text(employeeName);
        $('.continue-delete').attr('href', hrefValue);
    });

$(document).on('click',
    '.admin-user-modal',
    function () {
        $('.modal-title').text('Do you want to remove this user from admin role?');
        let employeeName = $(this).attr('value');
        let hrefValue = $(this).attr('href');
        $('.modal-body').text(employeeName);
        $('.continue-delete').attr('href', hrefValue);
    });

$(document).on('click',
    '.admin-company-modal',
    function () {
        $('.modal-title').text('Do you want to delete this company?');
        let employeeName = $(this).attr('value');
        let hrefValue = $(this).attr('href');
        $('.modal-body').text(employeeName);
        $('.continue-delete').attr('href', hrefValue);
    });


$(document).on('click',
    '.admin-add-modal',
    function () {
        $('.modal-title').text('Do you want to make this user admin?');
        let employeeName = $(this).attr('value');
        let hrefValue = $(this).attr('href');
        $('.modal-body').text(employeeName);
        $('.continue-delete').attr('href', hrefValue);
    });