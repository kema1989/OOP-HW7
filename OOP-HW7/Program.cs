using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;

namespace OOP_HW7
{
    class Program
    {
        static void Main(string[] args)
        {
            #region Task5
            //var students = new List<Student>()
            //{
            //      new Student() { Name = "Alice", Age = 18, Course = 1 },
            //      new Student() { Name = "Nick", Age = 21, Course = 1 },
            //      new Student() { Name = "Hannah", Age = 22, Course = 4 },
            //      new Student() { Name = "Roman", Age = 21, Course = 4 },
            //      new Student() { Name = "Aigul", Age = 23, Course = 4 }
            //};

            //var teachers = new List<Teacher>()
            //{
            //    new Teacher() { Name = "Olesya", Age = 29 },
            //    new Teacher() { Name = "Michael", Age = 31 }
            //};

            //StaticClass.RandomGroups(students, teachers, out var list);

            //StaticClass.MakeConnections(list, students, teachers);

            //foreach (var teacher in teachers)
            //{
            //    Console.WriteLine($"{teacher.Name}, {teacher.Age} ведет следующим студентам:");
            //    foreach (var student in teacher.students)
            //        Console.WriteLine($"{student.Name}, {student.Age}, {student.Course}");
            //}

            //foreach (var student in students)
            //    Console.WriteLine($"{student.Name} учится у {student.teacher.Name}");
            #endregion
            var students = new Student[]
            {
                  new Student() { Name = "Alice", Age = 18, Course = 1 },
                  new Student() { Name = "Nick", Age = 21, Course = 1 },
                  new Student() { Name = "Hannah", Age = 22, Course = 4 },
                  new Student() { Name = "Roman", Age = 21, Course = 4 },
                  new Student() { Name = "Aigul", Age = 23, Course = 4 }
            };

            //Array.Sort(students);
            //Array.Sort(students, Student.SortByNameLength);

            (var index, var min) = StaticClass.MinIndex<Student>(students);
            Console.WriteLine(index);
            Console.WriteLine(min);

            //foreach (var student in students)
            //    Console.WriteLine(student.Name);

            //Persons persons = new Persons();

            //persons.Add(new Student() { Name = "Alice", Age = 18, Course = 1 });
            //persons.Add(new Student() { Name = "Nick", Age = 21, Course = 1 });
            //persons.Add(new Student() { Name = "Roman", Age = 21, Course = 4 });
            //persons.Add(new Teacher() { Name = "Olesya", Age = 29 });
            //persons.Add(new Teacher() { Name = "Michael", Age = 31 });

            //foreach (var person in persons)
            //    Console.WriteLine($"{persons.list.IndexOf(person) + 1}) {person.Name}");
        }

        //---------------------------------------------------------------
        class Person : ICloneable
        {
            public string Name { get; set; }
            private int age;
            public int Age
            {
                get { return age; }
                set { if (value < 0) value = 0; age = value; }
            }

            public override string ToString()
            {
                return $"Name: {Name}, Age: {Age}";
            }

            public override bool Equals(object obj)
            {
                return base.Equals(obj);
            }

            public override int GetHashCode()
            {
                return base.GetHashCode();
            }

            public virtual void Print()
            {
                Console.WriteLine("Name: {0}, Age: {1}", Name, Age);
            }

            public virtual object Clone()
            {
                return new Person() { Name = this.Name, Age = this.Age };
            }
        }

        //---------------------------------------------------------------
        class Teacher : Person, ICloneable
        {
            public List<Student> students;
            public override void Print()
            {
                Console.WriteLine($"Name: {Name}, Age: {Age}\nStudents:");
                foreach (var student in students)
                    student.Print();
            }

            public Teacher(params Student[] students)
            {
                this.students = students.ToList<Student>();
            }

            public static Teacher RandomTeacher(Person[] people)
            {
                var teachers = new List<Teacher>();

                foreach (var person in people)
                {
                    if (person is Teacher)
                        teachers.Add(person as Teacher);
                }

                var random = new Random();

                return teachers[random.Next(0, teachers.Count)];
            }

            public override object Clone()
            {
                return new Teacher { Name = this.Name, Age = this.Age, students = this.students };
            }
        }

        //---------------------------------------------------------------
        class Student : Person, ICloneable, IComparable<Student>
        {
            public class StudentAgeComparer : IComparer<Student>
            {
                public int Compare(Student x, Student y)
                {
                    if (x.Age > y.Age)
                        return 1;
                    else if (x.Age == y.Age)
                        return 0;
                    else
                        return -1;
                }
            }

            public class StudentNameComparer : IComparer<Student>
            {
                public int Compare(Student x, Student y)
                {
                    if (x.Name.Length > y.Name.Length)
                        return 1;
                    else if (x.Name.Length == y.Name.Length)
                        return 0;
                    else
                        return -1;
                }
            }

            public static IComparer SortByAge { get { return (IComparer)new StudentAgeComparer(); } }

            public static IComparer SortByNameLength { get { return (IComparer)new StudentNameComparer(); } }

            public Teacher teacher;
            public int Course { get; set; }

            public override void Print()
            {
                Console.WriteLine("Name: {0}, Age: {1}, Course: {2}", Name, Age, Course);
            }

            public static Student RandomStudent(Person[] people)
            {
                var students = new List<Student>();

                foreach (var person in people)
                {
                    if (person is Student)
                        students.Add(person as Student);
                }

                var random = new Random();

                return students[random.Next(0, students.Count)];
            }

            public override object Clone()
            {
                return new Student { Name = this.Name, Age = this.Age, Course = this.Course, teacher = this.teacher };
            }

            public int CompareTo(Student other)
            {
                return this.Name.CompareTo(other.Name);
            }
        }

        static class StaticClass
        {
            public static void RandomGroups(List<Student> students, List<Teacher> teachers, out Dictionary<Student, Teacher> list)
            {
                list = new Dictionary<Student, Teacher>();
                var random = new Random();
                foreach (var student in students)
                {
                    list.Add(student, teachers[random.Next(0, teachers.Count)]);
                }
            }

            public static void MakeConnections(Dictionary<Student, Teacher> list, List<Student> students, List<Teacher> teachers)
            {
                foreach (var pair in list)
                {
                    foreach (var teacher in teachers)
                    {
                        if (pair.Value.Name == teacher.Name)
                            teacher.students.Add(pair.Key);
                    }

                    foreach (var student in students)
                    {
                        if (pair.Key.Name == student.Name)
                            student.teacher = pair.Value;
                    }
                }
            }

            public static (int, T) MinIndex<T>(T[] array) where T: IComparable<T>
            {
                if (array.Length < 1)
                    return (-1, default(T));
                else
                    return (Array.IndexOf(array, array.Min()), array.Min());
            }
        }

        class Persons : IEnumerable<Person>
        {
            public List<Person> list = new List<Person>();

            public void Add(Person p)
            {
                list.Add(p);
            }

            public IEnumerator<Person> GetEnumerator()
            {
                for (int i = 0; i < list.Count; i++)
                    yield return list[i];
            }

            IEnumerator IEnumerable.GetEnumerator()
            {
                throw new NotImplementedException();
            }
        }
    }
}
