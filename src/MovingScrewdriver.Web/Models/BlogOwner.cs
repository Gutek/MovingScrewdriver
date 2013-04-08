using System;

namespace MovingScrewdriver.Web.Models
{
    public class BlogOwner
    {
        const string ConstantSalt = "QxLUF1bgIAdeQX#";

        public string Id { get; set; }
        public string Email { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get { return "{0} {1}".FormatWith(FirstName, LastName); } }
        public string Nick { get; set; }
        public string Twitter { get; set; }

        protected string HashedPassword { get; private set; }
        private string _passwordSalt;
        private string PasswordSalt
        {
            get
            {
                return _passwordSalt ?? (_passwordSalt = BCrypt.Net.BCrypt.GenerateSalt(16));
            }
            set { _passwordSalt = value; }
        }


        public BlogOwner ResetPassword(string pwd)
        {
            HashedPassword = HashPassword(pwd);
            return this;
        }

        private string HashPassword(string pwd)
        {
            pwd += ConstantSalt;
            var password = BCrypt.Net.BCrypt.HashPassword(pwd, PasswordSalt);
            return password;
        }

        public bool ValidatePassword(string pwd)
        {
            if (HashedPassword == null)
            {
                return true;
            }
                
            return HashedPassword == HashPassword(pwd);
        }
    }
}