var user = {
    init: function () {//xử lí sự kiện ở đây
        user.registerEvents();
    },
    registerEvents: function () {
        $('.btnUser-active').off('click').on('click', function (e) {
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

var content = {
    init: function () {//xử lí sự kiện ở đây
        content.registerEvents();
    },
    registerEvents: function () {
        $('.btnContent-active').off('click').on('click', function (e) {
            e.preventDefault();//Để reset
            //gọi ajax
            //lấy dữ liệu nhận từ mục data có đuôi là id bên view (class btnContent-active data-id = @item.ID)
            var btn = $(this);
            var id = btn.data('id');
            $.ajax({
                url: "/Admin/Content/ChangeStatus",//gọi đến hàm trong controller
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
content.init();

var product = {
    init: function () {//xử lí sự kiện ở đây
        product.registerEvents();
    },
    registerEvents: function () {
        $('.btnProduct-active').off('click').on('click', function (e) {
            e.preventDefault();//Để reset
            //gọi ajax
            //lấy dữ liệu nhận từ mục data có đuôi là id bên view (class btnProduct-active data-id = @item.ID)
            var btn = $(this);
            var id = btn.data('id');
            $.ajax({
                url: "/Admin/Product/ChangeStatus",//gọi đến hàm trong controller
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
product.init();

var productCategory = {
    init: function () {//xử lí sự kiện ở đây
        productCategory.registerEvents();
    },
    registerEvents: function () {
        $('.btnProductCategory-active').off('click').on('click', function (e) {
            e.preventDefault();//Để reset
            //gọi ajax
            //lấy dữ liệu nhận từ mục data có đuôi là id bên view (class btnProduct-active data-id = @item.ID)
            var btn = $(this);
            var id = btn.data('id');
            $.ajax({
                url: "/Admin/ProductCategory/ChangeStatus",//gọi đến hàm trong controller
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
productCategory.init();

var Category = {
    init: function () {//xử lí sự kiện ở đây
        Category.registerEvents();
    },
    registerEvents: function () {
        $('.btnCategory-active').off('click').on('click', function (e) {
            e.preventDefault();//Để reset
            //gọi ajax
            //lấy dữ liệu nhận từ mục data có đuôi là id bên view (class btnProduct-active data-id = @item.ID)
            var btn = $(this);
            var id = btn.data('id');
            $.ajax({
                url: "/Admin/ContentCategory/ChangeStatus",//gọi đến hàm trong controller
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
Category.init();

var order = {
    init: function () {//xử lí sự kiện ở đây
        order.registerEvents();
    },
    registerEvents: function () {
        $('.btnOrder-active').off('click').on('click', function (e) {
            e.preventDefault();//Để reset
            //gọi ajax
            //lấy dữ liệu nhận từ mục data có đuôi là id bên view (class btn-active data-id = @item.ID)
            var btn = $(this);
            var id = btn.data('id');
            $.ajax({
                url: "/Admin/Order/ChangeStatus",//gọi đến hàm trong controller
                data: { id: id },//truyền dữ liệu này lên controller vừa gọi
                dataType: "json",
                type: "POST",
                success: function (response) {//Khi đã thành công, bắt đầu đổi dữ liệu hiển thị
                    console.log(response);
                    if (response.status == true) {
                        btn.text('đã xác nhận');
                    }
                    else {
                        btn.text('chưa xác nhận');
                    }
                }
            });
        });
    }
}
order.init();