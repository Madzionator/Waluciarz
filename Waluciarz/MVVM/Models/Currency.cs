namespace Waluciarz.MVVM.Models;

public record Currency(string Symbol, string Name)
{
    public string FullName { get; } = $"{Symbol} - {Name}";
}