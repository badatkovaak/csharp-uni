public class Vector
{
    private double[] values;

    public double this[int i]
    {
        get => this.values[i];
        set => this.values[i] = value;
    }

    public int Dimension
    {
        get => this.values.Length;
    }

    public Vector(double[] values)
    {
        this.values = values;
    }

    public Vector(int n)
    {
        this.values = new double[n];

        for (int i = 0; i < n; i++)
            this[i] = 0;
    }

    public static double DotProduct(Vector A, Vector B, SquareMatrix G)
    {
        if (A.Dimension != B.Dimension || G.Dims.Item1 != A.Dimension)
            throw new Exception();

        double result = 0;

        for (int i = 0; i < A.Dimension; i++)
        for (int j = 0; j < B.Dimension; j++)
            result += G[i, j] * A[i] * B[j];

        return result;
    }

    public static double DotProduct(Vector A, Vector B)
    {
        return DotProduct(A, B, SquareMatrix.GetIdMatrix(A.Dimension));
    }

    public double GetNorm(SquareMatrix G)
    {
        return DotProduct(this, this, G);
    }

    public double GetNorm()
    {
        return DotProduct(this, this);
    }

    public bool IsPositive()
    {
        foreach (var c in this.values)
            if (c < 0)
                return false;

        return true;
    }

    public (Vector, Vector) SplitAtIndex(int i)
    {
        if (i + 1 >= this.Dimension)
            throw new Exception();

        int l1 = i + 1;
        int l2 = this.Dimension - i - 1;

        Vector C1 = new Vector(l1);
        Vector C2 = new Vector(l2);

        for (int j = 0; j < l1; j++)
            C1[j] = this[j];

        for (int j = 0; j < l2; j++)
            C2[j] = this[l1 + j];

        return (C1, C2);
    }

    public static Vector Concat(Vector A, Vector B)
    {
        Vector C = new Vector(A.Dimension + B.Dimension);

        for (int i = 0; i < A.Dimension; i++)
            C[i] = A[i];

        for (int i = 0; i < B.Dimension; i++)
            C[i + A.Dimension] = B[i];

        return C;
    }

    public static Vector operator +(Vector A, Vector B)
    {
        if (A.Dimension != B.Dimension)
            throw new Exception();

        Vector C = new Vector(A.Dimension);

        for (int i = 0; i < A.Dimension; i++)
            C[i] = A[i] + B[i];

        return C;
    }

    public static Vector operator -(Vector A)
    {
        return -1 * A;
    }

    public static Vector operator -(Vector A, Vector B)
    {
        return A + (-B);
    }

    public static Vector operator *(double c, Vector A)
    {
        Vector B = new Vector(A.Dimension);

        for (int i = 0; i < A.Dimension; i++)
            B[i] = c * A[i];

        return B;
    }

    public static Vector operator *(Vector A, double c)
    {
        return c * A;
    }

    public static double operator *(Vector A, Vector B)
    {
        return DotProduct(A, B);
    }

    public override string ToString()
    {
        string res = "";

        for (int i = 0; i < this.Dimension; i++)
            res += this[i].ToString() + " ";

        return res;
    }
}
