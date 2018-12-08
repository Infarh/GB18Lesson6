using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lesson6
{
    /// <summary>Вещественная функция</summary>
    /// <param name="x">Аргумент функции</param>
    /// <returns>Значение функции</returns>
    public delegate double Function(double x);

    /// <summary>Программа для демонстрации работы с делегатами</summary>
    class Program
    {
        /// <summary>Точка входа в программу</summary>
        /// <param name="args">Аргументы командной строки</param>
        static void Main(string[] args)
        {
            // Делегат - объект, хранящий внутри себя ссылку на функцию. Его надо создавать указывая в параметрах конструктора функцию, которую он захватит 
            Function fun = new Function(Sqr);

            // Параметр функции
            var x = 2;

            // Вызываем делегат через метод Invoke, он, в свою очередь, вызовет функцию
            Console.WriteLine("Значение функции f({0}) = {1}", x, fun.Invoke(x));

            // Заменяем делегат в переменной на новый делегат, который будет работать с новой функцией
            fun = new Function(Sqr);

            // Вызовем новый делегат и получим новый результат
            Console.WriteLine("Значение функции f({0}) = {1}", x, fun.Invoke(x));

            // Можно создавать делегат неявно. Компилятор сам развернёт это в new Function(Sqr);
            fun = Sqr;
            Console.WriteLine("Значение функции f({0}) = {1}", x, fun(x));

            Console.WriteLine();

            // Передаём делегат в функцию. Делегат - объект ссылочного типа
            Tabulate(new Function(Sqr), 0, 4, 0.5);

            Console.WriteLine();

            // А теперь вызовем туже процедуру для другой функции
            Tabulate(Cube, 0, 4, 0.5);

            Console.WriteLine();

            // Если мы нигде не описывали никаких функций, а нам здесь и сейчас нужна функция,
            // мы можем задать её прямо здесь как анонимную
            // delegate(double x1) { return 2 * x1 + 3;  }
            //Tabulate(delegate(double x1) { return 2 * x1 + 3;  }, 0, 4, 0.5);
            // Либо, используя ламда-синтаксис можно упростить запись
            // x1 => 2 * x1 + 3
            // Это лямда-выражение.
            Tabulate(x1 => 2 * x1 + 3, 0, 4, 0.5);

            Console.WriteLine();

            // Можем описать функцию, которая будет вычислять сумму двух функций в виде другой функции
            fun = Summ(Sqr, Cube);
            Tabulate(fun, 0, 4, 0.5);

            Console.WriteLine();


            // Поинтегрируем...
            Console.WriteLine("Интеграл x^2 от 0 до 1 = {0}", Integrate(Sqr, 0, 1, 0.01));
            Console.WriteLine("Интеграл x^3 от 0 до 1 = {0}", Integrate(new Function(Cube), 0, 1, 0.01));

            Console.ReadLine();
        }

        /// <summary>Функция вычисления функции-суммы двух функций</summary>
        /// <param name="f1">Первое слагаемое</param>
        /// <param name="f2">Второе слагаемое</param>
        /// <returns>функция суммы</returns>
        public static Function Summ(Function f1, Function f2)
        {
            // Определяем новую функцию (анонимную), вычисляемую как сумму значений двух исходных функций
            return delegate(double x) { return f1(x) + f2(x); };
        }

        /// <summary>Функция вычисления функции-произведения двух функций</summary>
        /// <param name="f1">Первый сомножитель</param>
        /// <param name="f2">Второй сомножитель</param>
        /// <returns>Функция произведения значений двух исходных функций</returns>
        public static Function Multyply(Function f1, Function f2)
        {
            // Определяем результирующую функцию как лямда-выражение
            return x => f1(x) * f2(x);
        }

        /// <summary>Табулирование функции</summary>
        /// <param name="f">Функция, таблицу значений которой надо построить</param>
        /// <param name="a">Начало интервала расчёта</param>
        /// <param name="b">Окончание интервала расчёта</param>
        /// <param name="dx">Шаг вычисления функции</param>
        /// <exception cref="ArgumentNullException">Если f == null</exception>
        /// <exception cref="InvalidOperationException">Если b-a &lt; dx</exception>
        public static void Tabulate(Function f, double a, double b, double dx)
        {
            if(f is null) throw new ArgumentNullException(nameof(f));
            if(b - a < dx) throw new InvalidOperationException("Интервал расчёта функции меньше шага вычисления её аргумента");

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

        /// <summary>Вычисление интеграла функции методом тропеций</summary>
        /// <param name="f">Функция, интеграл которой требуется вычислить</param>
        /// <param name="a">Начало интервала интегрирования</param>
        /// <param name="b">Конец интервала интегрирования</param>
        /// <param name="dx">Шаг вычисления интеграла</param>
        /// <returns>Значение интеграла</returns>    
        /// <exception cref="ArgumentNullException">Если f == null</exception>
        /// <exception cref="InvalidOperationException">Если b-a &lt; dx</exception>
        public static double Integrate(Function f, double a, double b, double dx)
        {
            if (f is null) throw new ArgumentNullException(nameof(f));
            if (b - a < dx) throw new InvalidOperationException("Интервал расчёта функции меньше шага вычисления её аргумента");

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

        /// <summary>Квадрат значения</summary>
        /// <param name="x">Основание</param>
        /// <returns>Значение квадрата</returns>
        private static double Sqr(double x)
        {
            return x * x;
        }

        /// <summary>Куб значения</summary>
        /// <param name="x">Основание</param>
        /// <returns>Значение куба</returns>
        private static double Cube(double x)
        {
            return x * x * x;
        }
    }
}
