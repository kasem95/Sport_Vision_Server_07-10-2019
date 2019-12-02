using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web;

namespace Soccer_Vision.Models
{
    public class User
    {
        private string email;
        private string password;
        public int UserID { get; set; }
        public string Username { get; set; }
        public string PhotoName { get; set; }
        public string Email
        {
            get
            {
                return email;
            }
            set
            {
                if (IsValid(value))
                    email = value;
                else
                    email = "";
            }
        }
        public string Password
        {
            get
            {
                return password;
            }
            set
            {
                if (IsValidPassword(value))
                    password = value;
                else
                    password = "";
            }
        }

        private bool IsValidPassword(string input)
        {
            Match match = Regex.Match(input, @"(?=^.{8,12}$)((?=.*\d)(?=.*[A-Z])(?=.*[a-z])|(?=.*\d)(?=.*[^A-Za-z0-9])(?=.*[a-z])|(?=.*[^A-Za-z0-9])(?=.*[A-Z])(?=.*[a-z])|(?=.*\d)(?=.*[A-Z])(?=.*[^A-Za-z0-9]))^.*");

            if (match.Success && match.Index == 0 && match.Length == input.Length)
                return true;
            else
                return false;

        }

        private bool IsValid(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);

                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }
    }
}