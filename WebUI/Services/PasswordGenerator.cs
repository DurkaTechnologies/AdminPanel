using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebUI.Services
{
    public static class PasswordGenerator
    {
        public static string Generate(int length = 0) 
        {
            string symbols = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789~!@#$%^&*";

            if (length == 0)
                length = 10;

            string password = "";
            Random random = new Random();

            for (int i = 0; i < length; i++)
                password += symbols[random.Next(0, symbols.Length - 1)];

            return password;
        }
    }
}
