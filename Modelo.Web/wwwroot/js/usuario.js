function adicionarTelefone(elemento) {
    let posicao = Number($(elemento).parent().find("input").attr("posicao"));
    let elementoPrincipal = $(elemento).parent().parent();
    posicao++;
    let novoElemento = `
        <div class="row m-0 linhaTelefones" style="gap:4rem; margin-top:24rem!important">
        <div class="d-flex justify-content-between">
                        <label for="Telefone[${posicao}]" class="c-form__label form-label control-label">Telefone ${posicao + 1}</label>
                        <a href="#" class="c-form__action repeater justify-content-end" onclick="removerTelefone(this)" style="color:red!important">
                            <i class="icon-delete"></i> Remover
                        </a>
                    </div>
                    
                    <input name="Telefone[${posicao}]" data-mask="(00)00009-0000" data-rule-telefone="true" id="Telefone[${posicao}]" posicao="${posicao}" class="c-form__input form-control maskTelefone">
                    <a href="#" class="c-form__action repeater" onclick="adicionarTelefone(this)">
                        <i class="icon-add"></i> Adicionar outro telefone
                    </a>
                </div>
    `
    $(elemento).remove();
    $(elementoPrincipal).append(novoElemento);
    setTimeout(() => {
        $('.maskTelefone').each((i, e) => {
            $(e).mask("(00)00009-0000");
        })
    }, 1000)

}

function removerTelefone(elemento) {
    $(elemento).parent().parent().remove();
    $(".linhaTelefones").each((i, e) => {
        let novaPosicao = i + 1;
        $(e).find("label").attr("for", `Telefone[${novaPosicao}]`);
        $(e).find("label").html(`Telefone ${novaPosicao + 1}`);
        $(e).find("input").attr("name", `Telefone[${novaPosicao}]`);
        $(e).find("input").attr("id", `Telefone[${novaPosicao}]`);
        $(e).find("input").attr("posicao", `${novaPosicao}`);
    })

    if ($(".linhaTelefones").length < 1) {
        $("#addPrimeiroTelefoneSecundario").removeClass("d-none");
    }
    else {
        $("#addPrimeiroTelefoneSecundario").addClass("d-none");
    }
}

function adicionarEmail(elemento) {
    let posicao = Number($(elemento).parent().find("input").attr("posicao"));
    let elementoPai = $(elemento).parent();
    let elementoPrinciPal = $(elemento).parent().parent();
    posicao++;
    let novoElemento = `
        <div class="row m-0 w-100 linhaEmail" style="gap:4rem; margin-top:24rem!important">
        <div class="d-flex justify-content-between">
                        <label for="Emails[${posicao}]" class="c-form__label form-label control-label">E-mail ${posicao + 1}</label>
                        <a href="#" class="c-form__action repeater justify-content-end" onclick="removerEmail(this)" style="color:red!important">
                            <i class="icon-delete"></i> Remover
                        </a>
                    </div>
                    
                    <input name="Emails[${posicao}]" id="Emails[${posicao}]" posicao="${posicao}"  type="email" data-msg-email="E-mail Inválido !" class="c-form__input form-control"  data-msg-required="E-mail ${posicao} obrigatório !">
                    <a href="#" class="c-form__action repeater" onclick="adicionarEmail(this)">
                        <i class="icon-add"></i> Adicionar outro E-mail
                    </a>
                </div>
    `
    $(elemento).remove();
    $(elementoPrinciPal).append(novoElemento);
}

function removerEmail(elemento) {
    $(elemento).parent().parent().remove();
    $(".linhaEmail").each((i, e) => {
        let novaPosicao = i + 1;
        $(e).find("label").attr("for", `Emails[${novaPosicao}]`);
        $(e).find("label").html(`E-mail ${novaPosicao + 1}`);
        $(e).find("input").attr("name", `Emails[${novaPosicao}]`);
        $(e).find("input").attr("id", `Emails[${novaPosicao}]`);
        $(e).find("input").attr("posicao", `${novaPosicao}`);
    })

    if ($(".linhaEmail").length < 1) {
        $("#addPrimeiroEmailSecundario").removeClass("d-none");
    }
    else {
        $("#addPrimeiroTelefoneSecundario").addClass("d-none");
    }
}


function adicionarEndereco(elemento) {
    let posicao = Number($(elemento).closest(".c-form__group").find("select").attr("posicao"));
    let elementoPai = $(elemento).parents().find(".linhaEndereco");
    posicao++;
    let novoElemento = `
    <div class="linhaRemoverEndereco">
        <div class="row">
                <div class="c-form__group col-4">
                        <label for="Enderecos[${posicao}].CEP" class="c-form__label form-label control-label labelCEP">CEP</label>
                    <div class="row m-0">
                        <div class="input-group c-form__input--attach">
                            <input type="text" class="c-form__input form-control input-group-border-radius maskCEP inputCEP" posicao="${posicao}" aria-describedby="button-addon2"  data-msg-maxlength="CEP inválido !"  data-msg-minlength="CEP inválido !" data-rule-maxlength="9" data-rule-minlength="9" 
                            data-rule-required="true" data-val="true" data-msg-required="CEP é obrigatório !" id="Enderecos_${posicao}__CEP" name="Enderecos[${posicao}].CEP" value="" autocomplete="off" required>
                            <button class=" o-btn o-btn-default btn input-group-border-radius" type="button" onclick="buscarCep(this)" id="button-addon2">Buscar CEP</button>
                        </div>
                    </div>
                    <div class="row m-0">
                        <span class="form-text text-danger field-validation-valid spanCEP" data-valmsg-for="Enderecos[${posicao}].CEP" data-valmsg-replace="true"></span>
                    </div>
                </div>
                <div class="c-form__group col-6">
                    <label for="Enderecos[${posicao}].Logradouro" class="c-form__label form-label control-label labelLogradouro">Logradouro</label>
                    <input name="Enderecos[${posicao}].Logradouro" posicao="${posicao}" class="c-form__input form-control logradouro inputLogradouro" data-msg-required="Logradouro é obrigatório !" data-msg-maxlength="Logradouro possui tamanho máximo de 256 caracteres !" data-rule-maxlength="256" required>
                    <span class="form-text text-danger field-validation-valid spanLogradouro" data-valmsg-for="Enderecos[${posicao}].Logradouro" data-valmsg-replace="true"></span>
                </div>
                <div class="c-form__group col-2">
                    <label for="Enderecos[${posicao}].Numero" class="c-form__label form-label control-label labelNumero">Número</label>
                    <input name="Enderecos[${posicao}].Numero" posicao="${posicao}" class="c-form__input form-control complemento maskNumero inputNumero"  data-msg-maxlength="Complemento possui tamanho máximo de 256 caracteres !" data-rule-maxlength="256">
                    <span class="form-text text-danger field-validation-valid spanNumero" data-valmsg-for="Enderecos[${posicao}].Numero" data-valmsg-replace="true"></span>
                </div>
            </div>
            <div class="row">
                <div class="c-form__group col-4">
                    <label for="Enderecos[${posicao}].Complemento" class="c-form__label form-label control-label labelComplemento">Complemento</label>
                    <input name="Enderecos[${posicao}].Complemento" posicao="${posicao}" class="c-form__input form-control complemento inputComplemento"  data-msg-maxlength="Complemento possui tamanho máximo de 256 caracteres !" data-rule-maxlength="256">
                    <span class="form-text text-danger field-validation-valid spanComplemento" data-valmsg-for="Enderecos[${posicao}].Complemento" data-valmsg-replace="true"></span>
                </div>
                <div class="c-form__group col-4">
                    <label for="Enderecos[${posicao}].Estado" class="c-form__label labelEstado">Estado</label>
                    <select name="Enderecos[${posicao}].Estado" posicao="${posicao}" onchange="buscarMunicipios(this)" class="c-form__input c-form__input--select estado selectEstado" required data-msg-required="Estado é obrigatório !">
                        <option value="" selected disabled>Selecione ...</option>
                        <option value="SP">São Paulo</option>
                        <option value="AC">Acre</option>
                        <option value="AL">Alagoas</option>
                        <option value="AP">Amapá</option>
                        <option value="AM">Amazonas</option>
                        <option value="BA">Bahia</option>
                        <option value="CE">Ceará</option>
                        <option value="DF">Distrito Federal</option>
                        <option value="ES">Espírito Santo</option>
                        <option value="GO">Goiás</option>
                        <option value="MA">Maranhão</option>
                        <option value="MT">Mato Grosso</option>
                        <option value="MS">Mato Grosso do Sul</option>
                        <option value="MG">Minas Gerais</option>
                        <option value="PA">Pará</option>
                        <option value="PB">Paraíba</option>
                        <option value="PR">Paraná</option>
                        <option value="PE">Pernambuco</option>
                        <option value="PI">Piauí</option>
                        <option value="RJ">Rio de Janeiro</option>
                        <option value="RN">Rio Grande do Norte</option>
                        <option value="RS">Rio Grande do Sul</option>
                        <option value="RO">Rondônia</option>
                        <option value="RR">Roraima</option>
                        <option value="SC">Santa Catarina</option>
                        <option value="SE">Sergipe</option>
                        <option value="TO">Tocantins</option>
                    </select>
                    <span class="form-text text-danger field-validation-valid spanEstado" data-valmsg-for="Enderecos[${posicao}].Estado" data-valmsg-replace="true"></span>
                </div>
                <div class="c-form__group col-4">
                    <label for="Enderecos[${posicao}].Municipio" class="c-form__label labelMunicipio">Município</label>
                    <select name="Enderecos[${posicao}].Municipio" posicao="${posicao}" disabled class="c-form__input c-form__input--select selectMunicipio" required data-msg-required="Munícipio é obrigatório !">
                        <option value="" selected disabled>Selecione ...</option>
                    </select>
                    <span class="form-text text-danger field-validation-valid spanMunicipio" data-valmsg-for="Enderecos[${posicao}].Municipio" data-valmsg-replace="true"></span>
                    <div class="d-flex justify-content-between w-100">
                        <a href="#" class="c-form__action repeater" onclick="adicionarEndereco(this)">
                            <i class="icon-add"></i> Adicionar outro Endereço
                        </a>
                       <a href="#" class="c-form__action repeater justify-content-end" onclick="removerEndereco(this)" style="color:red!important">
                            <i class="icon-delete"></i> Remover
                        </a> 
                    </div>
                </div>
            </div>
            </div>
    `;
    $(elementoPai).append(novoElemento);
    $('.maskCEP').each((i, e) => {
        $(e).mask("00000-000");
    })
    $('.maskNumero').each((i, e) => {
        $(e).mask("999999");
    })
    $(elemento).remove();
}


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

function removerEndereco(elemento) {
    $(elemento).closest(".linhaRemoverEndereco").remove();
    $('.linhaRemoverEndereco').each((i, e) => {
        let novaPosicao = i + 1;
        $(e).find(".labelCEP").attr("for", `Enderecos[${novaPosicao}].CEP`);
        $(e).find(".inputCEP").attr("name", `Enderecos[${novaPosicao}].CEP`).attr("posicao", `${novaPosicao}`);
        $(e).find(".spanCEP").attr("data-valmsg-for", `Enderecos[${novaPosicao}].CEP`);
        $(e).find(".labelLogradouro").attr("for", `Enderecos[${novaPosicao}].Logradouro`);
        $(e).find(".inputLogradouro").attr("name", `Enderecos[${novaPosicao}].Logradouro`).attr("posicao", `${novaPosicao}`);
        $(e).find(".spanLogradouro").attr("data-valmsg-for", `Enderecos[${novaPosicao}].Logradouro`);
        $(e).find(".labelNumero").attr("for", `Enderecos[${novaPosicao}].Numero`);
        $(e).find(".inputNumero").attr("name", `Enderecos[${novaPosicao}].Numero`).attr("posicao", `${novaPosicao}`);
        $(e).find(".spanNumero").attr("data-valmsg-for", `Enderecos[${novaPosicao}].Numero`);
        $(e).find(".labelComplemento").attr("for", `Enderecos[${novaPosicao}].Complemento`);
        $(e).find(".inputComplemento").attr("name", `Enderecos[${novaPosicao}].Complemento`).attr("posicao", `${novaPosicao}`);
        $(e).find(".spanComplemento").attr("data-valmsg-for", `Enderecos[${novaPosicao}].Complemento`);
        $(e).find(".labelEstado").attr("for", `Enderecos[${novaPosicao}].Estado`);
        $(e).find(".selectEstado").attr("name", `Enderecos[${novaPosicao}].Estado`).attr("posicao", `${novaPosicao}`);
        $(e).find(".spanEstado").attr("data-valmsg-for", `Enderecos[${novaPosicao}].Estado`);
        $(e).find(".labelMunicipio").attr("for", `Enderecos[${novaPosicao}].Municipio`);
        $(e).find(".selectMunicipio").attr("name", `Enderecos[${novaPosicao}].Municipio`).attr("posicao", `${novaPosicao}`);
        $(e).find(".spanMunicipio").attr("data-valmsg-for", `Enderecos[${novaPosicao}].Municipio`);
    })
}

