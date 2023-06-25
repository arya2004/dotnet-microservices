var dataTable;

$(document).ready(function(){
    
    var url = window.location.search;
    if (url.includes("approved")) {
        loadDataTAble("approved")
    }
    else if (url.includes("readyforpickup")) {
        loadDataTAble("readyforpickup")
    }
    else if (url.includes("calcelled")) {
        loadDataTAble("calcelled")
    }
    else  {
        loadDataTAble("all")
    }

});

function loadDataTAble(status) {
    dataTable = $('#tblData').DataTable({
        order: [{0:'desc'}],
        "ajax": { url: "/order/GetAll?status="+status },
        "columns": [
            {
                data: "orderHeaderId", "width": "5%"
            },
            {
            data: "email", "width": "25%"
            },
            {
            data: "name", "width": "20%"
            },
            {
            data: "phone", "width": "10%"
            },
            {
            data: "status", "width": "10%"
            },
            {
            data: "orderTotal", "width": "10%"
            },
            {
            data: "orderHeaderId",
            "render": function(data){
                return `<div class="w-75 btn-group" role="group">

                    <a href="/order/orderDetail?orderId=${data}" class="btn btn-primary mx-2"><i class="bi bi-pencil"></i></a>
                </div>`
            },
             "width": "10%"
            }
        ]
    })
}