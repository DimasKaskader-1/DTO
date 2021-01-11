using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Linq;

namespace ExamPractice
{
    public class Account
    {
        public int Id { get; set; }
        public string Name { get; set; }
        ICollection<BankOperation> Incoming { get; set; }
        ICollection<BankOperation> Outgoing { get; set; }
    }
    public class BankOperation
    {
        public int Id { get; set; }
        public int ReceiverId { get; set; }
        public int Amount { get; set; }
        public DateTime DateTime { get; set; }
        public Account Receiver { get; set; }
    }

    public class Context : DbContext
    {
        public Context() : base("exam")
        { }
        public DbSet<Book> Books { get; set; }
        public DbSet<BookReview> Reviews { get; set; }
        public DbSet<Author> Authors { get; set; }
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Conventions.Remove<PluralizingTableNameConvention>();
            modelBuilder.Entity<Book>().HasRequired(x => x.Author).WithMany(x => x.Books).HasForeignKey(x => x.AuthorId);

            modelBuilder.Entity<Book>().Property(d => d.Title).IsRequired().HasMaxLength(512);
            modelBuilder.Entity<BookReview>().Property(x => x.ReviewName).IsRequired();
            modelBuilder.Entity<Author>().Property(x => x.Name).IsRequired();
        }
    }

    public class DTO
    {
        public string AuthorName { get; set; }
        public string BookTitle { get; set; }
        public double Rating { get; set; }
    }
    class Program
    {
        public static Context _context = new Context();
        public static IEnumerable<DTO> GetDTO(int rate)
        {
            return _context.Books.Include(x => x.BookReviews).Include(x => x.Author).Where(x => x.BookReviews.Count > 0)
            .Select(x => new DTO { AuthorName = x.Author.Name, BookTitle = x.Title, Rating = x.BookReviews.Average(y => y.Rating) });
        }


        static void Main(string[] args)
        {
            //var _context = new Context();

            //_context.Authors.Add(new Author { Name = "Author1" });
            //_context.Authors.Add(new Author { Name = "Author2" });
            //_context.Authors.Add(new Author { Name = "Author3" });

            //_context.Books.Add(new Book { AuthorId = 1, Title = "Book1" });
            //_context.Books.Add(new Book { AuthorId = 1, Title = "Book2" });
            //_context.Books.Add(new Book { AuthorId = 2, Title = "Book3" });
            //_context.Books.Add(new Book { AuthorId = 2, Title = "Book4" });
            //_context.Books.Add(new Book { AuthorId = 2, Title = "Book5" });


            //_context.Reviews.Add(new BookReview { BookId = 1, Rating = 7, ReviewName = "name1" });
            //_context.Reviews.Add(new BookReview { BookId = 1, Rating = 8, ReviewName = "name2" });
            //_context.Reviews.Add(new BookReview { BookId = 2, Rating = 1, ReviewName = "name3" });
            //_context.Reviews.Add(new BookReview { BookId = 2, Rating = 6, ReviewName = "name4" });


            //_context.SaveChanges();

            foreach (var dto in GetDTO(5))
            {
                Console.WriteLine($"{dto.AuthorName} {dto.BookTitle} {dto.Rating}");
            }
            Console.ReadLine();
        }
    }
}
