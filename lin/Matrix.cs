public class Matrix
{
    protected double[,] values;
    protected (int, int) dims;

    public double this[int i, int j]
    {
        get => this.values[i, j];
        set => this.values[i, j] = value;
    }

    public (int, int) Dims
    {
        get => this.dims;
        set => this.dims = value;
    }

    public Matrix(int m, int n, bool RandomInputs = true)
    {
        this.dims = (m, n);
        this.values = new double[m, n];
        if (RandomInputs)
        {
            Random r = new Random();
            for (int i = 0; i < m; i++)
            for (int j = 0; j < n; j++)
                this[i, j] = r.Next(1, 10);
        }
        else
        {
            for (int i = 0; i < m; i++)
            for (int j = 0; j < n; j++)
                this[i, j] = 0.0;
        }
    }

    public Matrix((int, int) dims, bool Random = false)
        : this(dims.Item1, dims.Item2, Random) { }

    public Matrix(double[,] values)
    {
        if (values.Rank != 2)
            throw new RankException();

        this.dims = (values.GetLength(0), values.GetLength(1));
        this.values = values;
    }

    public static Matrix GetIdMatrix(int n)
    {
        Matrix E = new Matrix(n, n, false);

        for (int i = 0; i < n; i++)
        for (int j = 0; j < n; j++)
            E[i, j] = (i == j) switch
            {
                true => 1,
                false => 0,
            };

        return E;
    }

    public SquareMatrix GetSquarePart()
    {
        if (this.Dims.Item1 == this.Dims.Item2)
            return SquareMatrix.Clone(this);

        int square_part;

        if (this.Dims.Item1 > this.Dims.Item2)
            square_part = this.Dims.Item2;
        else
            square_part = this.Dims.Item1;

        SquareMatrix C = new SquareMatrix(square_part);

        for (int i = 0; i < C.Dims.Item1; i++)
        for (int j = 0; j < C.Dims.Item2; j++)
            C[i, j] = this[i, j];

        return C;
    }

    public (SquareMatrix, Matrix?) SplitIntoSquareAndRest()
    {
        if (this.Dims.Item1 == this.Dims.Item2)
            return (SquareMatrix.Clone(this), null);

        int square_part;
        (int, int) rest_dims;

        if (this.Dims.Item1 > this.Dims.Item2)
        {
            square_part = this.Dims.Item2;
            rest_dims = (this.Dims.Item1 - this.Dims.Item2, this.Dims.Item2);
        }
        else
        {
            square_part = this.Dims.Item1;
            rest_dims = (this.Dims.Item1, this.Dims.Item2 - this.Dims.Item1);
        }

        SquareMatrix C = new SquareMatrix(square_part);
        Matrix D = new Matrix(rest_dims);

        for (int i = 0; i < C.Dims.Item1; i++)
        for (int j = 0; j < C.Dims.Item2; j++)
            C[i, j] = this[i, j];

        (int, int) diff = (this.Dims.Item1 - D.Dims.Item1, this.Dims.Item2 - D.Dims.Item2);
        for (int i = 0; i < D.Dims.Item1; i++)
        for (int j = 0; j < D.Dims.Item2; j++)
            D[i, j] = this[i + diff.Item1, j + diff.Item2];

        return (C, D);
    }

    public override string ToString()
    {
        string result = "";
        for (int i = 0; i < this.dims.Item1; i++)
        {
            for (int j = 0; j < this.dims.Item2; j++)
                result += $"{this[i, j]} ";

            result += "\n";
        }

        return result;
    }

    public static Matrix operator +(Matrix A, Matrix B)
    {
        if (A.dims.Item1 != B.dims.Item1 || A.dims.Item2 != B.dims.Item2)
            throw new Exception();

        Matrix C = new Matrix(A.dims, false);
        for (int i = 0; i < A.dims.Item1; i++)
        for (int j = 0; j < A.dims.Item2; j++)
            C[i, j] = A[i, j] + B[i, j];

        return C;
    }

    public static Matrix operator *(Matrix A, Matrix B)
    {
        if (A.dims.Item2 != B.dims.Item1)
            throw new Exception();

        Matrix C = new Matrix(A.dims.Item1, B.dims.Item2, false);
        for (int i = 0; i < A.dims.Item1; i++)
        for (int j = 0; j < B.dims.Item2; j++)
        for (int k = 0; k < A.dims.Item2; k++)
            C[i, j] += A[i, k] * B[k, j];

        return C;
    }

    public static Matrix operator *(Matrix A, double a)
    {
        Matrix C = new Matrix(A.dims, false);
        for (int i = 0; i < A.dims.Item1; i++)
        for (int j = 0; j < A.dims.Item2; j++)
            C[i, j] = A[i, j] * a;

        return C;
    }

    public static Matrix operator *(double a, Matrix A)
    {
        return A * a;
    }

    public static Vector operator *(Matrix A, Vector B)
    {
        if (A.Dims.Item2 != B.Dimension)
            throw new Exception();

        Vector res = new Vector(A.Dims.Item1);

        for (int i = 0; i < A.Dims.Item1; i++)
        for (int j = 0; j < A.Dims.Item2; j++)
            res[i] += A[i, j] * B[i];

        return res;
    }

    public static Vector operator *(Vector B, Matrix A)
    {
        if (B.Dimension != A.dims.Item1)
            throw new Exception();

        Vector res = new Vector(A.Dims.Item2);

        for (int i = 0; i < A.Dims.Item1; i++)
        for (int j = 0; j < A.Dims.Item2; j++)
            res[j] += A[i, j] * B[j];

        return res;
    }

    public Matrix SwapRows(int i, int j)
    {
        Matrix C = new Matrix(this.dims, false);
        for (int k = 0; k < this.dims.Item1; k++)
        for (int l = 0; l < this.dims.Item2; l++)
            if (k != i && k != j && k != l)
                C[k, l] = 0;
            else if (k != i && k != j && k == l)
                C[k, l] = 1;
            else if (k == i && l != j)
                C[k, l] = 0;
            else if (k == i && l == j)
                C[k, l] = 1;
            else if (k == j && l != i)
                C[k, l] = 0;
            else
                C[k, l] = 1;

        this.values = (C * this).values;
        return this;
    }

    public Matrix MultiplyRow(int i, double m)
    {
        Matrix C = new Matrix(this.dims, false);
        for (int k = 0; k < this.dims.Item1; k++)
        for (int l = 0; l < this.dims.Item2; l++)
            if (k != l)
                C[k, l] = 0;
            else if (k == l && k != i)
                C[k, l] = 1;
            else
                C[k, l] = m;

        this.values = (C * this).values;
        return this;
    }

    public Matrix AddRow(int j, int i, double m)
    {
        Matrix C = new Matrix(this.dims, false);
        for (int k = 0; k < this.dims.Item1; k++)
        for (int l = 0; l < this.dims.Item2; l++)
            if (k == i && l == j)
                C[k, l] = m;
            else if (k == l)
                C[k, l] = 1;
            else
                C[k, l] = 0;

        this.values = (C * this).values;
        return this;
    }

    public Matrix Clone()
    {
        Matrix A = new Matrix(this.dims);

        for (int i = 0; i < A.dims.Item1; i++)
        for (int j = 0; j < A.dims.Item2; j++)
            A[i, j] = this[i, j];

        return A;
    }

    public Matrix CreateWithout()
    {
        Matrix C = new Matrix(this.dims.Item1 - 1, this.dims.Item2 - 1);
        for (int i = 1; i < this.dims.Item1; i++)
        for (int j = 1; j < this.dims.Item2; j++)
            C[i - 1, j - 1] = this[i, j];

        return C;
    }

    public int CalculateRank()
    {
        Matrix C = this.Clone();
        return C.CalculateRankInner();
    }

    private int CalculateRankInner(int start = 0)
    {
        if (this.dims.Item1 - start == 1)
            return this[start, start] == 0.0 ? 0 : 1;

        if (this[start, start] == 0)
            for (int i = start + 1; i < this.dims.Item1; i++)
            {
                if (this[i, start] != 0)
                {
                    this.SwapRows(start, i);
                    break;
                }

                if (i == this.dims.Item2 - 1)
                    return this.CalculateRankInner(start + 1);
            }

        for (int i = start + 1; i < this.dims.Item1; i++)
            this.AddRow(start, i, -this[i, start] / this[start, start]);

        return this.CalculateRankInner(start + 1) + 1;
    }

    public Matrix ToRowEchelonForm(int start_i = 0, int start_j = 0)
    {
        if (this.dims.Item1 <= start_i + 1)
            return this;

        if (this[start_i, start_j] == 0)
            for (int i = start_i + 1; i < this.dims.Item1; i++)
            {
                if (this[i, start_j] != 0)
                {
                    this.SwapRows(start_i, i);
                    break;
                }

                if (i == this.dims.Item2 - 1)
                    return this.ToRowEchelonForm(start_i, start_j + 1);
            }

        for (int i = start_i + 1; i < this.dims.Item1; i++)
            this.AddRow(start_i, i, -this[i, start_j] / this[start_i, start_j]);

        return this.ToRowEchelonForm(start_i + 1, start_j + 1);
    }

    public Matrix GlueToTheSide(Matrix A, bool GlueHorizontally)
    {
        int GlueDir = GlueHorizontally ? 1 : 0;
        (int, int) new_dims = (
            this.Dims.Item1 + (1 - GlueDir) * A.Dims.Item1,
            this.Dims.Item2 + GlueDir * A.Dims.Item2
        );
        Matrix Res = new Matrix(new_dims);

        for (int i = 0; i < new_dims.Item1; i++)
        for (int j = 0; j < new_dims.Item2; j++)
        {
            if (i < this.Dims.Item1 || j < this.Dims.Item2)
                Res[i, j] = this[i, j];
            else
                Res[i, j] = A[i, j];
        }

        return Res;
    }
}

public class SquareMatrix : Matrix
{
    public SquareMatrix(int n, bool FillRandomly = true)
        : base(n, n, FillRandomly) { }

    public SquareMatrix(double[,] values)
        : base(values)
    {
        if (values.Rank != 2 || values.GetLength(0) != values.GetLength(1))
            throw new Exception();
    }

    public new SquareMatrix Clone()
    {
        SquareMatrix A = new SquareMatrix(this.Dims.Item1);

        for (int i = 0; i < A.Dims.Item1; i++)
        for (int j = 0; j < A.Dims.Item2; j++)
            A[i, j] = this[i, j];

        return A;
    }

    public static SquareMatrix Clone(Matrix A)
    {
        if (A.Dims.Item1 != A.Dims.Item2)
            throw new Exception();

        SquareMatrix B = new SquareMatrix(A.Dims.Item1);
        for (int i = 0; i < A.Dims.Item1; i++)
        for (int j = 0; j < A.Dims.Item1; j++)
            B[i, j] = A[i, j];

        return B;
    }

    public static SquareMatrix operator *(SquareMatrix A, SquareMatrix B)
    {
        if (A.dims.Item2 != B.dims.Item1)
            throw new Exception();

        int n = A.dims.Item1;
        SquareMatrix C = new SquareMatrix(n, false);
        for (int i = 0; i < n; i++)
        for (int j = 0; j < n; j++)
        for (int k = 0; k < n; k++)
            C[i, j] += A[i, k] * B[k, j];

        return C;
    }

    public static Matrix operator *(SquareMatrix A, Matrix B)
    {
        if (B.Dims.Item1 != B.Dims.Item2 || B.Dims.Item1 != A.Dims.Item1)
            throw new Exception();

        return (Matrix)A * B;
    }

    public static Matrix operator *(Matrix A, SquareMatrix B)
    {
        if (A.Dims.Item1 != A.Dims.Item2 || A.Dims.Item1 != B.Dims.Item1)
            throw new Exception();

        return A * (Matrix)B;
    }

    public static new SquareMatrix GetIdMatrix(int n)
    {
        SquareMatrix E = new SquareMatrix(n, false);

        for (int i = 0; i < n; i++)
        for (int j = 0; j < n; j++)
            E[i, j] = (i == j) switch
            {
                true => 1,
                false => 0,
            };

        return E;
    }

    private IEnumerable<Transform> ToRowEchelonFormInner(int start_i = 0, int start_j = 0)
    {
        List<Transform> res = new List<Transform>();

        if (this.Dims.Item1 <= start_i + 1)
        {
            if (this[start_i, start_j] != 1)
            {
                double c = 1 / this[start_i, start_j];
                this.MultiplyRow(start_i, c);
                res.Add(new Transform(TransformType.MultiplyRow, start_i, 0, c));
            }

            return res;
        }

        if (this[start_i, start_j] == 0)
            for (int i = start_i + 1; i < this.Dims.Item1; i++)
            {
                if (this[i, start_j] != 0)
                {
                    this.SwapRows(start_i, i);
                    res.Add(new Transform(TransformType.SwapRows, start_i, i));
                    break;
                }

                if (i == this.Dims.Item2 - 1)
                    return res.Concat(this.ToRowEchelonFormInner(start_i, start_j + 1));
            }

        for (int i = start_i + 1; i < this.Dims.Item1; i++)
        {
            double c = -this[i, start_j] / this[start_i, start_j];
            this.AddRow(start_i, i, c);
            res.Add(new Transform(TransformType.AddRow, start_i, i, c));
        }

        if (this[start_i, start_j] != 1)
        {
            double c = 1 / this[start_i, start_j];
            this.MultiplyRow(start_i, c);
            res.Add(new Transform(TransformType.MultiplyRow, start_i, 0, c));
        }

        return res.Concat(this.ToRowEchelonFormInner(start_i + 1, start_j + 1));
    }

    private IEnumerable<Transform> ToId()
    {
        List<Transform> res = new List<Transform>();
        for (int j = 0; j < this.Dims.Item1; j++)
        for (int i = 0; i < j; i++)
        {
            double c = -this[i, j];
            this.AddRow(j, i, c);
            res.Add(new Transform(TransformType.AddRow, j, i, c));
        }

        return res;
    }

    public SquareMatrix GetInverse()
    {
        if (this.CalculateRank() != this.Dims.Item1)
            throw new Exception();

        SquareMatrix A = this.Clone();
        List<Transform> transforms1 = new List<Transform>(A.ToRowEchelonFormInner());
        IEnumerable<Transform> transforms2 = A.ToId();
        List<Transform> transforms = new List<Transform>(transforms1.Concat(transforms2));
        SquareMatrix C = SquareMatrix.GetIdMatrix(this.Dims.Item1);

        for (int i = 0; i < transforms.Count; i++)
        {
            Transform t = transforms[i];
            _ = t.type switch
            {
                TransformType.SwapRows => C.SwapRows(t.i, t.j),
                TransformType.MultiplyRow => C.MultiplyRow(t.i, t.a),
                TransformType.AddRow => C.AddRow(t.i, t.j, t.a),
                _ => throw new Exception(),
            };
        }

        return C;
    }

    public double Determinant()
    {
        SquareMatrix A = this.Clone();
        List<Transform> transforms = new List<Transform>(A.ToRowEchelonFormInner());
        double res = 1;

        foreach (var t in transforms)
            res *= t.type switch
            {
                TransformType.MultiplyRow => t.a,
                TransformType.SwapRows => 1 - 2 * ((t.i - t.j) % 2),
                _ => 1,
            };

        for (int i = 0; i < A.Dims.Item1; i++)
            res *= A[i, i];

        return res;
    }
}

enum TransformType
{
    SwapRows,
    MultiplyRow,
    AddRow,
}

class Transform
{
    public TransformType type;
    public int i = 0;
    public int j = 0;
    public double a = 0;

    public Transform(TransformType t, int i = 0, int j = 0, double a = 0)
    {
        this.type = t;
        this.i = i;
        this.j = j;
        this.a = a;
    }
}
