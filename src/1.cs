using System;

interface Expression {
    public double evaluate();
    public int get_priority();
}


class Number : Expression {
    public double value;

    public Number(){}
    public Number(double val) {
        this.value = val;
    }

    public double evaluate() => this.value;

    public int get_priority() => int.MaxValue;
}

enum Operation {
    Addition,
    Subtraction,
    Multiplication,
    Division
}


class BinaryExpression : Expression {
    public Operation operation;
    public Expression left;
    public Expression right;

    public BinaryExpression(Operation op, Expression left, Expression right) {
        this.operation = op;
        this.left = left;
        this.right = right;
    }

    public double evaluate() => this.operation switch {
        Operation.Addition => left.evaluate() + right.evaluate(),
        Operation.Subtraction => left.evaluate() - right.evaluate(),
        Operation.Multiplication => left.evaluate() * right.evaluate(),
        Operation.Division => left.evaluate() / right.evaluate(),
    };
    
    public int get_priority() => Program.get_op_priority(this.operation);

}

// 2 + (3 * 4)
// 2 + ((3 * 4) + 5)


// ((2 + 3) + 4) + 5
// ((2 + 3) + 4) + (5 * 6)

// 2 + (3 * (4 & 5))
// 2 + (3 * ((4 & 5) * 6)

class Calculator {
    public double value;
    public Expression expression;

    public Calculator() {
        this.value = 0.0;
        this.expression = new Number();
    }

    public void add_operation(Operation op, double value) {
        if (this.expression.GetType() == typeof(Number)) {
            this.expression = new BinaryExpression(op, this.expression, new Number(value));
            return;
        }

        BinaryExpression current_expr = (BinaryExpression)this.expression;
        Expression next_expr = current_expr.right;

        while(true) {
            if (next_expr.get_priority() > Program.get_op_priority(op)) {
                current_expr.right = new BinaryExpression(op, next_expr, new Number(value));
            }
        }

        // while (current_expr.get_priority() < Program.get_op_priority(op)) {
            // if (next_expr.GetType() == typeof(Number)) {
            // }
        
    }
}

 


class Program
{
    public static int get_op_priority(Operation op) => op switch {
        Operation.Addition or Operation.Subtraction => 1,
        Operation.Multiplication or Operation.Division => 2,
    };   

    public static void Main(char[] args) {

    }
}
