using System;

namespace AssessmentTests
{
    class Program
    {

        static void run(string[] args)
        {
            string strnew = "kazak";
            Palindrome palidrome = new Palindrome();
            Console.WriteLine(palidrome.isPalindrome(strnew));

            //
            string browser = System.Configuration.ConfigurationManager.AppSettings["browser"];
            Console.WriteLine("Start using " + browser);
            if (browser == "chrome")
            {
                //todo
            }
            if (browser == "firefox")
            {
                //todo
            }
        }
    }
}

