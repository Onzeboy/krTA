using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lb1TA
{
    public class Handler
    {
        public static void CAS(string ss, System.Windows.Forms.RichTextBox writebox)
        {
            List<string> lexemes = new List<string>();
            lexemes = new List<string>();
            string sText = "";
            ss += ' '; 
            foreach (char s in ss)
            {
                if (Lexems.IsIDVariable(sText) && (s == ' ' || s == '<' || s == '>' || s == ';' || s == '+' || s == '-' || s == '*' || s == '/' || s == ',' || s == ':' || s == '.'))
                {
                    lexemes.Add(sText + " - Идентификатор;");
                    sText = "";
                }
                else if (Lexems.IsSeparator(sText) && (s == ' ' || s == '(' || char.IsDigit(s) || char.IsLetter(s)))
                {
                    lexemes.Add(sText + " - Разделитель;");
                    sText = "";
                }
                else if (Lexems.IsLiteral(sText) && (s == ' ' || s == ';' || s == ')' || s == ':' || s == '.' || s == '+' || s == '-' || s == '*' || s == '/'))
                { 
                    lexemes.Add(sText + " - Литератор;");
                    sText = "";
                }
                else if (sText == Environment.NewLine || sText == " " || sText == "\n")
                {
                    sText = "";
                }
                else if (s == '@' || s == '<' || s == '>' || s == '&' || s == '^' || s == '%' || s == '№' || s == '#' || s == '@' || s == '"')
                {
                    throw new Exception($"Разделитель {s} не поддерживается подмножеством");
                }
                else if (Lexems.IsLiteral(sText) && char.IsLetter(s))
                {
                    throw new Exception($"Cимвол {sText + s} не является литералом");
                }
                sText += s;
            }
            foreach (string lexem in lexemes)
            {
                writebox.Text += lexem + " " + Environment.NewLine;
            }
        }
    }
}