//Find product and load current product information
//And create new row
//TODO : Make ajax and load data from server. disable product when is already added
var products = [];
var productsNames = [];
var index = { index: -1 };
var numberInSequence = { number: -1 };

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
    $.ajax({
        url: "/Receipt/Add",
        success: function (data) {
            // render messages 
        },
        error: function error() {
            //TODO Log in console
            alert('error');
        }
    });
}

function getReceipt() {
    $.get({
        url: "/Receipt/LoadReceipt",
        success: function success(data) {
            console.log(data);
            renderReceipt(data);
        },
        error: function error(errorMessage) {
            console.log(errorMessage);
        }
    })
}

function renderReceipt(data) {
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

    $('#table-products').append(
        '<tr>'
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
        '<input type="submit" class="btn btn-dark container-fluid" value="Finish receipt" />'
        +
        '</th>'
        +
        '<th>'
        +
        '<input type="submit" class="btn btn-dark container-fluid" value="Delete receipt" />'
        +
        '</th>'
        +
        '<th>'
        +
        '<td>'
        +
        '<span class="d-flex justify-content-center mt-2 h5">'
        +
        'Total sum: ' + data.total + '$'
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

$(document).ready(() => {
    getReceipt();
});
