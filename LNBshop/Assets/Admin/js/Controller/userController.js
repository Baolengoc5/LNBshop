var user = {
    init: function () {//xử lí sự kiện ở đây
        user.registerEvents();
    },
    registerEvents: function () {
        $('.btn-active').off('click').on('click', function (e) {
            e.preventDefault();//Để reset
            //gọi ajax
            //lấy dữ liệu nhận từ mục data có đuôi là id bên view (class btn-active data-id = @item.ID)
            var btn = $(this);
            var id = btn.data('id');
            $.ajax({
                url: "/Admin/User/ChangeStatus",//gọi đến hàm trong controller
                data: { id: id },//truyền dữ liệu này lên controller vừa gọi
                dataType: "json",
                type: "POST",
                success: function (response) {//Khi đã thành công, bắt đầu đổi dữ liệu hiển thị
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