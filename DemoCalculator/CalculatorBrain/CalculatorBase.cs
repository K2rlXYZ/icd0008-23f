namespace CalculatorBrain;

public abstract class CalculatorBase
{
    public decimal CurrentState { get; protected set; }

    public void SetState(decimal state)
    {
        CurrentState = state;
    }
}