
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

