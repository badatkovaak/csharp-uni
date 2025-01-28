class Program
{
    public static void Main(string[] args)
    {
        /*Solver.solveNumberPuzzle("luba + lubit = arbvzj");*/
        /*Solver.solveNumberPuzzle("kto + kot = tok");*/
        /*Solver.solveNumberPuzzle("lisa + volk = zveri");*/
        /*Solver.solveNumberPuzzle("oxoxo + axaxa = axaxax");*/
        /*Solver.solveNumberPuzzle("abcde + sped = etebss");*/
        /*Solver.solveNumberPuzzle("sinus + sinus + kosinus = tangens");*/
        /*Solver.solveNumberPuzzle("bir + bir + bir + bir = dord");*/
        /*Solver.solveNumberPuzzle("aad + dab = bgg");*/
        /*Solver.solveNumberPuzzle("uran + uran = nauka");*/
        /*Solver.solveNumberPuzzle("a + b = v");*/
        /*Solver.solveNumberPuzzle("odin + odin = mnogo");*/

        /*Complex x = new Complex(1, 1);*/
        /*Complex y = new Complex(Math.Sqrt(2)/2, Math.Sqrt(2)/2);*/
        /*Console.WriteLine((++y).Abs());*/

        (double, double)[] points1 = { (10, 165), (11, 220), (12, 286), (13, 364) };
        Polynomial p1 = Polynomial.constructLagrangePolynomial(points1);
        Console.WriteLine(p1);

        (double, double)[] points2 =
        {
            (4, 36),
            (5, 126),
            (6, 336),
            (7, 756),
            (8, 1512),
            (9, 2772),
        };
        Polynomial p2 = Polynomial.constructLagrangePolynomial(points2);
        Console.WriteLine(p2);
    }
}
