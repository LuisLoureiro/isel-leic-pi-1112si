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
    validateRegisterForm: function () {
        var ret = true;
        // Para cada um dos anteriores, adiciona ao respectivo span o texto que está no atributo data-val-*
        $("input[data-val=true]", document.registerForm).each(
            function () {
                if ($(this).attr("data-val-required") != undefined)
                {
                    if ($(this).val() == "")
                    {
                        // Adiciona o comentário ao lado do input
						$("span[data-valmsg-for=" + this.id + "]").removeClass("field-validation-valid")
						.addClass("field-validation-error").html($(this).attr("data-val-required"));
						// Dá ênfase ao input que tem o erro de validação
						$(this).addClass("input-validation-error");
						ret = false;
						// termina a função para este index do each
						return;
                    }
					else 
					{
						// Remove o eventual comentário que esteja ao lado do input
						$("span[data-valmsg-for=" + this.id + "]").addClass("field-validation-valid")
						.removeClass("field-validation-error");
						// Retirar a eventual ênfase dada ao input
						$(this).removeClass("input-validation-error");
					}
                }
				if ($(this).attr("data-val-length") != undefined)
				{
					if (((max != undefined) && max < length) || 
						((min != undefined && min > length)))
					{
						// Adiciona comentário ao lado do input
						$("span[data-valmsg-for=" + this.id + "]").removeClass("field-validation-valid")
						.addClass("field-validation-error").html($(this).attr("data-val-length"));
						// Dá ênfase ao input que tem o erro de validação
						$(this).addClass("input-validation-error");
						ret = false;
						// termina a função para este index do each
						return;
					}
					else
					{
						// Remove o eventual comentário que esteja ao lado do input
						$("span[data-valmsg-for=" + this.id + "]").addClass("field-validation-valid")
						.removeClass("field-validation-error");
						// Retirar a eventual ênfase dada ao input
						$(this).removeClass("input-validation-error");
					}
				}
				if ($(this).attr("data-val-regex") != undefined)
				{
					if(this.value.match($(this).attr("data-val-regex-pattern")) == null)
					{
						// Adiciona comentário ao lado do input
						$("span[data-valmsg-for=" + this.id + "]").removeClass("field-validation-valid")
						.addClass("field-validation-error").html($(this).attr("data-val-regex"));
						// Dá ênfase ao input que tem o erro de validação
						$(this).addClass("input-validation-error");
						ret = false;
						// termina a função para este index do each
						return;
					}
					else
					{
						// Remove o eventual comentário que esteja ao lado do input
						$("span[data-valmsg-for=" + this.id + "]").addClass("field-validation-valid")
						.removeClass("field-validation-error");
						// Retirar a eventual ênfase dada ao input
						$(this).removeClass("input-validation-error");
					}
				}
				if ($(this).attr("data-val-equalto") != undefined)
				{
					// O atributo data-val-equalto-other deveria ser usado
					if ($(this).val() != $("input[id=Password]").val())
					{
						alert("Id = " + this.id + ", equalto = false");
						// Adiciona comentário ao lado do input
						$("span[data-valmsg-for=" + this.id + "]").removeClass("field-validation-valid")
						.addClass("field-validation-error").html($(this).attr("data-val-equalto"));
						// Dá ênfase ao input que tem o erro de validação
						$(this).addClass("input-validation-error");
						ret = false;
						// termina a função para este index do each
						return;
					}
					else
					{
						alert("Id = " + this.id + ", equalto = true");
						// Remove o eventual comentário que esteja ao lado do input
						$("span[data-valmsg-for=" + this.id + "]").addClass("field-validation-valid")
						.removeClass("field-validation-error");
						// Retirar a eventual ênfase dada ao input
						$(this).removeClass("input-validation-error");
					}
				}
            });
        return ret;
    }
}