var product = {
    init: function () {
        product.loadColor();
        product.showError();
        product.registerEvent();
    },
    registerEvent: function () {
        $('.mycolor').off('click').on('click', function () {
            product.loadData();
            product.showError();
        });

    },
    loadData: function () {
        var color = '';
        $("input[type=checkbox]:checked").each(function () {
            color += $(this).val() + ",";
        });
        color = color.slice(0, -1);
        $('#txtColor').val(color);
    },
    loadColor: function () {
        var color = ($('#color').val()).split(",");
        for (i = 0; i < color.length; i++) {
            $("input[type=checkbox]").each(function () {
                if ($(this).val() == color[i]) {
                    $(this).attr("checked", 'checked');
                }
            });
        }
        $('#txtColor').val(color);
    },

    showError: function () {
        var color = $('#txtColor').val();
        if (color == '') {
            $('.color-error').show();
        }
        else {
            $('.color-error').hide();
        }
    }
}
product.init();
