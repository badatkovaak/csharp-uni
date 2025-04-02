using System;
using System.Collections.Generic;

interface Expression
{
    public double evaluate();
    public int get_priority();
}

class Number : Expression
{
    public double value;

    public Number() { }

    public Number(double val)
    {
        this.value = val;
    }

    public double evaluate() => this.value;

    public int get_priority() => int.MaxValue;

    public override string ToString()
    {
        return this.value.ToString();
    }
}

enum UnaryOperation
{
    Minus,
}

class UnaryExpression : Expression
{
    UnaryOperation op;
    Expression left;

    public UnaryExpression(UnaryOperation op, Expression expr)
    {
        this.op = op;
        this.left = expr;
    }

    public int get_priority() =>
        this.op switch
        {
            UnaryOperation.Minus => 10,
            _ => throw new Exception("Never Happens"),
        };

    public double evaluate() =>
        this.op switch
        {
            UnaryOperation.Minus => -this.left.evaluate(),
            _ => throw new Exception(),
        };
}

enum BinaryOperation
{
    Addition,
    Subtraction,
    Multiplication,
    Division,
}

class OperationUtils
{
    public static int MaxBinaryPrecedence = 2;
    public static int MinBinaryPrecedence = 1;

    public static int get_op_priority(BinaryOperation op) =>
        op switch
        {
            BinaryOperation.Addition or BinaryOperation.Subtraction => 1,
            BinaryOperation.Multiplication or BinaryOperation.Division => 2,
            _ => throw new Exception("Never Happens"),
        };

    public static string to_string(BinaryOperation op) =>
        op switch
        {
            BinaryOperation.Addition => "+",
            BinaryOperation.Subtraction => "-",
            BinaryOperation.Multiplication => "*",
            BinaryOperation.Division => "/",
            _ => throw new Exception("Never Happens"),
        };

    public static BinaryOperation? from_string(string s) =>
        s switch
        {
            "+" => BinaryOperation.Addition,
            "-" => BinaryOperation.Subtraction,
            "*" => BinaryOperation.Multiplication,
            "/" => BinaryOperation.Division,
            _ => null,
        };
}

class BinaryExpression : Expression
{
    public BinaryOperation operation;
    public Expression left;
    public Expression right;

    public BinaryExpression(BinaryOperation op, Expression left, Expression right)
    {
        this.operation = op;
        this.left = left;
        this.right = right;
    }

    public double evaluate() =>
        this.operation switch
        {
            BinaryOperation.Addition => left.evaluate() + right.evaluate(),
            BinaryOperation.Subtraction => left.evaluate() - right.evaluate(),
            BinaryOperation.Multiplication => left.evaluate() * right.evaluate(),
            BinaryOperation.Division => left.evaluate() / right.evaluate(),
            _ => throw new Exception("Never Happens"),
        };

    public int get_priority() => OperationUtils.get_op_priority(this.operation);

    public override string ToString()
    {
        string res = "(";
        res += this.left.ToString();
        res += " " + OperationUtils.to_string(this.operation) + " ";
        res += this.right.ToString();
        res += ")";
        return res;
    }
}

/*class */

class Parser
{
    string input;
    int pos;
    int length;

    /*Expression*/

    public Parser(string input)
    {
        this.input = input;
        this.pos = 0;
        this.length = input.Length;
    }

    public char? Peek()
    {
        if (this.pos >= this.length)
            return null;
        return this.input[this.pos];
    }

    public char? Next()
    {
        if (this.pos >= this.length)
            return null;
        return this.input[this.pos++];
    }

    public void SkipWhitespace()
    {
        while (this.Peek() == ' ' || this.Peek() == '\n' || this.Peek() == '\t')
            this.Next();
    }

    public Number? ParseNumber()
    {
        string num = "";

        while (this.Peek() is not null)
        {
            if (this.Peek() < '0' || this.Peek() > '9')
                break;

            num += this.Next();
        }

        if (num.Length == 0)
            return null;

        double val = 0;
        bool success = Double.TryParse(num, out val);

        if (!success)
            return null;

        return new Number(val);
    }

    public Expression? ParseSimpleExpression()
    {
        /*Console.WriteLine($"Simple -- {this.pos}, {this.Peek()}");*/

        SkipWhitespace();
        char? c = this.Peek();

        if (c == '(')
        {
            this.Next();
            Expression? e = ParseBinaryExpression(OperationUtils.MinBinaryPrecedence);

            /*Console.WriteLine($"Simple result is {e}");*/

            if (e is null)
                return null;

            SkipWhitespace();

            if (this.Peek() != ')')
                return null;

            this.Next();
            return e;
        }
        else if (c >= '0' && c <= '9')
            return ParseNumber();
        else
            return null;
    }

    public Expression? ParseUnaryExpression()
    {
        /*Console.WriteLine($"Unary -- {this.pos}, {this.Peek()}");*/

        SkipWhitespace();
        char? c = this.Peek();

        if (c is null)
            return null;

        if (c != '+' && c != '-' && (c < '0' || c > '9') && c != '(')
            return null;

        if (c == '+' || c == '-')
            this.Next();

        Expression? e = ParseSimpleExpression();
        /*Console.WriteLine($"Unary result is {e}");*/

        if (e is null)
            return null;

        if (c == '-')
            return new UnaryExpression(UnaryOperation.Minus, e);

        return e;
    }

    public Expression? ParseBinaryExpression(int precedence)
    {
        /*Console.WriteLine($"Binary - {this.pos}, {this.Peek()}, {precedence}");*/

        SkipWhitespace();
        Expression? e1;

        if (precedence + 1 <= OperationUtils.MaxBinaryPrecedence)
        {
            e1 = ParseBinaryExpression(precedence + 1);

            if (e1 is null)
                return null;
        }
        else
        {
            e1 = ParseUnaryExpression();

            if (e1 is null)
                return null;
        }

        /*Console.WriteLine($"Binary E1 is {e1}");*/
        List<(BinaryOperation, Expression)> l = new List<(BinaryOperation, Expression)>();

        while (true)
        {
            SkipWhitespace();
            char? c = this.Peek();

            if (c is null)
                break;

            BinaryOperation? op = OperationUtils.from_string(((char)c).ToString());
            /*Console.WriteLine($"Binary op is {op}, {precedence}");*/

            if (op is null)
                break;

            if (OperationUtils.get_op_priority((BinaryOperation)op) != precedence)
                break;

            this.Next();
            Expression? e2 = ParseBinaryExpression(precedence + 1);
            /*Console.WriteLine($"Binary E2 is {e2}");*/

            if (e2 is null)
                return null;

            l.Add(((BinaryOperation)op, e2));
        }

        /*Console.WriteLine($"Binary List is ");*/
        /*foreach (var i in l)*/
        /*    Console.WriteLine($"{i.Item1}, {i.Item2}");*/
        /*Console.WriteLine();*/

        if (l.Count == 0)
            return e1;

        Expression res = e1;

        for (int i = 0; i < l.Count; i++)
        {
            res = new BinaryExpression(l[i].Item1, res, l[i].Item2);
        }

        return res;
    }

    public Expression? Parse()
    {
        Expression? res = ParseBinaryExpression(OperationUtils.MinBinaryPrecedence);

        if (res is null)
            return null;

        SkipWhitespace();

        if (this.pos != this.length)
            return null;

        return res;
    }
}
