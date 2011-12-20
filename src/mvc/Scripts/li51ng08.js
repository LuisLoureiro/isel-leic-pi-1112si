/*
TIPOS DE VALIDAÇÃO NO FORMULÁRIO DE REGISTO DE UTILIZADORES(e, se calhar, não só)
    + data-val -> se tem algum tipo de validação;
    + data-val-required -> se é campo de preenchimento obrigatório, com a respectiva mensagem;
    + data-val-length -> mensagem a retornar quando o número de caracteres não corresponde aos valores mínimo e/ou máximo;
    + data-val-length-max -> número máximo de caracteres;
    + data-val-length-min -> número mínimo de caracteres;
    + data-val-regex -> mensagem a retornar quando o valor introduzido não corresponde ao padrão definido;
    + data-val-regex-pattern -> padrão de validação do valor introduzido no campo;
    + data-val-equalto -> mensagem a retornar quando o valor do campo não corresponde ao valor de outro campo;
    + data-val-equalto-other -> ??
OUTROS FORMS
	+ data-val-number -> mensagem a retornar quando o valor do campo não é numérico(incluíndo vírgula);
	+ data-val-range -> mensagem a retornar quando o valor do campo está fora dos limites definidos;
	+ data-val-range-max -> valor máximo do campo;
	+ data-val-range-min -> valor mínimo do campo;
	
*/
var utils = {
	validateForm: function (elem) {
        var ret = true;
		// Se o nome contiver algum caracter especial, meta-character, é necessário
		// efectuar o escape desse caracter, utilizando \\ antes do caracter.
		var validateString = function(str) {
			return str.replace(/\./g, "\\.").replace(/\*/g, "\\*");
		};
		var invalid = function (attr, elem) {
			// Adiciona comentário ao lado do input
			$("span[data-valmsg-for=" + validateString(elem.name) + "]").removeClass("field-validation-valid")
			.addClass("field-validation-error").html($(elem).attr(attr));
			// Dá ênfase ao input que tem o erro de validação
			$(elem).addClass("input-validation-error");
			ret = false;
		};
		var valid = function (elem) {
			// Remove o eventual comentário que esteja ao lado do input
			$("span[data-valmsg-for=" + validateString(elem.name) + "]").addClass("field-validation-valid")
			.removeClass("field-validation-error");
			// Retirar a eventual ênfase dada ao input
			$(elem).removeClass("input-validation-error");
		};
        // Para cada um dos anteriores, adiciona ao respectivo span o texto que está no atributo data-val-*
        $("[data-val=true]", elem).each(
            function () {
                if ($(this).attr("data-val-required") != undefined)
                {
					if (($(this).attr("type") == "radio"))
					{
						if ($("input:checked", this.parentNode).length == 0)
						{
							invalid("data-val-required", this);
							// termina a verificação para este index do each
							return;
						}
						
						valid(this);
					}
					else if (this.tagName.toLowerCase() == "select")
					{
						if($("option:selected").length == 0)
						{
							invalid("data-val-required", this);
							// termina a verificação para este index do each
							return;
						}
						
						valid(this);
					}
                    else if ($(this).val() == "")
                    {
						
                        invalid("data-val-required", this);
						// termina a verificação para este index do each
						return;
                    }
					
					valid(this);
                }
				if ($(this).attr("data-val-length") != undefined)
				{
					var max = $(this).attr("data-val-length-max");
					var min = $(this).attr("data-val-length-min");
					var length = this.value.length;
					if (((max != undefined) && max < length) || 
						((min != undefined && min > length)))
					{
						invalid("data-val-length", this);
						// termina a verificação para este index do each
						return;
					}
					
					valid(this);
				}
				if ($(this).attr("data-val-regex") != undefined)
				{
					if(this.value.match($(this).attr("data-val-regex-pattern")) == null)
					{
						invalid("data-val-regex", this);
						// termina a verificação para este index do each
						return;
					}

					valid(this);
				}
				if ($(this).attr("data-val-equalto") != undefined)
				{
					if ($(this).val() != $("input[id=" + 
											validateString($(this).attr("data-val-equalto-other")) + 
											"]").val())
					{
						invalid("data-val-equalto", this);
						// termina a verificação para este index do each
						return;
					}
					
					valid(this);
				}
				if ($(this).attr("data-val-number") != undefined)
				{
					if (isNaN(parseFloat($(this).val())))
					{
						invalid("data-val-number", this);
						// termina a verificação para este index do each
						return;
					}
					
					var max = $(this).attr("data-val-range-max");
					var min = $(this).attr("data-val-range-min");
					var value = $(this).val();
					if ((max != undefined && max < value) || 
						(min != undefined && min > value))
					{
						invalid("data-val-range", this);
						// termina a verificação para este index do each
						return;
					}
					valid(this);
				}
            });
        return ret;
    }
}