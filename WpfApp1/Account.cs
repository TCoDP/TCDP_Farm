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
        private string rank, nickname;
        private int timestamp, steamid64, lvl;
        private bool online;
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
        public bool Online
        {
            get { return online; }
            set { online = value; }
        }
        public int Timestamp
        {
            get { return timestamp; }
            set { timestamp = value; }
        }
        public int Steamid64
        {
            get { return steamid64; }
            set { steamid64 = value; }
        }
        public string Rank
        {
            get { return rank; }
            set { rank = value; }
        }
        public int Lvl
        {
            get { return lvl; }
            set { lvl = value; }
        }
        public string Nickname
        {
            get { return nickname; }
            set { nickname = value; }
        }

        public Account() { }

        public Account(
            string login,
            string password,
            bool online,
            int timestamp,
            int steamid64,
            string rank,
            int lvl,
            string nickname)
        {
            this.login = login;
            this.password = password;
            this.online = online;
            this.timestamp = timestamp;
            this.steamid64 = steamid64;
            this.rank = rank;
            this.lvl = lvl;
            this.nickname = nickname;
        }
    }
}
