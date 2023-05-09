$(document).ready(function () {

    $('table.table thead tr th input[type="checkbox"]').click(function () {
        $('table tbody tr td input[name="' + $(this).prop("id") + '"]').prop("checked", $(this).is(":checked"));
    });

    $("input.form-control-plaintext").prop("readonly", "true");

    function setSelectScript() {
        $("div.select2 div select option").click(function () {
            var toRemove = $(this);
            var option = $(this).get(0).outerHTML;
            var value = $(this).prop("value"), text = $(this).html();
            var holder = $(this).parent().parent().parent();

            holder.children("div").each(function () {
                //console.log($(this).children("select option[value='" + value + "']").length);
                //console.log(value);
                var i = 0;
                $(this).children("select").each(function (index) {
                    console.log($(this).prop("class"));
                    console.log(index);

                    if ($(this).children("option[value='" + value + "']").length == 0) {
                        $(this).append(option);
                        setTimeout(function () {
                            toRemove.remove();
                            setSelectScript();
                        }, 100);
                    }
                    if ($(this).hasClass("selected")) {
                        console.log("meron");
                        $(this).children("option").prop("selected", true);
                    }
                    else {
                        $(this).children("option").prop("selected", false);
                    }
                    i++;
                })
            })
        });
    }
    setSelectScript();

    $("div.select2 div").each(function () {
        var options = $(this).children("select");
        $(this).children("input").keyup(function () {
            var currentValue = $(this).val().toLowerCase();

            options.children("option").filter(function () {
                $(this).toggle($(this).text().toLowerCase().indexOf(currentValue) > -1)
            });
        });
    });

    $("table").parent("div").addClass("table-responsive");

    $(".dataTables_filter").addClass("margin_top_5");

    //var unique_id = $.gritter.add({
    //    // (string | mandatory) the heading of the notification
    //    title: 'Welcome to Aveta!',
    //    // (string | mandatory) the text inside the notification
    //    text: 'test',
    //    // (string | optional) the image to display on the left
    //    //image: 'img/ui-sam.jpg',
    //    // (bool | optional) if you want it to fade out on its own or just sit there
    //    sticky: false,
    //    // (int | optional) the time you want it to be alive for before fading out
    //    time: 8000,
    //    // (string | optional) the class name you want to apply to that specific message
    //    class_name: 'my-sticky-class alert gritter-success'
    //});
});