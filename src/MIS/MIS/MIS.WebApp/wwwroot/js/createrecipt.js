﻿var products = [];
var productsForAutocomplete = [];
var index = { index: -1 };
var numberInSequence = { number: -1 };
var total = { total: 0 };

function loadData(num) {
    numberInSequence.number = num;
    var selector = '#product-name-number-' + numberInSequence.number;

    autoCompleteShowData(productsForAutocomplete, selector);

    setData(products, index, numberInSequence);
}

function getAllProducts(products, productsForAutocomplete) {
    $.ajax({
        url: "/Receipt/AllPorducts",
        success: function load(data) {
            debugger;
            
            for (let product of data) {
                let formatProduct = {
                    value : product.name,
                    desc : product.id
                }

                productsForAutocomplete.push(formatProduct);
            }

            for (let product of data) {
                products.push(product);
            }
        },
        error: function error() {
            alert('error');
        }
    });
}

function autoCompleteShowData(productsForAutocomplete, selector) {
    if (productsForAutocomplete.length > 0) {
        debugger;
        $(selector).autocomplete({
            minLength: 2,
            source: productsForAutocomplete,
            focus: function (event, ui) {
                console.log(ui.item);
                $(selector).val(ui.item.value);
                index.index = ui.item.desc;
                return setData(products, index, numberInSequence);
            },
            select: function (event, ui) {
                $(selector).val(ui.item.value);
                return false;
            }
        });
    }
}

function setData(products, index, numberInSequence) {
    if (index.index !== 0 && index.index !== -1) {
        index.index = products.findIndex(x => x.id === index.index);
        $('#product-id-number-' + numberInSequence.number).val(products[index.index].id).prop('disabled', true);
        $('#product-barcode-number-' + numberInSequence.number).val(products[index.index].barcode).prop('disabled', true);
        $('#product-price-number-' + numberInSequence.number).val(products[index.index].price);
        $('#product-quantity-number-' + numberInSequence.number)
            .prop('title', 'In stock there are: ' + products[index.index].quantity)
            .tooltip();
    };
}

function ajaxAddProduct() {
    let idValue = $('#product-id-number-' + numberInSequence.number).val();
    let quantityValue = $('#product-quantity-number-' + numberInSequence.number).val();
    $.post({
        url: "/Receipt/Add",
        headers: {
            'Content-Type': 'application/json'
        },
        data: JSON.stringify({
            id: idValue,
            quantity: quantityValue
        }),
        success: function(data) {
            disableLastProduct();
            addProduct(data);
        },
        error: function error(errors) {
            let result = "";
            for (let error in errors.responseJSON) {
                result = result + (error + " : ");
                result =  result + (errors.responseJSON[error][0]) + " ";
            }
            alert(result);
        }
    });
}

function disableLastProduct() {
    $('#product-name-number-' + numberInSequence.number).prop('disabled', true);
    $('#product-barcode-number-' + numberInSequence.number).prop('disabled', true);
    $('#product-quantity-number-' + numberInSequence.number).prop('disabled', true);
    let price = parseFloat($('#product-price-number-' + numberInSequence.number).val());
    let quantity = parseFloat($('#product-quantity-number-' + numberInSequence.number).val());
    total.total = (parseFloat(total.total) + (price * quantity)).toFixed(2);
    $('#product-total-number-' + numberInSequence.number).val(((price * quantity)).toFixed(2));
}

function addProduct() {
    renderEmptyProduct(numberInSequence.number + 1);
    $('#table-menu').remove();
    renderMenu(total);
}

function getReceipt() {
    $.get({
        url: "/Receipt/LoadReceipt",
        success: function success(data) {
            $('#table-tbody').empty();
            renderReceipt(data);
            total.total = data.total;
        },
        error: function error(errorMessage) {
            console.log(errorMessage);
        }
    });
}

function finishReceipt() {
    $.get({
        url: "/Receipt/Finish",
        success: function success() {
            getReceipt();
            products = [];
            productsForAutocomplete = [];
            getAllProducts(products, productsForAutocomplete);
        },
        error: function error(errorMessage) {
            console.log(errorMessage);
        }
    });
}

function renderReceipt(data) {
    let last = data.products.length;
    renderProducts(data);
    renderEmptyProduct(last);
    renderMenu(data);
}

function renderProducts(data) {
    for (let i = 0; i < data.products.length; i++) {
        $('#table-products').append(
            '<tr>'
            +
            '<td>'
            +
            '<input type="text" placeholder="Id" class="form-control" id="product-id-number-' + i + '" value="' + data.products[i].id + '" disabled="disabled"  />'
            +
            '</td>'
            +
            '<td>'
            +
            '<input type="text" class="form-control" id="product-name-number-' + i + '" value="' + data.products[i].productName + '" disabled="disabled"  />'
            +
            '</td>'
            +
            '<td>'
            +
            '<input type="number" class="form-control" id="product-quantity-number-' + i + '" value="' + data.products[i].quantity + '" disabled="disabled"  />'
            +
            '</td>'
            +
            '<td>'
            +
            '<input type="text" class="form-control" id="product-barcode-number-' + i + '" value="' + data.products[i].productBarcode + '" disabled="disabled"  />'
            +
            '</td>'
            +
            '<td>'
            +
            '<input type="number" class="form-control" id="product-price-number-' + i + '" value="' + data.products[i].productPrice + '" disabled="disabled"  />'
            +
            '</td>'
            +
            '<td>'
            +
            '<input type="number" class="form-control" id="product-total-number-' + i + '" value="' + data.products[i].total + '" disabled="disabled"  />'
            +
            '</td>'
            +
            '</tr>'
        );
    }
}

function renderEmptyProduct(last) {
    numberInSequence.number = last;
    $('#table-products').append(
        '<tr>' +
        '<td>' +
        '<input type="text" class="form-control" id="product-id-number-' +
        last +
        '" disabled="disabled" placeholder="Id" />' +
        '</td>' +
        '<td>' +
        '<input type="text" class="form-control" id="product-name-number-' +
        last + '" placeholder="Name" onkeypress="loadData(' + last + ')" />' +
        '</td>' +
        '<td>' +
        '<input type="number" class="form-control" id="product-quantity-number-' +
        last +
        '" value="0" step="0.5" />' +
        '</td>' +
        '<td>' +
        '<input type="text" class="form-control" disabled="disabled" id="product-barcode-number-' +
        last +
        '" placeholder="Barcode"/>' +
        '</td>' +
        '<td>' +
        '<input type="number" class="form-control" id="product-price-number-' +
        last +
        '" placeholder="Price" disabled="disabled"  />' +
        '</td>' +
        '<td>' +
        '<input type="number" class="form-control" id="product-total-number-' +
        last +
        '" placeholder="Total" disabled="disabled"  />' +
        '</td>' +
        '</tr>');
}

function renderMenu(total) {
    $('#table-products').append(
        '<tr id="table-menu">'
        +
        '<th>'
        +
        '</th>'
        +
        '<th>'
        +
        '<input type="submit" class="btn btn-info" value="Add product" onclick="ajaxAddProduct()" />'
        +
        '</th>'
        +
        '<th>'
        +
        '<input type="submit" class="btn btn-info" value="Finish receipt" onclick="finishReceipt()" />'
        +
        '</th>'
        +
        '<th>'
        +
        '<input type="submit" class="btn btn-info" value="Delete receipt" onclick="deleteReceipt()" />'
        +
        '</th>'
        +
        '<th>'
        +
        '<td>'
        +
        '<span class="d-flex justify-content-center mt-2 h5">'
        +
        'Total sum: ' + '$' + total.total 
        +
        '</span>'
        +
        '</td>'
        +
        '</th>'
        +
        '</tr>'
    );
}

function deleteReceipt() {
    $.get({
        url: "/Receipt/Delete",
        success: function success() {
            getReceipt();
        },
        error: function error(errorMessage) {
            console.log(errorMessage);
        }
    });
}

$(document).ready(() => {
    getReceipt();
    getAllProducts(products, productsForAutocomplete);
});