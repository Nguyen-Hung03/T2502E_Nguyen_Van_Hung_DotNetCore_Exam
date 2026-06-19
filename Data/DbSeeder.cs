using ComicSys.Models;
using Microsoft.EntityFrameworkCore;

namespace ComicSys.Data;

public static class DbSeeder
{
    private const int TargetBookCount = 24;
    private const int TargetCustomerCount = 18;
    private const int TargetRentalCount = 35;

    public static void Seed(AppDbContext context)
    {
        context.Database.EnsureCreated();

        SeedComicBooks(context);
        SeedCustomers(context);
        SeedRentals(context);
    }

    private static void SeedComicBooks(AppDbContext context)
    {
        var books = new (string Title, string Author, decimal Price)[]
        {
            ("Doraemon - Tập 1", "Fujiko F. Fujio", 5000),
            ("Thám Tử Lừng Danh Conan - Tập 10", "Gosho Aoyama", 7000),
            ("One Piece - Tập 50", "Eiichiro Oda", 8000),
            ("Naruto - Tập 25", "Masashi Kishimoto", 7500),
            ("Dragon Ball Super - Tập 15", "Akira Toriyama", 6500),
            ("Thần Thoại Olympus - Tập 5", "Cho Olympus", 6000),
            ("Spy x Family - Tập 8", "Tatsuya Endo", 8500),
            ("Kimetsu no Yaiba - Tập 12", "Koyoharu Gotouge", 9000),
            ("Attack on Titan - Tập 20", "Hajime Isayama", 8200),
            ("My Hero Academia - Tập 30", "Kohei Horikoshi", 7800),
            ("Tokyo Revengers - Tập 18", "Ken Wakui", 7200),
            ("Chainsaw Man - Tập 6", "Tatsuki Fujimoto", 8800),
            ("Blue Lock - Tập 14", "Muneyuki Kaneshiro", 7600),
            ("Jujutsu Kaisen - Tập 22", "Gege Akutami", 8400),
            ("Black Clover - Tập 28", "Yuki Tabata", 6800),
            ("Fairy Tail - Tập 35", "Hiro Mashima", 6200),
            ("Bleach - Tập 40", "Tite Kubo", 7100),
            ("Hunter x Hunter - Tập 33", "Yoshihiro Togashi", 9500),
            ("Thỏ Bảy Màu - Tập 3", "Nguyễn Nhật Ánh", 4500),
            ("Thánh Giả Rắc Rối - Tập 7", "Nguyễn Nhật Ánh", 4800),
            ("Solo Leveling - Tập 4", "Chugong", 9200),
            ("Đao Ha Kiếm - Tập 9", "Wuxia Studio", 5500),
            ("Berserk - Tập 11", "Kentaro Miura", 9800),
            ("Vinland Saga - Tập 16", "Makoto Yukimura", 8600)
        };

        var existingTitles = context.ComicBooks.Select(b => b.Title).ToHashSet();

        foreach (var book in books)
        {
            if (existingTitles.Contains(book.Title) || context.ComicBooks.Count() >= TargetBookCount)
            {
                continue;
            }

            context.ComicBooks.Add(new ComicBook
            {
                Title = book.Title,
                Author = book.Author,
                PricePerDay = book.Price
            });
        }

        context.SaveChanges();
    }

    private static void SeedCustomers(AppDbContext context)
    {
        var customers = new (string Name, string Phone, DateTime Date)[]
        {
            ("Nguyễn Văn An", "0901234567", new DateTime(2025, 12, 5)),
            ("Trần Thị Bích", "0912345678", new DateTime(2026, 1, 10)),
            ("Lê Hoàng Minh", "0923456789", new DateTime(2026, 2, 14)),
            ("Phạm Thu Hà", "0934567890", new DateTime(2026, 3, 2)),
            ("Hoàng Quốc Tuấn", "0945678901", new DateTime(2026, 4, 18)),
            ("Võ Ngọc Linh", "0956789012", new DateTime(2026, 5, 6)),
            ("Đặng Minh Khôi", "0967890123", new DateTime(2025, 11, 20)),
            ("Bùi Thảo My", "0978901234", new DateTime(2026, 1, 25)),
            ("Ngô Quang Huy", "0989012345", new DateTime(2026, 2, 8)),
            ("Lý Kim Ngân", "0990123456", new DateTime(2026, 3, 15)),
            ("Trịnh Văn Phúc", "0901122334", new DateTime(2026, 4, 1)),
            ("Mai Lan Chi", "0912233445", new DateTime(2026, 4, 22)),
            ("Phan Đức Thắng", "0923344556", new DateTime(2026, 5, 9)),
            ("Vương Thùy Dung", "0934455667", new DateTime(2026, 5, 28)),
            ("Cao Tiến Dũng", "0945566778", new DateTime(2026, 6, 3)),
            ("Hồ Ngọc Trâm", "0956677889", new DateTime(2026, 6, 8)),
            ("Dương Bảo Nam", "0967788990", new DateTime(2025, 10, 12)),
            ("Lương Thị Mai", "0978899001", new DateTime(2026, 6, 14))
        };

        var existingPhones = context.Customers.Select(c => c.PhoneNumber).ToHashSet();

        foreach (var customer in customers)
        {
            if (existingPhones.Contains(customer.Phone) || context.Customers.Count() >= TargetCustomerCount)
            {
                continue;
            }

            context.Customers.Add(new Customer
            {
                FullName = customer.Name,
                PhoneNumber = customer.Phone,
                RegistrationDate = customer.Date
            });
        }

        context.SaveChanges();
    }

    private static void SeedRentals(AppDbContext context)
    {
        var customers = context.Customers.OrderBy(c => c.CustomerID).ToList();
        var comicBooks = context.ComicBooks.OrderBy(c => c.ComicBookID).ToList();

        if (customers.Count == 0 || comicBooks.Count == 0)
        {
            return;
        }

        ComicBook BookAt(int index) => comicBooks[index % comicBooks.Count];

        if (!context.Rentals.Any())
        {
            AddRentalWithDetails(context, customers[0].CustomerID, new DateTime(2026, 3, 5), new DateTime(2026, 3, 10), "Đã trả",
                (BookAt(0), 2), (BookAt(1), 1));
            AddRentalWithDetails(context, customers[1].CustomerID, new DateTime(2026, 3, 18), new DateTime(2026, 3, 22), "Đã trả",
                (BookAt(2), 1));
            AddRentalWithDetails(context, customers[2].CustomerID, new DateTime(2026, 4, 2), new DateTime(2026, 4, 8), "Đã trả",
                (BookAt(3), 3));
            AddRentalWithDetails(context, customers[3].CustomerID, new DateTime(2026, 4, 10), new DateTime(2026, 4, 15), "Đã trả",
                (BookAt(0), 2), (BookAt(4), 1));
            AddRentalWithDetails(context, customers[4].CustomerID, new DateTime(2026, 4, 20), new DateTime(2026, 4, 25), "Đã trả",
                (BookAt(5), 1));
            AddRentalWithDetails(context, customers[5].CustomerID, new DateTime(2026, 5, 5), new DateTime(2026, 5, 12), "Đã trả",
                (BookAt(6), 2), (BookAt(7), 1));
            AddRentalWithDetails(context, customers[6].CustomerID, new DateTime(2026, 5, 15), new DateTime(2026, 5, 20), "Đã trả",
                (BookAt(8), 1));
            AddRentalWithDetails(context, customers[7].CustomerID, new DateTime(2026, 5, 22), new DateTime(2026, 5, 28), "Đã trả",
                (BookAt(9), 2));
            AddRentalWithDetails(context, customers[8].CustomerID, new DateTime(2026, 6, 1), new DateTime(2026, 6, 10), "Đang thuê",
                (BookAt(10), 1));
            AddRentalWithDetails(context, customers[9].CustomerID, new DateTime(2026, 6, 5), new DateTime(2026, 6, 12), "Đang thuê",
                (BookAt(11), 2));
            AddRentalWithDetails(context, customers[10].CustomerID, new DateTime(2026, 6, 8), new DateTime(2026, 6, 15), "Đang thuê",
                (BookAt(12), 1), (BookAt(13), 1));
            AddRentalWithDetails(context, customers[11].CustomerID, new DateTime(2026, 6, 12), new DateTime(2026, 6, 18), "Đang thuê",
                (BookAt(14), 3));
            AddRentalWithDetails(context, customers[0].CustomerID, new DateTime(2026, 6, 15), new DateTime(2026, 6, 22), "Đang thuê",
                (BookAt(15), 1), (BookAt(16), 2));
        }

        var random = new Random(20260619);
        while (context.Rentals.Count() < TargetRentalCount)
        {
            var customer = customers[random.Next(customers.Count)];
            var month = random.Next(3, 7);
            var day = random.Next(1, 25);
            day = Math.Min(day, DateTime.DaysInMonth(2026, month));
            var rentalDate = new DateTime(2026, month, day);
            var returnDate = rentalDate.AddDays(random.Next(3, 12));
            var status = returnDate < new DateTime(2026, 6, 19) ? "Đã trả" : "Đang thuê";

            var rental = new Rental
            {
                CustomerID = customer.CustomerID,
                RentalDate = rentalDate,
                ReturnDate = returnDate,
                Status = status
            };

            context.Rentals.Add(rental);
            context.SaveChanges();

            var detailCount = random.Next(1, 3);
            for (var i = 0; i < detailCount; i++)
            {
                var book = comicBooks[random.Next(comicBooks.Count)];
                context.RentalDetails.Add(new RentalDetail
                {
                    RentalID = rental.RentalID,
                    ComicBookID = book.ComicBookID,
                    Quantity = random.Next(1, 4),
                    PricePerDay = book.PricePerDay
                });
            }

            context.SaveChanges();
        }
    }

    private static void AddRentalWithDetails(
        AppDbContext context,
        int customerId,
        DateTime rentalDate,
        DateTime returnDate,
        string status,
        params (ComicBook Book, int Quantity)[] details)
    {
        var rental = new Rental
        {
            CustomerID = customerId,
            RentalDate = rentalDate,
            ReturnDate = returnDate,
            Status = status
        };

        context.Rentals.Add(rental);
        context.SaveChanges();

        foreach (var detail in details)
        {
            context.RentalDetails.Add(new RentalDetail
            {
                RentalID = rental.RentalID,
                ComicBookID = detail.Book.ComicBookID,
                Quantity = detail.Quantity,
                PricePerDay = detail.Book.PricePerDay
            });
        }

        context.SaveChanges();
    }
}
