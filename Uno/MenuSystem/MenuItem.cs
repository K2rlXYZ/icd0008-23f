namespace MenuSystem;

public class MenuItem
{
    public Func<string?>? MenuLabel { get; set; } = default!;
    public string? PromptText { get; set; } = default!;

    public Func<string?>? MethodToRun { get; set; } = null;
}

