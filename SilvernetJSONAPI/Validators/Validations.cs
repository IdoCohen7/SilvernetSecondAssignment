namespace SilvernetJSONAPI.Validators
{
    public static class Validations
    {
        // helper validation static class to help validate the phone and id fields
        public static void ValidateIsraeliId(string? id)
        {
            if (!IsValidIsraeliId(id))
                throw new InvalidOperationException("Invalid Israeli IdNumber.");
        }

        public static void ValidateIsraeliPhone(string? phone)
        {
            if (!IsValidIsraeliPhone(phone))
                throw new InvalidOperationException("Invalid Israeli Phone.");
        }

        public static bool IsValidIsraeliId(string? id)
        {
            if (string.IsNullOrWhiteSpace(id)) return false;

            var digitsOnly = new string(id.Where(char.IsDigit).ToArray());
            if (digitsOnly.Length > 9) return false;
            digitsOnly = digitsOnly.PadLeft(9, '0');
            if (digitsOnly.Distinct().Count() == 1) return false;

            var sum = 0;
            for (var i = 0; i < 9; i++)
            {
                var digit = digitsOnly[i] - '0';
                var step = digit * ((i % 2) + 1);
                if (step > 9) step -= 9;
                sum += step;
            }

            return sum % 10 == 0;
        }

        public static bool IsValidIsraeliPhone(string? phone)
        {
            if (string.IsNullOrWhiteSpace(phone)) return false;

            var digitsOnly = new string(phone.Where(char.IsDigit).ToArray());

            if (digitsOnly.StartsWith("972"))
                digitsOnly = "0" + digitsOnly.Substring(3);

            if (digitsOnly.Length < 9 || digitsOnly.Length > 10) return false;
            if (!digitsOnly.StartsWith("0")) return false;

            if (digitsOnly.StartsWith("05"))
                return digitsOnly.Length == 10;

            return digitsOnly.Length == 9 || digitsOnly.Length == 10;
        }

        public static string? MaskId(string? id)
        {
            if (string.IsNullOrWhiteSpace(id)) return id;
            var digits = new string(id.Where(char.IsDigit).ToArray());
            if (digits.Length <= 3) return "***";
            return new string('*', digits.Length - 3) + digits.Substring(digits.Length - 3);
        }

        public static string? MaskPhone(string? phone)
        {
            if (string.IsNullOrWhiteSpace(phone)) return phone;
            var digits = new string(phone.Where(char.IsDigit).ToArray());
            if (digits.Length <= 4) return "****";
            return digits.Substring(0, 2) + new string('*', digits.Length - 4) + digits.Substring(digits.Length - 2);
        }
    }
}
