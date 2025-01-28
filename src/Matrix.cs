using System;

public class Matrix
{
    private double[,] values;
    private (int, int) dims;
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
            {
                for (int j = 0; j < n; j++)
                {
                    this[i, j] = r.Next(1, 10);
                }
            }
        }
        else
        {
            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    this[i, j] = 0.0;
                }
            }
        }
    }

    public Matrix((int, int) dims, bool Random = false)
        : this(dims.Item1, dims.Item2, Random) { }

    public override string ToString()
    {
        string result = "";
        for (int i = 0; i < this.dims.Item1; i++)
        {
            for (int j = 0; j < this.dims.Item2; j++)
            {
                result += $"{this[i, j]} ";
            }

            result += "\n";
        }

        return result;
    }

    public static Matrix operator +(Matrix A, Matrix B)
    {
        if (A.dims.Item1 != B.dims.Item1 || A.dims.Item2 != B.dims.Item2)
        {
            throw new Exception();
        }

        Matrix C = new Matrix(A.dims, false);
        for (int i = 0; i < A.dims.Item1; i++)
        {
            for (int j = 0; j < A.dims.Item2; j++)
            {
                C[i, j] = A[i, j] + B[i, j];
            }
        }

        return C;
    }

    public static Matrix operator *(Matrix A, Matrix B)
    {
        if (A.dims.Item2 != B.dims.Item1)
        {
            throw new Exception();
        }

        Matrix C = new Matrix(A.dims.Item1, B.dims.Item2, false);
        for (int i = 0; i < A.dims.Item1; i++)
        {
            for (int j = 0; j < B.dims.Item2; j++)
            {
                for (int k = 0; k < A.dims.Item2; k++)
                {
                    C[i, j] += A[i, k] * B[k, j];
                }
            }
        }

        return C;
    }

    public static Matrix operator *(Matrix A, double a)
    {
        Matrix C = new Matrix(A.dims, false);
        for (int i = 0; i < A.dims.Item1; i++)
        {
            for (int j = 0; j < A.dims.Item2; j++)
            {
                C[i, j] = A[i, j] * a;
            }
        }

        return C;
    }

    public static Matrix operator *(double a, Matrix A)
    {
        return A * a;
    }

    public Matrix swap_rows(int i, int j)
    {
        Matrix C = new Matrix(this.dims, false);
        for (int k = 0; k < this.dims.Item1; k++)
        {
            for (int l = 0; l < this.dims.Item2; l++)
            {
                if (k != i && k != j && k != l)
                {
                    C[k, l] = 0;
                }
                else if (k != i && k != j && k == l)
                {
                    C[k, l] = 1;
                }
                else if (k == i && l != j)
                {
                    C[k, l] = 0;
                }
                else if (k == i && l == j)
                {
                    C[k, l] = 1;
                }
                else if (k == j && l != i)
                {
                    C[k, l] = 0;
                }
                else
                {
                    C[k, l] = 1;
                }
            }
        }

        this.values = (C * this).values;
        return this;
    }

    public Matrix multiply_row(int i, double m)
    {
        Matrix C = new Matrix(this.dims, false);
        for (int k = 0; k < this.dims.Item1; k++)
        {
            for (int l = 0; l < this.dims.Item2; l++)
            {
                if (k != l)
                {
                    C[k, l] = 0;
                }
                else if (k == l && k != i)
                {
                    C[k, l] = 1;
                }
                else
                {
                    C[k, l] = m;
                }
            }
        }

        this.values = (C * this).values;
        return this;
    }

    public Matrix add_row(int j, int i, double m)
    {
        Matrix C = new Matrix(this.dims, false);
        for (int k = 0; k < this.dims.Item1; k++)
        {
            for (int l = 0; l < this.dims.Item2; l++)
            {
                if (k == i && l == j)
                {
                    C[k, l] = m;
                }
                else if (k == l)
                {
                    C[k, l] = 1;
                }
                else
                {
                    C[k, l] = 0;
                }
            }
        }

        this.values = (C * this).values;
        return this;
    }

    public Matrix clone()
    {
        Matrix A = new Matrix(this.dims);
        A.values = this.values;
        return A;
    }

    public Matrix create_without()
    {
        Matrix C = new Matrix(this.dims.Item1 - 1, this.dims.Item2 - 1);
        for (int i = 1; i < this.dims.Item1; i++)
        {
            for (int j = 1; j < this.dims.Item2; j++)
            {
                C[i - 1, j - 1] = this[i, j];
            }
        }

        return C;
    }

    public int calculate_rank()
    {
        if (this.dims.Item1 == 1)
        {
            return this[0, 0] == 0.0 ? 0 : 1;
        }

        Matrix C = this.clone();
        if (C[0, 0] == 0)
        {
            for (int i = 1; i < C.dims.Item1; i++)
            {
                if (C[i, 0] != 0)
                {
                    C.swap_rows(0, i);
                    break;
                }

                if (i == C.dims.Item2 - 1)
                {
                    return C.create_without().calculate_rank();
                }
            }
        }

        for (int i = 1; i < C.dims.Item1; i++)
        {
            C.add_row(0, i, -C[i, 0] / C[0, 0]);
        }

        return C.create_without().calculate_rank() + 1;
    }

    public Matrix to_row_echelon_form(int start_i, int start_j)
    {
        Console.WriteLine(start_i.ToString());
        Console.WriteLine(start_j.ToString());

        if (this.dims.Item1 <= start_i + 1)
        {
            return this;
        }

        if (this[start_i, start_j] == 0)
        {
            for (int i = start_i + 1; i < this.dims.Item1; i++)
            {
                if (this[i, start_j] != 0)
                {
                    this.swap_rows(start_i, i);
                    break;
                }

                if (i == this.dims.Item2 - 1)
                {
                    Console.WriteLine(this);
                    return this.to_row_echelon_form(start_i, start_j + 1);
                }
            }
        }

        for (int i = start_i + 1; i < this.dims.Item1; i++)
        {
            Console.WriteLine($"added {start_i} {i} {-this[i, start_j] / this[start_i, start_j]}");
            Console.WriteLine(this);
            this.add_row(start_i, i, -this[i, start_j] / this[start_i, start_j]);
            Console.WriteLine(this);
        }

        Console.WriteLine(this);

        return this.to_row_echelon_form(start_i + 1, start_j + 1);
    }
}

public class SquareMatrix : Matrix
{
    public SquareMatrix(int n, bool FillRandomly = true)
        : base(n, n, FillRandomly) { }

    public static SquareMatrix operator *(SquareMatrix A, SquareMatrix B)
    {
        return A * B;
    }

    public static SquareMatrix operator *(SquareMatrix A, Matrix B)
    {
        if (B.Dims.Item1 != B.Dims.Item2)
        {
            throw new Exception();
        }

        return A * B;
    }

    public static SquareMatrix operator *(Matrix A, SquareMatrix B)
    {
        if (A.Dims.Item1 != A.Dims.Item2)
        {
            throw new Exception();
        }

        return A * B;
    }
}

/*public class Program*/
/*{*/
/*	public static void Main()*/
/*	{*/
/*		Matrix A = new Matrix(3,3);*/
/*		Matrix B = new SquareMatrix(3);*/
/*		Console.WriteLine(A);*/
/*		// Console.WriteLine(B);*/
/*		// Console.WriteLine(A + B);*/
/*		// Console.WriteLine(A * B);*/
/*		// Console.WriteLine(B * A);*/
/*		// Console.WriteLine(A.swap_rows(0, 1));*/
/*		// Console.WriteLine(A.add_row(0, 1, 2.0));*/
/*		// Console.WriteLine(A.multiply_row(0, 2.0));*/
/*		// Console.WriteLine(A.create_without());*/
/*		// Console.WriteLine(A.calculate_rank());*/
/*		// Console.WriteLine(B.calculate_rank());*/
/*		Console.WriteLine(A.to_row_echelon_form(0,0));*/
/*		// Console.WriteLine(A);*/
/*		// Console.WriteLine(B);*/
/*	}*/
/*}*/
