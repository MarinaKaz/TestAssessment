using System;
using System.Collections.Generic;
using System.Text;

namespace AssessmentTests
{
    class Palindrome
    {
        public string isPalindrome(string str)
        {
            string reverse = "";
            for (int i = str.Length - 1; i >= 0; i--)
            {
                reverse += str[i];
            }
            if (reverse.ToString() == str)
            {
                return "True";
            }
            else
            {
                return "False";
            }
        }

    }
}
