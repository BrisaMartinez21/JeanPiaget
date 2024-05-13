(function () {
    $("#Concepto").select2();
    $("#GradoId").select2();
}());

$("#Concepto").on('change', function () {
    console.log($("#Concepto").val(), "Hola desde cambio 2");
    var conceptoChange = $("#Concepto").val();
    if (conceptoChange != "14") {
        $("#formGrado").css("display", "none");
    }
    else {
        $("#formGrado").css("display", "block");
    }
});
