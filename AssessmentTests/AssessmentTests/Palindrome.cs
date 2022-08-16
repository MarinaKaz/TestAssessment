using System;
using System.Collections.Generic;
using System.Text;

namespace AssessmentTests
{
    class Palindrome
    {
        public bool isPalindrome(string str)
        {
            string reverse = "";
            for (int i = str.Length - 1; i >= 0; i--)
            {
                reverse += str[i];
            }
            return reverse.ToString() == str;

        }

    }
}
