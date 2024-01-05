

DateTime start = DateTime.Now;

for (int x = 0; x < 100000; x++)
{
    Console.Write(1);
}

DateTime stop = DateTime.Now;
var delta = stop - start;

Console.WriteLine("\n\n");
Console.WriteLine(delta.TotalMilliseconds + " ms");
