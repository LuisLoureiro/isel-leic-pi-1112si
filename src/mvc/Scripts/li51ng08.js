/*
TIPOS DE VALIDA��O NO FORMUL�RIO DE REGISTO DE UTILIZADORES(e, se calhar, n�o s�)
    + data-val -> se tem algum tipo de valida��o;
    + data-val-required -> se � campo de preenchimento obrigat�rio, com a respectiva mensagem;
    + data-val-length -> mensagem a retornar quando o n�mero de caracteres n�o corresponde aos valores m�nimo e/ou m�ximo;
    + data-val-length-max -> n�mero m�ximo de caracteres;
    + data-val-length-min -> n�mero m�nimo de caracteres;
    + data-val-regex -> mensagem a retornar quando o valor introduzido n�o corresponde ao padr�o definido;
    + data-val-regex-pattern -> padr�o de valida��o do valor introduzido no campo;
    + data-val-equalto -> mensagem a retornar quando o valor do campo n�o corresponde ao valor de outro campo;
    + data-val-equalto-other -> ??
OUTROS FORMS
	+ data-val-number -> mensagem a retornar quando o valor do campo n�o � num�rico(inclu�ndo v�rgula);
	+ data-val-range -> mensagem a retornar quando o valor do campo est� fora dos limites definidos;
	+ data-val-range-max -> valor m�ximo do campo;
	+ data-val-range-min -> valor m�nimo do campo;
	
*/
var utils = {
	validateForm: function (elem) {
        var ret = true;
		// Se o nome contiver algum caracter especial, meta-character, � necess�rio
		// efectuar o escape desse caracter, utilizando \\ antes do caracter.
		var validateString = function(str) {
			return str.replace(/\./g, "\\.").replace(/\*/g, "\\*");
		};
		var invalid = function (attr, elem) {
			// Adiciona coment�rio ao lado do input
			$("span[data-valmsg-for=" + validateString(elem.name) + "]").removeClass("field-validation-valid")
			.addClass("field-validation-error").html($(elem).attr(attr));
			// D� �nfase ao input que tem o erro de valida��o
			$(elem).addClass("input-validation-error");
			ret = false;
		};
		var valid = function (elem) {
			// Remove o eventual coment�rio que esteja ao lado do input
			$("span[data-valmsg-for=" + validateString(elem.name) + "]").addClass("field-validation-valid")
			.removeClass("field-validation-error");
			// Retirar a eventual �nfase dada ao input
			$(elem).removeClass("input-validation-error");
		};
        // Para cada um dos anteriores, adiciona ao respectivo span o texto que est� no atributo data-val-*
        $("[data-val=true]", elem).each(
            function () {
                if ($(this).attr("data-val-required") != undefined)
                {
					if (($(this).attr("type") == "radio"))
					{
						if ($("input:checked", this.parentNode).length == 0)
						{
							invalid("data-val-required", this);
							// termina a verifica��o para este index do each
							return;
						}
						
						valid(this);
					}
					else if (this.tagName.toLowerCase() == "select")
					{
						if($("option:selected").length == 0)
						{
							invalid("data-val-required", this);
							// termina a verifica��o para este index do each
							return;
						}
						
						valid(this);
					}
                    else if ($(this).val() == "")
                    {
						
                        invalid("data-val-required", this);
						// termina a verifica��o para este index do each
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
						// termina a verifica��o para este index do each
						return;
					}
					
					valid(this);
				}
				if ($(this).attr("data-val-regex") != undefined)
				{
					if(this.value.match($(this).attr("data-val-regex-pattern")) == null)
					{
						invalid("data-val-regex", this);
						// termina a verifica��o para este index do each
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
						// termina a verifica��o para este index do each
						return;
					}
					
					valid(this);
				}
				if ($(this).attr("data-val-number") != undefined)
				{
					if (isNaN(parseFloat($(this).val())))
					{
						invalid("data-val-number", this);
						// termina a verifica��o para este index do each
						return;
					}
					
					var max = $(this).attr("data-val-range-max");
					var min = $(this).attr("data-val-range-min");
					var value = $(this).val();
					if ((max != undefined && max < value) || 
						(min != undefined && min > value))
					{
						invalid("data-val-range", this);
						// termina a verifica��o para este index do each
						return;
					}
					valid(this);
				}
            });
        return ret;
    }
}