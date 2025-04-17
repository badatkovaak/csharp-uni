public class Program
{
    public static void Main(string[] args)
    {
        // double[,] vals = new double[3, 3]
        // {
        //     { 1, 2, 3 },
        //     { 4, 5, 6 },
        //     { 7, 8, 10 },
        // };
        // SquareMatrix A = new SquareMatrix(vals);
        // SquareMatrix B = new SquareMatrix(vals.GetLength(0));
        //
        // Console.WriteLine(A);
        // // Console.WriteLine(B);
        // // Console.WriteLine(A * B);
        // Console.WriteLine(A.Clone().to_row_echelon_form());
        // Console.WriteLine(A.calculate_rank());
        // Console.WriteLine(A);
        // SquareMatrix A_inv = A.GetInverse();
        // Console.WriteLine(A_inv);
        // Console.WriteLine(A * A_inv);

        double[,] vals = new double[2, 4]
        {
            { 2, 1, 1, 0 },
            { 1, 2, 0, 1 },
        };
        Matrix A = new Matrix(vals);
        Vector B = new Vector(new double[] { 3, 3 });
        Vector C = new Vector(new double[] { 1, 1, 0, 0 });
        Console.WriteLine(Solver.Solve(A, B, C));
    }
}
