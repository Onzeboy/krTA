using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lb1TA
{
    internal class LL
    {
        List<Token> token;
        public bool Succes = false;
        int i;
        public LL(List<Token> tokens)
        {
            this.token = tokens;
        }
        public void Start()
        {
            i = 0;
            try
            {
                Programm();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: #{ex.Message}");
                MessageBox.Show($"Errror! #{ex.Message}");
            }
        }
        public void Programm()
        {
            Succes = false;
            if (token[i].Type != Token.TokenType.VAR)
                throw new Exception($"1\nSTRING: {i + 1} - Ожидалось: VAR, а получено: {token[i].Type}");
            Next();
            SpisOpis();
            if (token[i].Type != Token.TokenType.BEGIN)
                throw new Exception($"2\nSTRING: {i + 1} - Ожидалось: BEGIN, а получено: {token[i].Type}");
            Next();
            SpisOper();
            if (token[i].Type != Token.TokenType.END)
                throw new Exception($"4\nSTRING: {i + 1} - Ожидалось: END, а получено: {token[i].Type}");
            Next();

            if (token[i].Type != Token.TokenType.POINT)
                throw new Exception($"5\nSTRING: {i + 1} - Ожидалось: POINT, а получено: {token[i].Type}");
            Succes = true;
        }

        public void SpisOpis()
        {
            if (token[i].Type != Token.TokenType.VARIABLE)
                throw new Exception($"6\nSTRING: {i + 1} - Ожидалось: VARIABLE, а получено: {token[i].Type}");
            Pref3();
            DopOpis();
        }
        public void Pref3()
        {
            SpisPerem();
            if (token[i].Type != Token.TokenType.COLON)
                throw new Exception($"7\nSTRING: {i + 1} - Ожидалось: COLON, а получено: {token[i].Type}");
            Next();
            Type();
            if (token[i].Type != Token.TokenType.SEMICOLON)
                throw new Exception($"8\nSTRING: {i + 1} - Ожидалось: SEMICOLON, а получено: {token[i].Type}");
            Next();
        }

        public void DopOpis()
        {
            switch (token[i].Type)
            {
                case Token.TokenType.BEGIN:
                    break;

                case Token.TokenType.VARIABLE:
                    SpisOpis();
                    break;

                default: throw new Exception($"9\nSTRING: {i + 1} - Ожидалось: VARIABLE, а получено: {token[i].Type}");
            }
        }

        public void SpisOper()
        {
            if (token[i].Type != Token.TokenType.VARIABLE && token[i].Type != Token.TokenType.CASE)
                throw new Exception($"10\nSTRING: {i + 1} - Ожидалось: CASE или VARIABLE (ID), а получено: {token[i].Type}");
            Oper();
            Pref2();
        }
        void Pref2()
        {
            switch (token[i].Type)
            {
                case Token.TokenType.VARIABLE:
                    DopOper();
                    break;
                case Token.TokenType.CASE:
                    DopOper();
                    break;
                default:
                    break;
            }
        }

        public void SpisPerem()
        {
            if (token[i].Type != Token.TokenType.VARIABLE)
                throw new Exception($"11\nSTRING: {i + 1} - Ожидалось: VARIABLE (ID), а получено: {token[i].Type}");
            Next();

            Pref1();
        }

        public void Pref1()
        {
            switch (token[i].Type)
            {
                case Token.TokenType.COLON:
                    break;

                case Token.TokenType.COMMA:
                    DopPerem();
                    break;

                default: throw new Exception($"12\nSTRING: {i + 1} - Ожидалось: COLON или COMMA а получено: {token[i].Type}");
            }
        }

        public void DopPerem()
        {
            if (token[i].Type != Token.TokenType.COMMA)
                throw new Exception($"13\nSTRING: {i + 1} - Ожидалось: COMMA, а получено: {token[i].Type}");
            Next();

            if (token[i].Type != Token.TokenType.VARIABLE)
                throw new Exception($"14\nSTRING: {i + 1} - Ожидалось: VARIABLE, а получено: {token[i].Type}");
            Next();

            Pref1();
        }

        public void Oper()
        {
            switch (token[i].Type)
            {
                case Token.TokenType.CASE:
                    Usl();
                    if (token[i].Type != Token.TokenType.SEMICOLON)
                        throw new Exception($"18\nSTRING: {i + 1} - Ожидалось: SEMICOLON, а получено: {token[i].Type}");
                    Next();
                    break;
                case Token.TokenType.VARIABLE:
                    Prisv();
                    if (token[i].Type != Token.TokenType.SEMICOLON)
                        throw new Exception($"18\nSTRING: {i + 1} - Ожидалось: SEMICOLON, а получено: {token[i].Type}");
                    Next();
                    break;
                default: throw new Exception($"15\nSTRING: {i + 1} - Ожидалось: CASE или VARIABLE (ID), а получено: {token[i].Type}");
            }
        }

        public void DopOper()
        {
            if (token[i].Type != Token.TokenType.VARIABLE && token[i].Type != Token.TokenType.CASE)
                throw new Exception($"10\nSTRING: {i + 1} - Ожидалось: CASE или VARIABLE (ID), а получено: {token[i].Type}");
            Oper();

            Pref2();
        }
        public void Usl()
        {
            if (token[i].Type != Token.TokenType.CASE)
                throw new Exception($"17\nSTRING: {i + 1} - Ожидалось: CASE, а получено: {token[i].Type}");
            Next();
            if (token[i].Type != Token.TokenType.VARIABLE)
                throw new Exception($"18\nSTRING: {i + 1} - Ожидалось: ID, а получено: {token[i].Type}");
            Next();
            if (token[i].Type != Token.TokenType.OF)
                throw new Exception($"19\nSTRING: {i + 1} - Ожидалось: OF, а получено: {token[i].Type}");
            Next();
            BlockOper();
            if (token[i].Type != Token.TokenType.ELSE)
                throw new Exception($"19\nSTRING: {i + 1} - Ожидалось: ELSE, а получено: {token[i].Type}");
            Next();
            BlockOper();
            if (token[i].Type != Token.TokenType.END)
                throw new Exception($"19\nSTRING: {i + 1} - Ожидалось: END, а получено: {token[i].Type}");
            Next();
            if (token[i].Type != Token.TokenType.SEMICOLON)
                throw new Exception($"19\nSTRING: {i + 1} - Ожидалось: SEMICOLON, а получено: {token[i].Type}");
        }

        public void BlockOper()
        {

            if (token[i].Type == Token.TokenType.VARIABLE || token[i].Type == Token.TokenType.CASE)
            {
                Oper();
                Pref4();
            }

            else if (token[i].Type == Token.TokenType.BEGIN)
            {
                Next();
                SpisOper();

                if (token[i].Type != Token.TokenType.END)
                    throw new Exception($"20\nSTRING: {i + 1} - Ожидалось: END, а получено: {token[i].Type}");
                Next();
                if (token[i].Type != Token.TokenType.SEMICOLON)
                    throw new Exception($"20\nSTRING: {i + 1} - Ожидалось: SEMICOLON, а получено: {token[i].Type}");
                Next();

            }
            else if (token[i].Type == Token.TokenType.LITERAL)
            {
                Next();
                Pref5();
            }
            else throw new Exception($"20\nSTRING: {i + 1} - Ожидалось: LITERAL, а получено: {token[i].Type}");
        }
        void Pref4()
        {
            switch (token[i].Type)
            {
                case Token.TokenType.LITERAL:
                    BlockOper();
                    break;
                case Token.TokenType.CASE:
                    BlockOper();
                    break;
                case Token.TokenType.BEGIN:
                    BlockOper();
                    break;
                case Token.TokenType.VARIABLE:
                    BlockOper();
                    break;
                default:
                    break;
            }
        }
        public void Pref5()
        {
            if (token[i].Type == Token.TokenType.COLON)
            {
                Next();
                Pref6();
            }
            if (token[i].Type == Token.TokenType.DOUBLEPOINT)
            {
                Next();
            }
            if (token[i].Type == Token.TokenType.LITERAL)
            {
                Next();
                if (token[i].Type == Token.TokenType.COLON)
                {
                    Next();
                    BlockOper();
                }
            }
        }
        void Pref6()
        {
            if (token[i].Type == Token.TokenType.BEGIN)
            {
                Next();
                SpisOper();
                Next();
            }
            if (token[i].Type == Token.TokenType.VARIABLE | token[i].Type == Token.TokenType.CASE)
            {
                Oper();
                Pref4();
            }
        }

        public void Prisv()
        {
            if (token[i].Type != Token.TokenType.VARIABLE)
                throw new Exception($"21\nSTRING: {i + 1} - Ожидалось: VARIABLE, а получено: {token[i].Type}");
            Next();
            if (token[i].Type != Token.TokenType.ASSIGNMENT)
                throw new Exception($"23\nSTRING: {i + 1} - Ожидалось: ASSIGMENT, а получено: {token[i].Type}");
            Next();
            Expr();
            if (token[i].Type != Token.TokenType.SEMICOLON)
                throw new Exception($"23\nSTRING: {i + 1} - Ожидалось: SEMICOLON, а получено: {token[i].Type}");
        }

        public void Expr()
        {
            while (token[i].Type != Token.TokenType.SEMICOLON)
            {
                Next();
            }
        }

        public void DopOper2()
        {
            if (token[i].Type != Token.TokenType.VARIABLE && token[i].Type != Token.TokenType.CASE)
                throw new Exception($"24\nSTRING: {i + 1} - Ожидалось: VARIABLE или CASE, а получено: {token[i].Type}");
            SpisOper();
        }

        public void Type()
        {
            if (token[i].Type != Token.TokenType.INTEGER
            && token[i].Type != Token.TokenType.BOOLEAN
            && token[i].Type != Token.TokenType.DOUBLE)
                throw new Exception($"27\nSTRING: {i + 1} - Ожидалось: INTEGER, REAL или DOUBLE, а получено: {token[i].Type}");
            Next();
        }
        public void Operand()
        {
            if (token[i].Type != Token.TokenType.VARIABLE && token[i].Type != Token.TokenType.NUMBER)
                throw new Exception($"28\nSTRING: {i + 1} - Ожидалось: VARIABLE или NUMBER, а получено: {token[i].Type}");
            Next();
        }
        public void Sign()
        {
            if (token[i].Type != Token.TokenType.PLUS &&
            token[i].Type != Token.TokenType.MINUS &&
            token[i].Type != Token.TokenType.MULTIPLY &&
            token[i].Type != Token.TokenType.MORE &&
            token[i].Type != Token.TokenType.LESS &&
            token[i].Type != Token.TokenType.DIVISION)
                throw new Exception($"29\nSTRING: {i + 1} - Ожидалось: SIGN, а получено: {token[i].Type}");
            Next();
        }
        public void Next()
        {
            if (i < token.Count - 1)
            {
                i++;
            }
        }
    }
}