using System.ComponentModel.DataAnnotations;

namespace EncryptionService.Models.Attributes
{
	public class AtLeastOneFieldRequiredAttribute : ValidationAttribute
	{
		private readonly string _otherPropertyName;

		public AtLeastOneFieldRequiredAttribute(string otherPropertyName,
			string errorMessage = "")
			: base(errorMessage)
		{
			_otherPropertyName = otherPropertyName;
		}

		protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
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
