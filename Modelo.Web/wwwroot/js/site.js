// Please see documentation at https://docs.microsoft.com/aspnet/core/client-side/bundling-and-minification
// for details on configuring this project to bundle and minify static web assets.

// Write your JavaScript code.
function ativarbtn(e) {
    if ($(e).hasClass('active')) {
        $(".btn-action.active").removeClass("active");
    }
    else {
        $(".btn-action.active").removeClass("active");
        $(e).addClass("active");
    }
}

function obterExclusaoId(elemento) {
    var id = $(elemento).attr('data-id');
    $('.btn-excluir').attr('data-id', id);
}

const tooltipTriggerList = document.querySelectorAll('[data-bs-toggle="tooltip"]')
const tooltipList = [...tooltipTriggerList].map(tooltipTriggerEl => new bootstrap.Tooltip(tooltipTriggerEl))


$(function () {
    mascaras();

    // Oculta os alertas depois de 10 segundos
    $("#avisos>.alert").delay(10000).fadeTo("slow", 500).slideUp(500, function () {
        $("#avisos>.alert").slideUp(500);
    });

    //mensagensValidatorPtBr();
});

function mensagensValidatorPtBr() {
    jQuery.extend(jQuery.validator.messages, {
        required: "Campo obrigatório.",
        remote: "Por favor, corrija este campo.",
        email: "Por favor, forneça um e-mail válido.",
        url: "Por favor, forneça uma URL válida.",
        date: "Por favor, forneça uma data válida.",
        dateISO: "Por favor, forneça uma data válida (ISO).",
        number: "Por favor, forneça um número válido.",
        digits: "Por favor, forneça somente dígitos.",
        creditcard: "Por favor, forneça um cartão de crédito válido.",
        equalTo: "Por favor, forneça o mesmo valor novamente.",
        accept: "Por favor, forneça um valor com uma extensão válida.",
        maxlength: jQuery.validator.format("Por favor, forneça não mais que {0} caracteres."),
        minlength: jQuery.validator.format("Por favor, forneça ao menos {0} caracteres."),
        rangelength: jQuery.validator.format("Por favor, forneça um valor entre {0} e {1} caracteres de comprimento."),
        range: jQuery.validator.format("Por favor, forneça um valor entre {0} e {1}."),
        max: jQuery.validator.format("Por favor, forneça um valor menor ou igual a {0}."),
        min: jQuery.validator.format("Por favor, forneça um valor maior ou igual a {0}.")
    });
}

function mascaras() {

    // Inserir no input data-tipo=" tipo de mascara "

    $("input[data-tipo='cnpj']").mask("00.000.000/0000-00");
    $("input[data-tipo='cpf']").mask("000.000.000-00");
    $("input[data-tipo='cep']").mask("00000-000");
    $("input[data-tipo='cnae']").mask("0000000");

    $("input[data-tipo='rg']").mask("00.000.000-A");
    //$("input[data-tipo='nis']").mask("000.00000.00-0"); // Esperando para adicionar os 11 caracteres no banco
    $("input[data-tipo='nis']").mask("000.00000.00");
    $("input[data-tipo='inss']").mask("00.000.00000/00");
    $("input[data-tipo='cei']").mask("00.000.00000/00");
    $("input[data-tipo='caepf']").mask("000.000.000/000-00");
    $("input[data-tipo='cno']").mask("00.000.00000/00");
    $("input[data-tipo='telefone']").mask("(00) 0000-0000");
    $("input[data-tipo='decimal-percent']").mask("000.00");

    // máscara para celular fixo e celular
    var comportamentoTelefone = function (val) {
        return val.replace(/\D/g, '').length === 11 ? '(00) 00000-0000' : '(00) 0000-00009';
    },
        spOptions = {
            onKeyPress: function (val, e, field, options) {
                field.mask(comportamentoTelefone.apply({}, arguments), options);
            }
        };

    $("input[data-tipo='telefone-celular']").mask(comportamentoTelefone, spOptions);



    var optionsCpfCnpj = {
        onKeyPress: function (cpf, ev, el, op) {
            var masks = ['000.000.000-000', '00.000.000/0000-00'];
            $("input[data-tipo='cpfcnpj']").mask((cpf.length > 14) ? masks[1] : masks[0], op);
        }
    }

    $("input[data-tipo='cpfcnpj']").length > 11 ? $("input[data-tipo='cpfcnpj']").mask('00.000.000/0000-00', optionsCpfCnpj) : $("input[data-tipo='cpfcnpj']").mask('000.000.000-00#', optionsCpfCnpj);

}

$('.mvc-grid-pager button:contains("«")').remove();
$('.mvc-grid-pager button:contains("‹")').html('<i class="icon-keyboard-arrow-left"></i>');
$('.mvc-grid-pager button:contains("›")').html('<i class="icon-keyboard-arrow-right"></i>');
$('.mvc-grid-pager button:contains("»")').remove();
$('.mvc-grid-page-sizes').prepend('<span class="c-tabledata__footer--label">Itens por página: </span>')
$('.mvc-grid-pager-rows').addClass('o-form-input o-form-input-select')


$(function () {

    $('.o-dropdown').on('click', function (e) {

        if ($(this).children('.o-btn-alt') && !$(this).children('.o-btn-alt').hasClass('active')) {
            $(this).children('.o-btn-alt').addClass('active');
        } else {
            $(this).children('.o-btn-alt').removeClass('active');
        }
        $(this).children('.o-dropdown-list').toggle();

        var element = $(this).children('.o-dropdown-list')[0];

        var space = $(window).height() - element.getBoundingClientRect().bottom;

        if (space < 10) {
            $(this).children('.o-dropdown-list').css('bottom', '30px');
        }

    });
    $(document).on('click', function (e) {
        var target = e.target;
        if (!$(target).is('.o-dropdown') && !$(target).parents().is('.o-dropdown')) {
            $('.o-dropdown-list').hide();

            if ($('.o-btn-alt.active')) {
                $('.o-btn-alt.active').removeClass('active');
            }

        }
    });

    //const dropdownElementList = document.querySelectorAll('.dropdown-toggle')
    //const dropdownList = [...dropdownElementList].map(dropdownToggleEl => {
    //    let drop = new bootstrap.Dropdown(dropdownToggleEl)
    //    console.log(drop);
    //})



    var formRequired = $('.c-form__input[required]');
    formRequired.each(function () {
        $(this).prev('.c-form__label').append(' <span>*</span>')
    });


    //var reg = /\/(.*?)\//;
    //var pageDir = reg.exec(window.location.pathname)[1];


    //$('.c-nav--list__item-link').each(function () {
    //    var sidebarURL = reg.exec($(this).attr('href'))[1];

    //    if (sidebarURL == pageDir) {
    //        $(this).addClass('active');
    //    }
    //});

    jQuery.validator.methods.range = function (value, element, param) {
        var globalizedValue = value.replace(",", ".");
        return this.optional(element) || (globalizedValue >= param[0] && globalizedValue <= param[1]);
    }

    jQuery.validator.methods.number = function (value, element) {
        return this.optional(element) || /-?(?:\d+|\d{1,3}(?:[\s\.,]\d{3})+)(?:[\.,]\d+)?$/.test(value);
    }


});

const pathUrl = window.location.pathname;

$(document).ready(() => {
    let arrayPathUrl = pathUrl.split('/')[1];
    if (arrayPathUrl === "Administracao" || arrayPathUrl == "Finalidades") {
        $('#admMenu').find('a').addClass('active');
    }
    else if (arrayPathUrl === "Empreendimentos") {
        $('#empreendimentoMenu').find('a').addClass('active');
    }
    else if (arrayPathUrl === "Perfis") {
        $('#perfilMenu').find('a').addClass('active');
    }

    if (jQuery("#pesquisaGrid").length > 0) {
        document.getElementById("pesquisaGrid").addEventListener("input", function () {
            const grid = new MvcGrid(document.querySelector(".mvc-grid"));

            grid.url.searchParams.set("filtro", this.value);

            grid.reload();
        })
    }

})

if (jQuery("#pesquisaGrid").length > 0) {
    document.getElementById("pesquisaGrid").addEventListener("input", function () {
        const grid = new MvcGrid(document.querySelector(".mvc-grid"));

        grid.url.searchParams.set("filtro", this.value);

        grid.reload();
    })
}


function buscarNomeMes(numeroMes) {
    const date = new Date();
    date.setMonth(numeroMes - 1);

    let nome = date.toLocaleString('pt-BR', {
        month: 'long',
    });
    return nome.charAt(0).toUpperCase() + nome.slice(1);
}