var utils = {
    submitPageSize: function () {
        $('#pageSize').val(utils.getParameterByName('pageSize'));

        $('#pageSize').change(function () {
            $(this).closest('form').submit();
        });
    },
    ajaxPagination: function () {
        $('.pagination a').click(function (event) {
            var eventTarget = this;
            var uri = $(eventTarget).attr('href');

            $('#loading-info').ajaxStart(function () {
                $(this).text("A obter informacao...");
            });

            $('#loading-info').ajaxStop(function () {
                $(this).text("");
            });

            if (uri != undefined) {
                $('#paginated-content').load(uri + '&partial=true', function () {
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
    ajaxSearch: function (searchElem) {
        var fadeInOutTime = 1000;
        var func;
        $(searchElem).keyup(function () {
            if ($.trim(this.value) != "") {
                if (func == undefined) {
                    func = new XMLHttpRequest();
                    func.onreadystatechange = function () {
                        if ((func.readyState == 4) && (func.status == 200)) {
                            if ($.trim(func.responseText) != "") {
                                $("#suggestions").css('display', 'block').hide().fadeIn(fadeInOutTime);
                                $("#suggestions > table > tbody").html(func.responseText);
                            } else {
                                $("#suggestions").fadeOut(fadeInOutTime);
                            }
                        }
                    };
                }
                func.open("GET", "/home/ajaxsearch?search=" + this.value, true);
                func.send(null);
            } else {
                $("#suggestions").fadeOut(fadeInOutTime);
            }
        });
        $(searchElem).blur(function () {
            $("#suggestions").fadeOut(fadeInOutTime);
        });
        $(searchElem).focus(function () {
            if ($.trim(this.value) != "") {
                $(this).keyup();
            }
        });
    },
    bindConfirmationMessageOnSubmit: function () {
        $("form").submit(function () {
            var value = $("[type=submit]", this).filter(".danger, .success").first().attr("value");
            return value != undefined ? confirm("Tem a certeza que quer " + value + "?") : true;
        });
    },
    disableAndOnChangeEnableSubmit: function () {
        $("form").filter(function () {
            return $(this).find(":input:not(:submit)").length > 0;
        }).find(":submit").each(function () { this.disabled = true; });
        // Tirando partido do modelo de eventos Javascript, ao ser despoletado um evento 
        //  onChange em qualquer elemento filho do formul�rio, este � capturado pelo formul�rio.
        $("form").filter(function () {
            return $(this).find(":input:not(:submit)").length > 0;
        }).change(function () {
            $(":submit", this).each(function () { this.disabled = false; });
        });
    },
    setFocus: function () {
        // Selector de multiplos atributos;
        // Verifica todos os elementos que respeitam o conjunto de atributos;
        $("form [type!=search]").filter(":input:not(:submit,:image):visible:enabled:first").focus();
    },
    validateForm: function (elem) {
        var ret = true;
        // Se o nome contiver algum caracter especial, meta-character, � necess�rio
        // efectuar o escape desse caracter, utilizando \ antes do caracter.
        var validateString = function (str) {
            return str.replace(/\./g, "\\.").replace(/\*/g, "\\*");
        };

        var invalid = function (attr, invalidElem) {
            // Adiciona coment�rio ao lado do input
            $("span[data-valmsg-for=" + validateString(invalidElem.name) + "]").removeClass("field-validation-valid")
			.addClass("field-validation-error").html($(invalidElem).attr(attr));
            // D� �nfase ao input que tem o erro de valida��o
            $(invalidElem).addClass("input-validation-error");
            ret = false;
        };

        var valid = function (validElem) {
            // Remove o eventual coment�rio que esteja ao lado do input
            $("span[data-valmsg-for=" + validateString(validElem.name) + "]").addClass("field-validation-valid")
			.removeClass("field-validation-error");
            // Retirar a eventual �nfase dada ao input
            $(validElem).removeClass("input-validation-error");
        };
        // Para cada um dos anteriores, adiciona ao respectivo span o texto que est� no atributo data-val-*
        $("[data-val=true]", elem).each(
            function () {
                if ($(this).attr("data-val-required") != undefined) {
                    if (($(this).attr("type") == "radio")) {
                        if ($("input:checked", this.parentNode).length == 0) {
                            invalid("data-val-required", this);
                            // termina a verifica��o para este index do each
                            return;
                        }
                        valid(this);
                    }
                    else if (this.tagName.toLowerCase() == "select") {
                        if ($("option:selected").length == 0) {
                            invalid("data-val-required", this);
                            // termina a verifica��o para este index do each
                            return;
                        }
                        valid(this);
                    }
                    else if ($.trim(this.value) == "") {
                        invalid("data-val-required", this);
                        // termina a verifica��o para este index do each
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
                        // termina a verifica��o para este index do each
                        return;
                    }

                    valid(this);
                }
                if ($(this).attr("data-val-regex") != undefined) {
                    if (this.value.match($(this).attr("data-val-regex-pattern")) == null) {
                        invalid("data-val-regex", this);
                        // termina a verifica��o para este index do each
                        return;
                    }

                    valid(this);
                }
                if ($(this).attr("data-val-equalto") != undefined) {
					// Como a pesquisa est� a ser feita pelo valor exacto de validateString($(this).attr("data-val-equalto-other")),
					//  n�o � encontrado nenhum valor, pois o elemento correspondente tem id=Password.
					// Se a pesquisa estivesse a procurar por uma substring, input[id*=...] ou input[id$=...], tamb�m n�o era
					//  retornado nenhum valor, porque o elemento tem id=Password.
					var toCompareElementId = validateString($(this).attr("data-val-equalto-other"));
                    if ($(this).val() != $("input[id$=" + 
											// A soma de 2 � necess�ria porque o valor do from para a fun��o substring
											//  � exclusivo e porque a seguir ao �ltimo '\' ainda se encontra o '.'
											toCompareElementId.substring(toCompareElementId.lastIndexOf("\\")+2)
											 + "]").val()) {
                        invalid("data-val-equalto", this);
                        // termina a verifica��o para este index do each
                        return;
                    }

                    valid(this);
                }
                if ($(this).attr("data-val-number") != undefined) {
                    var value = parseFloat($(this).val());
                    if (isNaN(value)) {
                        invalid("data-val-number", this);
                        // termina a verifica��o para este index do each
                        return;
                    }
                    max = parseFloat($(this).attr("data-val-range-max"));
                    min = parseFloat($(this).attr("data-val-range-min"));
                    if ((!isNaN(max) && max < value) ||
						(!isNaN(min) && min > value)) {
                        invalid("data-val-range", this);
                        // termina a verifica��o para este index do each
                        return;
                    }
                    valid(this);
                }
            });
        return ret;
    },
    getParameterByName: function (name) {
        var match = RegExp('[?&]' + name + '=([^&]*)').exec(window.location.search);
        return match && decodeURIComponent(match[1].replace( /\+/g , ' '));
    }
}