var cart = {
    init: function () {
        cart.registerEvents();
    },
    registerEvents: function () {

        $('.btnAddToCartDetail').off('click').on('click', function (e) {
            e.preventDefault();
            var productId = parseInt($('#hidProductIDPopup').val());
            var quantity = parseInt($('#ProductDetailsPopupForm .product-quantity input.qty').val());          
            var color = document.getElementById("productPopup-color").value;
            $.ajax({
                url: '/ShoppingCart/Add',
                data: {
                    productId: productId,
                    quantity: quantity,
                    color: color
                },
                type: 'POST',
                dataType: 'json',
                success: function (response) {
                    if (response.status) {
                        alert('Thêm sản phẩm thành công.');
                        document.location.reload();
                    }
                    else {
                        alert(response.message);
                    }
                }
            });
        });
    }
}
cart.init();