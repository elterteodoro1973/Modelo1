
var urlPost = ''


window.onload = function () {

    var $vars = $('#Historico-componente\\.js').data();

    urlPost = $vars.urlpost;

};


function zerarTr() {
    location.reload()
}

$('[data-bs-target="#modalHistorico"]').click(function () {
    document.getElementById("id").value = $(this).attr('id');
});

//Finalidade
function historicoFinalidade(id) {

    let url = urlPost + `?id=${id}`;
    var modalHistorico = new bootstrap.Modal('#modalHistorico');

    let table = $('#tableHistorico').DataTable();
    $('#tableHistorico').DataTable().clear();
    $('#tableHistorico').DataTable().destroy();

    $.ajax({
        type: "GET",
        url: `${url}`,
        dataType: "json",
        async: false,
        success: function (json, status) {

            var dataSet = JSON.parse(json);
            var detalhes = dataSet.detalhes;

            table = new DataTable('#tableHistorico', {
                "language": {
                    "search": "",
                    "lengthMenu": "_MENU_",
                    "zeroRecords": "Nenhum registro encontrado",
                    "info": "",
                    "infoEmpty": "Nenhum registro encontrado",
                    "infoFiltered": "(filtrado do total de _MAX_ registros)",
                    "searchPlaceholder": "🔍︎ Pesquisar",
                    "paginate": {
                        "first": '<i class="fa fa-angle-double-left"></i>',
                        "last": '<i class="fa fa-angle-double-right"></i>',
                        "previous": '<i class="fa fa-angle-left"></i>',
                        "next": '<i class="fa fa-angle-right"></i>'
                    }
                },

                data: dataSet,
                columns: [
                    { data: 'Data' },
                    { data: 'Usuario' },
                    { data: 'Campo' },

                    {
                        className: 'dt-control',
                        orderable: false,
                        data: null,
                        defaultContent: ''
                    }
                ],
                
                order: [[1, 'asc']]
            });

            modalHistorico.show();

        },
        error: function (xhr, status, error) {
            console.log(error)
        },
        complete: function (result) {
            console.log(result)


        }
    });

    // Add event listener for opening and closing details
    table.on('click', 'td.dt-control', function (e) {
        let tr = e.target.closest('tr');
        let row = table.row(tr);

        if (row.child.isShown()) {
            // This row is already open - close it
            row.child.hide();
        }
        else {
            // Open this row
            row.child(formatFinalidade(row.data())).show();
        }
    });
}

//Formatting function for row details - modify as you need
function formatFinalidade(d) {
    // `d` is the original data object for the row
    return (
        '<dl>' +
        '<dt>Alterado para:</dt>' +

        '<br />' +
        '<dt>Finalidade cobrável : ' + d.Cobravel + '</dt>' +
        

        '<br />' +
        '<dt>Finalidade Ativa: ' + d.Inativo + '</dt>' +

        '</dl>'

        
    );
}


//Coeficientes
function historicoCoeficientes() {

    let url = urlPost;
    var modalHistorico = new bootstrap.Modal('#modalHistorico');

    let tableCoeficiente = $('#tableHistoricoCoeficiente').DataTable();
    $('#tableHistoricoCoeficiente').DataTable().clear();
    $('#tableHistoricoCoeficiente').DataTable().destroy();

    $.ajax({
        type: "GET",
        url: `${url}`,
        dataType: "json",
        async: false,
        success: function (json, status) {

            var dataSet = JSON.parse(json);
            var detalhes = dataSet.detalhes;

            tableCoeficiente = new DataTable('#tableHistoricoCoeficiente', {
                "language": {
                    "search": "",
                    "lengthMenu": "_MENU_",
                    "zeroRecords": "Nenhum registro encontrado",
                    "info": "",
                    "infoEmpty": "Nenhum registro encontrado",
                    "infoFiltered": "(filtrado do total de _MAX_ registros)",
                    "searchPlaceholder": "🔍︎ Pesquisar",
                    "paginate": {
                        "first": '<i class="fa fa-angle-double-left"></i>',
                        "last": '<i class="fa fa-angle-double-right"></i>',
                        "previous": '<i class="fa fa-angle-left"></i>',
                        "next": '<i class="fa fa-angle-right"></i>'
                    }
                },

                data: dataSet,
                columns: [
                    { data: 'DataFormatada' },
                    { data: 'Usuario' },
                    { data: 'Tipo' },

                    {
                        className: 'dt-control',
                        orderable: false,
                        data: null,
                        defaultContent: ''
                    }
                ],

                order: [[1, 'asc']]
            });

            modalHistorico.show();

        },
        error: function (xhr, status, error) {
            console.log(error)
        },
        complete: function (result) {
            console.log(result)


        }
    });

    // Add event listener for opening and closing details
    tableCoeficiente.on('click', 'td.dt-control', function (e) {

        var delayInMilliseconds = 1000;

        setTimeout(function () {

            let tr = e.target.closest('tr');
            let row = tableCoeficiente.row(tr);

            if (row.child.isShown()) {
                // This row is already open - close it
                row.child.hide();
            }
            else {
                // Open this row
                row.child(formatCoeficiente(row.data())).show();
            }
        }, delayInMilliseconds);

        
    });

    //Formatting function for row details - modify as you need
    function formatCoeficiente(d) {
        // `d` is the original data object for the row
        return (
            '<dl>' +
            '<dt>Alterado para:</dt>' +

            '<br />' +
            '<dt>Campo: ' + d.Campo + '</dt>' +


            '<br />' +
            '<dt>Valor: ' + d.Valor + '</dt>' +

            '</dl>'


        );
    }
}


//ParametroCalculoCBH
function historicoParametroCalculoCBH() {

    let url = urlPost;
    var modalHistorico = new bootstrap.Modal('#modalHistorico');

    let tableParametroCalculo = $('#tableHistoricoParametroCalculo').DataTable();
    $('#tableHistoricoParametroCalculo').DataTable().clear();
    $('#tableHistoricoParametroCalculo').DataTable().destroy();

    $.ajax({
        type: "GET",
        url: `${url}`,
        dataType: "json",
        async: false,
        success: function (json, status) {

            var dataSet = JSON.parse(json);
            var detalhes = dataSet.detalhes;

            tableParametroCalculo = new DataTable('#tableHistoricoParametroCalculo', {
                "language": {
                    "search": "",
                    "lengthMenu": "_MENU_",
                    "zeroRecords": "Nenhum registro encontrado",
                    "info": "",
                    "infoEmpty": "Nenhum registro encontrado",
                    "infoFiltered": "(filtrado do total de _MAX_ registros)",
                    "searchPlaceholder": "🔍︎ Pesquisar",
                    "paginate": {
                        "first": '<i class="fa fa-angle-double-left"></i>',
                        "last": '<i class="fa fa-angle-double-right"></i>',
                        "previous": '<i class="fa fa-angle-left"></i>',
                        "next": '<i class="fa fa-angle-right"></i>'
                    }
                },

                data: dataSet,
                columns: [
                    { data: 'DataFormatada' },
                    { data: 'Usuario' },
                    { data: 'AnoBase' },
                    { data: 'PeriodoLiberacao' },
                    { data: 'MesInicioParcelas' },
                    { data: 'QuantidadeParcelas' }
                ],
                columnDefs: [{ width: '30%', targets: 3 }, {
                    type: 'date-br',
                    targets: 2
}],
                order: [[1, 'asc']]
            });

            modalHistorico.show();

        },
        error: function (xhr, status, error) {
            console.log(error)
        },
        complete: function (result) {
            console.log(result)


        }
    });
    
}


//Usuarios

function historico(id) {
    $('#loadingDiv').show();
    let url = urlPost + `?id=${id}`;
    var modalHistorico = new bootstrap.Modal('#modalHistorico');

    let table = $('#tableHistorico').DataTable();
    $('#tableHistorico').DataTable().clear();
    $('#tableHistorico').DataTable().destroy();
    
    $.ajax({
        type: "GET",
        url: `${url}`,
        dataType: "json",
        async: true,
        success: function (json, status) {

            var dataSet = JSON.parse(json);
            var detalhes = dataSet.detalhes;

            table = new DataTable('#tableHistorico', {
                "language": {
                    "search": "",
                    "lengthMenu": "_MENU_",
                    "zeroRecords": "Nenhum registro encontrado",
                    "info": "",
                    "infoEmpty": "Nenhum registro encontrado",
                    "infoFiltered": "(filtrado do total de _MAX_ registros)",
                    "searchPlaceholder": "🔍︎ Pesquisar",
                    "paginate": {
                        "first": '<i class="fa fa-angle-double-left"></i>',
                        "last": '<i class="fa fa-angle-double-right"></i>',
                        "previous": '<i class="fa fa-angle-left"></i>',
                        "next": '<i class="fa fa-angle-right"></i>'
                    }
                },

                data: dataSet,
                columns: [
                    { data: 'DataFormatada' },
                    { data: 'Usuario' },
                    { data: 'Campo' },

                    {
                        className: 'dt-control',
                        orderable: false,
                        data: null,
                        defaultContent: ''
                    }
                ],

                order: [[1, 'asc']]
            });
            $('#loadingDiv').hide();
            modalHistorico.show();

        },
        error: function (xhr, status, error) {
            $('#loadingDiv').hide();
            alertBootstrapp(error, 'danger');
            console.log(error)
        },
        complete: function (result) {
            console.log(result)


        }
    });

    // Add event listener for opening and closing details
    table.on('click', 'td.dt-control', function (e) {
        let tr = e.target.closest('tr');
        let row = table.row(tr);

        if (row.child.isShown()) {
            // This row is already open - close it
            row.child.hide();
        }
        else {
            // Open this row
            row.child(format(row.data())).show();
        }
    });


    function format(d) {
        console.log(d);
        let titulo = d.Tipo === "Inclusão" ? "Incluído :" : d.Tipo === "Exclusão" ? "Excluído :" : "Alterado para :"
        // `d` is the original data object for the row
        return (
            '<dl>' +
            `<dt>${titulo}</dt>` +

            '<br />' +
            '<dt>Campo: ' + d.Campo + '</dt>' +


            '<br />' +
            '<dt>Valor: ' + d.Valor + '</dt>' +

            '</dl>'


        );
    }
}

