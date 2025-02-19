/*const { left } = require("@popperjs/core");*/

function buscarCep(elemento) {
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
            $(`.logradouro[posicao=${posicao}]`).val(data.logradouro);
            $(`.estado[posicao=${posicao}]>option:selected`).removeAttr("selected");
            $(`.estado[posicao=${posicao}]>option`).each((i, e) => {
                if ($(e).val() == data.uf.toUpperCase()) {
                    $(e).attr("selected", "selected");
                }
            });

            let urlMunicipios = `https://servicodados.ibge.gov.br/api/v1/localidades/estados/${data.uf}/municipios`;

            $.get(urlMunicipios)
                .done((dataMunicipios) => {
                    let opcaoSelecione = '<option value="" selected disabled>Selecione ...</option>';
                    $(`.selectMunicipio[posicao=${posicao}]`).append(opcaoSelecione);
                    $(dataMunicipios).each((i, e) => {
                        let opcao = `<option value="${e.nome.toUpperCase()}">${e.nome}</option>`;
                        $(`.selectMunicipio[posicao=${posicao}]`).append(opcao);
                    })
                    $(`.selectMunicipio[posicao=${posicao}]`).removeAttr("disabled");
                    $(`.selectMunicipio[posicao=${posicao}]>option`).each((i, e) => {
                        if ($(e).val().toUpperCase() == data.localidade.toUpperCase()) {
                            $(e).attr("selected", "selected");
                        }
                    });
                    $(`.numero[posicao=${posicao}]`).focus();
                })
                .fail((jqXHR, textStatus) => {

                })

        })
        .fail((jqXHR, textStatus) => {

        })
}

function buscarMunicipios(elemento) {

    let posicao = $(elemento).attr("posicao");
    let uf = $(elemento).find(":selected").val();
    $(`.selectMunicipios[posicao=${posicao}]`).html("");
    $(`.selectMunicipios[posicao=${posicao}]`).attr("disabled", "disabled");
    let url = `https://servicodados.ibge.gov.br/api/v1/localidades/estados/${uf}/municipios`;

    $.get(url)
        .done((data) => {
            let opcaoSelecione = '<option value="" selected disabled>Selecione ...</option>';
            $(`.selectMunicipios[posicao=${posicao}]`).append(opcaoSelecione);
            $(data).each((i, e) => {
                let opcao = `<option value="${e.nome.toUpperCase()}">${e.nome}</option>`;
                $(`.selectMunicipios[posicao=${posicao}]`).append(opcao);
            })
            $(`.selectMunicipios[posicao=${posicao}]`).removeAttr("disabled");
        })
        .fail((jqXHR, textStatus) => {

        })
}

function senhaForte(p) {
    if (p.length < 6) {
        return retorno;
    }

    if (p.length > 30) {
        return retorno;
    }

    let retorno = false;
    let auxMaiuscula = 0;
    let auxMinuscula = 0;
    let auxNumero = 0;
    let auxEspecial = 0;

    let letrasMaiusculas = /[A-Z]/;
    let letrasMinusculas = /[a-z]/;
    let numeros = /[0-9]/;
    let caracteresEspeciais = /[!|@|#|$|%|^|&|*|(|)|-|_]/;
    
    for (var i = 0; i < p.length; i++) {
        if (letrasMaiusculas.test(p[i]))
            auxMaiuscula++;
        else if (letrasMinusculas.test(p[i]))
            auxMinuscula++;
        else if (numeros.test(p[i]))
            auxNumero++;
        else if (caracteresEspeciais.test(p[i]))
            auxEspecial++;
    }

    if (auxMaiuscula > 0) {
        if (auxMinuscula > 0) {
            if (auxNumero > 0) {
                if (auxEspecial) {
                    retorno = true;
                }
            }
        }
    }

    return retorno;
}	

function removerCaracteresEspeciais(texto) {
    return texto.replaceAll(/[^a-zA-Z0-9]/g, "");
}

function removerCaracteresEspeciaisSoNumeros(texto) {
    return texto.replaceAll(/[^Z0-9]/g, "");
}

//function validaEmail(emailTexto) {
//    usuario = emailTexto.substring(0, emailTexto.indexOf("@"));
//    dominio = emailTexto.substring(emailTexto.indexOf("@") + 1, emailTexto.length);

//    if ((usuario.length >= 1) && (usuario.search(" ") == -1) && (usuario.search("@") == -1) &&
//        (dominio.length >= 3) && (dominio.search("@") == -1) && (dominio.search(" ") == -1) &&
//        (dominio.search(".") != -1) && (dominio.indexOf(".") >= 1) && (dominio.lastIndexOf(".") < dominio.length - 1))    {
//        var validadorEmail = /^\w+([.-_+]?\w+)*@\w+([.-]?\w+)*(\.\w{2,10})+$/;

//        if (validadorEmail.test(emailTexto)) {
//            return true;
//        } else {
//            return false;
//        }
//    }
//    else {
//        return false;
//    }
//}


//function validaCPF(cpf) {
//    var resto = 0;
//    var soma = 0;

//    cpf = removerCaracteresEspeciaisSoNumeros(cpf);

//    if (cpf.length != 11 ||
//        cpf == "00000000000" ||
//        cpf == "11111111111" ||
//        cpf == "22222222222" ||
//        cpf == "33333333333" ||
//        cpf == "44444444444" ||
//        cpf == "55555555555" ||
//        cpf == "66666666666" ||
//        cpf == "77777777777" ||
//        cpf == "88888888888" ||
//        cpf == "99999999999")
//        return false;

//    for (i = 1; i <= 9; i++)
//        soma = soma + parseInt(cpf.substring(i - 1, i)) * (11 - i);
//    resto = (soma * 10) % 11;

//    if ((resto == 10) || (resto == 11))
//        resto = 0;

//    if (resto != parseInt(cpf.substring(9, 10)))
//        return false;

//    soma = 0;
//    for (i = 1; i <= 10; i++)
//        soma = soma + parseInt(cpf.substring(i - 1, i)) * (12 - i);
//    resto = (soma * 10) % 11;

//    if ((resto == 10) || (resto == 11))
//        resto = 0;

//    if (resto != parseInt(cpf.substring(10, 11)))
//        return false;

//    return true;
//}
