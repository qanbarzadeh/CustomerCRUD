using com.google.i18n.phonenumbers;
using System;
using System.Globalization;

namespace Mc2.CrudTest.Shared.Utilities
{
    public static class ValidationUtility
    {
        public static bool IsValidPhoneNumber(string phoneNumber)
        {
            PhoneNumberUtil phoneUtil = PhoneNumberUtil.getInstance();
            try
            {
                var parsedPhoneNumber = phoneUtil.parse(phoneNumber, null);
                return phoneUtil.isValidNumber(parsedPhoneNumber);
            }
            catch (NumberParseException)
            {
                return false;
            }
        }
    }
}
