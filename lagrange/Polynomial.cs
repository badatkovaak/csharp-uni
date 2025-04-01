public class Monom : IComparable<Monom>
{
    double coefficient;
    List<(char, int)> powers;

    public double Coefficient
    {
        get { return this.coefficient; }
    }

    public Monom(double c, List<(char, int)> p)
    {
        this.coefficient = c;
        this.powers = p;
    }

    public Monom(double s, List<int> p, char c)
    {
        this.coefficient = s;
        this.powers = new List<(char, int)>(p.Count);

        foreach (int a in p)
            powers.Add((c, a));
    }

    public Monom(double s, List<int> p)
        : this(s, p, 'x') { }

    public Monom(double s)
        : this(s, [], 'x') { }

    public static Monom operator *(Monom A, Monom B)
    {
        List<(char, int)> p = new List<(char, int)>();

        if (A.powers.Count == 0 || B.powers.Count == 0)
        {
            if (A.powers.Count == 0)
                return new Monom(A.coefficient * B.coefficient, B.powers).cleanup();
            if (B.powers.Count == 0)
                return new Monom(A.coefficient * B.coefficient, A.powers).cleanup();
        }

        foreach ((char, int) a in A.powers)
        foreach ((char, int) b in B.powers)
            if (a.Item1 == b.Item1)
                p.Add((a.Item1, a.Item2 + b.Item2));

        return new Monom(A.coefficient * B.coefficient, p).cleanup();
    }

    public static Monom operator *(Monom A, double s)
    {
        return new Monom(A.coefficient * s, A.powers);
    }

    public static Monom operator *(double s, Monom A)
    {
        return A * s;
    }

    public static Monom operator +(Monom A, Monom B)
    {
        if (A != B)
            throw new Exception();

        List<(char, int)> l = new List<(char, int)>();
        foreach ((char, int) a in A.powers)
        foreach ((char, int) b in B.powers)
            if (a.Item1 == b.Item1)
                l.Add((a.Item1, a.Item2));

        return new Monom(A.coefficient + B.coefficient, l).cleanup();
    }

    public Monom cleanup()
    {
        for (int i = 0; i < this.powers.Count; i++)
        {
            if (this.powers[i].Item2 == 0)
                this.powers.RemoveAt(i);
        }

        this.powers.Sort(
            ((char, int) item1, (char, int) item2) => item1.Item1.CompareTo(item2.Item1)
        );

        int j = 0;
        while (j + 1 < this.powers.Count)
        {
            if (this.powers[j].Item1 == this.powers[j + 1].Item1)
            {
                this.powers[j] = (
                    this.powers[j].Item1,
                    this.powers[j].Item2 + this.powers[j + 1].Item2
                );
                this.powers.RemoveAt(j + 1);
            }
        }

        for (int i = 0; i < this.powers.Count; i++)
        {
            if (this.powers[i].Item2 == 0)
                this.powers.RemoveAt(i);
        }

        return this;
    }

    public static bool operator <(Monom A, Monom B)
    {
        foreach ((char, int) a in A.powers)
        foreach ((char, int) b in B.powers)
            if (a.Item1 == b.Item1 && a.Item2 != b.Item2)
                return a.Item2 < b.Item2;

        return A.powers.Count < B.powers.Count;
    }

    public static bool operator >(Monom A, Monom B)
    {
        if (A == B)
            return false;

        return !(A < B);
    }

    public static bool operator ==(Monom A, Monom B)
    {
        foreach ((char, int) a in A.powers)
        foreach ((char, int) b in B.powers)
            if (a.Item1 == b.Item1 && a.Item2 != b.Item2)
                return false;

        return A.powers.Count == B.powers.Count;
    }

    public static bool operator !=(Monom A, Monom B)
    {
        return !(A == B);
    }

    public int CompareTo(Monom? other)
    {
        if ((object?)other == null)
            return -1;

        if (other == this)
            return 0;

        return this > other ? 1 : -1;
    }

    public override string ToString()
    {
        string result = "";

        if (this.coefficient != 1.0)
        {
            if (this.coefficient < 0)
                result += "- ";

            result += System.Math.Abs(this.coefficient).ToString();
        }

        if (this.powers.Count == 1)
        {
            if (this.powers[0].Item2 != 0)
                result += $"{this.powers[0].Item1}^{this.powers[0].Item2}";

            return result;
        }

        for (int i = 0; i < this.powers.Count; i++)
        {
            if (this.powers[i].Item2 != 0)
            {
                if (result.Length != 0)
                    result += "*";

                result += $"{this.powers[i].Item1}^{this.powers[i].Item2}";
            }
        }

        if (result.Length == 0)
            result += "1";

        return result;
    }
}

public class Polynomial
{
    List<Monom> monoms;

    public Polynomial(List<Monom> m)
    {
        this.monoms = m;
        this.cleanup();
    }

    public Polynomial(Monom m)
    {
        this.monoms = new List<Monom>();
        this.monoms.Add(m);
        this.cleanup();
    }

    public Polynomial(params Monom[] args)
    {
        this.monoms = new List<Monom>(args);
        this.cleanup();
    }

    public Polynomial cleanup()
    {
        foreach (Monom m in this.monoms)
        {
            m.cleanup();
        }

        this.monoms.Sort((Monom A, Monom B) => -(A.CompareTo(B)));

        int i = 0;
        while (i + 1 < this.monoms.Count)
        {
            if (this.monoms[i] == this.monoms[i + 1])
            {
                this.monoms[i] = this.monoms[i] + this.monoms[i + 1];
                this.monoms.RemoveAt(i + 1);
            }

            if (Math.Abs(this.monoms[i].Coefficient) < 0.000000000001)
                this.monoms.RemoveAt(i);

            i++;
        }

        return this;
    }

    public static Polynomial operator +(Polynomial A, Polynomial B)
    {
        return (new Polynomial(Utils.Concat(A.monoms, B.monoms))).cleanup();
    }

    public static Polynomial operator *(Polynomial A, Polynomial B)
    {
        List<Monom> l = new List<Monom>();

        foreach (Monom a in A.monoms)
        foreach (Monom b in B.monoms)
            l.Add(a * b);

        return new Polynomial(l).cleanup();
    }

    public static Polynomial operator *(Polynomial A, double s)
    {
        List<Monom> res = new List<Monom>();

        foreach (Monom a in A.monoms)
            res.Add(a * s);

        return new Polynomial(res).cleanup();
    }

    public static Polynomial operator *(double s, Polynomial A)
    {
        return A * s;
    }

    public static Polynomial constructLagrangeBasisPolynomial((double, double)[] points, int j)
    {
        Polynomial res = new Polynomial(new List<Monom>([new Monom(1)]));

        for (int i = 0; i < points.Length; i++)
        {
            if (i != j)
            {
                double coef = 1 / (points[j].Item1 - points[i].Item1);
                res *= new Polynomial(
                    new List<Monom>([new Monom(coef, [1]), new Monom(-coef * points[i].Item1)])
                );
            }
        }

        return res;
    }

    public static Polynomial constructLagrangeBasisPolynomial(List<(double, double)> points, int j)
    {
        Polynomial res = new Polynomial(new List<Monom>([new Monom(1)]));

        for (int i = 0; i < points.Count; i++)
        {
            if (i != j)
            {
                double coef = 1 / (points[j].Item1 - points[i].Item1);
                res *= new Polynomial(
                    new List<Monom>([new Monom(coef, [1]), new Monom(-coef * points[i].Item1)])
                );
            }
        }

        return res;
    }

    public static Polynomial constructLagrangePolynomial((double, double)[] points)
    {
        Polynomial res = new Polynomial();

        for (int i = 0; i < points.Length; i++)
        {
            res += points[i].Item2 * constructLagrangeBasisPolynomial(points, i);
        }

        return res;
    }

    public static Polynomial constructLagrangePolynomial(List<(double, double)> points)
    {
        Polynomial res = new Polynomial();

        for (int i = 0; i < points.Count; i++)
        {
            res += points[i].Item2 * constructLagrangeBasisPolynomial(points, i);
        }

        return res;
    }

    public override string ToString()
    {
        string result = "";

        for (int i = 0; i < this.monoms.Count; i++)
        {
            if (i > 0)
                result += this.monoms[i].Coefficient > 0 ? " + " : " ";
            result += this.monoms[i].ToString();
        }

        return result;
    }
}

class Utils
{
    public static List<T> Concat<T>(params List<T>[] args)
    {
        int len = 0;
        foreach (List<T> l in args)
            len += l.Count;

        List<T> res = new List<T>(len);

        foreach (List<T> l in args)
        foreach (T item in l)
            res.Add(item);

        return res;
    }
}
