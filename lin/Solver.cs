public class Solver
{
    public static (Vector, double) Solve(Matrix A, Vector B, Vector C)
    {
        Vector x_b,
            x_c,
            c_b,
            c_c;

        (SquareMatrix A_b, Matrix? A_c) = A.SplitIntoSquareAndRest();

        if (A_c is null)
            throw new NotImplementedException();

        x_c = new Vector(A_c.Dims.Item2);
        (c_b, c_c) = C.SplitAtIndex(A_c.Dims.Item2 - 1);
        SquareMatrix A_b_inv = A_b.GetInverse();
        x_b = A_b_inv * (B - A_c * x_c);

        if (!x_b.IsPositive()) { }

        return solve_inner(A, B, C);
    }

    private static (Vector, double) solve_inner(Matrix A, Vector B, Vector C)
    {
        Vector x_b,
            x_c,
            c_b,
            c_c;

        while (true)
        {
            (SquareMatrix A_b, Matrix? A_c) = A.SplitIntoSquareAndRest();

            if (A_c is null)
                throw new NotImplementedException();

            x_c = new Vector(A_c.Dims.Item2);
            (c_b, c_c) = C.SplitAtIndex(A_c.Dims.Item2 - 1);
            SquareMatrix A_b_inv = A_b.GetInverse();
            x_b = A_b_inv * (B - A_c * x_c);

            Matrix alpha = A_b_inv * A_c;
            Vector delta = c_c - c_b * alpha;
            Vector beta = A_b_inv * B;

            Console.WriteLine(A_b_inv);
            Console.WriteLine(A_b * A_b_inv);
            Console.WriteLine(x_b);
            Console.WriteLine(alpha);
            Console.WriteLine(delta);

            double max_value = 0;
            int max_index = -1;
            for (int i = 0; i < delta.Dimension; i++)
            {
                if (delta[i] > 0 && delta[i] > max_value)
                {
                    max_value = delta[i];
                    max_index = i;
                }
            }

            if (max_value == 0)
                break;

            int k = -1;
            double min_alpha = double.PositiveInfinity;
            for (int i = 0; i < alpha.Dims.Item1; i++)
            {
                if (alpha[i, max_index] < 0 && -beta[i] / alpha[i, max_index] < min_alpha)
                {
                    min_alpha = -beta[i] / alpha[i, max_index];
                    k = i;
                }
            }

            if (k == -1)
                return (Vector.Concat(x_b, x_c), double.PositiveInfinity);

            A.SwapRows(k, max_index);
        }

        return (Vector.Concat(x_b, x_c), Vector.Concat(x_b, x_c) * Vector.Concat(c_b, c_c));
    }

    private static void EnsurePositiveBasis(Matrix A, Vector B, Vector C)
    {
        for (int i = 0; i < B.Dimension; i++)
            if (B[i] < 0)
            {
                B[i] *= -1;
                A.MultiplyRow(i, -1);
            }

        // Matrix A1 = A.

        return;
    }
}
