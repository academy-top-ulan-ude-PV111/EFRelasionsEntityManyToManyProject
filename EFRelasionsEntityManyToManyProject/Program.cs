using Microsoft.EntityFrameworkCore;

namespace EFRelasionsEntityManyToManyProject
{
    public class Student
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public List<Modul> Moduls { set; get; } = new List<Modul>();
        public List<ModulStudent> ModulsStudents { set; get; } = new();
    }

    public class Modul
    {
        public int Id { get; set; }
        public string? Title { get; set; }
        public List<Student> Students { set; get; } = new List<Student>();
        public List<ModulStudent> ModulsStudents { set; get; } = new();
    }

    public class ModulStudent
    {
        public int StudentId { get; set; }
        public Student Student { set; get; }
        public int ModulId { get; set; }
        public Modul Modul { set; get; }
        public int Level { set; get; }
    }

    public class AppContext : DbContext
    {
        public DbSet<Student> Students { get; set; } = null!;
        public DbSet<Modul> Moduls { get; set; } = null!;
        public AppContext()
        {
            //Database.EnsureDeleted();
            //Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=AcademyDb;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            /*
            modelBuilder.Entity<Student>()
                        .HasMany(s => s.Moduls)
                        .WithMany(m => m.Students)
                        .UsingEntity(j => j.ToTable("ModulsStudents"));
            */
            modelBuilder.Entity<Modul>()
                        .HasMany(m => m.Students)
                        .WithMany(s => s.Moduls)
                        .UsingEntity<ModulStudent>(
                        j => j.HasOne(ms => ms.Student)
                              .WithMany(st => st.ModulsStudents)
                              .HasForeignKey(ms => ms.StudentId),
                        j => j.HasOne(ms => ms.Modul)
                              .WithMany(md => md.ModulsStudents)
                              .HasForeignKey(ms => ms.ModulId),
                        j =>
                        {
                            j.Property(ms => ms.Level).HasDefaultValue(3);
                            j.HasKey(t => new {t.StudentId, t.ModulId });
                            //j.ToTable("ModulsStudents");
                        }
                        );
        }
    }
        internal class Program
    {
        static void Main(string[] args)
        {
            /*
            using(AppContext context = new())
            {
                Student[] students = new Student[]
                {
                    new Student(){ Name = "Bob"},
                    new Student(){ Name = "Jim"},
                    new Student(){ Name = "Sam"},
                };
                context.Students.AddRange(students);
                context.SaveChanges();

                Modul[] moduls = new Modul[]
                {
                    new Modul(){ Title = "Programing with C++"},
                    new Modul(){ Title = "Programing with C#"},
                    new Modul(){ Title = "Algorithm and Data"},
                };
                context.Moduls.AddRange(moduls);
                context.SaveChanges();


                students[0].Moduls.Add(moduls[0]);
                students[0].Moduls.Add(moduls[2]);

                moduls[1].Students.Add(students[1]);
                moduls[1].Students.Add(students[2]);

                students[1].Moduls.Add(moduls[2]);
                students[2].Moduls.Add(moduls[0]);

                context.SaveChanges();
            }
            */
            /*
            using (AppContext context = new())
            {
                Student student = context.Students.FirstOrDefault();
                context.Students.Remove(student);
                context.SaveChanges();
            }
            */
            /*
            using (AppContext context = new())
            {
                var students = context.Students.Include(s => s.Moduls).ToList();

                foreach(var student in students)
                {
                    Console.WriteLine($"Student: {student.Name}");
                    foreach(var modul in student.Moduls)
                        Console.WriteLine($"\tModul: {modul.Title}");
                    Console.WriteLine();
                }
                Console.WriteLine("-------------------");

                var moduls = context.Moduls.Include(m => m.Students).ToList();
                foreach (var modul in moduls)
                {
                    Console.WriteLine($"Modul: {modul.Title}");
                    foreach (var student in modul.Students)
                        Console.WriteLine($"\tStudent: {student.Name}");
                    Console.WriteLine();
                }
            }
            */
            /*
            int StId = 2;
            int MdId = 3;

            using(AppContext context = new())
            {
                var student = context.Students.Include(s => s.ModulsStudents).FirstOrDefault(s => s.Id == StId);
                var modul = context.Moduls.FirstOrDefault(m => m.Id == MdId);

                student.ModulsStudents.FirstOrDefault(ms => ms.Modul == modul).Level = 5;
                context.SaveChanges();
            }
            */
            using (AppContext context = new())
            {
                var students = context.Students.Include(s => s.Moduls).ToList();
                foreach(var student in students)
                {
                    Console.WriteLine($"Student: {student.Name}");
                    foreach(var sm in student.ModulsStudents)
                        Console.WriteLine($"\tModul: {sm.Modul.Title} Level: {sm.Level}");
                }
            }
                

        }
    }
}