var user = {
    init: function () {
        user.registerEvents();
    },
    registerEvents: function () {
        $('.btn-active').off('click').on('click', function (e) {
            e.preventDefault();
            var btn = $(this);
            var groupId = btn.data('id');
            var roleID = btn.data('role');
            $.ajax({
                url: "/Admin/Credential/ChangeStatus",
                data: {
                    groupId: groupId,
                    roleID: roleID
                },
                dataType: "json",
                type: "POST",
                success: function (response) {
                    console.log(response);
                    if (response.status == true) {
                        btn.text('Kích hoạt');
                    }
                    else {
                        btn.text('Khoá');
                    }
                }
            });
        });
    }
}
user.init();