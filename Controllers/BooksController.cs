using Microsoft.AspNetCore.Mvc;

using Task5.Models;
using Task5.Services;

namespace Task5.Controllers
{
    public class BooksController : Controller
    {
        private readonly BookGeneratorService _bookService;

        public BooksController()
        {
            _bookService = new BookGeneratorService();
        }

        // Отображение главной страницы
        public IActionResult Index()
        {
            var model = new BooksViewModel
            {
                Locale = "en_US", // значение по умолчанию
                UserSeed = 42,
                AvgLikes = 3.7,
                AvgReviews = 4.7,
                PageNumber = 1,
                Books = _bookService.GenerateBooks("en_US", 42, 1, 20, 3.7, 4.7)
            };
            return View(model);
        }

        // Метод для подгрузки дополнительных страниц (AJAX)
        [HttpGet]
        public IActionResult LoadMore(string locale, int userSeed, double avgLikes, double avgReviews, int pageNumber)
        {
            // Для первой страницы 20 записей, далее – по 10
            int count = pageNumber == 1 ? 20 : 10;
            List<Book> books = _bookService.GenerateBooks(locale, userSeed, pageNumber, count, avgLikes, avgReviews);
            return PartialView("_BooksPartial", books);
        }
    }
}
