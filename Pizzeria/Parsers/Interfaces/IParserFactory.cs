namespace Pizzeria.Parsers.Interfaces;

public interface IParserFactory
{
    IParser GetParser(string path);
}