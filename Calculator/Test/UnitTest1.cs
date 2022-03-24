using NUnit.Framework;

namespace Test;

public class Tests
{
    double a = 12.4;
    double b = 4.2;
    [SetUp]
    public void Setup()
    {
    }

    [Test]
    public void TestAdd()
    {
        Assert.That(Calculator.Program.Add(a, b), Is.EqualTo(16.6));
    }
    [Test]
    public void TestMinus()
    {
        Assert.That(Calculator.Program.Minus(a, b), Is.EqualTo(8.2));
    }
    [Test]
    public void TestDivision()
    {
        Assert.That(Calculator.Program.Division(a, b), Is.EqualTo(2.95));
    }
    [Test]
    public void TestMultiplication()
    {
        Assert.That(Calculator.Program.Multiplication(a, b), Is.EqualTo(52.08));
    }
}