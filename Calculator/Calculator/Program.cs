using System.Globalization;
using System.Runtime.CompilerServices;
using System.Diagnostics;
using System.Threading;
namespace Calculator
{
    public class Program
    {
        static void Main(string[] args)
        {
            double a;
            double b;
            double result = 0;
            string operation;
            
            Console.WriteLine("Podaj pierwszą liczbę: ");
            a = Double.Parse(Console.ReadLine());
            Console.WriteLine("Podaj drugą liczbę: ");
            b = Double.Parse(Console.ReadLine());
            Console.WriteLine("Podaj działanie jakie ma być wykonane na liczbach(-, +, /, *): ");
            operation = Console.ReadLine();
            
            
            switch (operation)
            {
                case "-":
                    result = Minus(a, b);
                    break;
                case "+":
                    result = Add(a, b);
                    break;
                case "/":
                    result = Division(a, b);
                    break;
                case "*":
                    result = Multiplication(a, b);
                    break;
                default:
                    Console.WriteLine("Nieprawidłowe polecenie!");
                    break;
            }
            Console.WriteLine("Wynik podanego działania to: " + result);
            
        }

        public static double Add(double a, double b)
        {
            return Math.Round(a + b, 2);
        }
        
        public static double Minus(double a, double b)
        {
            return Math.Round(a - b, 2);
        }
        
        public static double Division(double a, double b)
        {
            return Math.Round(a / b, 2);
        }
        
        public static double Multiplication(double a, double b)
        {
            return Math.Round(a * b, 2);
        }
    }
}