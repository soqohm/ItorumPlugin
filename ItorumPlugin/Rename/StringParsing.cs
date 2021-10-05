using System;

namespace Itorum
{
    public static class StringParsing
    {
        public static string[] SplitS(this string s)
        {
            return s.Split(new string[] { " _ " }, StringSplitOptions.None);
        }

        public static string DeleteBrackets(this string s)
        {
            var count = 0;
            for (var i = s.Length - 1; i > 0; i--)
            {
                if (s[i] == ')') count++;
                if (s[i] == '(')
                {
                    if (count == 1) return s.Remove(i);
                    else count--;
                }
            }
            return s;
        }

        public static string DeleteRevision(this string s)
        {
            var x = s.Length;
            if (s[x - 2] == '0' && s[x - 3] == '_')
                return s.Remove(x - 3);
            else return s;
        }

        public static string DeletePrefix(this string s)
        {
            if (s[0] == 'S') return s.Substring(1);
            else if (s[1] == 'S') return s.Substring(2);
            else return s;
        }

        public static bool WithS(this string s)
        {
            return s[0] == 'S' || s[1] == 'S';
        }

        public static string Correct(this string s)
        {
            var e = s.Split(new string[] { @"\" }, StringSplitOptions.None);
            return e[e.Length - 1];
        }
    }
}
