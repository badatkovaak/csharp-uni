using Avalonia.Controls;

class Calculator
{
    double total;
    double current_input;
    TextBlock? total_display;
    TextBlock? current_display;
    public Expression expression;
    public BinaryOperation? current_op;

    public double Total
    {
        get => this.total;
        set
        {
            this.total = value;
            if (this.expression.GetType() == typeof(Number))
            {
                Number n = (Number)this.expression;
                n.value = value;
            }
            if (total_display is not null)
                total_display.Text = this.total.ToString();
        }
    }

    public double CurrentInput
    {
        get => this.current_input;
        set
        {
            this.current_input = value;
            if (current_display is not null)
                current_display.Text = this.current_input.ToString();
        }
    }

    public Calculator(TextBlock b1, TextBlock b2)
    {
        this.total = 0.0;
        this.expression = new Number();
        this.total_display = b1;
        this.current_display = b2;
    }

    public Calculator(Expression expr, double value)
    {
        this.total = value;
        this.expression = expr;
    }

    public void AddDigitToInput(double value)
    {
        this.CurrentInput = this.CurrentInput * 10 + value;
    }

    public void DeleteDigitFromInput()
    {
        this.CurrentInput = (this.CurrentInput - this.CurrentInput % 10) / 10;
    }

    public void ClearAll()
    {
        this.Total = 0;
        this.CurrentInput = 0;
        this.expression = new Number(0);
    }

    public void UpdateTotal()
    {
        this.Total = this.expression.evaluate();
    }

    public void AddOperation(BinaryOperation op, double value)
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
