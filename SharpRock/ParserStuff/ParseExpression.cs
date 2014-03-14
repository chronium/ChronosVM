using SharpRock.Language;
using SharpRock.Language.ControlFlow;
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

            for (int i = 0; i < tokens.Count; i++)
            {
                if (isOperator(tokens[i]))
                {
                    while (operators.Count != 0 && operators.Peek().op != '(' && hasHigherPrecedence(operators.Peek(), new Operator(tokens[i].ToString()[0])))
                    {
                        e.value.Add(operators.Pop());
                    }
                    operators.Push(new Operator(tokens[i].ToString()[0]));
                }
                else if (tokens[i] is Tokens.IntLiteral)
                {
                    e.value.Add(new ShortLiteral((short)(tokens[i] as Tokens.IntLiteral).Value));
                }
                else if (tokens[i] is Tokens.openParenthesis)
                {
                    operators.Push(new Operator(tokens[i].ToString()[0]));
                }
                else if (tokens[i] is Tokens.closeParenthesis)
                {
                    while (operators.Count != 0 && operators.Peek().op != '(')
                    {
                        e.value.Add(operators.Pop());
                    }
                    operators.Pop();
                }
                else if (tokens[i] is Tokens.Statement)
                {
                    if (i != tokens.Count - 1)
                    {
                        if (tokens[i + 1] is Tokens.openParenthesis)
                        {

                            string name = tokens[i++].ToString();
                            int parentheses = 1;
                            i++;

                            List<Expression> args = new List<Expression>();
                            while (parentheses > 0)
                            {
                                List<Token> tok = new List<Token>();
                                int startParenth = parentheses - 1;
                                while (!(tokens[i] is Tokens.Comma) && parentheses != startParenth)
                                {
                                    if (tokens[i] is Tokens.openParenthesis)
                                        parentheses++;
                                    else if (tokens[i] is Tokens.closeParenthesis)
                                        parentheses--;
                                    if (startParenth == parentheses)
                                        break;
                                    tok.Add(tokens[i++]);
                                }
                                args.Add(parseExpression(tok));
                                if (parentheses == 0) break;
                                if (tokens[i] is Tokens.Comma)
                                    i++;
                                else if (tokens[i] is Tokens.openParenthesis)
                                {
                                    parentheses++;
                                    i++;
                                }
                                else if (tokens[i] is Tokens.closeParenthesis)
                                {
                                    parentheses--;
                                    i++;
                                }
                            }
                            i++;
                            e.value.Add(new FunctionCall(name, args));
                        }
                        else
                            e.value.Add(new VarPlaceholder(tokens[i].ToString()));
                    }
                    else
                        e.value.Add(new VarPlaceholder(tokens[i].ToString()));
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
