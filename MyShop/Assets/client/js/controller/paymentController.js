var payment = {
    init: function () {
        payment.getLoginUser();
        payment.loadProvince();
        payment.registerEvent();
    },
    registerEvent: function () {
        $('#forminfo').validate({
            rules: {
                full_name: "required",
                address: "required",
                phone: "required",
                email: {
                    required: true,
                    email: true
                },
                province_id: "required"
            },
            messages: {
                full_name: "Nhập họ tên",
                address: "Nhập địa chỉ",
                phone: "Nhập số điện thoại",
                email: {
                    required: "Bạn cần nhập email",
                    email: "Định dạng email chưa đúng"
                },
                province_id: "Chọn tỉnh thành"
            }
        });

        $('.btn-checkout').off('click').on('click', function () {
            var isValid = $('#forminfo').valid();
            if (isValid) {
                payment.payMent();
            }
            else {
                $('.summary').show();
            }
        });

        $('#ddlProvince').off('change').on('change', function () {
            var id = $(this).val();
            if (id != '') {
                payment.loadDistrict(parseInt(id));
            }
            else {
                $('#ddlDistrict').html('');
            }
        });
    },
    getLoginUser: function () {
        $.ajax({
            url: '/ShoppingCart/GetUser',
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status) {
                    var user = response.data;
                    $('.user-login').hide();
                    $('.color-blue').hide();
                    $('#txtName').val(user.Name);
                    $('#txtAddress').val(user.Address);
                    $('#txtEmail').val(user.Email);
                    $('#txtPhone').val(user.Phone);
                    $('#txtEmail').attr('readonly', true);                   
                }               
            }
        });
    },
    loadProvince: function () {

        $.ajax({
            url: '/ShoppingCart/LoadProvince',
            type: "POST",
            dataType: "json",
            success: function (response) {
                if (response.status == true) {
                    var html = '<option value="">Vui lòng họn tỉnh/thành phố.</option>';
                    var data = response.data;
                    $.each(data, function (i, item) {
                        html += '<option value="' + item.ID + '">' + item.Name + '</option>'
                    });
                    $('#ddlProvince').html(html);
                }
            }
        })
    },
    loadDistrict: function (id) {
        $.ajax({
            url: '/ShoppingCart/LoadDistrict',
            type: "POST",
            data: { provinceID: id },
            dataType: "json",
            success: function (response) {
                if (response.status == true) {
                    var html = '<option value="">Vui lòng chọn quận/huyện.</option>';
                    var data = response.data;
                    $.each(data, function (i, item) {
                        html += '<option value="' + item.ID + '">' + item.Name + '</option>'
                    });
                    $('#ddlDistrict').html(html);
                }
            }
        })
    },
    payMent: function () {
        var PaymentMethod = $("input:radio:checked").next().text();
        var ddlProvince = $("#ddlProvince option:selected").text();
        var ddlDistrict = $("#ddlDistrict option:selected").text();
        var order = {
            ID: 0,
            CustomerName: $('#txtName').val(),
            CustomerAddress: $('#txtAddress').val() + ' ' + ddlDistrict + ',' + ddlProvince,
            CustomerEmail: $('#txtEmail').val(),
            CustomerMobile: $('#txtPhone').val(),
            CustomerMessage: $('#txtMessage').val(),
            PaymentMethod: PaymentMethod,
            PaymentStatus: false,
            Status: false
        };
        $.ajax({
            url: '/ShoppingCart/CreateOrder',
            type: 'POST',
            dataType: 'json',
            data: {
                orderViewModel: JSON.stringify(order)
            },
            success: function (response) {

                if (response.status) {
                    alert('Gởi đơn hàng thành công. Chúng tôi sẽ liên hệ sớm nhất.');
                    window.location.href = "/";
                }
                else {
                    alert('Có lỗi xảy ra.');
                }
            }
        });
    }
}
payment.init();
