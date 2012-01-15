var utils = {
    ajaxPagination: function () {
        $('.pagination a').click(function () {
            var eventTarget = this;
            var uri = $(eventTarget).attr('href');

            //Porque não funciona???
            $('#loading-info').ajaxStart(function() {
                $(this).text("A obter informação");
            });
            
//            $('#loading-info').ajaxStop(function () {
//                $(this).text("");
//            });

            if (uri != undefined) {
                //$.ajax({
                //    type: "GET",
                //    url: uri + '&partial=true',     //acrescenta ao URI o parametro partial, para obter apenas o conteúdo da tabela
                $('#paginated-content').get(uri + '&partial=true',  function () {
                //        $('#paginated-content').html(html);

                        var currPage = $('.pagination li[class*="active"]').removeClass("active");
                        var nextButton = $('.pagination li[class*="next"]');
                        var prevButton = $('.pagination li[class*="prev"]');


                        if ($(eventTarget).parents("li").hasClass("next")) {
                            currPage = currPage.next().addClass("active");
                        } else if ($(eventTarget).parents("li").hasClass("prev")) {
                            currPage = currPage.prev().addClass("active");
                        } else {
                            //Adicionar classe 'active' ao clicado
                            currPage = $(eventTarget).parents("li").addClass("active");
                        }

                        if (currPage.next().hasClass("next")) {
                            nextButton.addClass("disabled");
                            nextButton.children('a').removeAttr('href');
                        } else {
                            nextButton.removeClass("disabled");
                            nextButton.children('a').attr('href', currPage.next().children('a').attr('href'));
                        }

                        if (currPage.prev().hasClass("prev")) {
                            prevButton.addClass("disabled");
                            prevButton.children('a').removeAttr('href');
                        } else {
                            prevButton.removeClass("disabled");
                            prevButton.children('a').attr('href', currPage.prev().children('a').attr('href'));
                        }

                        //Colocar URI actual no user-agent
                        history.pushState(document.html, document.title, uri);
                    }
                );
            }

            event.preventDefault();
        });
    },
	ajaxSearch: function(searchElem) {
		var fadeInOutTime = 1000;
		var func;
		$(searchElem).keyup( function() {
			console.log("At keyup -> "+this.value);
			if ($.trim(this.value) != "") {
				if (func == undefined) {
					func = new XMLHttpRequest();
					func.onreadystatechange = function() {
						if((func.readyState == 4) && (func.status == 200)) {
							console.log($("#suggestions").queue().length);
							if ($.trim(func.responseText) != "") {
								console.log(func.responseText);
								// auxfunc(func.responseText);
								console.log("before fadein -> "+ $(searchElem).val());
								$("#suggestions").css('display', 'block').hide().fadeIn(fadeInOutTime);
								console.log("after fadein -> "+ $(searchElem).val());
								$("#suggestions > table > tbody").html(func.responseText);
							} else {
								console.log("before fadeout");
								$("#suggestions").fadeOut(fadeInOutTime);
								console.log("after fadeout");
							}
						}
					};
				}
				func.open("GET", "/home/ajaxsearch?search=" + this.value, true);
				func.send(null);
			}  else {
				console.log("before fadeout at keyup");
				$("#suggestions").fadeOut(fadeInOutTime);
				console.log("after fadeout at keyup");
			}
		});
		$(searchElem).blur( function() {
			console.log("before fadeout at blur");
			$("#suggestions").fadeOut(fadeInOutTime);
			console.log("after fadeout at blur");
		});
		$(searchElem).focus( function() {
			console.log("At focus -> "+this.value);
			if ($.trim(this.value) != "") {
				$(this).keyup();
			}
		});
	},
	bindConfirmationMessageOnSubmit: function() {
		$("form").submit( function() {
			var value = $("[type=submit]", this).filter(".danger, .success").first().attr("value");
			return value != undefined ? confirm("Tem a certeza que quer " + value + "?") : true;
		});
	},
	disableAndOnChangeEnableSubmit: function() {
		$("form").filter( function() {
				return $(this).find(":input:not(:submit)").length > 0;
			}).find(":submit").each( function() { this.disabled = true; } );
		// Tirando partido do modelo de eventos Javascript, ao ser despoletado um evento 
		//  onChange em qualquer elemento filho do formulário, este é capturado pelo formulário.
		$("form").filter( function() {
				return $(this).find(":input:not(:submit)").length > 0;
			}).change( function() {
				$(":submit", this).each( function() { this.disabled = false; } );
		});
	},
	setFocus: function() {
		// Selector de multiplos atributos;
		// Verifica todos os elementos que respeitam o conjunto de atributos;
		$("form [name!=search]").filter(":input:not(:submit):visible:enabled:first").focus();
	},
	validateForm: function (elem) {
        var ret = true;
        // Se o nome contiver algum caracter especial, meta-character, é necessário
        // efectuar o escape desse caracter, utilizando \ antes do caracter.
        var validateString = function (str) {
            return str.replace(/\./g, "\\.").replace(/\*/g, "\\*");
        };

        var invalid = function (attr, invalidElem) {
            // Adiciona comentário ao lado do input
            $("span[data-valmsg-for=" + validateString(invalidElem.name) + "]").removeClass("field-validation-valid")
			.addClass("field-validation-error").html($(invalidElem).attr(attr));
            // Dá ênfase ao input que tem o erro de validação
            $(invalidElem).addClass("input-validation-error");
            ret = false;
        };

        var valid = function (validElem) {
            // Remove o eventual comentário que esteja ao lado do input
            $("span[data-valmsg-for=" + validateString(validElem.name) + "]").addClass("field-validation-valid")
			.removeClass("field-validation-error");
            // Retirar a eventual ênfase dada ao input
            $(validElem).removeClass("input-validation-error");
        };
        // Para cada um dos anteriores, adiciona ao respectivo span o texto que está no atributo data-val-*
        $("[data-val=true]", elem).each(
            function () {
                if ($(this).attr("data-val-required") != undefined) {
					if (($(this).attr("type") == "radio")) {
						if ($("input:checked", this.parentNode).length == 0)
						{
							invalid("data-val-required", this);
							// termina a verificação para este index do each
							return;
						}
						valid(this);
					}
					else if (this.tagName.toLowerCase() == "select") {
						if($("option:selected").length == 0) {
							invalid("data-val-required", this);
							// termina a verificação para este index do each
							return;
						}
						valid(this);
					}
                    else if ($.trim(this.value) == "") {
						invalid("data-val-required", this);
                        // termina a verificação para este index do each
                        return;
                    }

                    valid(this);
                }
                var max;
                var min;
                if ($(this).attr("data-val-length") != undefined) {
                    max = $(this).attr("data-val-length-max");
                    min = $(this).attr("data-val-length-min");
                    var length = this.value.length;
                    if (((max != undefined) && max < length) ||
						((min != undefined && min > length))) {
                        invalid("data-val-length", this);
                        // termina a verificação para este index do each
                        return;
                    }

                    valid(this);
                }
                if ($(this).attr("data-val-regex") != undefined) {
                    if (this.value.match($(this).attr("data-val-regex-pattern")) == null) {
                        invalid("data-val-regex", this);
                        // termina a verificação para este index do each
                        return;
                    }

                    valid(this);
                }
                if ($(this).attr("data-val-equalto") != undefined) {
                    if ($(this).val() != $("input[id=" +
											validateString($(this).attr("data-val-equalto-other")) +
											"]").val()) {
                        invalid("data-val-equalto", this);
                        // termina a verificação para este index do each
                        return;
                    }

                    valid(this);
                }
                if ($(this).attr("data-val-number") != undefined) {
                    var value = parseFloat($(this).val());
                    if (isNaN(value)) {
                        invalid("data-val-number", this);
                        // termina a verificação para este index do each
                        return;
                    }
                    max = parseFloat($(this).attr("data-val-range-max"));
                    min = parseFloat($(this).attr("data-val-range-min"));
                    if ((!isNaN(max) && max < value) ||
						(!isNaN(min) && min > value)) {
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