


function exibirPermissoes() {
    let opcaoSelecionada = $("#categorias option:selected").val();

    if ($(`#${opcaoSelecionada}`).length > 0) {
        $('.categorias').each((i, e) => {
            if ($(e).attr('id') != opcaoSelecionada) {
                $(e).addClass('d-none')
                let inputcheck = 'check' + opcaoSelecionada;
                $(`${inputcheck}`).prop('checked', false);
            }
            else {
                $(e).removeClass('d-none')
            }
        })
        $('#idTodasPermissoes').addClass('d-none');
    }
    else {
        $('.categorias').each((i, e) => {
            $(e).removeClass('d-none');
            $('#idTodasPermissoes').removeClass('d-none');
        })
    }
}