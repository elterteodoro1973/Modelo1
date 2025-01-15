function validarData(e) {
    var dataInicio = document.getElementById("dataInicio").value;
    var dataFim = document.getElementById("dataFim").value;

    var dataInicioCompleta = dataInicio + "-01";
    var dataFimCompleta = dataFim + "-01";

    var arrDataInicio = dataInicioCompleta.split('-');
    var arrDataFim = dataFimCompleta.split('-');

    var dataInicioDate = new Date(arrDataInicio[0], arrDataInicio[1] - 1, arrDataInicio[2]);
    var dataFinalDate = new Date(arrDataFim[0], arrDataFim[1] - 1, arrDataFim[2]);

    if (!dataInicio || !dataFim) return false

    if (dataInicioDate > dataFinalDate) {
        alert("Data Inicial não pode ser maior que a Data Final");
        return false;
    } else {
        return true
    }
}