using System;
using System.Collections;
using System.Collections.Generic;
using static Lab4.Program;

namespace Lab4
{

    internal class Program
    {
        class MyMatrix
        {
            private int[,] matrix;
            private int rows;
            private int columns;



            public MyMatrix(int rows, int columns, int minValue, int maxValue)
            {
                if (rows <= 0 || columns <= 0)
                    
                    throw new ArgumentException("Число строк и столбцов должно быть положительным.");

                this.rows = rows;
                this.columns = columns;
                matrix = new int[rows, columns];
                Random random = new Random();

                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < columns; j++)
                    {
                        matrix[i, j] = random.Next(minValue, maxValue + 1);
                    }
                }
            }

            public int this[int row, int column] //Индексатор
            {
                get
                {
                    if (row < 0 || row >= rows || column < 0 || column >= columns)
                        throw new IndexOutOfRangeException("Индекс находится за пределами границ матрицы.");

                    return matrix[row, column];
                }
                set
                {
                    if (row < 0 || row >= rows || column < 0 || column >= columns)
                        throw new IndexOutOfRangeException("Индекс находится за пределами границ матрицы.");

                    matrix[row, column] = value;
                }
            }

            public static MyMatrix operator +(MyMatrix a, MyMatrix b) //Переопределение оператора сложения
            {
                if (a.rows != b.rows || a.columns != b.columns)
                    
                    throw new ArgumentException("Матрицы должны иметь одинаковые размеры для сложения.");

                MyMatrix result = new MyMatrix(a.rows, a.columns, 0, 0);
                for (int i = 0; i < a.rows; i++)
                {
                    for (int j = 0; j < a.columns; j++)
                    {
                        result[i, j] = a[i, j] + b[i, j];
                    }
                }
                return result;
            }

            public static MyMatrix operator -(MyMatrix a, MyMatrix b) //Переопределение оператора вычитания
            {
                if (a.rows != b.rows || a.columns != b.columns)
                    throw new ArgumentException("Матрицы должны иметь одинаковые размеры для вычитания.");

                MyMatrix result = new MyMatrix(a.rows, a.columns, 0, 0);
                for (int i = 0; i < a.rows; i++)
                {
                    for (int j = 0; j < a.columns; j++)
                    {
                        result[i, j] = a[i, j] - b[i, j];
                    }
                }
                return result;
            }

            public static MyMatrix operator *(MyMatrix a, MyMatrix b) //Переопределение оператора умножения матрицы на матрицу
            {
                if (a.columns != b.rows)
                    throw new ArgumentException("Количество столбцов первой матрицы должно быть равно количеству строк второй матрицы.");

                MyMatrix result = new MyMatrix(a.rows, b.columns, 0, 0);
                for (int i = 0; i < a.rows; i++)
                {
                    for (int j = 0; j < b.columns; j++)
                    {
                        for (int k = 0; k < a.columns; k++)
                        {
                            result[i, j] += a[i, k] * b[k, j];
                        }
                    }
                }
                return result;
            }

            public static MyMatrix operator *(MyMatrix matrix, int scalar) ////Переопределение оператора умножения матрицы на число
            {
                MyMatrix result = new MyMatrix(matrix.rows, matrix.columns, 0, 0);
                for (int i = 0; i < matrix.rows; i++)
                {
                    for (int j = 0; j < matrix.columns; j++)
                    {
                        result[i, j] = matrix[i, j] * scalar;
                    }
                }
                return result;
            }

            public static MyMatrix operator /(MyMatrix matrix, int divisor) //Переопределение оператора деления матрицы на число
            {
                if (divisor == 0)
                    throw new DivideByZeroException("Деление на ноль недопустимо.");

                MyMatrix result = new MyMatrix(matrix.rows, matrix.columns, 0, 0);
                for (int i = 0; i < matrix.rows; i++)
                {
                    for (int j = 0; j < matrix.columns; j++)
                    {
                        result[i, j] = matrix[i, j] / divisor;
                    }
                }
                return result;
            }

            public void Print()
            {
                for (int i = 0; i < rows; i++)
                {
                    for (int j = 0; j < columns; j++)
                    {
                        Console.Write(matrix[i, j] + "\t");
                    }
                    Console.WriteLine();
                }
            }
        }


        //====================================================================

        public class Car
        {
            public string Name { get; set; }
            public int ProductionYear { get; set; }
            public int MaxSpeed { get; set; }

            public Car(string name, int productionYear, int maxSpeed)
            {
                Name = name;
                ProductionYear = productionYear;
                MaxSpeed = maxSpeed;
            }
        }

        public class CarComparer : IComparer<Car>
        {
            public enum CompareBy { Name, ProductionYear, MaxSpeed }

            public CompareBy CompareProperty { get; set; }

            public int Compare(Car x, Car y)
            {
                switch (CompareProperty)
                {
                    case CompareBy.Name:
                        return string.Compare(x.Name, y.Name);
                    case CompareBy.ProductionYear:
                        return x.ProductionYear.CompareTo(y.ProductionYear);
                    case CompareBy.MaxSpeed:
                        return x.MaxSpeed.CompareTo(y.MaxSpeed);
                    default:
                        throw new ArgumentException("Invalid CompareBy value");
                }
            }
        }
        /// <summary>
        /// ///////////////////
        /// </summary>
        public class CarCatalog : IEnumerable<Car>
        {
            private readonly Car[] cars;

            public CarCatalog(Car[] cars)
            {
                this.cars = cars;
            }

            public IEnumerator<Car> GetEnumerator() // метод реализует интерфейс и возвращает перечислитель для итерации по элементам массива 
            {
                return ((IEnumerable<Car>)cars).GetEnumerator();
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                return GetEnumerator();
            }

            public IEnumerable<Car> GetReverseEnumerator()// метод принимает год выпуска в качестве параметра и возвращает итератор, который фильтрует элементы массива Car по указанному году выпуска
            {
                for (int i = cars.Length - 1; i >= 0; i--)
                {
                    yield return cars[i];
                }
            }

            public IEnumerable<Car> GetCarsByProductionYear(int year) //метод принимает год выпуска в качестве параметра и возвращает итератор, который фильтрует элементы массива Car по указанному году выпуска

            {
                foreach (var car in cars)
                {
                    if (car.ProductionYear == year)
                    {
                        yield return car;
                    }
                }
            }

            public IEnumerable<Car> GetCarsByMaxSpeed(int maxSpeed) //метод принимает максимальную скорость в качестве параметра и возвращает итератор, который фильтрует элементы массива Car по указанной максимальной скорости
            {
                foreach (var car in cars)
                {
                    if (car.MaxSpeed == maxSpeed)
                    {
                        yield return car;
                    }
                }
            }
        }


        static void Main(string[] args)
        {

            Console.WriteLine("Введите количество строк матрицы:");
            int rows = int.Parse(Console.ReadLine());

            Console.WriteLine("Введите количество столбцов матрицы:");
            int columns = int.Parse(Console.ReadLine());

            Console.WriteLine("Введите минимальное значение элементов матрицы:");
            int minValue = int.Parse(Console.ReadLine());

            Console.WriteLine("Введите максимальное значение элементов матрицы:");
            int maxValue = int.Parse(Console.ReadLine());

            Console.WriteLine("Введите количество строк матрицы:");
            int rows2 = int.Parse(Console.ReadLine());

            Console.WriteLine("Введите количество столбцов матрицы:");
            int columns2 = int.Parse(Console.ReadLine());

            MyMatrix matrix1 = new MyMatrix(rows, columns, minValue, maxValue);
            Console.WriteLine("\nМатрица 1:");
            matrix1.Print();

            MyMatrix matrix2 = new MyMatrix(rows2, columns2, minValue, maxValue);
            Console.WriteLine("\nМатрица 2:");
            matrix2.Print();

            Console.WriteLine("\nСложение матриц:");
            MyMatrix sum = matrix1 + matrix2;
            sum.Print();

            Console.WriteLine("\nВычитание матриц:");
            MyMatrix difference = matrix1 - matrix2;
            difference.Print();

            Console.WriteLine("Введите количество строк матрицы:");
            int rows3 = int.Parse(Console.ReadLine());

            Console.WriteLine("Введите количество столбцов матрицы:");
            int columns3 = int.Parse(Console.ReadLine());

            MyMatrix matrix3 = new MyMatrix(rows3, columns3, minValue, maxValue);
            Console.WriteLine("\nМатрица 1:");
            matrix1.Print();

            Console.WriteLine("Введите количество строк матрицы:");
            int rows4 = int.Parse(Console.ReadLine());

            Console.WriteLine("Введите количество столбцов матрицы:");
            int columns4 = int.Parse(Console.ReadLine());

            MyMatrix matrix4 = new MyMatrix(rows4, columns4, minValue, maxValue);
            Console.WriteLine("\nМатрица 2:");
            matrix2.Print();


            Console.WriteLine("\nУмножение матриц:");
            MyMatrix product = matrix3 * matrix4;
            product.Print();

            Console.WriteLine("\nУмножение матрицы на число:");
            Console.Write("Введите число: ");
            int scalar = int.Parse(Console.ReadLine());
            MyMatrix scaled = matrix1 * scalar;
            scaled.Print();

            Console.WriteLine("\nДеление матрицы на число:");
            Console.Write("Введите число: ");
            int divisor = int.Parse(Console.ReadLine());
            MyMatrix divided = matrix1 / divisor;
            divided.Print();



            //2===============================================================

            Console.WriteLine();
            Console.WriteLine("Задание 2");

            Car[] cars = new Car[]
        {
            new Car("Toyota", 2020, 180),
            new Car("Honda", 2019, 170),
            new Car("Ford", 2021, 200),
            new Car("Chevrolet", 2018, 190),
        };

            CarCatalog catalog = new CarCatalog(cars);

            Console.WriteLine("Сортировка по названию:");
            Array.Sort(cars, new CarComparer { CompareProperty = CarComparer.CompareBy.Name });
            PrintCars(cars);

            Console.WriteLine("\nСортировка по году выпуска:");
            Array.Sort(cars, new CarComparer { CompareProperty = CarComparer.CompareBy.ProductionYear });
            PrintCars(cars);

            Console.WriteLine("\nСортировка по максимальной скорости:");
            Array.Sort(cars, new CarComparer { CompareProperty = CarComparer.CompareBy.MaxSpeed });
            PrintCars(cars);

            //3=======================================================================

            Console.WriteLine();
            Console.WriteLine("Задание 3");

            Console.WriteLine("Прямой проход с первого элемента до последнего:");
            foreach (var car in catalog)
            {
                Console.WriteLine($"Название: {car.Name}, Год выпуска: {car.ProductionYear}, Максимальная скорость: {car.MaxSpeed}");
            }

            Console.WriteLine("\nОбратный проход от последнего к первому:");
            foreach (var car in catalog.GetReverseEnumerator())
            {
                Console.WriteLine($"Название: {car.Name}, Год выпуска: {car.ProductionYear}, Максимальная скорость: {car.MaxSpeed}");
            }

            int yearFilter = 2019;
            Console.WriteLine($"\nПроход по элементам массива с фильтром по году выпуска ({yearFilter}):");
            foreach (var car in catalog.GetCarsByProductionYear(yearFilter))
            {
                Console.WriteLine($"Название: {car.Name}, Год выпуска: {car.ProductionYear}, Максимальная скорость: {car.MaxSpeed}");
            }

            int maxSpeedFilter = 190;
            Console.WriteLine($"\nПроход по элементам массива с фильтром по максимальной скорости ({maxSpeedFilter}):");
            foreach (var car in catalog.GetCarsByMaxSpeed(maxSpeedFilter))
            {
                Console.WriteLine($"Название: {car.Name}, Год выпуска: {car.ProductionYear}, Максимальная скорость: {car.MaxSpeed}");
            }

            Console.ReadLine();

        }
        static void PrintCars(Car[] cars)
        {
            foreach (var car in cars)
            {
                Console.WriteLine($"Название: {car.Name}, Год выпуска: {car.ProductionYear}, Максимальная скорость: {car.MaxSpeed}");
            }
        }
    }
}
