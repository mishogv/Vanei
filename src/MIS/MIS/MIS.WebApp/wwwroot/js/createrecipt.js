//Find product and load current product information
//And create new row
//TODO : Make ajax and load data from server. disable product when is already added
var products = [];
var productsNames = [];
var index = { index: -1 };
var numberInSequence = { number: -1 };
var total = { total: 0 };

function loadData(num) {
    numberInSequence.number = num;
    var selector = '#product-name-number-' + numberInSequence.number;

    ajaxLoadData(products, productsNames);

    autoCompleteShowData(productsNames, selector);

    setData(productsNames, selector, products, index, numberInSequence);
}

function ajaxLoadData(products, productsNames) {
    if (products.length === 0) {
        $(function () {
            $.ajax({
                url: "/Receipt/AllPorducts",
                success: function load(data) {
                    for (let product of data) {
                        productsNames.push(product.name);
                    }

                    for (let product of data) {
                        products.push(product);
                    }
                },
                error: function error() {
                    //TODO Log in console
                    alert('error');
                }
            });
        });
    }
}

function autoCompleteShowData(productsNames, selector) {
    if (productsNames.length > 0) {
        $(selector).autocomplete({
            minLength: 2,
            source: productsNames,
            focus: function (event, ui) {
                //TODO : add category under product
                $(selector).val(ui.item.label);
                return false;
            },
            select: function (event, ui) {
                $(selector).val(ui.item.label);
                return false;
            }
        });
    }
}

function setData(productsNames, selector, products, index, numberInSequence) {
    if (productsNames.includes($(selector).val())) {
        index.index = products.findIndex(x => x.name === $(selector).val());
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
            //TODO : FIND LAST PRODUCT AND GET INFO
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
    total.total = (parseFloat(total.total) + (price * quantity)).toPrecision(6);
    $('#product-total-number-' + numberInSequence.number).val(((price * quantity)).toPrecision(6));
}

function addProduct(product) {
    //TODO : disable product and render new empty product and security
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
            '<input type="number" class="form-control" id="product-id-number-' + i + '" value="' + data.products[i].id + '" disabled="disabled"  />'
            +
            '</td>'
            +
            '<td>'
            +
            '<input type="text" class="form-control" id="product-name-number-' + i + '" value="' + data.products[i].name + '" disabled="disabled"  />'
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
            '<input type="text" class="form-control" id="product-barcode-number-' + i + '" value="' + data.products[i].barcode + '" disabled="disabled"  />'
            +
            '</td>'
            +
            '<td>'
            +
            '<input type="number" class="form-control" id="product-price-number-' + i + '" value="' + data.products[i].price + '" disabled="disabled"  />'
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
        '<input type="number" class="form-control" id="product-id-number-' +
        last +
        '" value="0' +
        '" disabled="disabled"  />' +
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
        '<input type="text" class="form-control" id="product-barcode-number-' +
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
        '<input type="submit" class="btn btn-dark container-fluid" value="Delete products" />'
        +
        '</th>'
        +
        '<th>'
        +
        '<input type="submit" class="btn btn-dark container-fluid" value="Add product" onclick="ajaxAddProduct()" />'
        +
        '</th>'
        +
        '<th>'
        +
        '<input type="submit" class="btn btn-dark container-fluid" value="Finish receipt" onclick="finishReceipt()" />'
        +
        '</th>'
        +
        '<th>'
        +
        '<input type="submit" class="btn btn-dark container-fluid" value="Delete receipt" onclick="deleteReceipt()" />'
        +
        '</th>'
        +
        '<th>'
        +
        '<td>'
        +
        '<span class="d-flex justify-content-center mt-2 h5">'
        +
        'Total sum: ' + total.total + '$'
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
    getReceipt();
}

$(document).ready(() => {
    getReceipt();
});