using System;
using System.Collections.Generic;
using static lb1TA.Token;

namespace lb1TA
{
    public class LR
    {
        List<Token> tokens = new List<Token>();
        Stack<Token> lexemStack = new Stack<Token>();
        Stack<int> stateStack = new Stack<int>();
        int nextLex = 0;
        int state = 0;
        bool isEnd = false;
        public LR(List<Token> vvodtoken)
        {
            tokens = vvodtoken;
        }
        private Token GetLexeme(int nextLex)
        {
            return tokens[nextLex];
        }

        private void Shift()
        {
            lexemStack.Push(GetLexeme(nextLex));
            nextLex++;
        }
        private void GoToState(int state)
        {
            stateStack.Push(state);
            this.state = state;
        }
        private void Reduce(int num, string neterm)
        {
            for (int i = 0; i < num; i++)
            {
                lexemStack.Pop();
                stateStack.Pop();
            }
            state = stateStack.Peek();
            Token k = new Token(TokenType.NETERM);
            k.Value = neterm;
            lexemStack.Push(k);
        }
        private void Expression()
        {
            Expression expr = new Expression();
            while (GetLexeme(nextLex).Type != TokenType.SEMICOLON)
            {
                expr.TakeToken(GetLexeme(nextLex));
                Shift();
            }
            Token k = new Token(TokenType.EXPR);
            lexemStack.Push(k);
            expr.Start();
        }
        void State0()
        {
            if (lexemStack.Count == 0)
                Shift();
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<программа>":
                            if (nextLex == tokens.Count)
                                isEnd = true;
                            break;
                        default:
                            throw new Exception($"State 0\n Ожидалось: <программа>, а получено: {lexemStack.Peek().srt}");
                    }
                    break;
                case TokenType.VAR:
                    GoToState(1);
                    break;
                default:
                    throw new Exception($"State 0\n Ожидалось: VAR, а получено: {lexemStack.Peek().srt}");
                    break;
            }
        }
        void State1()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<спис_опис>":
                            GoToState(6);
                            break;
                        case "<опис>":
                            GoToState(3);
                            break;
                        case "<спис_перем>":
                            GoToState(4);
                            break;
                        default:
                            throw new Exception();
                    }
                    break;
                case TokenType.VARIABLE:
                    GoToState(5);
                    break;
                case TokenType.VAR:
                    Shift();
                    break;
                default:
                    throw new Exception($"State 1\nОжидалось: Переменная или VAR, а получено: { lexemStack.Peek().srt}");
            }
        }
        void State2()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    if (lexemStack.Peek().Value == "<спис_опис>")
                        Shift();
                    else
                        throw new Exception($"State 2\nОжидалось: Переменная или VAR, а получено: {lexemStack.Peek().srt}");
                    break;
                case TokenType.SEMICOLON:
                    GoToState(6);
                    break;
                default:
                    throw new Exception($"State 2\nОжидалось: Точка с запятой(;) , а получено: { lexemStack.Peek().srt}");
            }
        }
        void State3()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<опис>":
                            Shift();
                            break;
                        default:
                            throw new Exception();
                            break;
                    }
                    break;
                case TokenType.SEMICOLON:
                    GoToState(7);
                    break;
                default:
                    throw new Exception($"State 3\nОжидалось: Точка с запятой(;) , а получено: { lexemStack.Peek().srt}");
                    break;
            }
        }
        void State4()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    if (lexemStack.Peek().Value == "<спис_перем>")
                        Shift();
                    else
                        throw new Exception();
                    break;
                case TokenType.COLON:
                    GoToState(10);
                    break;
                default:
                    throw new Exception($"State 4\nОжидалось: Двоеточие(:) , а получено: {lexemStack.Peek().srt}");
                    break;
            }
        }
        void State5()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.VARIABLE:
                    if (GetLexeme(nextLex).Type == TokenType.COMMA)
                    {
                        Shift();
                        break;
                    }
                    else
                    {
                        Reduce(1, "<спис_перем>");
                        break;
                    }
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<префикс1>":
                            GoToState(11);
                            break;
                        case "<доп_перем>":
                            GoToState(12);
                            break;
                        default:
                            throw new Exception($"State 5\nОжидалось: Двоеточие(:) , а получено: {lexemStack.Peek().srt}");
                            break;
                    }
                    break;
                case TokenType.COMMA:
                    GoToState(13);
                    break;
                default:
                    throw new Exception($"State 5\nОжидалось: Запятая(,) , а получено: {lexemStack.Peek().srt}");
                    break;
            }
        }
        void State6()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<спис_опис>":
                            Shift();
                            break;
                        default:
                            throw new Exception();
                            break;
                    }
                    break;
                case TokenType.SEMICOLON:
                    Shift();
                    break;
                case TokenType.BEGIN:
                    GoToState(14);
                    break;
                default:
                    throw new Exception($"State 6\nОжидалось: BEGIN или Точка с запятой(;) , а получено: {lexemStack.Peek().srt}");
                    break;
            }
        }
        void State7()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.SEMICOLON:
                    if (GetLexeme(nextLex).Type == TokenType.VARIABLE)
                    {
                        Shift();
                        break;
                    }
                    else
                    {
                        Reduce(2, "<спис_опис>");
                        break;
                    }
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<префикс3>":
                            GoToState(71);
                            break;
                        case "<доп_опис>":
                            GoToState(8);
                            break;
                        case "<опис>":
                            GoToState(9);
                            break;
                        case "<спис_перем>":
                            GoToState(4);
                            break;
                        default:
                            throw new Exception();
                            break;
                    }
                    break;
                case TokenType.VARIABLE:
                    GoToState(5);
                    break;
                default:
                    throw new Exception($"State 7\nОжидалось: Переменная, а получено: {lexemStack.Peek().srt}");
                    break;
            }
        }
        void State71()
        {
            if (lexemStack.Peek().Type == TokenType.NETERM && lexemStack.Peek().Value == "<префикс3>")
                Reduce(3, "<спис_опис>");
            else
                throw new Exception();
        }
        void State8()
        {
            if (lexemStack.Peek().Type == TokenType.NETERM && lexemStack.Peek().Value == "<доп_опис>")
                Reduce(1, "<префикс3>");
            else
                throw new Exception();
        }
        void State9()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.SEMICOLON:
                    GoToState(15);
                    break;
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<опис>":
                            Shift();
                            break;
                    }
                    break;
                default:
                    throw new Exception($"State 9\nОжидалось: Точка с запятой(;) , а получено: {lexemStack.Peek().srt}");
                    break;
            }
        }
        void State10()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.COLON:
                    Shift();
                    break;
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<тип>":
                            GoToState(16);
                            break;
                    }
                    break;
                case TokenType.INTEGER:
                    GoToState(17);
                    break;
                case TokenType.BOOLEAN:
                    GoToState(18);
                    break;
                case TokenType.DOUBLE:
                    GoToState(19);
                    break;
                default:
                    throw new Exception($"State 10\nОжидалось: INTEGER, BOOLEAN, DOUBLE или Запятая(,), а получено: {lexemStack.Peek().srt}");
                    break;
            }
        }
        void State11()
        {
            if (lexemStack.Peek().Type == TokenType.NETERM && lexemStack.Peek().Value == "<префикс1>")
                Reduce(2, "<спис_перем>");
            else
                throw new Exception();
        }
        void State12()
        {
            if (lexemStack.Peek().Type == TokenType.NETERM && lexemStack.Peek().Value == "<доп_перем>")
                Reduce(1, "<префикс1>");
            else
                throw new Exception();
        }
        void State13()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.COMMA:
                    Shift();
                    break;
                case TokenType.VARIABLE:
                    GoToState(20);
                    break;
                default:
                    throw new Exception($"State 13\nОжидалось: Запятая(,) или Переменная, а получено: {lexemStack.Peek().srt}");
                    break;
            }
        }
        void State14()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.BEGIN:
                    Shift();
                    break;
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<спис_опер>":
                            GoToState(21);
                            break;
                        case "<опер>":
                            GoToState(22);
                            break;
                        case "<условн.>":
                            GoToState(23);
                            break;
                        case "<присв.>":
                            GoToState(24);
                            break;
                        default:
                            throw new Exception();
                    }
                    break;
                case TokenType.CASE:
                    GoToState(25);
                    break;
                case TokenType.VARIABLE:
                    GoToState(25);
                    break;
                default:
                    throw new Exception($"State 14\nОжидалось: BEGIN, CASE или Переменная, а получено: {lexemStack.Peek().srt}");
                    break;
            }
        }
        void State15()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.SEMICOLON:
                    if (GetLexeme(nextLex).Type == TokenType.VARIABLE)
                    {
                        Shift();
                        break;
                    }
                    else
                    {
                        Reduce(2, "<доп_опис>");
                        break;
                    }
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<префикс3>":
                            GoToState(72);
                            break;
                        case "<доп_опис>":
                            GoToState(8);
                            break;
                        case "<опис>":
                            GoToState(9);
                            break;
                        case "<спис_перем>":
                            GoToState(4);
                            break;
                        default:
                            throw new Exception();
                    }
                    break;
                case TokenType.VARIABLE:
                    GoToState(5);
                    break;
                default:
                    throw new Exception($"State 15\nОжидалось: Точка с запятой(;) или Переменная, а получено: {lexemStack.Peek().srt}");
                    break;
            }
        }
        void State15_1()
        {
            if (lexemStack.Peek().Type == TokenType.NETERM && lexemStack.Peek().Value == "<префикс3>")
                Reduce(3, "<доп_опис>");
            else
                throw new Exception();
        }
        void State16()
        {
            if (lexemStack.Peek().Type == TokenType.NETERM && lexemStack.Peek().Value == "<тип>")
                Reduce(3, "<опис>");
            else
                throw new Exception();
        }
        void State17()
        {
            if (lexemStack.Peek().Type == TokenType.INTEGER)
                Reduce(1, "<тип>");
            else
                throw new Exception();
        }
        void State18()
        {
            if (lexemStack.Peek().Type == TokenType.BOOLEAN)
                Reduce(1, "<тип>");
            else
                throw new Exception();
        }
        void State19()
        {
            if (lexemStack.Peek().Type == TokenType.DOUBLE)
                Reduce(1, "<тип>");
            else
                throw new Exception();
        }
        void State20()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.VARIABLE:
                    if (GetLexeme(nextLex).Type == TokenType.COMMA)
                    {
                        Shift();
                        break;
                    }
                    else
                    {
                        Reduce(2, "<доп_перем>");
                        break;
                    }

                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<префикс1>":
                            GoToState(28);
                            break;
                        case "<доп_перем>":
                            GoToState(12);
                            break;
                        default:
                            throw new Exception();
                    }
                    break;
                case TokenType.COMMA:
                    GoToState(13);
                    break;
                default:
                    throw new Exception($"State 20\nОжидалось: Переменная или Запятая(,), а получено: {lexemStack.Peek().srt}");
                    break;
            }
        }
        void State21()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    if (lexemStack.Peek().Value == "<спис_опер>")
                        Shift();
                    else
                        throw new Exception();
                    break;
                case TokenType.END:
                    GoToState(29);
                    break;
                default:
                    throw new Exception($"State 21\nОжидалось: END, а получено: {lexemStack.Peek().srt}");
                    break;
            }
        }
        void State22()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<опер>":
                            Shift();
                            break;
                        default:
                            throw new Exception();
                    }
                    break;
                case TokenType.SEMICOLON:
                    GoToState(30);
                    break;
                default:
                    throw new Exception($"State 22\nОжидалось: Точка с запятой(;), а получено: {lexemStack.Peek().srt}");
            }
        }
        void State22_1()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.SEMICOLON:
                    if (GetLexeme(nextLex).Type == TokenType.BEGIN | GetLexeme(nextLex).Type == TokenType.LITERAL | GetLexeme(nextLex).Type == TokenType.VARIABLE | GetLexeme(nextLex).Type == TokenType.CASE)
                    {
                        Shift();
                        break;
                    }
                    else
                    {
                        Reduce(2, "<доп_опер>");
                        break;
                    }
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<префикс2>":
                            GoToState(30);
                            break;
                        case "<доп_опер>":
                            GoToState(31);
                            break;
                        case "<опер>":
                            GoToState(32);
                            break;
                        case "<условн.>":
                            GoToState(23);
                            break;
                        case "<присв.>":
                            GoToState(24);
                            break;
                        default:
                            throw new Exception();
                    }
                    break;
                case TokenType.CASE:
                    GoToState(25);
                    break;
                case TokenType.VARIABLE:
                    GoToState(26);
                    break;
                default:
                    throw new Exception($"State 22_1\nОжидалось: Точка с запятой(;), CASE или Переменная, а получено: {lexemStack.Peek().srt}");
            }
        }
        void State23()
        {
            if (lexemStack.Peek().Type == TokenType.NETERM && lexemStack.Peek().Value == "<условн.>")
                Reduce(1, "<опер>");
            else
                throw new Exception();
        }
        void State24()
        {
            if (lexemStack.Peek().Type == TokenType.NETERM && lexemStack.Peek().Value == "<присв.>")
                Reduce(1, "<опер>");
            else
                throw new Exception();
        }
        void State25()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.CASE:
                    Shift();
                    break;
                case TokenType.VARIABLE:
                    GoToState(33);
                    break;
                default:
                    throw new Exception($"State 25\nОжидалось: CASE или Переменная, а получено: {lexemStack.Peek().srt}");
            }
        }
        void State26()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.VARIABLE:
                    Shift();
                    break;
                case TokenType.ASSIGNMENT:
                    GoToState(34);
                    break;
                default:
                    throw new Exception($"State 26\nОжидалось: Переменная или Символ присваивания(:=), а получено: {lexemStack.Peek().srt}");
            }
        }
        void State27()
        {
            if (lexemStack.Peek().Type == TokenType.NETERM && lexemStack.Peek().Value == "<префикс3>")
                Reduce(3, "<доп_опис>");
            else
                throw new Exception();
        }
        void State27_1()
        {
            if (lexemStack.Peek().Type == TokenType.SEMICOLON)
                Reduce(4, "<опис>");
            else
                throw new Exception();
        }
        void State28()
        {
            if (lexemStack.Peek().Type == TokenType.NETERM && lexemStack.Peek().Value == "<префикс1>")
                Reduce(3, "<доп_перем>");
            else
                throw new Exception();
        }
        void State29()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.END:
                    Shift();
                    break;
                case TokenType.POINT:
                    GoToState(35);
                    break;
                default:
                    throw new Exception($"State 29\nОжидалось: END или Точка, а получено: {lexemStack.Peek().srt}");
            }
        }
        void State30()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<префикс2>":
                            GoToState(69);
                            break;
                        case "<доп_опер>":
                            GoToState(31);
                            break;
                        case "<опер>":
                            GoToState(32);
                            break;
                        case "<условн.>":
                            GoToState(23);
                            break;
                        case "<присв.>":
                            GoToState(24);
                            break;
                    }
                    break;
                case TokenType.CASE:
                    GoToState(25);
                    break;
                case TokenType.VARIABLE:
                    GoToState(26);
                    break;
                case TokenType.SEMICOLON:
                    if (GetLexeme(nextLex).Type == TokenType.BEGIN | GetLexeme(nextLex).Type == TokenType.LITERAL | GetLexeme(nextLex).Type == TokenType.VARIABLE | GetLexeme(nextLex).Type == TokenType.CASE)
                    {
                        Shift();
                        break;
                    }
                    else
                    {
                        Reduce(2, "<спис_опер>");
                        break;
                    }
                default:
                    throw new Exception($"State 30\nОжидалось: CASE, точка с запятой(;) или Переменная, а получено: {lexemStack.Peek().srt}");
            }
        }
        void State30_2()
        {
            if (lexemStack.Peek().Type == TokenType.NETERM && lexemStack.Peek().Value == "<префикс2>")
                Reduce(3, "<спис_опер>");
            else
                throw new Exception();
        }
        void State30_1()
        {
            if (lexemStack.Peek().Type == TokenType.SEMICOLON)
                Reduce(3, "<спис_опер>");
            else
                throw new Exception();

        }
        void State31()
        {
            if (lexemStack.Peek().Type == TokenType.NETERM && lexemStack.Peek().Value == "<доп_опер>")
                Reduce(1, "<префикс2>");
            else
                throw new Exception();
        }
        void State32()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.SEMICOLON:
                    GoToState(36);
                    break;
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<опер>":
                            Shift();
                            break;
                        default:
                            throw new Exception();
                            break;
                    }
                    break;
                default:
                    throw new Exception($"State 32\nОжидалось: Точка с запятой(;), а получено: {lexemStack.Peek().srt}");
            }
        }
        void State32_1()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.SEMICOLON:
                    if (GetLexeme(nextLex).Type == TokenType.BEGIN | GetLexeme(nextLex).Type == TokenType.LITERAL | GetLexeme(nextLex).Type == TokenType.VARIABLE | GetLexeme(nextLex).Type == TokenType.CASE)
                    {
                        Shift();
                        break;
                    }
                    else
                    {
                        Reduce(2, "<доп_опер>");
                        break;
                    }
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<доп_опер>":
                            GoToState(31);
                            break;
                        case "<префикс2>":
                            GoToState(40);
                            break;
                        case "<опер>":
                            GoToState(23);
                            break;
                        case "<условн.>":
                            GoToState(23);
                            break;
                        case "<присв.>":
                            GoToState(24);
                            break;
                        default:
                            throw new Exception();
                            break;
                    }
                    break;
                case TokenType.CASE:
                    GoToState(25);
                    break;
                case TokenType.VARIABLE:
                    GoToState(26);
                    break;
                default:
                    throw new Exception($"State 32_1\nОжидалось: CASE, точка с запятой(;) или Переменная, а получено: {lexemStack.Peek().srt}");
            }
        }
        void State33()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.VARIABLE:
                    Shift();
                    break;
                case TokenType.OF:
                    GoToState(37);
                    break;
                default:
                    throw new Exception($"State 33\nОжидалось: Переменная или OF, а получено: {lexemStack.Peek().srt}");
            }
        }
        void State34()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.ASSIGNMENT:
                    Expression();
                    break;
                case TokenType.EXPR:
                    GoToState(38);
                    break;
                default:
                    throw new Exception($"State 34\nОжидалось: Символ присваивания(:=), а получено: {lexemStack.Peek().srt}");
            }
        }
        void State35()
        {
            if (lexemStack.Peek().Type == TokenType.POINT)
                Reduce(6, "<программа>");
            else
                throw new Exception();
        }
        void State36()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.SEMICOLON:
                    if (GetLexeme(nextLex).Type == TokenType.BEGIN | GetLexeme(nextLex).Type == TokenType.LITERAL | GetLexeme(nextLex).Type == TokenType.VARIABLE | GetLexeme(nextLex).Type == TokenType.CASE)
                    {
                        Shift();
                        break;
                    }
                    else
                    {
                        Reduce(2, "<доп_опер>");
                        break;
                    }
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<опер>":
                            GoToState(32);
                            break;
                        case "<префикс2>":
                            GoToState(40);
                            break;
                        case "<доп_опер>":
                            GoToState(31);
                            break;
                        case "<условн.>":
                            GoToState(23);
                            break;
                        case "<присв.>":
                            GoToState(24);
                            break;
                        default:
                            throw new Exception();
                    }
                    break;
                case TokenType.CASE:
                    GoToState(25);
                    break;
                case TokenType.VARIABLE:
                    GoToState(26);
                    break;
                default:
                    throw new Exception($"State 36\nОжидалось: CASE, точка с запятой(;) или Переменная, а получено: {lexemStack.Peek().srt}");
            }
        }
        void State37()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.OF:
                    Shift();
                    break;
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<блок_опер.>":
                            GoToState(41);
                            break;
                        case "<опер>":
                            GoToState(42);
                            break;
                        case "<условн.>":
                            GoToState(23);
                            break;
                        case "<присв.>":
                            GoToState(24);
                            break;
                        default:
                            throw new Exception();
                    }
                    break;
                case TokenType.BEGIN:
                    GoToState(43);
                    break;
                case TokenType.LITERAL:
                    GoToState(44);
                    break;
                case TokenType.CASE:
                    GoToState(25);
                    break;
                case TokenType.VARIABLE:
                    GoToState(26);
                    break;
                default:
                    throw new Exception($"State 37\nОжидалось: CASE или OF или Точка с запятой(;) или  Число или Переменная, а получено: {lexemStack.Peek().srt}");
            }
        }
        void State38()
        {
            if (lexemStack.Peek().Type == TokenType.EXPR)
                Reduce(3, "<присв.>");
            else
                throw new Exception();


        }
        void State38_1()
        {
            if (lexemStack.Peek().Type == TokenType.SEMICOLON)
                Reduce(4, "<присв.>");
            else
                throw new Exception();


        }

        void State39()
        {
            if (lexemStack.Peek().Type == TokenType.POINT)
                Reduce(8, "<программа>");
            else
                throw new Exception();
        }
        void State40()
        {
            if (lexemStack.Peek().Type == TokenType.NETERM && lexemStack.Peek().Value == "<префикс2>")
                Reduce(3, "<доп_опер>");
            else
                throw new Exception();
        }
        void State41()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    if (lexemStack.Peek().Value == "<блок_опер.>")
                        Shift();
                    else
                        throw new Exception();
                    break;
                case TokenType.ELSE:
                    GoToState(45);
                    break;
                default:
                    throw new Exception($"State 41\nОжидалось: ELSE, а получено: {lexemStack.Peek().srt}");
            }
        }
        void State42()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    if (lexemStack.Peek().Value == "<опер>")
                        Shift();
                    else
                        throw new Exception();
                    break;
                case TokenType.SEMICOLON:
                    GoToState(46);
                    break;
                default:
                    throw new Exception($"State 42\nОжидалось: Точка с запятой(;), а получено: {lexemStack.Peek().srt}");
            }
        }
        void State43()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.BEGIN:
                    Shift();
                    break;
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<спис_опер>":
                            GoToState(47);
                            break;
                        case "<опер>":
                            GoToState(22);
                            break;
                        case "<условн.>":
                            GoToState(23);
                            break;
                        case "<присв.>":
                            GoToState(24);
                            break;
                        default:
                            throw new Exception();
                    }
                    break;
                case TokenType.CASE:
                    GoToState(25);
                    break;
                case TokenType.VARIABLE:
                    GoToState(26);
                    break;
                default:
                    throw new Exception($"State 43\nОжидалось: CASE, BEGIN или Переменная, а получено: {lexemStack.Peek().srt}");
            }
        }
        void State44()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.LITERAL:
                    Shift();
                    break;
                case TokenType.COLON:
                    GoToState(48);
                    break;
                case TokenType.DOUBLEPOINT:
                    GoToState(49);
                    break;
                default:
                    throw new Exception($"State 44\nОжидалось: Двоеточие(:), Число или Две точки(..) , а получено: {lexemStack.Peek().srt}");
            }

        }
        void State45()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.ELSE:
                    Shift();
                    break;
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<блок_опер.>":
                            GoToState(50);
                            break;
                        case "<опер>":
                            GoToState(42);
                            break;
                        case "<условн.>":
                            GoToState(23);
                            break;
                        case "<присв.>":
                            GoToState(24);
                            break;
                        default:
                            throw new Exception();
                    }
                    break;
                case TokenType.BEGIN:
                    GoToState(43);
                    break;
                case TokenType.LITERAL:
                    GoToState(44);
                    break;
                case TokenType.CASE:
                    GoToState(25);
                    break;
                case TokenType.VARIABLE:
                    GoToState(26);
                    break;
                default:
                    throw new Exception($"State 45\nОжидалось: ELSE, CASE, BEGIN, Число или Переменная, а получено: {lexemStack.Peek().srt}");
            }
        }
        void State46()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.SEMICOLON:
                    if (GetLexeme(nextLex).Type == TokenType.BEGIN | GetLexeme(nextLex).Type == TokenType.LITERAL | GetLexeme(nextLex).Type == TokenType.VARIABLE | GetLexeme(nextLex).Type == TokenType.CASE)
                    {
                        Shift();
                        break;
                    }
                    else
                    {
                        Reduce(2, "<блок_опер.>");
                        break;
                    }
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<блок_опер.>":
                            GoToState(51);
                            break;
                        case "<опер>":
                            GoToState(42);
                            break;
                        case "<условн.>":
                            GoToState(23);
                            break;
                        case "<присв.>":
                            GoToState(24);
                            break;
                        default:
                            throw new Exception();
                    }
                    break;
                case TokenType.BEGIN:
                    GoToState(43);
                    break;
                case TokenType.LITERAL:
                    GoToState(44);
                    break;
                case TokenType.CASE:
                    GoToState(25);
                    break;
                case TokenType.VARIABLE:
                    GoToState(26);
                    break;
                default:
                    throw new Exception($"State 46\nОжидалось: CASE, BEGIN, Число, Точка с запятой(;) или Переменная, а получено: {lexemStack.Peek().srt}");
            }
        }
        void State47()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    if (lexemStack.Peek().Value == "<спис_опер>")
                        Shift();
                    else
                        throw new Exception();
                    break;
                case TokenType.END:
                    GoToState(52);
                    break;
                default:
                    throw new Exception($"State 47\nОжидалось: END, а получено: {lexemStack.Peek().srt}");
            }
        }
        void State48()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.COLON:
                    Shift();
                    break;
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<опер>":
                            GoToState(58);
                            break;
                        case "<условн.>":
                            GoToState(23);
                            break;
                        case "<присв.>":
                            GoToState(24);
                            break;
                        default:
                            throw new Exception();
                    }
                    break;
                case TokenType.BEGIN:
                    GoToState(54);
                    break;
                case TokenType.CASE:
                    GoToState(25);
                    break;
                case TokenType.VARIABLE:
                    GoToState(26);
                    break;
                default:
                    throw new Exception($"State 48\nОжидалось: CASE, BEGIN, Двоеточие(:) или Переменная, а получено: {lexemStack.Peek().srt}");
            }
        }
        void State49()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.DOUBLEPOINT:
                    Shift();
                    break;
                case TokenType.LITERAL:
                    GoToState(55);
                    break;
                default:
                    throw new Exception($"State 49\nОжидалось: Число или Две точки(..), а получено: {lexemStack.Peek().srt}");
            }
        }
        void State50()
        {
            {
                switch (lexemStack.Peek().Type)
                {
                    case TokenType.NETERM:
                        switch (lexemStack.Peek().Value)
                        {
                            case "<блок_опер.>":
                                Shift();
                                break;
                        }
                        break;
                    case TokenType.END:
                        GoToState(56);
                        break;
                    default:
                        throw new Exception($"State 50\nОжидалось: END, а получено: {lexemStack.Peek().srt}");
                }
            }
        }
        void State51()
        {
            if (lexemStack.Peek().Type == TokenType.NETERM && lexemStack.Peek().Value == "<блок_опер.>")
                Reduce(3, "<блок_опер.>");
            else
                throw new Exception();
        }
        void State52()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.END:
                    Shift();
                    break;
                case TokenType.SEMICOLON:
                    GoToState(57);
                    break;
                default:
                    throw new Exception($"State 52\nОжидалось: END или Точка с запятой(;), а получено: {lexemStack.Peek().srt}");
            }
        }
        void State53()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<опер>":
                            Shift();
                            break;
                    }
                    break;
                case TokenType.SEMICOLON:
                    GoToState(58);
                    break;
                default:
                    throw new Exception($"State 53\nОжидалось: Точка с запятой(;), а получено: {lexemStack.Peek().srt}");
            }
        }
        void State54()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.BEGIN:
                    Shift();
                    break;
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<спис_опер>":
                            GoToState(59);
                            break;
                        case "<опер>":
                            GoToState(22);
                            break;
                        case "<условн.>":
                            GoToState(23);
                            break;
                        case "<присв.>":
                            GoToState(24);
                            break;
                        default:
                            throw new Exception();
                    }
                    break;
                case TokenType.CASE:
                    GoToState(25);
                    break;
                case TokenType.VARIABLE:
                    GoToState(26);
                    break;
                default:
                    throw new Exception($"State 54\nОжидалось: CASE, BEGIN или Переменная, а получено: {lexemStack.Peek().srt}");
            }
        }
        void State55()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.LITERAL:
                    Shift();
                    break;
                case TokenType.COLON:
                    GoToState(60);
                    break;
                default:
                    throw new Exception($"State 55\nОжидалось: Число или Двоеточие(:), а получено: {lexemStack.Peek().srt}");
            }
        }
        void State56()
        {
            if (lexemStack.Peek().Type == TokenType.END)
                Reduce(7, "<условн.>");
            else
                throw new Exception();
        }
        void State70()
        {
            if (lexemStack.Peek().Type == TokenType.SEMICOLON)
                Reduce(8, "<условн.>");
            else
                throw new Exception();
        }
        void State57()
        {
            if (lexemStack.Peek().Type == TokenType.SEMICOLON)
                Reduce(4, "<блок_опер.>");
            else
                throw new Exception();
        }
        void State58()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.SEMICOLON:
                    if (GetLexeme(nextLex).Type == TokenType.BEGIN && GetLexeme(nextLex).Type == TokenType.LITERAL && GetLexeme(nextLex).Type == TokenType.VARIABLE)
                    {
                        Shift();
                        break;
                    }
                    else
                    {
                        Reduce(4, "<блок_опер.>");
                        break;
                    }
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<блок_опер.>":
                            GoToState(61);
                            break;
                        case "<опер>":
                            GoToState(42);
                            break;
                        case "<условн.>":
                            GoToState(23);
                            break;
                        case "<присв.>":
                            GoToState(24);
                            break;
                        default:
                            throw new Exception();
                    }
                    break;
                case TokenType.BEGIN:
                    GoToState(43);
                    break;
                case TokenType.LITERAL:
                    GoToState(44);
                    break;
                case TokenType.CASE:
                    GoToState(25);
                    break;
                case TokenType.VARIABLE:
                    GoToState(26);
                    break;
                default:
                    throw new Exception($"State 58\nОжидалось: CASE, BEGIN, Число, Точка с запятой(;) или Переменная, а получено: {lexemStack.Peek().srt}");
            }
        }
        void State59()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<спис_опер>":
                            Shift();
                            break;
                    }
                    break;
                case TokenType.SEMICOLON:
                    GoToState(62);
                    break;
                default:
                    throw new Exception($"State 59\nОжидалось: Точка с запятой(;), а получено: {lexemStack.Peek().srt}");
            }
        }
        void State60()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.COLON:
                    Shift();
                    break;
                case TokenType.NETERM:
                    switch (lexemStack.Peek().Value)
                    {
                        case "<блок_опер.>":
                            GoToState(63);
                            break;
                        case "<опер>":
                            GoToState(42);
                            break;
                        case "<условн.>":
                            GoToState(23);
                            break;
                        case "<присв.>":
                            GoToState(24);
                            break;
                        default:
                            throw new Exception();
                    }
                    break;
                case TokenType.BEGIN:
                    GoToState(43);
                    break;
                case TokenType.CASE:
                    GoToState(25);
                    break;
                case TokenType.VARIABLE:
                    GoToState(26);
                    break;
                default:
                    throw new Exception($"State 60\nОжидалось: CASE, BEGIN, Двоеточие(:) или Переменная, а получено: {lexemStack.Peek().srt}");
            }
        }
        void State61()
        {
            if (lexemStack.Peek().Type == TokenType.NETERM && lexemStack.Peek().Value == "<блок_опер.>")
                Reduce(4, "<блок_опер.>");
            else
                throw new Exception();
        }
        void State62()
        {
            switch (lexemStack.Peek().Type)
            {
                case TokenType.SEMICOLON:
                    Shift();
                    break;
                case TokenType.END:
                    GoToState(64);
                    break;
                default:
                    throw new Exception($"State 62\nОжидалось: END или Точка с запятой(;), а получено: {lexemStack.Peek().srt}");
            }
        }
        void State63()
        {
            if (lexemStack.Peek().Type == TokenType.NETERM && lexemStack.Peek().Value == "<блок_опер.>")
                Reduce(5, "<блок_опер.>");
            else
                throw new Exception();
        }
        void State64()
        {
            if (lexemStack.Peek().Type == TokenType.END)
                Reduce(6, "<блок_опер.>");
            else
                throw new Exception();
        }
        public void Start()
        {
            stateStack.Push(0);
            while (isEnd != true)
                switch (state)
                {
                    case 0:
                        State0();
                        break;
                    case 1:
                        State1();
                        break;
                    case 2:
                        State2();
                        break;
                    case 3:
                        State3();
                        break;
                    case 4:
                        State4();
                        break;
                    case 5:
                        State5();
                        break;
                    case 6:
                        State6();
                        break;
                    case 7:
                        State7();
                        break;
                    case 8:
                        State8();
                        break;
                    case 9:
                        State9();
                        break;
                    case 10:
                        State10();
                        break;
                    case 11:
                        State11();
                        break;
                    case 12:
                        State12();
                        break;
                    case 13:
                        State13();
                        break;
                    case 14:
                        State14();
                        break;
                    case 15:
                        State15();
                        break;
                    case 16:
                        State16();
                        break;
                    case 17:
                        State17();
                        break;
                    case 18:
                        State18();
                        break;
                    case 19:
                        State19();
                        break;
                    case 20:
                        State20();
                        break;
                    case 21:
                        State21();
                        break;
                    case 22:
                        State22();
                        break;
                    case 23:
                        State23();
                        break;
                    case 24:
                        State24();
                        break;
                    case 25:
                        State25();
                        break;
                    case 26:
                        State26();
                        break;
                    case 27:
                        State27();
                        break;
                    case 28:
                        State28();
                        break;
                    case 29:
                        State29();
                        break;
                    case 30:
                        State30();
                        break;
                    case 31:
                        State31();
                        break;
                    case 32:
                        State32();
                        break;
                    case 33:
                        State33();
                        break;
                    case 34:
                        State34();
                        break;
                    case 35:
                        State35();
                        break;
                    case 36:
                        State36();
                        break;
                    case 37:
                        State37();
                        break;
                    case 38:
                        State38();
                        break;
                    case 39:
                        State39();
                        break;
                    case 40:
                        State40();
                        break;
                    case 41:
                        State41();
                        break;
                    case 42:
                        State42();
                        break;
                    case 43:
                        State43();
                        break;
                    case 44:
                        State44();
                        break;
                    case 45:
                        State45();
                        break;
                    case 46:
                        State46();
                        break;
                    case 47:
                        State47();
                        break;
                    case 48:
                        State48();
                        break;
                    case 49:
                        State49();
                        break;
                    case 50:
                        State50();
                        break;
                    case 51:
                        State51();
                        break;
                    case 52:
                        State52();
                        break;
                    case 53:
                        State53();
                        break;
                    case 54:
                        State54();
                        break;
                    case 55:
                        State55();
                        break;
                    case 56:
                        State56();
                        break;
                    case 57:
                        State57();
                        break;
                    case 58:
                        State58();
                        break;
                    case 59:
                        State59();
                        break;
                    case 60:
                        State60();
                        break;
                    case 61:
                        State61();
                        break;
                    case 62:
                        State62();
                        break;
                    case 63:
                        State63();
                        break;
                    case 64:
                        State64();
                        break;
                    case 65:
                        State27_1();
                        break;
                    case 66:
                        State30_1();
                        break;
                    case 67:
                        State22_1();
                        break;
                    case 68:
                        State32_1();
                        break;
                    case 69:
                        State30_2();
                        break;
                    case 70:
                        State70();
                        break;
                    case 71:
                        State71();
                        break;
                    case 72:
                        State15_1();
                        break;
                }
        }
    }
}