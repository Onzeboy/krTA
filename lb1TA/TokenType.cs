using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lb1TA
{
    public class Token
    {
        public enum TokenType
        {
            VAR, INTEGER, DO, LITERAL, NUMBER, IDENTIFIER, BEGIN, END, CASE, OF, ELSE,
            TO, PLUS,
            MINUS, EQUAL, MORE, LESS, TWOEQUAL, SEMICOLON, MULTIPLY, COMMA, DIVISION, POINT, COLON,
            VARIABLE, DOUBLEPOINT, ASSIGNMENT, DOUBLE, BOOLEAN, NETERM, EXPR, LIT, LPAR, RPAR
        }
        public TokenType Type;
        public string Value;
        public string srt;
        public Token(TokenType type)
        {
            Type = type;
        }
        public override string ToString()
        {
            return string.Format("{0}, {1}", Type, Value);
        }
        public static TokenType[] Delimiters = new TokenType[]
        {
             TokenType.PLUS, TokenType.MINUS,
             TokenType.EQUAL, TokenType.MORE, TokenType.LESS,
             TokenType.TWOEQUAL, TokenType.SEMICOLON, TokenType.MULTIPLY,
             TokenType.COMMA,TokenType.DIVISION, TokenType.POINT, TokenType.COLON, TokenType.LPAR, TokenType.RPAR
        };
        public static bool IsDelimiter(Token token)
        {
            return Delimiters.Contains(token.Type);
        }
        public static Dictionary<string, TokenType> SpecialWords = new Dictionary<string, TokenType>()
        {
             { "case", TokenType.CASE },
             { "of", TokenType.OF },
             { "else", TokenType.ELSE },
             { "integer", TokenType.INTEGER },
             { "boolean", TokenType.BOOLEAN },
             { "double", TokenType.DOUBLE },
             { "var", TokenType.VAR },
             { "begin", TokenType.BEGIN },
             { "end", TokenType.END },
             { "lit", TokenType.LIT }
         };
        public static bool IsSpecialWord(string word)
        {
            if (string.IsNullOrEmpty(word))
            {
                return false;

            }
            return SpecialWords.ContainsKey(word);
        }
        public static Dictionary<string, TokenType> SpecialSymbols = new Dictionary<string, TokenType>()
             {
             { "+", TokenType.PLUS },
             { "-", TokenType.MINUS },
             { "=", TokenType.EQUAL },
             { ":=", TokenType.ASSIGNMENT },
             { "*", TokenType.MULTIPLY },
             { "/", TokenType.DIVISION },
             { ",", TokenType.COMMA },
             { ":", TokenType.COLON },
             { ".", TokenType.POINT },
             { "..", TokenType.DOUBLEPOINT },
             { ";", TokenType.SEMICOLON },
             { "(", TokenType.LPAR },
             { ")", TokenType.RPAR },
             };
        public static bool IsSpecialSymbol(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return false;
            }
            return SpecialSymbols.ContainsKey(str);
        }
    }
}