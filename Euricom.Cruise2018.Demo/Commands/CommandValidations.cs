using System;

namespace Euricom.Cruise2018.Demo.Commands
{
    public static class CommandValidations
    {
        private static string _requiredMessageTemplate = "{0} is verplicht mee te geven.";
        private static string _requiredStringWithMaxlengthMessageTemplate = "{0} is verplicht mee te geven met een maximum lengte van {1}. Waarde in command was '{2}'.";
        private static string _requiredDateMessageTemplate = "{0} is verplicht mee te geven. Waarde in command was {1}.";
        private static string _requiredGuidMessageTemplate = "{0} is verplicht mee te geven. Waarde in command was {1}.";
        private static string _requiredNullableGuidMessageTemplate = "{0} is verplicht mee te geven.";
        private static string _stringWithMaxLengthMessageTemplate = "{0} heeft een maximum lengte van {1}. Waarde in command was '{2}'.";

        public static bool ValidateRequiredString(string value, int maxLength = 0)
        {
            if (string.IsNullOrEmpty(value))
                return false;

            if (maxLength > 0 && value.Length > maxLength)
                return false;

            return true;
        }

        public static bool ValidateStringLength(string value, int maxLength = 0)
        {
            if (!string.IsNullOrEmpty(value))
            {
                if (maxLength > 0 && value.Length > maxLength)
                    return false;
            }

            return true;
        }

        public static string GetRequiredValidationErrorMessage(string name)
        {
            return string.Format(_requiredMessageTemplate, name);
        }

        public static string GetRequiredStringValidationErrorMessage(string name, string value, int maxLength)
        {
            return string.Format(_requiredStringWithMaxlengthMessageTemplate, name, maxLength, value);
        }

        public static string GetStringLengthValidationErrorMessage(string name, string value, int maxLength)
        {
            return string.Format(_stringWithMaxLengthMessageTemplate, name, maxLength, value);
        }

        public static bool ValidateRequiredGuid(Guid value)
        {
            if (value == Guid.Empty)
                return false;

            return true;
        }

        public static bool ValidateRequiredDate(DateTime value)
        {
            if (value == default(DateTime))
                return false;

            return true;
        }

        public static string GetRequiredDateValidationErrorMessage(string name, DateTime value)
        {
            return string.Format(_requiredDateMessageTemplate, name, value.ToString());
        }

        public static string GetRequiredGuidValidationErrorMessage(string name, Guid value)
        {
            return string.Format(_requiredGuidMessageTemplate, name, value.ToString());
        }

        public static bool ValidateRequiredNullableGuid(Guid? value)
        {
            if (!value.HasValue)
                return false;

            return ValidateRequiredGuid(value.Value);
        }

        public static string GetRequiredNullableGuidValidationErrorMessage(string name)
        {
            return string.Format(_requiredNullableGuidMessageTemplate, name);
        }
    }
}
