namespace lab3.figures
{
    public class Square : Rectangle
    {
        public Square(double a) : base(a, a) {}
        public override string ToString()
        {
            return "Side = " + this.Width + ", Square = " + this.Square();
        }
    }
}