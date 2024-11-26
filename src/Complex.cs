class Complex(double a, double b) {
    public double a {get; set;} = a;
    public double b {get; set;} = b;

    public static Complex operator +(Complex lhs, Complex rhs){
        return new Complex(lhs.a + rhs.a, lhs.b + rhs.b);
    }

    public static Complex operator -(Complex val){
        return new Complex(-val.a, -val.b);
    }

    public static Complex operator -(Complex lhs, Complex rhs) {
        return lhs + (-rhs);
    }

    public static Complex operator *(Complex lhs, Complex rhs){
        return new Complex(lhs.a*rhs.a - lhs.b*rhs.b, lhs.a*rhs.b + lhs.b*rhs.a);
    }

    public static Complex operator /(Complex lhs, Complex rhs){
        double mod = rhs.a*rhs.a + rhs.b+rhs.b;
        double a = (lhs.a*rhs.a + lhs.b*rhs.b)/mod;
        double b = (lhs.b*rhs.a-lhs.a*rhs.b)/mod;
        return new Complex(a, b);
    }

    public double Abs(){
        return Math.Sqrt(this.a*this.a + this.b*this.b);
    }

    public static Complex operator ++(Complex val){
        double r = val.Abs();
        return new Complex((r+1)/r*val.a, (r+1)/r*val.b);
    }

    override public string ToString() {
        return $"{this.a} + {this.b}i";
    }
}
