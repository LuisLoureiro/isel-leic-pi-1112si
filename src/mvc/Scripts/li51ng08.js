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
    validateRegisterForm: function () {
        var ret = true;
        // Para cada um dos anteriores, adiciona ao respectivo span o texto que est� no atributo data-val-*
        $("input[data-val=true]", document.registerForm).each(
            function () {
                if ($(this).attr("data-val-required") != undefined)
                {
                    if ($(this).val() == "")
                    {
                        // Adiciona o coment�rio ao lado do input
						$("span[data-valmsg-for=" + this.id + "]").removeClass("field-validation-valid")
						.addClass("field-validation-error").html($(this).attr("data-val-required"));
						// D� �nfase ao input que tem o erro de valida��o
						$(this).addClass("input-validation-error");
						ret = false;
						// termina a fun��o para este index do each
						return;
                    }
					else 
					{
						// Remove o eventual coment�rio que esteja ao lado do input
						$("span[data-valmsg-for=" + this.id + "]").addClass("field-validation-valid")
						.removeClass("field-validation-error");
						// Retirar a eventual �nfase dada ao input
						$(this).removeClass("input-validation-error");
					}
                }
				if ($(this).attr("data-val-length") != undefined)
				{
					if (((max != undefined) && max < length) || 
						((min != undefined && min > length)))
					{
						// Adiciona coment�rio ao lado do input
						$("span[data-valmsg-for=" + this.id + "]").removeClass("field-validation-valid")
						.addClass("field-validation-error").html($(this).attr("data-val-length"));
						// D� �nfase ao input que tem o erro de valida��o
						$(this).addClass("input-validation-error");
						ret = false;
						// termina a fun��o para este index do each
						return;
					}
					else
					{
						// Remove o eventual coment�rio que esteja ao lado do input
						$("span[data-valmsg-for=" + this.id + "]").addClass("field-validation-valid")
						.removeClass("field-validation-error");
						// Retirar a eventual �nfase dada ao input
						$(this).removeClass("input-validation-error");
					}
				}
				if ($(this).attr("data-val-regex") != undefined)
				{
					if(this.value.match($(this).attr("data-val-regex-pattern")) == null)
					{
						// Adiciona coment�rio ao lado do input
						$("span[data-valmsg-for=" + this.id + "]").removeClass("field-validation-valid")
						.addClass("field-validation-error").html($(this).attr("data-val-regex"));
						// D� �nfase ao input que tem o erro de valida��o
						$(this).addClass("input-validation-error");
						ret = false;
						// termina a fun��o para este index do each
						return;
					}
					else
					{
						// Remove o eventual coment�rio que esteja ao lado do input
						$("span[data-valmsg-for=" + this.id + "]").addClass("field-validation-valid")
						.removeClass("field-validation-error");
						// Retirar a eventual �nfase dada ao input
						$(this).removeClass("input-validation-error");
					}
				}
				if ($(this).attr("data-val-equalto") != undefined)
				{
					// O atributo data-val-equalto-other deveria ser usado
					if ($(this).val() != $("input[id=Password]").val())
					{
						alert("Id = " + this.id + ", equalto = false");
						// Adiciona coment�rio ao lado do input
						$("span[data-valmsg-for=" + this.id + "]").removeClass("field-validation-valid")
						.addClass("field-validation-error").html($(this).attr("data-val-equalto"));
						// D� �nfase ao input que tem o erro de valida��o
						$(this).addClass("input-validation-error");
						ret = false;
						// termina a fun��o para este index do each
						return;
					}
					else
					{
						alert("Id = " + this.id + ", equalto = true");
						// Remove o eventual coment�rio que esteja ao lado do input
						$("span[data-valmsg-for=" + this.id + "]").addClass("field-validation-valid")
						.removeClass("field-validation-error");
						// Retirar a eventual �nfase dada ao input
						$(this).removeClass("input-validation-error");
					}
				}
            });
        return ret;
    }
}