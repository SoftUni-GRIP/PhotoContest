function invokeModalForm(modalContentId, modalContainerId, modalAttribute) {
    $("a[data-modal="+modalAttribute+"]").on("click", function () {
        $("#" + modalContentId).load(this.href, function () {
            $("#"+ modalContainerId).modal({ keyboard: true }, "show");
        });
        return false;
    });
}

function closeModalFrom(modalContainerId) {
    $('#' + modalContainerId).modal('hide');
}
