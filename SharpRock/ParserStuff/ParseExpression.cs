using SharpRock.Language;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SharpRock.ParserStuff
{
    public partial class Parser
    {
        public Expression parseExpression(List<Token> tokens)
        {
            Expression e = new Expression();

            Stack<Operator> operators = new Stack<Operator>();

            foreach (Token t in tokens)
            {
                if (isOperator(t))
                {
                    while (operators.Count != 0 && operators.Peek().op != '(' && hasHigherPrecedence(operators.Peek(), new Operator(t.ToString()[0])))
                    {
                        e.value.Add(operators.Pop());
                    }
                    operators.Push(new Operator(t.ToString()[0]));
                }
                else if (t is Tokens.IntLiteral)
                {
                    e.value.Add(new ShortLiteral((short)(t as Tokens.IntLiteral).Value));
                }
                else if (t is Tokens.openParenthesis)
                {
                    operators.Push(new Operator(t.ToString()[0]));
                }
                else if (t is Tokens.closeParenthesis)
                {
                    while (operators.Count != 0 && operators.Peek().op != '(')
                    {
                        e.value.Add(operators.Pop());
                    }
                    operators.Pop();
                }
            }

            while (operators.Count != 0)
            {
                e.value.Add(operators.Pop());
            }
            return e;
        }

        public static int getWeight(Operator op)
        {
            int weight = -1;
            switch (op.op)
            {
                case '+':
                case '-':
                    weight = 1;
                    break;
                case '*':
                case '/':
                    weight = 2;
                    break;
            }

            return weight;
        }

        public static bool isOperator(Token t)
        {
            return t is Tokens.Add || t is Tokens.Sub || t is Tokens.Mul || t is Tokens.Div;
        }

        public static bool hasHigherPrecedence(Operator op1, Operator op2)
        {
            return getWeight(op1) >= getWeight(op2);
        }
    }
}
