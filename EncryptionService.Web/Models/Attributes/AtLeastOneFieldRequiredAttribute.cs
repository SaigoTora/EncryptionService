using System.ComponentModel.DataAnnotations;

namespace EncryptionService.Web.Models.Attributes
{
	public class AtLeastOneFieldRequiredAttribute(string otherPropertyName,
		string errorMessage = "") : ValidationAttribute(errorMessage)
	{
		private readonly string _otherPropertyName = otherPropertyName;

		protected override ValidationResult? IsValid(object? value,
			ValidationContext validationContext)
		{
			var otherProperty = validationContext.ObjectType.GetProperty(_otherPropertyName);
			if (otherProperty == null)
				return new ValidationResult($"Field {_otherPropertyName} not found");

			var otherValue = otherProperty.GetValue(validationContext.ObjectInstance) as string;
			var thisValue = value as string;

			if (string.IsNullOrWhiteSpace(thisValue) && string.IsNullOrWhiteSpace(otherValue))
				return new ValidationResult(ErrorMessage);

			return ValidationResult.Success;
		}
	}
}