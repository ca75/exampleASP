using System.ComponentModel.DataAnnotations;
using MaritimeCode.Models;

namespace Maritimecode.ValidationAttributes
{
    class HeadersAllowedAttribute : ValidationAttribute
    {
        public HeadersAllowedAttribute(string startsWith, string endsWith)
        {
            StartsWith = startsWith;
            EndsWith = endsWith;
        }

        public string StartsWith { get; }
        public string EndsWith { get; }
        public string GetErrorMessage(string typeUsed) =>
        $"Flie upload {typeUsed} is not allowed.";

        protected override ValidationResult IsValid(object value,
        ValidationContext validationContext)
        {
            var valConObject = (UploadModel)validationContext.ObjectInstance;
            if (valConObject.File.ContentType.StartsWith(StartsWith)
                && valConObject.File.FileName.EndsWith(EndsWith))
            {
                return ValidationResult.Success;
            }
            return new ValidationResult(GetErrorMessage(valConObject.File.ContentType));
        }
    }
}