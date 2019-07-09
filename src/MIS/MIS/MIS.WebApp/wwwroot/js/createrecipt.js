//Find product and load current product information
//And create new row
//TODO : Make ajax and load data from server. disable product when is already added
var products = [];
var productsNames = [];
var index = -1;
var numberInSequence = -1;

function loadData(num) {
    numberInSequence = num;
    var selector = '#product-name-number-' + numberInSequence;

    ajaxLoadData(products, productsNames);

    autoCompleteShowData(productsNames, selector);

    setData(productsNames, selector, products, index, numberInSequence);
}

function ajaxLoadData(products, productsNames) {
    if (products.length === 0) {
        $(function () {
            $.ajax({
                url: "/All/Products",
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
        index = products.findIndex(x => x.name === $(selector).val());
        $('#product-id-number-' + numberInSequence).val(products[index].id).prop('disabled', true);
        $('#product-barcode-number-' + numberInSequence).val(products[index].barcode).prop('disabled', true);
        $('#product-price-number-' + numberInSequence).val(products[index].price);
        $('#product-quantity-number-' + numberInSequence)
            .prop('title', 'In stock there are: ' + products[index].quantity)
            .tooltip({ placement: "top" });
    };
}

function ajaxAddProduct() {
    console.log(numberInSequence);
    console.log(index);
}