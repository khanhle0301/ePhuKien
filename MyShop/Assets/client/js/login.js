var common = {
    init: function () {
        common.registerEvents();
    },
    registerEvents: function () {
        $('#recover-form-password').validate({
            rules: {               
                email: {
                    required: true,
                    email: true
                }
            },
            messages: {               
                email: {
                    required: "Bạn cần nhập email",
                    email: "Định dạng email chưa đúng"
                }
            }
        });

        $('#recover-password-submit').off('click').on('click', function (e) {
            e.preventDefault();
            var isValid = $('#recover-form-password').valid();
            if (isValid) {
                common.recoverPassword();
            }
        });


        $('#customer_login').validate({
            rules: {
                userName: "required",
                password: "required"
            },
            messages: {
                userName: "Nhập tài khoản",
                password: "Nhập mật khẩu"
            }
        });


        $('#login-form-submit').off('click').on('click', function (e) {
            e.preventDefault();
            var isValid = $('#customer_login').valid();
            if (isValid) {
                common.createLogin();
            }
        });

        $('#create_customer').validate({
            rules: {
                first_name: "required",
                last_name: "required",
                email: {
                    required: true,
                    email: true
                },
                userName: "required",
                password: "required",
                repassword: {
                    equalTo: "#register-form-password"
                }
            },
            messages: {
                first_name: "Nhập họ",
                last_name: "Nhập tên",
                userName: "Nhập tài khoản",
                password: "Nhập mật khẩu",
                email: {
                    required: "Bạn cần nhập email",
                    email: "Định dạng email chưa đúng"
                },
                repassword: "Mật khẩu nhập lại không đúng"
            }
        });

        $('#register-form-submit').off('click').on('click', function (e) {
            e.preventDefault();
            var isValid = $('#create_customer').valid();
            if (isValid) {
                common.register();
            }
        });

    },
    recoverPassword: function () {
        var email = $('#recover-email').val();
        $.ajax({
            url: '/Account/RecoverPassword',
            data: {
                email: email              
            },
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status) {
                    alert('Gởi thành công.');
                    window.location.href = "/dang-nhap.html";
                }
                else {
                    setTimeout(function () {
                        alert('Gởi thất bại.');
                    });                   
                }
            }
        });
    },
    createLogin: function () {
        var userName = $('#login-form-username').val();
        var passWord = $('#login-form-password').val();
        $.ajax({
            url: '/Account/Login',
            data: {
                userName: userName,
                passWord: passWord
            },
            type: 'POST',
            dataType: 'json',
            success: function (response) {
                if (response.status) {
                    window.location.href = "/";
                }
                else {
                    setTimeout(function () {
                        $('#loginResult').show();
                        $('#login-form-username').val(''),
                        $('#login-form-password').val('')
                    });
                }
            }
        });
    },
    register: function () {
        var register = {
            ID: 0,
            UserName: $('#register-form-userName').val(),
            Password: $('#register-form-password').val(),
            GroupID: 'MEMBER',
            Name: $('#first_name').val() + ' ' + $('#register-form-name').val(),
            Address: '',
            Email: $('#register-form-email').val(),
            Phone: ''
        }
        $.ajax({
            url: '/Account/Register',
            type: 'POST',
            dataType: 'json',
            data: {
                accountViewModel: JSON.stringify(register)
            },
            success: function (response) {

                if (response.status) {
                    alert('Đăng ký thành công.');
                    window.location.href = "/dang-nhap.html";
                }
                else {
                    setTimeout(function () {                                           
                        $('#existsResult').text(response.message);
                        $('#registerResult').show();
                    });
                }
            }
        });
    }
}
common.init();