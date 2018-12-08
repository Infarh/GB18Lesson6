using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson6
{
    class Program
    {
        static void Main(string[] args)
        {
            Function fun = new Function(Sqr); 

            var x = 2;

            Console.WriteLine("Значение функции f({0}) = {1}", x, fun.Invoke(x));

            fun = new Function(Sqr);

            Console.WriteLine("Значение функции f({0}) = {1}", x, fun.Invoke(x));

            fun = Sqr;
            Console.WriteLine("Значение функции f({0}) = {1}", x, fun(x));

            Console.WriteLine();

            Tabulate(Sqr, 0, 4, 0.5);

            Console.WriteLine();

            Tabulate(Cube, 0, 4, 0.5);

            Console.WriteLine();

            //Tabulate(delegate(double x1) { return 2 * x1 + 3;  }, 0, 4, 0.5);
            Tabulate(x1 => 2 * x1 + 3, 0, 4, 0.5);

            Console.WriteLine();

            fun = Summ(Sqr, Cube);
            Tabulate(fun, 0, 4, 0.5);

            Console.WriteLine();


            Console.WriteLine("Интеграл x^2 от 0 до 1 = {0}", Integrate(Sqr, 0, 1, 0.01));
            Console.WriteLine("Интеграл x^3 от 0 до 1 = {0}", Integrate(new Function(Cube), 0, 1, 0.01));

            Console.ReadLine();
        }

        public static Function Summ(Function f1, Function f2)
        {
            return delegate(double x) { return f1(x) + f2(x); };
        }

        public static Function Multyply(Function f1, Function f2)
        {
            return x => f1(x) * f2(x);
        }

        public static void Tabulate(Function f, double a, double b, double dx)
        {
            Console.WriteLine("------ X ------ Y ------");
            var x = a;
            while (x <= b)
            {
                var y = f(x);
                Console.WriteLine("| {0,8:f3} | {1,8:f3} |", x, y);
                x += dx;
            }
            Console.WriteLine("------------------------");
        }

        public static double Integrate(Function f, double a, double b, double dx)
        {
            var x = a;
            var sum = 0d;

            while (x < b)
            {
                var f1 = f(x);
                var f2 = f(x + dx);
                sum += dx * (f1 + f2) / 2;
                x += dx;
            }

            return sum;
        }


        private static double Sqr(double x)
        {
            return x * x;
        }

        private static double Cube(double x)
        {
            return x * x * x;
        }
    }

    /// <summary>
    /// Вещественная функция
    /// </summary>
    /// <param name="x">Аргумент функции</param>
    /// <returns>Значение функции</returns>
    public delegate double Function(double x);
}
