public class CalculatorModel
{
    public double Evaluate(double left, double right, string op)
    {
        return op switch
        {
            "+" => left + right,
            "-" => left - right,
            "*" => left * right,
            "/" => right != 0 ? left / right : double.NaN,
            _ => 0
        };
    }
}
