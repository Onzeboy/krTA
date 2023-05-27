using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static lb1TA.Token;
namespace lb1TA
{
    public class TokenH
    {
        public static List<Token> Tokenhnd(string ss, System.Windows.Forms.RichTextBox writebox)
        {
            List<Token> tokens = new List<Token>();
            List<string> listBuf = new List<string>();
            List<string> forToken = new List<string>();
            List<char> forChar = new List<char>();
            int i = 0;
            ss += ' ';
            string subText = "";
            foreach (char s in ss)
            {
                if (Lexems.IsOperator(subText) && (s == ' ' || s == '<' || s == '>' || s == ';' || s == '+' || s == '-' || s == '*' || s == '/' || s == ',' || s == ':' || s == '.'))
                {
                    listBuf.Add(subText + " ");
                    forToken.Add("I");
                    forChar.Add(' ');
                    subText = "";
                }
                else if (Lexems.IsLiteral(subText) && (s == ' ' || s == ';' || s == ')' || s == ':' || s == '.' || s == '+' || s == '-' || s == '*' || s == '/'))
                {
                    listBuf.Add(subText + " ");
                    forToken.Add("D");
                    forChar.Add(' ');
                    subText = "";
                }
                else if (Lexems.IsSeparator(subText) && (s == ' ' || s == '(' || char.IsDigit(s) || char.IsLetter(s)))
                {
                    listBuf.Add(subText + " ");
                    forToken.Add("R");
                    forChar.Add(' ');
                    subText = "";
                }
                else if (Lexems.IsIDVariable(subText) && !Lexems.IsOperator(subText) && (s == ' ' || s == '<' || s == '>' || s == ';' || s == '+' || s == '-' || s == '*' || s == '/' || s == ',' || s == ':' || s == '.'))
                {
                    listBuf.Add(subText + " ");
                    forToken.Add("P");
                    forChar.Add(' ');
                    subText = "";
                }
                else if (subText == Environment.NewLine || subText == " " )
                {
                    subText = "";
                }
                else if (s == '@' || s == '<' || s == '>' || s == '&' || s == '^' || s == '%' || s == '№' || s == '#' || s == '@' || s == '"')
                {
                    throw new Exception($"Разделитель {s} не поддерживается подмножеством");
                }
                else if (Lexems.IsLiteral(subText) && char.IsLetter(s))
                {
                    throw new Exception($"Cимвол {subText + s} не является литералом");
                }
                    subText += s;
            }
            string str;
            string type;
            Token token;
            for (i = 0; i < listBuf.Count; i++)
            {
                str = listBuf[i].Split()[0];
                type = forToken[i];
                if (type == "I")
                {
                    if (Token.IsSpecialWord(str))
                    {
                        token = new Token(Token.SpecialWords[str]);
                        token.srt = str;
                        tokens.Add(token);
                        continue;
                    }
                    else
                    {
                        token = new Token(TokenType.IDENTIFIER);
                        token.Value = str;
                        token.srt = str;
                        tokens.Add(token);
                        continue;
                    }
                }
                else if (type == "D")
                {
                    token = new Token(TokenType.LITERAL);
                    token.Value = str;
                    token.srt = str;
                    tokens.Add(token);
                    continue;
                }
                else if (type == "P")
                {
                    token = new Token(TokenType.VARIABLE);
                    token.Value = str;
                    token.srt = str;
                    tokens.Add(token);
                    continue;
                }
                else if (type == "R")
                {
                        if (Token.IsSpecialSymbol(str))
                        {
                            token = new Token(Token.SpecialSymbols[str]);
                            token.srt = str;
                            tokens.Add(token);
                            continue;
                        }
                }
            }
            foreach (Token lexem in tokens)
            {
                writebox.Text += lexem + " " + Environment.NewLine;
            }
            return tokens;
        }
    }
}
       