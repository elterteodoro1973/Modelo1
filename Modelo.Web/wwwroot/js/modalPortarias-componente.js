var urlPost = ''
var dataPortaria = [];
window.onload = function ()
{
    var $vars = $('#Portaria-componente\\.js').data();
    urlPost = $vars.urlpost;
};


function zerarTr() {
    location.reload()
}

$('[data-bs-target="#modalPortaria"]').click(function () {
    document.getElementById("id").value = $(this).attr('id');
});


function PortariasUsos(empreendimentoId, codigo)
{

    let url = urlPost + `?empreendimentoId=${empreendimentoId}&codigo=${codigo}`;
    var modalPortaria = new bootstrap.Modal('#modalPortaria');

    let table = $('#tablePortaria').DataTable();
    $('#tablePortaria').DataTable().clear();
    $('#tablePortaria').DataTable().destroy();

    $.ajax({
        type: "GET",
        url: `${url}`,
        dataType: "json",
        async: false,
        success: function (json, status) {

            var dataSet = JSON.parse(json);
            var detalhes = dataSet.detalhes;

            table = new DataTable('#tablePortaria',
                {
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
                    { data: 'Codigo' },
                    { data: 'NumeroPortaria' },
                    { data: 'InicioPortaria' },
                    { data: 'FimVigenciaPortaria' },
                    { data: 'Vigente' },
                    { data: 'Acoes' },

                    //{
                    //    className: 'dt-control',
                    //    orderable: false,
                    //    data: null,
                    //    defaultContent: ''
                    //}
                ],

                order: [[1, 'asc']]
            });

            modalPortaria.show();
        },
        error: function (xhr, status, error) {
            console.log(error)
        },
        complete: function (result) {
            console.log(result)
        }
    });

    //// Add event listener for opening and closing details
    //table.on('click', 'td.dt-control', function (e) {
    //    let tr = e.target.closest('tr');
    //    let row = table.row(tr);

    //    if (row.child.isShown()) {
    //        // This row is already open - close it
    //        row.child.hide();
    //    }
    //    else {
    //        // Open this row
    //        row.child(format(row.data())).show();
    //    }
    //});
}

