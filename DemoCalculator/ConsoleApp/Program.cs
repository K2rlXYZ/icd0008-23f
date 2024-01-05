// See https://aka.ms/new-console-template for more information

using CalculatorBrain;

Console.WriteLine("Hello, World!");

var calc = new SimpleCalculator();

Console.Write("Give me a number: ");
var numberString = Console.ReadLine();

var number = decimal.Parse(numberString);

calc.SetState(number);

while (true)
{
    Console.Write("Give me an operation(+, -, *, /, q): ");
    var operation = Console.ReadLine();

    if (operation == "q")
    {
        return;
    }

    Console.Write("Give me a number: ");
    numberString = Console.ReadLine();

    number = decimal.Parse(numberString);

    switch (operation)
    {
        case "+":
            calc.Add(number);
            break;
        case "-":
            calc.Subtract(number);
            break;
        case "*":
            calc.Multiply(number);
            break;
        case "/":
            calc.Divide(number);
            break;
        default:
            Console.WriteLine("No support for " + operation);
            break;
    }

    Console.WriteLine("Result is: " + calc.CurrentState);
}