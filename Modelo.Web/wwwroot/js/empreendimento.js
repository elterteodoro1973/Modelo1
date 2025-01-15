
var tipoDocumento = $("#documento option:selected").val();
function cadastroPrimeiraParte() {
    let tipoUtilizacaoValido = $("#tipoEmpreendimento").valid();
    let documentoValido = $("#documento").valid();

    if (tipoUtilizacaoValido && documentoValido) {

        if ($("#documento option:selected").val() == "CPF" && $("#documento option:selected").val() != tipoDocumento) {
            tipoDocumento = $("#documento option:selected").val();
            $("#numeroDocumento").parent().find("label").html("CPF <span>*</span>");
            $("#numeroDocumento").val("");
            $("#numeroDocumento").rules('remove');
            $("#numeroDocumento").rules('add', {
                cpf: true,
                required: true,
                minlength: 14,
                maxlength: 14,
                messages: {
                    required: 'CPF é obrigatório !',
                    minlength: 'CPF inválido !',
                    maxlength: 'CPF inválido'
                }
            });
            setTimeout(() => {
                $("#numeroDocumento").mask("000.000.000-00");
            }, 1000);
        }
        else if ($("#documento option:selected").val() == "CNPJ" && $("#documento option:selected").val() != tipoDocumento) {
            tipoDocumento = $("#documento option:selected").val();
            $("#numeroDocumento").parent().find("label").html("CNPJ <span>*</span>");
            $("#numeroDocumento").val("");
            $("#numeroDocumento").rules('remove');
            $("#numeroDocumento").rules('add', {
                required: true,
                minlength: 18,
                maxlength: 19,
                messages: {
                    required: 'CNPJ é obrigatório !',
                    minlength: 'CNPJ inválido !',
                    maxlength: 'CNPJ inválido'
                }
            });
            setTimeout(() => {
                $("#numeroDocumento").mask("00.000.000/0000-00");
            }, 1000);
        }

        $("#parte1").addClass("d-none");
        $("#parte2").removeClass("d-none");
        $("[required]").each((i, e) => {
            $(this).parent().find("label").append("<span>*</span>");
        })
    }

}

function alterarTipoEmpreendimento() {
    $("#parte2").addClass("d-none");
    $("#parte1").removeClass("d-none");

}

function adicionarEmailResponsavelLegal(elemento) {
    let posicao = Number($(elemento).parent().find("input").attr("posicao"));
    let elementoPai = $(elemento).parent();
    let elementoPrinciPal = $(elemento).parent().parent();
    posicao++;
    let novoElemento = `
        <div class="row m-0 w-100 linhaEmailResponsavelLegal" style="gap:4rem; margin-top:24rem!important">
        <div class="d-flex justify-content-between">
                        <label for="ResponsavelLegal.Emails[${posicao}]" class="c-form__label form-label control-label">E-mail ${posicao + 1}</label>
                        <a href="#" class="c-form__action repeater justify-content-end" onclick="removerEmailResponsavelLegal(this)" style="color:red!important">
                            <i class="icon-delete"></i> Remover
                        </a>
                    </div>
                    
                    <input name="ResponsavelLegal.Emails[${posicao}]" id="ResponsavelLegal.Emails[${posicao}]" posicao="${posicao}"  type="email" data-msg-email="E-mail Inválido !" class="c-form__input form-control"  data-msg-required="E-mail ${posicao} obrigatório !" placeholder="Digite o e-mail !">
                    <a href="#" class="c-form__action repeater btnAdicionarEmailResponsavelLegal" onclick="adicionarEmailResponsavelLegal(this)">
                        <i class="icon-add"></i> Adicionar outro E-mail
                    </a>
                </div>
    `
    if ($(elemento).attr("id") == "addPrimeiroEmailSecundarioResponsavelLegal") {
        $(elemento).addClass("d-none");
    }
    else {
        $(elemento).remove();

    }
    $(elementoPrinciPal).append(novoElemento);
}

function removerEmailResponsavelLegal(elemento) {
    $(elemento).parent().parent().remove();
    $(".linhaEmailResponsavelLegal").each((i, e) => {
        let novaPosicao = i + 1;
        $(e).find("label").attr("for", `ResponsavelLegal.Emails[${novaPosicao}]`);
        $(e).find("label").html(`E-mail ${novaPosicao + 1}`);
        $(e).find("input").attr("name", `ResponsavelLegal.Emails[${novaPosicao}]`);
        $(e).find("input").attr("id", `ResponsavelLegal.Emails[${novaPosicao}]`);
        $(e).find("input").attr("posicao", `${novaPosicao}`);
        if (i == $(".linhaEmailResponsavelLegal").length - 1 && $(".btnAdicionarEmailResponsavelLegal").length == 0) {
            let btnAdd = `<a href="#" class="c-form__action repeater btnAdicionarEmailResponsavelLegal" onclick="adicionarEmailResponsavelLegal(this)">
                        <i class="icon-add"></i> Adicionar outro E-mail
                    </a>`

            $(e).append(btnAdd);
        }

    });

    if ($(".linhaEmailResponsavelLegal").length < 1) {
        $("#addPrimeiroEmailSecundarioResponsavelLegal").removeClass("d-none");
    }
    else {
        $("#addPrimeiroEmailSecundarioResponsavelLegal").addClass("d-none");
    }
}

function adicionarTelefoneResponsavelLegal(elemento) {
    let posicao = Number($(elemento).parent().find("input").attr("posicao"));
    let elementoPrincipal = $(elemento).parent().parent();
    posicao++;
    let novoElemento = `
        <div class="row m-0 w-100 linhaTelefonesResponsavelLegal" style="gap:4rem; margin-top:24rem!important">
        <div class="d-flex justify-content-between">
                        <label for="ResponsavelLegal.Telefones[${posicao}]" class="c-form__label form-label control-label">Telefone ${posicao + 1}</label>
                        <a href="#" class="c-form__action repeater justify-content-end" onclick="removerTelefoneResponsavelLegal(this)" style="color:red!important">
                            <i class="icon-delete"></i> Remover
                        </a>
                    </div>
                    
                    <input name="ResponsavelLegal.Telefones[${posicao}]" data-rule-telefone="true" data-mask="(00)00009-0000" id="ResponsavelLegal.Telefones[${posicao}]" posicao="${posicao}" class="c-form__input form-control maskTelefone">
                    <a href="#" class="c-form__action repeater btnAdicionarTelefoneResponsavelLegal" onclick="adicionarTelefoneResponsavelLegal(this)">
                        <i class="icon-add"></i> Adicionar outro telefone
                    </a>
                </div>
    `
    if ($(elemento).attr("id") == "addPrimeiroTelefoneSecundarioResposanvelLegal") {
        $(elemento).addClass("d-none");
    }
    else {
        $(elemento).remove();

    }
    $(elementoPrincipal).append(novoElemento);
    setTimeout(() => {
        $('.maskTelefone').each((i, e) => {
            $(e).mask("(00)00009-0000");
        })
    }, 1000)
}

function removerTelefoneResponsavelLegal(elemento) {
    $(elemento).parent().parent().remove();
    $(".linhaTelefonesResponsavelLegal").each((i, e) => {
        let novaPosicao = i + 1;
        $(e).find("label").attr("for", `ResponsavelLegal.Telefones[${novaPosicao}]`);
        $(e).find("label").html(`Telefone ${novaPosicao + 1}`);
        $(e).find("input").attr("name", `ResponsavelLegal.Telefones[${novaPosicao}]`);
        $(e).find("input").attr("id", `ResponsavelLegal.Telefones[${novaPosicao}]`);
        $(e).find("input").attr("posicao", `${novaPosicao}`);
        if (i == $(".linhaTelefonesResponsavelLegal").length - 1 && $(".btnAdicionarTelefoneResponsavelLegal").length == 0) {
            let btnAdd = `<a href="#" class="c-form__action repeater btnAdicionarTelefoneResponsavelLegal" onclick="adicionarEmailResponsavelLegal(this)">
                        <i class="icon-add"></i> Adicionar outro Telefone
                    </a>`

            $(e).append(btnAdd);
        }


    })

    if ($(".linhaTelefonesResponsavelLegal").length < 1) {
        $("#addPrimeiroTelefoneSecundarioResposanvelLegal").removeClass("d-none");
    }
    else {
        $("#addPrimeiroTelefoneSecundarioResposanvelLegal").addClass("d-none");
    }
}

function adicionarEmailRepresentanteLegal(elemento) {
    let posicao = Number($(elemento).parent().find("input").attr("posicao"));
    let elementoPai = $(elemento).parent();
    let elementoPrinciPal = $(elemento).parent().parent();
    posicao++;
    let novoElemento = `
        <div class="row m-0 w-100 linhaEmailRepresentanteLegal" style="gap:4rem; margin-top:24rem!important">
        <div class="d-flex justify-content-between">
                        <label for="RepresentanteLegal.Emails[${posicao}]" class="c-form__label form-label control-label">E-mail ${posicao + 1}</label>
                        <a href="#" class="c-form__action repeater justify-content-end" onclick="removerEmailRepresentanteLegal(this)" style="color:red!important">
                            <i class="icon-delete"></i> Remover
                        </a>
                    </div>
                    
                    <input name="RepresentanteLegal.Emails[${posicao}]" id="RepresentanteLegal.Emails[${posicao}]" posicao="${posicao}"  type="email" data-msg-email="E-mail Inválido !" class="c-form__input form-control"  data-msg-required="E-mail ${posicao} obrigatório !">
                    <a href="#" class="c-form__action repeater btnAdicionarEmailRepresentanteLegal" onclick="adicionarEmailRepresentanteLegal(this)">
                        <i class="icon-add"></i> Adicionar outro E-mail
                    </a>
                </div>
    `
    if ($(elemento).attr("id") == "addPrimeiroEmailSecundarioRepresentanteLegal") {
        $(elemento).addClass("d-none");
    }
    else {
        $(elemento).remove();

    }
    $(elementoPrinciPal).append(novoElemento);
}

function removerEmailRepresentanteLegal(elemento) {
    $(elemento).parent().parent().remove();
    $(".linhaEmailRepresentanteLegal").each((i, e) => {
        let novaPosicao = i + 1;
        $(e).find("label").attr("for", `RepresentanteLegal.Emails[${novaPosicao}]`);
        $(e).find("label").html(`E-mail ${novaPosicao + 1}`);
        $(e).find("input").attr("name", `RepresentanteLegal.Emails[${novaPosicao}]`);
        $(e).find("input").attr("id", `RepresentanteLegal.Emails[${novaPosicao}]`);
        $(e).find("input").attr("posicao", `${novaPosicao}`);
        if (i == $(".linhaEmailRepresentanteLegal").length - 1 && $(".btnAdicionarEmailRepresentanteLegal").length == 0) {
            let btnAdd = `<a href="#" class="c-form__action repeater btnAdicionarEmailRepresentanteLegal" onclick="adicionarEmailRepresentanteLegal(this)">
                        <i class="icon-add"></i> Adicionar outro E-mail
                    </a>`

            $(e).append(btnAdd);
        }

    });

    if ($(".linhaEmailRepresentanteLegal").length < 1) {
        $("#addPrimeiroEmailSecundarioRepresentanteLegal").removeClass("d-none");
    }
    else {
        $("#addPrimeiroEmailSecundarioRepresentanteLegal").addClass("d-none");
    }
}

function adicionarTelefoneRepresentanteLegal(elemento) {
    let posicao = Number($(elemento).parent().find("input").attr("posicao"));
    let elementoPrincipal = $(elemento).parent().parent();
    posicao++;
    let novoElemento = `
        <div class="row m-0 w-100 linhaTelefonesRepresentanteLegal" style="gap:4rem; margin-top:24rem!important">
        <div class="d-flex justify-content-between">
                        <label for="RepresentanteLegal.Telefones[${posicao}]" class="c-form__label form-label control-label">Telefone ${posicao + 1}</label>
                        <a href="#" class="c-form__action repeater justify-content-end" onclick="removerTelefoneRepresentanteLegal(this)" style="color:red!important">
                            <i class="icon-delete"></i> Remover
                        </a>
                    </div>
                    
                    <input name="RepresentanteLegal.Telefones[${posicao}]" data-mask="(00)00009-0000" data-rule-telefone="true" id="RepresentanteLegal.Telefones[${posicao}]" posicao="${posicao}" class="c-form__input form-control maskTelefone">
                    <a href="#" class="c-form__action repeater btnAdicionarTelefoneRepresentanteLegal" onclick="adicionarTelefoneRepresentanteLegal(this)">
                        <i class="icon-add"></i> Adicionar outro telefone
                    </a>
                </div>
    `
    if ($(elemento).attr("id") == "addPrimeiroTelefoneSecundarioRepresentanteLegal") {
        $(elemento).addClass("d-none");
    }
    else {
        $(elemento).remove();

    }
    $(elementoPrincipal).append(novoElemento);
    setTimeout(() => {
        $('.maskTelefone').each((i, e) => {
            $(e).mask("(00)00009-0000");
        })
    }, 1000)
}

function removerTelefoneRepresentanteLegal(elemento) {
    $(elemento).parent().parent().remove();
    $(".linhaTelefonesRepresentanteLegal").each((i, e) => {
        let novaPosicao = i + 1;
        $(e).find("label").attr("for", `RepresentanteLegal.Telefones[${novaPosicao}]`);
        $(e).find("label").html(`Telefone ${novaPosicao + 1}`);
        $(e).find("input").attr("name", `RepresentanteLegal.Telefones[${novaPosicao}]`);
        $(e).find("input").attr("id", `RepresentanteLegal.Telefones[${novaPosicao}]`);
        $(e).find("input").attr("posicao", `${novaPosicao}`);
        if (i == $(".linhaTelefonesRepresentanteLegal").length - 1 && $(".btnAdicionarTelefoneRepresentanteLegal").length == 0) {
            let btnAdd = `<a href="#" class="c-form__action repeater btnAdicionarTelefoneRepresentanteLegal" onclick="adicionarEmailRepresentanteLegal(this)">
                        <i class="icon-add"></i> Adicionar outro Telefone
                    </a>`

            $(e).append(btnAdd);
        }


    })

    if ($(".linhaTelefonesRepresentanteLegal").length < 1) {
        $("#addPrimeiroTelefoneSecundarioRepresentanteLegal").removeClass("d-none");
    }
    else {
        $("#addPrimeiroTelefoneSecundarioRepresentanteLegal").addClass("d-none");
    }
}

function buscarCepEmpreendimento(elemento, tipoEndereco) {
    let cep = $(elemento).parent().find("input").val();
    let posicao = $(elemento).parent().find("input").attr("posicao");
    if (cep.length != 9) {
        $(elemento).parent().find("input").addClass("is-invalid");
        $(elemento).parent().find("input").focus();
        return;
    }


    let cepFormatado = cep.replace("-", "");
    let url = `https://viacep.com.br/ws/${cepFormatado}/json/`;

    $.get(url)
        .done((data) => {
            console.log(data);
            $(`#logradouro${tipoEndereco}`).val(data.logradouro);
            $(`#bairro${tipoEndereco}`).val(data.bairro);
            $(`#estado${tipoEndereco} option:selected`).removeAttr("selected");
            $(`#estado${tipoEndereco} option`).each((i, e) => {
                if ($(e).val() == data.uf.toUpperCase()) {
                    $(e).attr("selected", "selected");
                }
            });

            let urlMunicipios = `https://servicodados.ibge.gov.br/api/v1/localidades/estados/${data.uf}/municipios`;

            $.get(urlMunicipios)
                .done((dataMunicipios) => {
                    let opcaoSelecione = '<option value="" selected disabled>Selecione ...</option>';
                    $(`.selectMunicipio${tipoEndereco}`).append(opcaoSelecione);
                    $(dataMunicipios).each((i, e) => {
                        let opcao = `<option value="${e.nome.toUpperCase()}">${e.nome}</option>`;
                        $(`.selectMunicipio${tipoEndereco}`).append(opcao);
                    })
                    $(`.selectMunicipio${tipoEndereco}`).removeAttr("disabled");
                    $(`.selectMunicipio${tipoEndereco} option`).each((i, e) => {
                        if ($(e).val().toUpperCase() == data.localidade.toUpperCase()) {
                            $(e).attr("selected", "selected");
                        }
                    });
                    $(`.numero${tipoEndereco}`).focus();
                })
                .fail((jqXHR, textStatus) => {

                })

        })
        .fail((jqXHR, textStatus) => {

        })
}

function buscarMunicipiosEmpreendimento(elemento, tipoEndereco) {

    let posicao = $(elemento).attr("posicao");
    let uf = $(elemento).find(":selected").val();
    $(`.selectMunicipios${tipoEndereco}`).html("");
    $(`.selectMunicipios${tipoEndereco}`).attr("disabled", "disabled");
    let url = `https://servicodados.ibge.gov.br/api/v1/localidades/estados/${uf}/municipios`;

    $.get(url)
        .done((data) => {
            let opcaoSelecione = '<option value="" selected disabled>Selecione ...</option>';
            $(`.selectMunicipios${tipoEndereco}`).append(opcaoSelecione);
            $(data).each((i, e) => {
                let opcao = `<option value="${e.nome.toUpperCase()}">${e.nome}</option>`;
                $(`.selectMunicipios${tipoEndereco}`).append(opcao);
            })
            $(`.selectMunicipios${tipoEndereco}`).removeAttr("disabled");
        })
        .fail((jqXHR, textStatus) => {

        })


}

$("#enderecoEmpreendimentoIgualEnderecoCobranca").on("change", () => {
    let enderecoCobrancaDiferenteEnderecoEmpreendimento = $("#enderecoEmpreendimentoIgualEnderecoCobranca").is(":checked");

    if (enderecoCobrancaDiferenteEnderecoEmpreendimento) {
        ativarEnderecoCobranca();
        $("#enderecoCobranca").show();
    }
    else {
        $("#enderecoCobranca").hide();
        desativarEnderecoCobranca();
    }
})

function ativarEnderecoCobranca() {
    $("#cepCobrancaEmpreendimento").removeAttr("disabled");
    $("#cepCobrancaEmpreendimento").removeAttr("readonly");
    $("#cepCobrancaEmpreendimento").removeClass("valid");
    $("#cepCobrancaEmpreendimento").attr("required", "required");
    $("#cepCobrancaEmpreendimento").attr("data-msg-required", "CEP é obrigatório !");
    $("#cepCobrancaEmpreendimento").attr("data-rule-minlength", "9");
    $("#cepCobrancaEmpreendimento").attr("data-msg-minlength", "CEP inválido !");
    $("#cepCobrancaEmpreendimento").attr("data-rule-maxlength", "9");
    $("#cepCobrancaEmpreendimento").attr("data-msg-maxlength", "CEP inválido !");
    $("#logradouroEnderecoCobranca").removeAttr("disabled");
    $("#logradouroEnderecoCobranca").removeAttr("readonly");
    $("#logradouroEnderecoCobranca").removeClass("valid");
    $("#logradouroEnderecoCobranca").attr("required", "required");
    $("#logradouroEnderecoCobranca").attr("data-msg-required", "Logradouro é obrigatório !");
    $("#logradouroEnderecoCobranca").attr("data-rule-maxlength", "256");
    $("#logradouroEnderecoCobranca").attr("data-msg-maxlength", "Logradouro possui tamanho máximo de 256 caracteres !");
    $("#numeroEnderecoEnderecoCobranca").removeAttr("disabled");
    $("#numeroEnderecoEnderecoCobranca").removeAttr("readonly");
    $("#numeroEnderecoEnderecoCobranca").removeClass("valid");
    $("#complementoEnderecoCobranca").removeAttr("disabled");
    $("#complementoEnderecoCobranca").removeAttr("readonly");
    $("#complementoEnderecoCobranca").removeClass("valid");
    $("#complementoEnderecoCobranca").attr("data-rule-maxlength", "256");
    $("#complementoEnderecoCobranca").attr("data-msg-maxlength", "Logradouro possui tamanho máximo de 256 caracteres !");
    $("#estadoEnderecoCobranca").removeAttr("disabled");
    $("#estadoEnderecoCobranca").removeClass("valid");
    $("#estadoEnderecoCobranca").attr("required", "required");
    $("#estadoEnderecoCobranca").attr("data-msg-required", "Estado é obrigatório !");
    $("#municipioEnderecoCobranca").removeAttr("disabled");
    $("#municipioEnderecoCobranca").removeClass("valid");
    $("#municipioEnderecoCobranca").attr("required", "required");
    $("#municipioEnderecoCobranca").attr("data-msg-required", "Municipio é obrigatório !");
}

function desativarEnderecoCobranca() {
    $("#cepCobrancaEmpreendimento").attr("disabled", "disabled");
    $("#cepCobrancaEmpreendimento").attr("readonly", "readonly")
    $("#cepCobrancaEmpreendimento").val("");
    $("#cepCobrancaEmpreendimento").addClass("valid");
    $("#cepCobrancaEmpreendimento").removeAttr("required");
    $("#cepCobrancaEmpreendimento").removeAttr("data-msg-required");
    $("#cepCobrancaEmpreendimento").removeAttr("data-rule-minlength");
    $("#cepCobrancaEmpreendimento").removeAttr("data-msg-minlength");
    $("#cepCobrancaEmpreendimento").removeAttr("data-rule-maxlength");
    $("#cepCobrancaEmpreendimento").removeAttr("data-msg-maxlength");
    $("#logradouroEnderecoCobranca").attr("disabled", "disabled");
    $("#logradouroEnderecoCobranca").attr("readonly", "readonly")
    $("#logradouroEnderecoCobranca").val("");
    $("#logradouroEnderecoCobranca").addClass("valid");
    $("#logradouroEnderecoCobranca").removeAttr("required");
    $("#logradouroEnderecoCobranca").removeAttr("data-msg-required");
    $("#logradouroEnderecoCobranca").removeAttr("data-rule-maxlength");
    $("#logradouroEnderecoCobranca").removeAttr("data-msg-maxlength");
    $("#numeroEnderecoEnderecoCobranca").attr("disabled", "disabled");
    $("#numeroEnderecoEnderecoCobranca").attr("readonly", "readonly")
    $("#numeroEnderecoEnderecoCobranca").val("");
    $("#numeroEnderecoEnderecoCobranca").addClass("valid");
    $("#complementoEnderecoCobranca").attr("disabled", "disabled");
    $("#complementoEnderecoCobranca").attr("readonly", "readonly")
    $("#complementoEnderecoCobranca").val("");
    $("#complementoEnderecoCobranca").addClass("valid");
    $("#complementoEnderecoCobranca").removeAttr("data-rule-maxlength");
    $("#complementoEnderecoCobranca").removeAttr("data-msg-maxlength");
    $("#estadoEnderecoCobranca").attr("disabled", "disabled");
    $("#estadoEnderecoCobranca").val("");
    $("#estadoEnderecoCobranca").addClass("valid");
    $("#estadoEnderecoCobranca").removeAttr("required");
    $("#estadoEnderecoCobranca").removeAttr("data-msg-required");
    $("#municipioEnderecoCobranca").attr("disabled", "disabled");
    $("#municipioEnderecoCobranca").val("");
    $("#municipioEnderecoCobranca").removeAttr("required");
    $("#municipioEnderecoCobranca").removeAttr("data-msg-required");
    $("#municipioEnderecoCobranca").addClass("valid");
}