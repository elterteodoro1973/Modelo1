
//Inicio  calculo do volumes

$('#horasMedia, .horas').mask('99,00');

$('#diasMedia, .dias').mask('99,00');



$('.horas').on('blur', function ()
{
    SetValores();

    var hora = parseFloat($(this).val().replace(',', '.')).toFixed(2);
    //var hora = parseFloat($(this).val()).toFixed(2);

    if (hora > 24)
    {
        $(this).val('24,00');
    }

    var horasMediaFormatada = $(this).val().replace('.', ',');
    if (horasMediaFormatada.indexOf(",") == -1) {
        horasMediaFormatada = horasMediaFormatada + ",00";
    }

    $(this).val(horasMediaFormatada);

});

$('#horasMedia').on('blur', function ()
{
    SetValores();

    var hora = parseFloat($(this).val().replace(',', '.')).toFixed(2);
    //var hora = parseFloat($(this).val()).toFixed(2);

    if (hora > 24) {
        $(this).val('24,00');
    }

    var horasMediaFormatada = $(this).val().replace('.', ',');;

    if (horasMediaFormatada.indexOf(",") == -1) {
        horasMediaFormatada = horasMediaFormatada + ",00";
    }

    $(this).val(horasMediaFormatada);
    $('.horas').val(horasMediaFormatada);

});

$('#diasMedia').on('keyup', function ()
{
    SetValores();
    var dia = parseInt($(this).val().replace(',', '').replace('.', ''));

    if (dia < 0 || dia > 31) {
        $(this).val('31');
    }
});

$('#diasMedia').on('blur', function () {
    $('.dias').val($(this).val().replace(',', '').replace('.,', ''));
    AjustarDiasMes();

    $('#vazao-mes-1').focus();
    $('#horas-mes-1').focus();
    $('#dias-mes-1').focus();
    $('#vazaoMedia').focus();

});

$('.dias').on('keyup', function ()
{    
    SetValores();
    AjustarDiasMes();
    AtualizarVolumes();
});

$('.vazao, .horas').on('input', function ()
{
    SetValores();
   
});
function SetValores()
{
    if ($('#horasMedia').val() == null || $('#horasMedia').val() == "")
    {
        $('#horasMedia').val("0,00");
    }

   

    if ($('#diasMedia').val() == null || $('#diasMedia').val() == "")
    {
        $('#diasMedia').val("30");
    }
    /
}

function AjustarDiasMes()
{
    $('.dias').each(function () {
        var id = $(this).prop('id');
        var partes = id.split('-');
        var mes = parseInt(partes[2]);
        var dias = $(this).val();

        switch (mes) {
            // Meses com 31 dias
            case 1:  // Janeiro
            case 3:  // Março
            case 5:  // Maio
            case 7:  // Julho
            case 8:  // Agosto
            case 10: // Outubro
            case 12: // Dezembro
                if (dias > 31) {
                    $(this).val('31');
                }
                break;

            // Fevereiro (28 ou 29 dias em anos bissextos)
            case 2:
                if (dias > 29) {
                    $(this).val('29');
                }
                break;

            // Meses com 30 dias
            default:
                if (dias > 30) {
                    $(this).val('30');
                }
        }
    });
}

//fim calculo do volumes

function scrollToBottom()
{
    var pageHeight = $(document).height();
    $("html, body").animate({ scrollTop: pageHeight }, "slow");
}
function alterarProgressBar(elementId, novoValor)
{
    var progressBar = document.getElementById(elementId);

    progressBar.style.width = novoValor + '%';
    progressBar.setAttribute('aria-valuenow', novoValor);
    progressBar.innerHTML = novoValor + '%';
}




$('.porcentagem').mask('000,00', {
    reverse: true,
    translation: {
        '0': { pattern: /[0-9]/ },
    },
});
$('.porcentagem').on('blur', function ()
{
    var valor = $(this).val();

    if (valor === '')
    {
        
        return;
    }

    var floatValue = parseFloat(valor.replace(',', '.'));

    if (isNaN(floatValue) || floatValue <= 0)
    {
        $(this).val('0,01');
    }
    else if (floatValue > 100.00)
    {
        $(this).val('100,00');
    }

    var totalPorcentagem = (100 - TotalPorcentagem()).toFixed(2);

    if (totalPorcentagem < 0)
    {
        var porcentagemValida = (TotalPorcentagem() - floatValue).toFixed(2);
        var restanteValido = (100 - porcentagemValida).toFixed(2);
        $(this).val(restanteValido.replace('.', ','));
    }

    valor = $(this).val();

    if (!valor.includes(',')) {
        $(this).val(valor + ',00');
    }

    
});
function TotalPorcentagem()
{
    total = 0.00;
    $('.item').each(function ()
    {
        var valor = $(this).find(".porcentagem").val();
        if (valor === '')
        {
            valor = '0,00';
        }
        var porcentagem = parseFloat(valor.replace(',', '.'));

        total = total + porcentagem;
    });

    return total.toFixed(2);
}


$(document).ready(() =>
{
    SetValores();
});