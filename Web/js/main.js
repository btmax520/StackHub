$(document).ready(function () {
    var globlQuery = {
    };
    var queryTimer;
    $('#left-nav a').click(function () {
        $(this).siblings().removeClass('active');
        $(this).addClass('active');
    })
    //$("#showFilter").click(function () {
    //    $("#filterPanel ul").not(":first").toggleClass("am-hide");
    //    if($(this).text() == 
    //});
    $('a.list-query').click(function () {
        if (queryTimer)
            clearTimeout(queryTimer);
        queryTimer = setTimeout(function () {
            //alert($('#searchResult').html());
            var params = {
                KeyWords: $('#searchKeywords').val(),
                CurrentPage : 1,
                QueryFilters: {}
            };
            $('.filter-item li.am-active a').each(function () {
                var key = $(this).attr("key");
                if(key){
                    var value = $(this).attr("value");
                    //var param = {
                    //    Category: key,
                    //    Value:value
                    //};
                    //if (globlQuery[key]) {
                    //    globlQuery[key].push(value);
                    //}
                    //else {
                    //    globlQuery[key] = [];
                    //    globlQuery[key].push(value);
                    //}
                    //param.Catetory = key;
                    //param.Value = value;
                    //$('<input type="hidden" name="' + param.Category + '" value="' + param.Value + '" />').appendTo('#searchForm')
                    if (params.QueryFilters[key]) {
                        params.QueryFilters[key].push(value);
                    }
                    else {
                        params.QueryFilters[key] = [];
                        params.QueryFilters[key].push(value);
                    }
                    
                }
            });
            $("#searchS").val(JSON.stringify(params));
            $('#searchForm').submit();
            //$.get("/job", { s: JSON.stringify(params) }, function (data) {
            //    //$("#searchResult",data).append($(data).find("#searchResult").children());
            //    alert($("#searchResult", data).html());
            //})
        }, 888);
        
        if ($(this).text() == "所有")
        {
            $(this).parent('li').addClass("am-active").siblings(":not(.all)").removeClass("am-active");
        }
        else{
            $(this).parent('li').toggleClass("am-active").siblings(".all").removeClass("am-active");
        }
        
        return false;
    });
})