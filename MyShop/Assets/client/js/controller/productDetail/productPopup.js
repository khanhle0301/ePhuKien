var productPopup = {
    init: function () {
        productPopup.registerEvent();
    },
    registerEvent: function () {

        $('.btn-productPopup').off('click').on('click', function (e) {
            e.preventDefault();
            $('#productPopupManange').modal('show');
            $('#hidProductIDPopup').val($(this).data('id'));
            var productId = parseInt($('#hidProductIDPopup').val());
            productPopup.loadColor(productId);        
            productPopup.loadData(productId);
        });

    },

    loadColor: function (id) {
        $.ajax({
            url: '/Product/GetColor',
            data: {
                id: id
            },
            type: 'GET',
            dataType: 'json',
            success: function (res) {
                if (res.status) {
                    var template = $('#tpl-productPopup-color').html();
                    var html = '';
                    var data = res.data;
                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {
                            ProductColor: item.Name,
                        });
                    });

                    $('#productPopup-color').html(html);
                }
            }
        })
    },

    loadData: function (id) {
        $.ajax({
            url: '/Product/GetAll',
            data: {
                id: id
            },
            type: 'GET',
            dataType: 'json',
            success: function (res) {
                if (res.status) {
                    var productName = res.data.Name;
                    $('#productPopupName').html(productName);
                }
            }
        })
    }
}
productPopup.init();