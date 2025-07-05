namespace Pizzeria.Utils;

public class ParserRegistryKeyComparer : IEqualityComparer<(Type, string)>
{
    public bool Equals((Type, string) x, (Type, string) y)
    {
        return x.Item1 == y.Item1 &&
               string.Equals(x.Item2, y.Item2, StringComparison.OrdinalIgnoreCase);
    }

    public int GetHashCode((Type, string) obj)
    {
        var typeHash = obj.Item1.GetHashCode();
        var extHash = StringComparer.OrdinalIgnoreCase.GetHashCode(obj.Item2);
        return HashCode.Combine(typeHash, extHash);
    }
}