var cart = {
    init: function () {
        cart.loadData();
        cart.registerEvent();
    },
    registerEvent: function () {
        $('.cart_checkout_btn').off('click').on('click', function (e) {
            e.preventDefault();
            window.location.href = "/thanh-toan.html";
        });
        $('#btnUpdate').off('click').on('click', function () {
            var cartList = [];
            $.each($('.txtQuantity'), function (i, item) {
                cartList.push({
                    ProductId: $(item).data('id'),
                    Quantity: $(item).val(),
                    Color: $(item).data('color')
                });
            });
            $.ajax({
                url: '/ShoppingCart/Update',
                type: 'POST',
                data: {
                    cartData: JSON.stringify(cartList)
                },
                dataType: 'json',
                success: function (response) {
                    if (response.status) {
                        window.location.href = "/gio-hang.html";
                    }
                }
            });
        });

        $('.btnDeleteItem').off('click').on('click', function (e) {
            e.preventDefault();
            var productId = parseInt($(this).data('id'));
            var Color = $(this).data('color');
            cart.deleteItem(productId, Color);
        });

        $('#btnContinue').off('click').on('click', function (e) {
            e.preventDefault();
            window.location.href = "/";
        });
    },

    deleteItem: function (productId, Color) {
        $.ajax({
            url: '/ShoppingCart/DeleteItem',
            data: {
                productId: productId,
                Color: Color
            },
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status) {
                    window.location.href = "/gio-hang.html";
                }
            }
        });
    },

    getTotalOrder: function () {
        var listTextBox = $('.txtQuantity');
        var total = 0;
        $.each(listTextBox, function (i, item) {
            total += parseInt($(item).val()) * parseFloat($(item).data('price'));
        });
        return total;
    },

    loadData: function () {
        $.ajax({
            url: '/ShoppingCart/GetAll',
            type: 'GET',
            dataType: 'json',
            success: function (res) {
                if (res.status) {
                    var cartTotal = $("#cartTotal").html();
                    var template = $('#tplCart').html();
                    var html = '';
                    var data = res.data;
                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {
                            ProductCategoryAlias: item.Product.ProductCategory.Alias,
                            ProductAlias: item.Product.Alias,
                            ProductId: item.ProductId,
                            ProductName: item.Product.Name,
                            Image: item.Product.Image,
                            Price: (item.Product.PromotionPrice == null) ? item.Product.Price : item.Product.PromotionPrice,
                            Color: item.Color,
                            PriceF: (item.Product.PromotionPrice == null) ? numeral(item.Product.Price).format('0,0') : numeral(item.Product.PromotionPrice).format('0,0'),
                            Quantity: item.Quantity,
                            Amount: (item.Product.PromotionPrice == null) ? numeral(item.Quantity * item.Product.Price).format('0,0') : numeral(item.Quantity * item.Product.PromotionPrice).format('0,0')
                        });
                    });

                    $('#cartBody').html(html + cartTotal);

                    if (html == '') {
                        $('#cartContent').html('<p>Không có sản phẩm nào trong giỏ hàng</p><a href="/san-pham/all/"><i class="icon-line2-action-undo"></i> Tiếp tục mua hàng</a>');
                        $('#note').hide();
                    }
                    $('#lblTotalOrder').text(numeral(cart.getTotalOrder()).format('0,0') + '₫');
                    cart.registerEvent();
                }
            }
        })
    }
}
cart.init();