using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1
{
    class Account
    {
        public int id { get; set; }
        private string login, password;
        public string Login
        {
            get { return login; }
            set { login = value; }
        }
        public string Password
        {
            get { return password; }
            set { password = value; }
        }

        public Account() { }

        public Account(string login, string password)
        {
            this.login = login;
            this.password = password;
        }
    }
}
