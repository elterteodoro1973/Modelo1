
var urlPost = ''
var dataHistorico = [];

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


function historicoUso(id) {

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
            row.child(format(row.data())).show();
        }
    });
}

//Formatting function for row details - modify as you need
function format(d) {
    // `d` is the original data object for the row
    return (
        '<dl style="text-align: left;margin-left :3%;">' +
        '<dt>Alterado para:</dt>' +   
        '<dt><br></dt>' +

            '<dt>Empreendimento: <span style="color: #6699FF;">' + d.Empreendimento + '</span></dt>' +
            '<dt>Ugrhi: <span style="color: #6699FF;">' + d.Ugrhi + '</span></dt>' +
            '<dt>Código: <span style="color: #6699FF;">' + d.Codigo + '</span></dt>' +
            '<dt>Data de Criação: <span style="color: #6699FF;">' + d.DataCriacao + '</span></dt>' +
            '<dt>Identificação: <span style="color: #6699FF;">' + d.Descricao + '</span></dt>' +
            /*'<dt>Nome: <span style="color: #6699FF;">' + d.TipoUso + '</span></dt>' +*/
            '<dt>Latitude: <span style="color: #6699FF;">' + d.Latitude + '</span></dt>' +
            '<dt>Longitude: <span style="color: #6699FF;">' + d.Longitude + '</span></dt>' +


        
        '<dt>Tipo de uso: <span style="color: #6699FF;">' + d.TipoUso + '</span></dt>' +

        '<dt>Observacao: <span style="color: #6699FF;">' + d.Observacao + '</span></dt>' +

        '<dt>Situação: <span style="color: #6699FF;">' + d.Situacao + '</span></dt>' + 
            

        '</dl><br>' +

        '<dl style="text-align: left;margin-left : 3%;">' +
            
        '<dt><br></dt>' +

        '<dt>' + d.PortariaUsos + '</dt>' +
        '<dt><br></dt>' +

        '<dt>' + d.LancamentoSuperficialUsos + '</dt>' +
        '<dt><br></dt>' +

       

        '<dt>' + d.FinalidadeUsos + '</dt>' +
        '<dt><br></dt>' +

        '<dt>' + d.VazaoDadoOutorgadosUsos + '</dt>' +
        '<dt><br></dt>' +


        '</dl>' 
    );
}