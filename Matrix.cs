using System;

public class Matrix
{
	private int[] values;
	private (int, int) dims;
	
	public Matrix(int m, int n, bool RandomInputs = true)
	{
		this.dims = (m, n);
		this.values = new int[n * m];
		if (RandomInputs)
		{
			Random r = new Random();
			for (int i = 0; i < n * m; i++)
			{
				this.values[i] = r.Next(1, 100);
			}
		}
		else
		{
			for (int i = 0; i < n * m; i++)
			{
				this.values[i] = 0;
			}
		}
	}

	public override string ToString()
	{
		string result = "";
		for (int i = 0; i < this.dims.Item1 * this.dims.Item2; i++)
		{
			result += $"{this.values[i]} ";
			if ((i + 1) % this.dims.Item2 == 0)
			{
				result += "\n";
			}
		}

		return result;
	}
	
	public int[] this[int i]
	{
		get;
	}

	public static Matrix operator +(Matrix A, Matrix B)
	{
		Matrix C = new Matrix(A.dims.Item1, A.dims.Item2, false);
		for (int i = 0; i < A.dims.Item1 * A.dims.Item2; i++)
		{
			C.values[i] = A.values[i] + B.values[i];
		}

		return C;
	}
	
	public static Matrix operator *(Matrix A, Matrix B) {
		Matrix C = new Matrix(A.dims.Item1, B.dims.Item2, false);
		
		for (int i = 0; i < A.dims.Item1; i++){
			for(int j = 0; j < B.dims.Item2; j++){
				for (int k = 0; k < 0; k++){
					C.values[i] += ;
				}
			}
		}
		
		return C;
	}
	
	public static Matrix operator *(Matrix A, int a){
		Matrix C = new Matrix(A.dims.Item1, A.dims.Item2, false);
		
		for (int i = 0; i < A.dims.Item1 * A.dims.Item2; i++){
			C.values[i] = A.values[i] * a;
		}
		
		return C;
	}
	
	public static Matrix operator *(int a, Matrix A){
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
