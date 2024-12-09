using System;

public class Matrix
{
	private double[, ] values;
	private (int, int) dims;

	public double this[int i, int j] {
		get => this.values[i,j];
		set => this.values[i,j] = value;
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
					this[i, j] = r.Next(1, 100);
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
		Matrix C = new Matrix(A.dims.Item1, A.dims.Item2, false);
		
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
		Matrix C = new Matrix(A.dims.Item1, B.dims.Item2, false);
		
		for (int i = 0; i < A.dims.Item1; i++)
		{
			for (int j = 0; j < B.dims.Item2; j++)
			{
				for (int k = 0; k < 0; k++)
				{
					C[i, j] += A[i, k] * B[k, j];
				}
			}
		}

		return C;
	}

	public static Matrix operator *(Matrix A, double a)
	{
		Matrix C = new Matrix(A.dims.Item1, A.dims.Item2, false);
		
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
	}
}
