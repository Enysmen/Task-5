using Task5.Models;
using Bogus;

namespace Task5.Services
{
    public class BookGeneratorService
    {
        /// <summary>
        /// Генерация списка книг.
        /// </summary>
        /// <param name="locale">Локаль, например "en_US", "de", "fr"</param>
        /// <param name="userSeed">Начальное значение пользователя</param>
        /// <param name="pageNumber">Номер страницы (1 для первых 20 записей, далее – 10 записей)</param>
        /// <param name="count">Количество записей для генерации</param>
        /// <param name="avgLikes">Среднее количество лайков (0–10, дробное)</param>
        /// <param name="avgReviews">Среднее количество отзывов (например, 4,7)</param>
        /// <returns>Список книг</returns>
        public List<Book> GenerateBooks(string locale, int userSeed, int pageNumber, int count, double avgLikes, double avgReviews)
        {
            // Комбинируем seed и номер страницы для воспроизводимости
            int combinedSeed = userSeed + pageNumber * 1000;
            Randomizer.Seed = new Random(combinedSeed);

            var faker = new Faker(locale);
            var books = new List<Book>();

            // Определим стартовый индекс для текущей страницы:
            int startIndex = (pageNumber == 1 ? 1 : 20 + (pageNumber - 2) * 10);

            for (int i = 0; i < count; i++)
            {
                int index = startIndex + i;

                // Генерация базовых данных книги
                var book = new Book
                {
                    Index = index,
                    ISBN = faker.Commerce.Ean13(),
                    Title = faker.Commerce.ProductName(),
                    Authors = faker.Name.FullName(),
                    Publisher = faker.Company.CompanyName(),
                    CoverUrl = $"https://picsum.photos/seed/{combinedSeed}_{i}/200/300"
                };

                // Генерация лайков:
                // Целая часть всегда добавляется, а дополнительный лайк – с вероятностью, равной дробной части
                int baseLikes = (int)Math.Floor(avgLikes);
                double extraLikeChance = avgLikes - baseLikes;
                book.Likes = baseLikes + (faker.Random.Double() < extraLikeChance ? 1 : 0);

                // Генерация отзывов:
                int baseReviews = (int)Math.Floor(avgReviews);
                double extraReviewChance = avgReviews - baseReviews;
                book.Reviews = baseReviews + (faker.Random.Double() < extraReviewChance ? 1 : 0);

                books.Add(book);
            }

            return books;
        }
    }
}

