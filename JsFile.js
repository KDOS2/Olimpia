function validarNumeros(e) {
    var key = window.Event ? e.which : e.keyCode
    return (key >= 48 && key <= 57) || key == 45
}

function Editar(valor) {
    __doPostBack('Editar', valor);
}