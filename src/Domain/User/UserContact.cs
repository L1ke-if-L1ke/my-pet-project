using System.Text.RegularExpressions;
using Domain.Book;

namespace Domain.User
{
    public record UserContact
    {
        public string UserEmail { get; }
        public string UserPhone { get; }
        private UserContact(string email, string phone)
        {
            UserEmail = email;
            UserPhone = phone;
        }
        private static readonly Regex _phoneValidationRegex = new Regex(
            @"\B[+]\d\s([(]\d{3}[)])\s(\d{3})\s(\d{2}[-]\d{2})\b",RegexOptions.Compiled
            );
        public static UserContact Create(string valueM, string valueP)
        {
            if (string.IsNullOrEmpty(valueM))
                throw new ArgumentNullException("Почта была пустой");
            if (string.IsNullOrEmpty(valueP))
                throw new ArgumentNullException("Номер телефона был пустым");
            Regex emailRegex = new Regex(@"\b([^\d]\w+)[@]([^\d]\w+)[.](com|ru)\b");
            Match match = emailRegex.Match(valueM);
            if (!match.Success)
                throw new ArgumentException("Почта некорректного формата");
            match = _phoneValidationRegex.Match(valueP);
            if (!match.Success)
                throw new ArgumentException("Номер телефона имеет некорректный формат");
            return new UserContact(valueM, valueP);
        }
    }
}
