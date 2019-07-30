﻿// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.


$(document).ready(function () {
    $('#dtBasicExample').DataTable({
        "scrollX": true
    });
    $('.dataTables_length').addClass('bs-select');
    $('.dataTables_length').addClass('table-responsive');
    $('.dataTables_length').addClass('container-fluid');
    $('.dataTables_length').addClass('custom-table');
});

