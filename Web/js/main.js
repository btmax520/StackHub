$(document).ready(function () {
    $('#left-nav a').click(function () {
        $(this).siblings().removeClass('active');
        $(this).addClass('active');
    })

    $('a.tagx').click(function () {
        alert(this);
    })

    $("form").submit(function () {
        var $form = $(this);
        var parms = $form.serializeArray();
        var key = $form.attr("name");
        var url = "/api/form/resume";
        $.post(
            url,
            parms,
            function (data) {
                if (data.success) {
                    //window.location.href = "index.html";
                    alert(JSON.stringify(data))
                }
                else {
                    alert('error')
                }

            },
            "json"
        )
        return false;
    });
})