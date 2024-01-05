namespace CalculatorBrain;

public class SimpleCalculator : CalculatorBase, ISimpleCalculation
{
    public void Add(decimal a)
    {
        CurrentState = CurrentState + a;
    }

    public void Subtract(decimal a)
    {
        CurrentState = CurrentState - a;
    }

    public void Divide(decimal a)
    {
        CurrentState = CurrentState / a;
    }

    public void Multiply(decimal a)
    {
        CurrentState = CurrentState * a;
    }
}