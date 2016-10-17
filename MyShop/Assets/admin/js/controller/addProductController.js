var product = {
    init: function () {
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
