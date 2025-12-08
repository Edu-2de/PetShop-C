using System.Text.RegularExpressions;

namespace SIGA_PET.Validators
{
    public static class ValidationHelper
    {
        // Valida telefone brasileiro (fixo ou celular)
        public static bool IsValidPhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone)) return false;
            
            // Remove caracteres não numéricos
            var digits = Regex.Replace(phone, @"[^\d]", "");
            
            // Telefone fixo: (XX) XXXX-XXXX = 10 dígitos
            // Celular: (XX) 9XXXX-XXXX = 11 dígitos
            return digits.Length == 10 || digits.Length == 11;
        }

        // Valida email
        public static bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        // Valida CPF
        public static bool IsValidCpf(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf)) return false;
            
            cpf = Regex.Replace(cpf, @"[^\d]", "");
            
            if (cpf.Length != 11) return false;
            
            // Verifica se todos os dígitos são iguais
            if (cpf.All(c => c == cpf[0])) return false;
            
            // Validação do CPF
            var multiplicador1 = new int[9] { 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            var multiplicador2 = new int[10] { 11, 10, 9, 8, 7, 6, 5, 4, 3, 2 };
            
            var tempCpf = cpf.Substring(0, 9);
            var soma = 0;
            
            for (int i = 0; i < 9; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador1[i];
            
            var resto = soma % 11;
            resto = resto < 2 ? 0 : 11 - resto;
            
            var digito = resto.ToString();
            tempCpf += digito;
            soma = 0;
            
            for (int i = 0; i < 10; i++)
                soma += int.Parse(tempCpf[i].ToString()) * multiplicador2[i];
            
            resto = soma % 11;
            resto = resto < 2 ? 0 : 11 - resto;
            digito += resto.ToString();
            
            return cpf.EndsWith(digito);
        }

        // Valida CNPJ
        public static bool IsValidCnpj(string cnpj)
        {
            if (string.IsNullOrWhiteSpace(cnpj)) return false;
            
            cnpj = Regex.Replace(cnpj, @"[^\d]", "");
            
            if (cnpj.Length != 14) return false;
            
            // Verifica se todos os dígitos são iguais
            if (cnpj.All(c => c == cnpj[0])) return false;
            
            // Validação do CNPJ
            var multiplicador1 = new int[12] { 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            var multiplicador2 = new int[13] { 6, 5, 4, 3, 2, 9, 8, 7, 6, 5, 4, 3, 2 };
            
            var tempCnpj = cnpj.Substring(0, 12);
            var soma = 0;
            
            for (int i = 0; i < 12; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador1[i];
            
            var resto = soma % 11;
            resto = resto < 2 ? 0 : 11 - resto;
            
            var digito = resto.ToString();
            tempCnpj += digito;
            soma = 0;
            
            for (int i = 0; i < 13; i++)
                soma += int.Parse(tempCnpj[i].ToString()) * multiplicador2[i];
            
            resto = soma % 11;
            resto = resto < 2 ? 0 : 11 - resto;
            digito += resto.ToString();
            
            return cnpj.EndsWith(digito);
        }

        // Formata telefone
        public static string FormatPhone(string phone)
        {
            if (string.IsNullOrWhiteSpace(phone)) return "";
            
            var digits = Regex.Replace(phone, @"[^\d]", "");
            
            if (digits.Length == 10)
                return $"({digits.Substring(0, 2)}) {digits.Substring(2, 4)}-{digits.Substring(6, 4)}";
            
            if (digits.Length == 11)
                return $"({digits.Substring(0, 2)}) {digits.Substring(2, 5)}-{digits.Substring(7, 4)}";
            
            return phone;
        }

        // Formata CPF
        public static string FormatCpf(string cpf)
        {
            if (string.IsNullOrWhiteSpace(cpf)) return "";
            
            var digits = Regex.Replace(cpf, @"[^\d]", "");
            
            if (digits.Length == 11)
                return $"{digits.Substring(0, 3)}.{digits.Substring(3, 3)}.{digits.Substring(6, 3)}-{digits.Substring(9, 2)}";
            
            return cpf;
        }

        // Formata CNPJ
        public static string FormatCnpj(string cnpj)
        {
            if (string.IsNullOrWhiteSpace(cnpj)) return "";
            
            var digits = Regex.Replace(cnpj, @"[^\d]", "");
            
            if (digits.Length == 14)
                return $"{digits.Substring(0, 2)}.{digits.Substring(2, 3)}.{digits.Substring(5, 3)}/{digits.Substring(8, 4)}-{digits.Substring(12, 2)}";
            
            return cnpj;
        }

        // Valida CEP
        public static bool IsValidCep(string cep)
        {
            if (string.IsNullOrWhiteSpace(cep)) return false;
            
            var digits = Regex.Replace(cep, @"[^\d]", "");
            return digits.Length == 8;
        }

        // Formata CEP
        public static string FormatCep(string cep)
        {
            if (string.IsNullOrWhiteSpace(cep)) return "";
            
            var digits = Regex.Replace(cep, @"[^\d]", "");
            
            if (digits.Length == 8)
                return $"{digits.Substring(0, 5)}-{digits.Substring(5, 3)}";
            
            return cep;
        }
    }
}
