var utils = {
	validateRegisterForm : function () {
		var formElems = document.registerForm.elements;
		var count = 0;
		for (var idx=0; idx<document.registerForm.length; idx++)
		{
			if (formElems[idx].getAttribute("data-val-required") != null)
			{
				if (formElems[idx].value == null || formElems[idx].value == "")
				{
					alert("Dentro do if -> "+formElems[idx].id);
					return false;
				}
			}
        }
		alert("Total = " + document.registerForm.length);
		return true;
	}
}