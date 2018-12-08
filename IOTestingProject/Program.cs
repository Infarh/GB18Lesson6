using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
//using System.IO.Compression;

namespace IOTestingProject
{
    /// <summary>Программа демонстрации работы с потоками данных</summary>
    internal class Program
    {
        /// <summary>Точка входа в программу</summary>
        /// <param name="args">Аргументы командной строки</param>
        private static void Main(string[] args)
        {
            #region Виды потоков данных

            Stream data = null;
            FileStream file_stream = null;
            BufferedStream buffered_stream;
            MemoryStream memory_stream;
            System.IO.Compression.DeflateStream deflate_stream;
            System.IO.Compression.GZipStream gzip_stream;

            #endregion

            #region Пример работы с сырыми потоками

            //byte[] buffer = new byte[1024];
            //var readed = data.Read(buffer, 0, buffer.Length);
            //data.Write(buffer, 0, buffer.Length); 

            #endregion

            #region Объекты для работы с потоками для чтения текстовых данных

            //StreamReader text_reader = new StreamReader(file_stream, Encoding.Default);
            //StreamWriter text_writer = new StreamWriter(data, Encoding.UTF32);

            //string str = text_reader.ReadLine();
            //text_reader.ReadToEnd();
            //var ch = (char)text_reader.Read();
            //text_writer.WriteLine(); 

            #endregion

            #region Объекты для работы с двоичными данными

            BinaryReader bin_reader;
            BinaryWriter bin_writer;

            //bin_reader.BaseStream
            //bin_reader.ReadBoolean()
            //bin_reader.ReadInt16()
            //bin_reader.ReadInt32()
            //bin_reader.ReadInt64()
            //bin_reader.ReadDouble() 

            #endregion

            const string DataFileName = "Data.csv";
            const string CopyDataFileName = "Data(1).xlsx";

            // Наш собственный метод для побайтового копирования файлов
            //CopyFile(DataFileName, CopyDataFileName);

            // Стандартный метод копирования файлов
            //File.Copy(DataFileName, CopyDataFileName, true);
            //File.Create()             // Создаёт новый файл и возвращает FileStream в который сразу же можно писать данные
            //File.AppendAllLines()     // Добавляет все указанные строки в конец файла
            //File.Encrypt();           // Зашифровать файл с учётными данными текущего пользователя и текущей машины
            //File.Decrypt();           // Расшифровать....
            //File.Delete();            // Удалить файл
            //File.Open()               // Открыть файл для чтения/записи

            // Класс для работы с каталогами
            //Directory.CreateDirectory()      // Создать каталог
            //Directory.Delete();              // Удалить
            //Directory.GetFiles()             // Получить все файлы каталога в виде массива
            //Directory.GetCurrentDirectory()  // Текущий каталог
            //Environment.CurrentDirectory     // Тоже текущий каталог. Environment - очень полезный класс информации о системе 

            // Класс для работы с путями к файлам
            //Path.Combine("c:\\", "123", "data_file.txt");           // Собрать путь к файлу из частей
            //Path.GetFileNameWithoutExtension()                      // Получить имя файла без расширения
            //Path.ChangeExtension("c:\\123\\data_file.txt", ".xml")  // Заменить расширение файла

            // Создаём объект с информацией о файле (в конструктор передаём строку с путём к файлу)
            //FileInfo file = new FileInfo(DataFileName)
            //file.CreationTime = DateTime.Now.Add(TimeSpan.FromDays(365)); // Меняем дату создания файла

            ////file.Create() // Можем создать файл

            // Файл с информацией о каталоге
            //DirectoryInfo dir = new DirectoryInfo(Environment.CurrentDirectory);

            // Перечисляем файлы в каталоге
            //foreach (var f in dir.EnumerateFiles("*.xlsx"))
            //{
            //    Console.WriteLine("{0} - {1} - {2:d}", f.Name, f.Length, f.CreationTime);
            //}

            // Объект с информацией о дисках
            //DriveInfo[] drive = DriveInfo.GetDrives();

            // Считываем информацию о студентах
            var students = ReadData(DataFileName);

            // Сортируем студентов по имени
            students.Sort(new Comparison<Student>(NameComare));
            // Сортируем студнтов по возрасту
            students.Sort(AgeComare);
            // Сортируем студентов по городу
            // Метод сортировки указываем на месте через лямда-выражение
            students.Sort((s1, s2) => string.CompareOrdinal(s1.City, s1.City));

            // Нетипизированный список
            System.Collections.ArrayList list = new ArrayList
            {
                "qwe",
                123,
                students
            };

            // Придётся использовать приведение типов
            int int_var = (int)list[1] + 3;

            // Типизированный список с универсальным типом ячеек
            List<object> universal_list = new List<object>();

            // Связный список
            LinkedList<Student> linked_students = new LinkedList<Student>(students);

            //foreach (var student in linked_students)
            //{
            //}
            Stack<Student> stack = new Stack<Student>();  // Стек
            Queue<Student> queue = new Queue<Student>();  // Очередь
            Dictionary<string, Student> students_dictionary = new Dictionary<string, Student>(); // Словарь

            // По связному списку можно перемещаться перебирая его узлы
            var linked_node = linked_students.First; // Список хранит ссылки на первый и последний элемент
            while (linked_node.Next != null) // До тех пор пока следующий узел очередного узла не пуст
            {
                // Берём значение узла
                var student = linked_node.Value;
                stack.Push(student); // Добавить элемент в стек
                //stack.Pop()        // Излвечь элемент из стека
                //stack.Peek();      // Посмотреть первый элемент стека

                queue.Enqueue(student); // Добавить элемент в очередь
                //queue.Dequeue()       // Извлечь элемент из очереди

                var name = student.Name.FirstName;
                // Осторожно добавляем элемент в словарь:
                if (!students_dictionary.ContainsKey(name)) // Проверяем - есть ли уже такой ключ в словаре?
                    students_dictionary.Add(name, student); // если ключа не было, то добавляем новое значение

                // Замещаем элемент в словаре. Если там что-то было, то плевать. Если не было, то будет.
                students_dictionary[name] = student;

                // Извлекаем из словаря элемент (небрежно)
                student = students_dictionary[name]; // Если ключа там не было, то тут будет ошибка

                // Осторожно извлекаем элемент из словаря. Так надо действовать в большенстве случаев
                if (!students_dictionary.TryGetValue(name, out student))    // Если нам не удалось найти элемент в словаре
                    Console.WriteLine("Студент с именем {0} отсутствует"); // то вощмущаемся. Или, например, добавляем новый элемент в словарь

                Console.WriteLine(student);
                // Перемещаемся к следующему элементу связанного списка
                linked_node = linked_node.Next;
            }

            Console.ReadLine();
        }

        /// <summary>Метод сравнения студентов по имени</summary>
        private static int NameComare(Student s1, Student s2)
        {
            return string.Compare(s1.Name.FirstName, s2.Name.FirstName);
        }

        /// <summary>Метод сравнения двух студентов по возрасту</summary>
        private static int AgeComare(Student s1, Student s2)
        {
            return Comparer<int>.Default.Compare(s1.Age, s2.Age);
        }

        /// <summary>Читаем данные о списке студентов из файла</summary>
        /// <param name="FileName">Путь к файлу с данными списка студентов</param>
        /// <returns>Список студентов</returns>
        private static List<Student> ReadData(string FileName)
        {
            if (FileName == null) throw new ArgumentNullException(nameof(FileName));
            if (!File.Exists(FileName)) throw new FileNotFoundException("Файл с данными не найден", FileName);

            var students = new List<Student>();

            using (var data = new FileStream(FileName, FileMode.Open, FileAccess.Read))
            using (var buffered_data = new BufferedStream(data, 10 * 1024))
            using (var reader = new StreamReader(buffered_data))
            {
                // Пропускаем первую строку файла
                if (!reader.EndOfStream)
                    reader.ReadLine();
                // Читаем файл до конца
                while (!reader.EndOfStream)
                {
                    // Считываем из файла одну строку
                    var line = reader.ReadLine();
                    // Если строка пуста, то пропускаем итерацию
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    const char determiner = ';';
                    // Разбиваем строку по символу-разделителю
                    var components = line.Split(determiner);
                    // Создаём объект-студент
                    var student = new Student(components);
                    // Добавляем студента в список.
                    students.Add(student);
                }
            }

            return students;
        }

        /// <summary>Скопировать файл</summary>
        /// <param name="DataFileName">Путь исходного файла</param>
        /// <param name="CopyDataFileName">Путь конечного файла</param>
        private static void CopyFile(string DataFileName, string CopyDataFileName)
        {
            // FileStream, BinaryReader, BinaryWriter - все классы реализуют интерфейс IDisposable. Значит предназначены для конструкции using!
            using (var source_stream = new FileStream(DataFileName, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (var destination_stream = new FileStream(CopyDataFileName, FileMode.Create, FileAccess.Write, FileShare.None))
            using (var reader = new BinaryReader(source_stream))
            using (var writer = new BinaryWriter(destination_stream))
                while (source_stream.Position != source_stream.Length)
                {
                    var @byte = reader
                        .ReadByte(); // "byte" является ключевым словом. Его нельзя использовать просто так. Поэтому добавляем @ в начале.
                    writer.Write(@byte);
                }
        }
    }

    /// <summary>Студент</summary>
    internal class Student
    {
        /// <summary>Имя студента</summary>
        public struct StudentName
        {
            /// <summary>Имя студента</summary>
            public string FirstName;
            /// <summary>Фамилия студента</summary>
            public string SecondName;
            public override string ToString()
            {
                return $"{FirstName} {SecondName}";
            }
        }

        /// <summary>Имя студента</summary>
        public StudentName Name { get; }

        /// <summary>Возраст студента</summary>
        public int Age { get; }

        /// <summary>Институт</summary>
        public string Univercity { get; }

        /// <summary>Факультет</summary>
        public string Faculty { get; }

        /// <summary>Кафедра</summary>
        public string Departament { get; }

        /// <summary>Номер курса</summary>
        public int Course { get; }

        /// <summary>Группа</summary>
        public string Group { get; }

        /// <summary>Город</summary>
        public string City { get; }

        /// <summary>Инициализация нового студента</summary>
        /// <param name="data">Массив с элементами данных студента в текстовом виде</param>
        public Student(string[] data)
        {
            if (data == null) throw new ArgumentNullException(nameof(data));
            if (data.Length != 9) throw new ArgumentException("Размер массива данных не равен 9", nameof(data));

            Name = new StudentName
            {
                FirstName = data[0],
                SecondName = data[1]
            };
            Age = int.Parse(data[2]);
            Univercity = data[3];
            Faculty = data[4];
            Departament = data[5];
            Course = int.Parse(data[6]);
            Group = data[7];
            City = data[8];
        }

        public override string ToString()
        {
            return Name.ToString();
        }
    }
}
