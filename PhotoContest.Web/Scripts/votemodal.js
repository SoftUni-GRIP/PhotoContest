function modal() {
    $("a[data-modal]").on("click", function () {
        $("#vote-modal-content").load(this.href, function () {
            $("#vote-modal").modal({ keyboard: true }, "show");
        });
        return false;
    });
}

modal();


