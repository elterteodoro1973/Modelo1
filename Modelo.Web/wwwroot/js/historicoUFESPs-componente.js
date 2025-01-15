
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


function historicoUFESPs(id) {

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

            '<dt>Ano: <span style="color: #6699FF;font-weight: bold;">' + d.Ano + '</span></dt>' +

            '<dt>Valor: <span style="color: #6699FF;">' + d.Valor + '</span></dt>' +       

        '</dl>' 
    );
}