class Parser
{
    string input;
    int pos;
    uint len;

    public Parser(string input)
    {
        this.input = input;
        this.pos = 0;
        this.len = (uint)input.Length;
    }

    public char? Peek()
    {
        if (this.pos >= this.len)
            return null;
        return this.input[this.pos];
    }

    public char? Next()
    {
        if (this.pos >= this.len)
            return null;
        return this.input[this.pos++];
    }

    public void SkipWhitespace()
    {
        while (this.Peek() == ' ' || this.Peek() == '\n' || this.Peek() == '\t')
            this.Next();
    }

    public double? ParseNumber()
    {
        string num = "";

        while (this.Peek() is not null)
        {
            if (this.Peek() < '0' || this.Peek() > '9' || this.Peek() == '.')
                break;

            num += this.Next();
        }

        if (num.Length == 0)
            return null;

        double val = 0;
        bool success = Double.TryParse(num, out val);

        if (!success)
            return null;

        return val;
    }

    public (double, double)? ParsePoint()
    {
        SkipWhitespace();

        if (this.Peek() != '(')
            return null;

        this.Next();
        SkipWhitespace();
        double? num1 = ParseNumber();

        if (num1 is null)
            return null;

        SkipWhitespace();
        if (this.Peek() != ',')
            return null;

        this.Next();
        SkipWhitespace();
        double? num2 = ParseNumber();

        if (num2 is null)
            return null;

        SkipWhitespace();
        if (this.Peek() != ')')
            return null;

        this.Next();
        return ((double)num1, (double)num2);
    }

    public List<(double, double)>? ParsePoints()
    {
        List<(double, double)> result = new List<(double, double)>();

        while (true)
        {
            SkipWhitespace();

            if (this.Peek() is null)
                break;

            (double, double)? point = ParsePoint();

            if (point is null)
                return null;

            result.Add(((double, double))point);
            SkipWhitespace();

            if (this.Peek() == ',')
                this.Next();
        }

        if (result.Count == 0)
            return null;

        return result;
    }
}
