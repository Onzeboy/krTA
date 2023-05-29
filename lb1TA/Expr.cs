using System.Text.RegularExpressions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Threading.Tasks;

namespace lb1TA
{
    public class Expression
    {
        int countLpar = 0;
        int countRpar = 0;
        List<Token> ExpressionStack = new List<Token>();
        Stack<string> Operations = new Stack<string>();
        Stack<int> Prioritis = new Stack<int>();
        int index = 0;
        string output = null;
        Dictionary<string, int> priority = new Dictionary<string, int>() //Список приоритетов для операций
{
{"+", 2}, {"-", 2},
{"*", 3}, {"/", 3},
{"(", 0}, {")", 1}
};
        public void TakeToken(Token token)//Получение из анализатора логического выражения
        {

            ExpressionStack.Add(token);
        }
        public void Start()//Запуск программы
        {
            Decstra();

            PolishNotation();
        }
        private void HighPriority(string operation)//Вспомогательная функция для метода Дейкстры
        {
            int count = Operations.Count();
            Stack<string> temp = new Stack<string>();//Вспомогательный стек для выполнения выталкивания операций с большим приоритетом
            Stack<int> priorityTemp = new Stack<int>();//Вспомогательный стек приоритетов для выполнения выталкивания операций с большим приоритетом
            for (int i = 0; i < count; i++)//Цикл выталкивания
            {
                if (Prioritis.Peek() >= priority[operation])
                {
                    output += Operations.Pop();
                    Prioritis.Pop();
                }
                else
                {
                    temp.Push(Operations.Pop());
                    priorityTemp.Push(Prioritis.Pop());
                }
            }
            temp.Reverse();
            priorityTemp.Reverse();
            int countTemp = temp.Count();//Цикл возврата операций, которые не были вытолкнуты
            for (int i = 0; i < countTemp; i++)
            {
                Operations.Push(temp.Pop());
                Prioritis.Push(priorityTemp.Pop());
            }
            Operations.Push(ExpressionStack[index].srt);
            Prioritis.Push(priority[operation]);
        }

       private void Decstra()
        {
            if (ExpressionStack[index].Type == Token.TokenType.LPAR || ExpressionStack[index].Type == Token.TokenType.LITERAL || ExpressionStack[index].Type == Token.TokenType.VARIABLE)
            {
                Prioritis.Push(0);
                while (index != ExpressionStack.Count())
                {
                    if (ExpressionStack[index].Type == Token.TokenType.LITERAL || ExpressionStack[index].Type == Token.TokenType.VARIABLE)
                    {
                        output += ExpressionStack[index].srt + " ";
                        index++;
                    }
                    else if (ExpressionStack[index].Type == Token.TokenType.PLUS)
                    {
                        string operation = "+";

                        if ((priority[operation] > Prioritis.Peek()) || Operations.Count() == 0)
                        {
                            Operations.Push(ExpressionStack[index].srt);
                            Prioritis.Push(priority[operation]);
                        }
                        else
                        {
                            HighPriority(operation);
                        }
                        index++;
                    }

                    else if (ExpressionStack[index].Type == Token.TokenType.MINUS)
                    {
                        string operation = "-";

                        if ((priority[operation] > Prioritis.Peek()) || Operations.Count() == 0)
                        {
                            Operations.Push(ExpressionStack[index].srt);
                            Prioritis.Push(priority[operation]);
                        }
                        else
                        {
                            HighPriority(operation);
                        }
                        index++;
                    }

                    else if (ExpressionStack[index].Type == Token.TokenType.MULTIPLY)
                    {
                        string operation = "*";

                        if ((priority[operation] > Prioritis.Peek()) || Operations.Count() == 0)
                        {
                            Operations.Push(ExpressionStack[index].srt);
                            Prioritis.Push(priority[operation]);
                        }
                        else
                        {
                            HighPriority(operation);
                        }
                        index++;
                    }

                    else if (ExpressionStack[index].Type == Token.TokenType.DIVISION)
                    {
                        string operation = "/";

                        if ((priority[operation] > Prioritis.Peek()) || Operations.Count() == 0)
                        {
                            Operations.Push(ExpressionStack[index].srt);
                            Prioritis.Push(priority[operation]);
                        }
                        else
                        {
                            HighPriority(operation);
                        }
                        index++;
                    }

                    else if (ExpressionStack[index].Type == Token.TokenType.LPAR)
                    {
                        string operation = "(";
                        countLpar++;
                        if ((priority[operation] > Prioritis.Peek()) || Operations.Count() == 0)
                        {
                            Operations.Push(ExpressionStack[index].srt);
                            Prioritis.Push(priority[operation]);
                        }
                        else
                        {
                            Operations.Push(operation);
                            Prioritis.Push(priority[operation]);
                        }
                        index++;
                    }

                    else if (ExpressionStack[index].Type == Token.TokenType.RPAR)
                    {
                        string operation = ")";
                        countRpar++;

                        if ((priority[operation] > Prioritis.Peek() || Operations.Count() == 0) && countLpar == countRpar)
                        {
                            Operations.Push(ExpressionStack[index].srt);
                            Prioritis.Push(priority[operation]);
                        }
                        else
                        {
                            HighPriority(operation);
                            Operations.Pop();
                            Operations.Pop();
                            Prioritis.Pop();
                            Prioritis.Pop();
                        }
                        index++;
                    }
                }
                int countOperations = Operations.Count();
                for (int i = 0; i < countOperations; i++)
                {
                    output += Operations.Pop();
                }
            }
        }
        public void PolishNotation()//Метод выполняющий преобразование обратную польскую нотацию в матричный вид
        {
            Dictionary<int, string> M = new Dictionary<int, string>();
            Stack<string> stackOperand = new Stack<string>();
            int key = 1;
            for (int i = 0; i < output.Count(); i++)
            {
                char currentChar = output[i];
                switch (currentChar)
                {

                    case ('+'):
                        {
                            M.Add(key, stackOperand.Pop() + " " + stackOperand.Pop() + " " + "+");
                            stackOperand.Push("M" + key.ToString());
                            key++;
                            break;
                        }
                    case ('-'):
                        {
                            M.Add(key, stackOperand.Pop() + " " + stackOperand.Pop() + " " + "-");
                            stackOperand.Push("M" + key.ToString());
                            key++;
                            break;
                        }
                    case ('*'):
                        {
                            M.Add(key, stackOperand.Pop() + " " + stackOperand.Pop() + " " + "*");
                            stackOperand.Push("M" + key.ToString());
                            key++;
                            break;
                        }
                    case ('/'):
                        {
                            M.Add(key, stackOperand.Pop() + " " + stackOperand.Pop() + " " + "/");
                            stackOperand.Push("M" + key.ToString());
                            key++;
                            break;
                        }
                    default:
                        {
                            if (Regex.IsMatch(currentChar.ToString(), "^[a-zA-Z]+$") || Regex.IsMatch(currentChar.ToString(), "^[0-9]+$"))
                            {
                                string temp = null;
                                while (output[i] != ' ')
                                {
                                    temp += output[i].ToString();
                                    i++;
                                }
                                stackOperand.Push(temp);
                            }
                            else if (currentChar == ' ')
                            {

                            }
                            else
                            {
                                throw new System.Exception("Ошибка перевода в матричный вид");
                            }
                            break;
                        }
                }
            }
            Form1._Form1.Printt("Обратная польская нотация:");
            Form1._Form1.Printt(output);

            Form1._Form1.Printt("Матречный вид:");
            int countM = M.Count;
            for (int i = 1; i < countM + 1; i++)
            {
                Form1._Form1.Printt("M" + i + ":" + M[i]);
            }
            Form1._Form1.Printt("=======================");
        }
    }
}