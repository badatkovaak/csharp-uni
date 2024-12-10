using System;

public class Matrix
{
	private double[, ] values;
	private (int, int) dims;
	public double this[int i, int j] { get => this.values[i, j]; set => this.values[i, j] = value; }

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

	public Matrix((int, int) dims, bool Random = false) : this(dims.Item1, dims.Item2, Random)
	{
	}

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

	public Matrix add_row(int i, int j, double m)
	{
		Matrix C = new Matrix(this.dims, false);
		for (int k = 0; k < this.dims.Item1; k++)
		{
			for (int l = 0; l < this.dims.Item2; l++)
			{
				if (k != l && k != i && l != j)
				{
					C[k, l] = 0;
				}
				else if (k == l)
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

	public Matrix clone()
	{
		Matrix A = new Matrix(this.dims);
		A.values = this.values;
		return A;
	}
}

public class Program
{
	public static void Main()
	{
		Matrix A = new Matrix(2, 2);
		Matrix B = new Matrix(2, 2);
		Console.WriteLine(A);
		Console.WriteLine(B);
		Console.WriteLine(A + B);
		Console.WriteLine(A * B);
		Console.WriteLine(A.swap_rows(0, 1));
		Console.WriteLine(A.add_row(0, 1, 2.0));
		Console.WriteLine(A.multiply_row(0, 2.0));
	}
}
