using com.google.i18n.phonenumbers;
using IbanNet;
using System;
using System.Globalization;

namespace Mc2.CrudTest.Shared.Utilities
{
    public static class ValidationUtility
    {
        //validate Phone number 
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
        //validate Email 
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrEmpty(email))
            {
                return false;
            }

            try
            {
                var mailAddress = new System.Net.Mail.MailAddress(email);
                return mailAddress.Address == email;
            }
            catch
            {
                return false;
            }
        }

        //validate bnank account number 
        public static bool IsValidBankAccountNumber(string bankAccountNumber)
        {
            if (string.IsNullOrEmpty(bankAccountNumber))
            {
                return false;
            }

            // Create an instance of the IbanValidator
            IbanValidator ibanValidator = new IbanValidator();

            // Validate the bank account number as an IBAN
            ValidationResult validationResult = ibanValidator.Validate(bankAccountNumber);

            return validationResult.IsValid;
        }


    }
}
