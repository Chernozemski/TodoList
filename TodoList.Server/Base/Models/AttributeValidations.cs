using System.ComponentModel.DataAnnotations;

namespace TodoList.Server.Base.Models;

public static class AttributeValidations
{
	public static bool Validate(object model, out List<ValidationResult> errors)
	{
		errors = [];
		return Validator.TryValidateObject(model, new(model), errors, true);
	}
}