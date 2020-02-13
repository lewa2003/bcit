namespace lab2.figures
{
    public class Square : Rectangle, IPrint
    {
        public Square(double a) : base(a, a) {}
        public override string ToString()
        {
            return "Side = " + this.Width + ", Square = " + this.Square();
        }
    }
}