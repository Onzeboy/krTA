using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lb1TA
{
    public class Lexems
    {
        public static bool IsOperator(string text)
        {
            return text == "main"||text == "program"|| text == "var"|| text == "integer"|| text == "double"|| text == "boolean"|| text == "begin"|| text == "end"|| text == "if"|| text == "then"|| text == "else"|| text == "while"|| text == "do"|| text == "read"|| text == "write"|| text == "true"|| text == "false" || text == "case" || text == "of";
        }

        public static bool IsSeparator(string text)
        {
            return text == "<" || text == ">" || text == "," || text == ":" || text == ":=" || text == "==" || text == "=" || text == "+" || text == "-" || text == "*" || text == "/" || text == ";" || text == "(" || text == ")" || text == ".." || text == ".";
        }
            public static bool IsLiteral(string text)
        {
            int x;
            return int.TryParse(text, out x);
        }
        public static bool IsErrorLetter(string text)
        {
            return text == "@" || text == "$" || text == "[" || text == "]" || text == "`" || text == "~";
        }

        public static bool IsIDVariable(string text)
        {
            bool Flag = true;

            if (text.Length == 0 || !char.IsLetter(text[0]))
                Flag = false;
            else
                foreach (char s in text)
                {
                    if (s != '_' && !char.IsDigit(s) && !char.IsLetter(s))
                        Flag = false;
                }
            return Flag;
        }

    }
}
