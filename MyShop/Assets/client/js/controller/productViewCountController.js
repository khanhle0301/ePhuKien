var productconfig = {
    pageSize: 12,
    pageIndex: 1,
}
var productController = {
    init: function () {
        productController.loadData();
        productController.registerEvent();
    },
    registerEvent: function () {
        $('.btn-popup').off('click').on('click', function (e) {
            e.preventDefault();
            $('#productManange').modal('show');
            $('#hidProductID').val($(this).data('id'));
            var productId = parseInt($(this).data('id'));
            productController.loadColor(productId);
            productController.loadProductName(productId);
            productController.loadData(productId);
        });

        $('#sortControl').off('click').on('click', function () {
            productController.loadData(true);
        });
        $('.price').off('click').on('click', function () {
            productController.loadData(true);
        });

        $('.color_wrapper').off('click').on('click', function () {
            productController.loadData(true);
        });

    },
    loadData: function (changePageSize) {
        var price = '';
        $(".prices li input[type=checkbox]:checked").each(function () {
            price += $(this).val() + ",";
        });
        price = price.slice(0, -1);

        var color = '';
        $(".colors li input[type=checkbox]:checked").each(function () {
            color += $(this).val() + ",";
        });
        color = color.slice(0, -1);

        var sort = $('#sortControl').val();
        $.ajax({
            url: '/Product/LoadDataPopular',
            type: 'GET',
            data: {
                sort: sort,
                price: price,
                color: color,
                page: productconfig.pageIndex,
                pageSize: productconfig.pageSize
            },
            dataType: 'json',
            success: function (response) {
                if (response.status) {
                    var data = response.data;
                    var html = '';
                    var template = $('#data-template').html();
                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {
                            sale: (item.PromotionPrice != null) ? true : false,
                            PromotionPrice: numeral(item.PromotionPrice).format('0,0'),
                            ProductImage: item.Image,
                            ProductImage2: item.Image2,
                            ProductName: item.Name,
                            ProductID: item.ID,
                            ProductPrice: numeral(item.Price).format('0,0'),
                            url: '/san-pham/' + item.ProductCategory.Alias + '/' + item.Alias + '-' + item.ID + '.html'
                        });

                    });
                    if (html == '') {
                        $('.notfound-product').show();
                        $('.sort-wrapper').hide();
                        $('#grid_pagination').hide();
                    }
                    else {
                        $('.notfound-product').hide();
                        $('.sort-wrapper').show();
                        $('#grid_pagination').show();
                        $('#tblData').html(html);
                        $('#productCount').text(response.total);
                        productController.paging(response.total, function () {
                            productController.loadData();
                        }, changePageSize);
                    }
                    productController.registerEvent();
                }
            }
        })
    },
    paging: function (totalRow, callback, changePageSize) {
        var totalPage = Math.ceil(totalRow / productconfig.pageSize);
        if ($('#pagination a').length === 0 || changePageSize === true) {
            $('#pagination').empty();
            $('#pagination').removeData("twbs-pagination");
            $('#pagination').unbind("page");
        }

        $('#pagination').twbsPagination({
            totalPages: totalPage,
            first: "«",
            next: "›",
            last: "»",
            prev: "‹",
            visiblePages: 10,
            onPageClick: function (event, page) {
                productconfig.pageIndex = page;
                setTimeout(callback, 200);
            }
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
                    var template = $('#tpl-product-color').html();
                    var html = '';
                    var data = res.data;
                    $.each(data, function (i, item) {
                        html += Mustache.render(template, {
                            ProductNameColor: item.Name,
                        });
                    });

                    $('#product-color').html(html);
                }
            }
        })
    },

    loadProductName: function (id) {
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
                    $('#productName').html(productName);
                }
            }
        })
    }
}
productController.init();