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

enum Operation
{
    Addition,
    Subtraction,
    Multiplication,
    Division,
}

class OperationUtils
{
    public static int get_op_priority(Operation op) =>
        op switch
        {
            Operation.Addition or Operation.Subtraction => 1,
            Operation.Multiplication or Operation.Division => 2,
            _ => throw new Exception("Never Happens"),
        };

    public static string to_string(Operation op) =>
        op switch
        {
            Operation.Addition => "+",
            Operation.Subtraction => "-",
            Operation.Multiplication => "*",
            Operation.Division => "/",
            _ => throw new Exception("Never Happens"),
        };
}

class BinaryExpression : Expression
{
    public Operation operation;
    public Expression left;
    public Expression right;

    public BinaryExpression(Operation op, Expression left, Expression right)
    {
        this.operation = op;
        this.left = left;
        this.right = right;
    }

    public double evaluate() =>
        this.operation switch
        {
            Operation.Addition => left.evaluate() + right.evaluate(),
            Operation.Subtraction => left.evaluate() - right.evaluate(),
            Operation.Multiplication => left.evaluate() * right.evaluate(),
            Operation.Division => left.evaluate() / right.evaluate(),
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

// 2 + (3 * 4)
// 2 + ((3 * 4) + 5)


// ((2 + 3) + 4) + 5
// ((2 + 3) + 4) + (5 * 6)

// 2 + (3 * (4 & 5))
// 2 + (3 * ((4 & 5) * 6)

class Calculator
{
    public double value;
    public Expression expression;

    public Calculator()
    {
        this.value = 0.0;
        this.expression = new Number();
    }

    public Calculator(Expression expr, double value)
    {
        this.value = value;
        this.expression = expr;
    }

    public void add_operation(Operation op, double value)
    {
        if (this.expression.GetType() == typeof(Number))
        {
            this.expression = new BinaryExpression(op, this.expression, new Number(value));
            return;
        }

        BinaryExpression current_expr = (BinaryExpression)this.expression;
        Expression next_expr = current_expr.right;

        while (true)
        {
            if (
                next_expr.get_priority() >= OperationUtils.get_op_priority(op)
                || next_expr.GetType() == typeof(Number)
            )
            {
                current_expr.right = new BinaryExpression(op, next_expr, new Number(value));
                return;
            }

            current_expr = (BinaryExpression)next_expr;
            next_expr = current_expr.right;
        }
    }
}
