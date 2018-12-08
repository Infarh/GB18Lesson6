using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
//using System.IO.Compression;

namespace IOTestingProject
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Stream data = null;
            FileStream file_stream = null;
            BufferedStream buffered_stream;
            MemoryStream memory_stream;
            System.IO.Compression.DeflateStream deflate_stream;
            System.IO.Compression.GZipStream gzip_stream;

            //byte[] buffer = new byte[1024];
            //var readed = data.Read(buffer, 0, buffer.Length);
            //data.Write(buffer, 0, buffer.Length);

            //StreamReader text_reader = new StreamReader(file_stream, Encoding.Default);
            //StreamWriter text_writer = new StreamWriter(data, Encoding.UTF32);

            //string str = text_reader.ReadLine();
            //text_reader.ReadToEnd();
            //var ch = (char)text_reader.Read();
            //text_writer.WriteLine();

            BinaryReader bin_reader;
            BinaryWriter bin_writer;

            //bin_reader.BaseStream
            //bin_reader.ReadBoolean()
            //bin_reader.ReadInt16()
            //bin_reader.ReadInt32()
            //bin_reader.ReadInt64()
            //bin_reader.ReadDouble()

            const string DataFileName = "Data.csv";
            const string CopyDataFileName = "Data(1).xlsx";

            //CopyFile(DataFileName, CopyDataFileName);

            //File.Copy(DataFileName, CopyDataFileName, true);
            //File.Create()
            //File.AppendAllLines();
            //File.Encrypt();
            //File.Decrypt();
            //File.Delete();
            //File.Open()

            //Directory.CreateDirectory()
            //Directory.Delete();
            //Directory.GetFiles()
            //Directory.GetCurrentDirectory()
            //Environment.CurrentDirectory

            //Path.Combine("c:\\", "123", "data_file.txt");

            //Path.GetFileNameWithoutExtension()
            //Path.ChangeExtension("c:\\123\\data_file.txt", ".xml")

            //FileInfo file = new FileInfo(DataFileName)
            //{
            //    CreationTime = DateTime.Now.Add(TimeSpan.FromDays(365))
            //};

            ////file.Create()
            ////file.

            //DirectoryInfo dir = new DirectoryInfo(Environment.CurrentDirectory);

            //foreach (var f in dir.EnumerateFiles("*.xlsx"))
            //{
            //    Console.WriteLine("{0} - {1} - {2:d}", f.Name, f.Length, f.CreationTime);
            //}

            //DriveInfo[] drive = DriveInfo.GetDrives();

            var students = ReadData(DataFileName);

            students.Sort(new Comparison<Student>(NameComare));
            students.Sort(AgeComare);

            System.Collections.ArrayList list = new ArrayList
            {
                "qwe",
                123,
                students
            };

            int int_var = (int)list[1] + 3;

            List<object> universal_list = new List<object>();



            LinkedList<Student> linked_students = new LinkedList<Student>(students);

            //foreach (var student in linked_students)
            //{
            //}
            Stack<Student> stack = new Stack<Student>();
            Queue<Student> queue = new Queue<Student>();
            Dictionary<string, Student> students_dictionary = new Dictionary<string, Student>();


            var linked_node = linked_students.First;
            while (linked_node.Next != null)
            {
                var student = linked_node.Value;
                stack.Push(student);
                //stack.Pop()
                //stack.Peek();

                queue.Enqueue(student);
                //queue.Dequeue()

                var name = student.Name.FirstName;
                if (!students_dictionary.ContainsKey(name))
                    students_dictionary.Add(name, student);

                students_dictionary[name] = student;

                student = students_dictionary[name];

                if(!students_dictionary.TryGetValue(name, out student))
                    Console.WriteLine("Студент с именем {0} отсутствует");

                Console.WriteLine(student);
                linked_node = linked_node.Next;
            }


            Console.ReadLine();
        }

        private static int NameComare(Student s1, Student s2)
        {
            return string.Compare(s1.Name.FirstName, s2.Name.FirstName);
        }

        private static int AgeComare(Student s1, Student s2)
        {
            return Comparer<int>.Default.Compare(s1.Age, s2.Age);
        }

        private static List<Student> ReadData(string FileName)
        {
            if (FileName == null) throw new ArgumentNullException(nameof(FileName));
            if (!File.Exists(FileName)) throw new FileNotFoundException("Файл с данными не найден", FileName);

            List<Student> students = new List<Student>();

            using (var data = new FileStream(FileName, FileMode.Open, FileAccess.Read))
            using (var buffered_data = new BufferedStream(data, 10 * 1024))
            using (var reader = new StreamReader(buffered_data))
            {
                if (!reader.EndOfStream)
                    reader.ReadLine();
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    if (string.IsNullOrWhiteSpace(line)) continue;

                    const char determiner = ';';
                    var components = line.Split(determiner);
                    var student = new Student(components);
                    students.Add(student);
                }
            }

            return students;
        }

        private static void CopyFile(string DataFileName, string CopyDataFileName)
        {
            using (var source_stream = new FileStream(DataFileName, FileMode.Open, FileAccess.Read, FileShare.Read))
            using (var destination_stream = new FileStream(CopyDataFileName, FileMode.Create, FileAccess.Write, FileShare.None))
            using (var reader = new BinaryReader(source_stream))
            using (var writer = new BinaryWriter(destination_stream))
            {
                while (source_stream.Position != source_stream.Length)
                {
                    var @byte = reader.ReadByte();
                    writer.Write(@byte);
                }
            }
        }
    }

    internal class Student
    {
        public struct StudentName
        {
            public string FirstName;
            public string SecondName;
            public override string ToString()
            {
                return $"{FirstName} {SecondName}";
            }
        }

        public StudentName Name { get; }

        public int Age { get; }

        public string Univercity { get; }

        public string Faculty { get; }

        public string Departament { get; }

        public int Course { get; }

        public string Group { get; }

        public string City { get; }

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
