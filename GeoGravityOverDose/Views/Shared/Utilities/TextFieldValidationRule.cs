using System.Globalization;
using System.Text.RegularExpressions;
using System.Windows.Controls;

namespace GeoGravityOverDose.Views.Shared.Utilities
{
    public class NumericValidationRule : ValidationRule
    {
        public enum FieldType
        {
            Default,  // Допускает все
            Number,   // Допускает int, float, double
            String    // Допускает только символы, кроме знаков препинания и пробелов
        }
        public FieldType TypeField { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            // Проверяем пустое значение
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return ValidationResult.ValidResult; // Пропускаем пустые значения
            }

            var inputValue = value.ToString();

            switch (TypeField)
            {
                case FieldType.Number:
                    // Проверка на числовые значения (int, float, double)
                    if (int.TryParse(inputValue, out _) || float.TryParse(inputValue, out _) || double.TryParse(inputValue, out _))
                    {
                        return ValidationResult.ValidResult;
                    }
                    return new ValidationResult(false, "Недопустимый формат числа");

                case FieldType.String:
                    // Проверка на недопустимые символы (только буквы)
                    if (Regex.IsMatch(inputValue, @"^[^\d\s\W]+$")) // Допускаются только буквы
                    {
                        return ValidationResult.ValidResult;
                    }
                    return new ValidationResult(false, "Допустимы только буквы без пробелов и знаков препинания");

                case FieldType.Default:
                default:
                    // Допускает все
                    return ValidationResult.ValidResult;
            }
        }
    }
}
