using ICSServer.Models;
using Microsoft.EntityFrameworkCore;
using System;

namespace ICSServer.Database
{
    public class DBContext:DbContext
    {
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }
        public DbSet<AcademicDegree> AcademicDegrees { get; set; }
        
        public DbSet<Teacher> Teachers { get; set; }
        public DbSet<Consultation> Consultations { get; set; }
        public DbSet<Course> Courses { get; set; }
        public DbSet<Specialty> Specialties { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<Publics> Publics { get; set; }
        public DbSet<Departament> Departaments { get; set; }
        public DbSet<News> News { set; get; }
        
        public DbSet<Semester> Semesters { set; get; }
        public DbSet<Module> Modules { set; get; }
      

        public DbSet<File> Files { set; get; }
        public DBContext(DbContextOptions<DBContext> options)
         : base(options)
        {

            Database.EnsureCreated();
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //главный - доступ ко всему
            string GeneralRoleName = "General";
            //creator - доступ к просмотру, созданию, редактированию и удалению
            //edito - доступ к просмотру, редактированию и удалению
            string newsCreatorRoleName = "NewsCreator";
            string newsEditorRoleName = "NewsEditor";
            string teacherCreatorRoleName = "TeachersCreator";
            string teacherEditorRoleName = "TeachersEditor";
            string DepartamentCreatorRoleName = "DepartamentCreator";//departament- роли могути также работать с курсами, группами и специальностями
            string DepartamentEditorRoleName = "DepartamentEditor";
            string UsersAdminRoleName = "UsersAdmin";


            string adminEmail = "general";
            string adminPassword = "general";
            string salt = BCrypt.GenerateSalt();
            string HashPassword = BCrypt.HashPassword(adminPassword, salt);
            adminPassword = HashPassword;

            // добавляем роли
            Role GeneralRole = new Role { Id = 1, Name = GeneralRoleName, Description="" };
            Role newsCreatorRole = new Role { Id = 2, Name = newsCreatorRoleName };
            Role newsEditorRole = new Role { Id = 3, Name = newsEditorRoleName };
            Role teacherCreatorRole = new Role { Id = 4, Name = teacherCreatorRoleName };
            Role teacherEditorRole = new Role { Id = 5, Name = teacherEditorRoleName };
            Role DepartamentCreatorRole = new Role { Id = 6, Name = DepartamentCreatorRoleName };
            Role DepartamentEditorRole = new Role { Id = 7, Name = DepartamentEditorRoleName };
            Role UsersAdminRole = new Role {Id=8,Name=UsersAdminRoleName };

            User generalUser = new User { Id = 1, Login = adminEmail, Password = adminPassword, RoleId = GeneralRole.Id };
            
            //добавляем роли в бд
            modelBuilder.Entity<Role>().HasData(new Role[] { GeneralRole, newsCreatorRole, newsEditorRole, teacherCreatorRole,
                teacherEditorRole, DepartamentCreatorRole, DepartamentEditorRole,UsersAdminRole});
            //добавляем админ в бд
            modelBuilder.Entity<User>().HasData(new User[] { generalUser });
            //сразу создадим 4 курса
            Course one = new Course { Id = 1, CourseNumber = 1 };
            Course two = new Course { Id = 2, CourseNumber = 2 };
            Course three = new Course { Id = 3, CourseNumber = 3 };
            Course four = new Course { Id = 4, CourseNumber = 4 };
            modelBuilder.Entity<Course>().HasData(new Course[] { one, two,three,four });
            //и пару специальностей
            Specialty at = new Specialty { Id = 1, SpecialtyCode = "AT", Name="Автоматизация и компютерно-интегрированные технологии",SpecialtyNumberCode=151 };
            Specialty ac = new Specialty { Id = 2, SpecialtyCode = "Аи", Name= "Инженерия программного обеспечения", SpecialtyNumberCode=122 };
            modelBuilder.Entity<Specialty>().HasData(new Specialty[] { at,ac });
            //А ЕЩЕ закреейтим несколько степеней
            AcademicDegree bakalavr = new AcademicDegree { Id = 1, Name = "Бакалавр" };
            AcademicDegree professor = new AcademicDegree { Id = 2, Name = "Профессор" };
            AcademicDegree doctor = new AcademicDegree { Id = 3, Name = "Доктор Наук" };
            AcademicDegree kand = new AcademicDegree { Id = 4, Name = "Кандидат Наук" };
            modelBuilder.Entity<AcademicDegree>().HasData(new AcademicDegree[] { bakalavr, professor,doctor,kand});
            

            base.OnModelCreating(modelBuilder);
        }
                            
              

        
    }

    
}
