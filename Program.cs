using System.Text.RegularExpressions;
using TestTaskAwwcor;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder();
var app = builder.Build();


app.Run(async (context) =>
{
    var response = context.Response;
    var request = context.Request;
    var path = request.Path;

    string expressionForGuid = @"^/api/ads/\w{8}-\w{4}-\w{4}-\w{4}-\w{12}$";

    if (path == "/api/ads" && request.Method == "GET")
    {
        // Получение всех объвлений
        await GetAllAds(response, request);
    }
    else if (Regex.IsMatch(path, expressionForGuid) && request.Method == "GET")
    {
        // Извлекаем id из пути запроса
        string? id = path.Value?.Split("/")[3];

        // Получение конкретного объявления по id
        await GetAdById(id, response, request);
    }
    else if (path == "/api/ads" && request.Method == "POST")
    {
        // Добавление нового объявления в базу данных
        await CreateNewAd(response, request);
    }
    else if (path == "/AddNewAd.html" && request.Method == "GET")
    {
        // Добавление нового объявления в базу данных
        response.ContentType = "text/html; charset=utf-8";
        await response.SendFileAsync("html/AddNewAd.html");
    }
    else if (path == "/ViewAdFull.html" && request.Method == "GET")
    {
        // Добавление нового объявления в базу данных
        response.ContentType = "text/html; charset=utf-8";
        await response.SendFileAsync("html/ViewAdFull.html");
    }
    else
    {
        // Отправка html страницы по-умолчанию
        response.ContentType = "text/html; charset=utf-8";
        await response.SendFileAsync("html/index.html");
    }
});

app.Run();

// Метод получения списка обьявлений
async Task GetAllAds(HttpResponse response, HttpRequest request)
{
    try
    {
        // Получаем параметр постраничного отображения - номер страницы
        int page;
        string a = request.Query["Pages"];
        if (a == null) 
        {
            page = 1;
        }       
        else
        {
            page = int.Parse(a);
        }
        if (page < 0) { page = 1; }

        /*Проверяем введенный параметр сортировки и получаем  отсортированный список
        с базы данных */
        using var db = new AppDbContext();
        IQueryable<Ad> adsList;
        if (request.Query.ContainsKey("SortByPriceUp"))
        {
            adsList = db.Ads.OrderBy(ad => ad.Price);
        }
        else if (request.Query.ContainsKey("SortByPriceDown"))
        {
            adsList = db.Ads.OrderByDescending(ad => ad.Price);
        }
        else if (request.Query.ContainsKey("SortByDateUp"))
        {
            adsList = db.Ads.OrderBy(ad => ad.DateOfCreation);
        }
        else if (request.Query.ContainsKey("SortByDateDown"))
        {
            adsList = db.Ads.OrderByDescending(ad => ad.DateOfCreation);
        }
        else 
        {
            adsList = db.Ads.OrderBy(ad => ad.Title);
        }
        
        //Реализация алгоритма постраничного вывода (pagenation)
        int pageSize = 10;
        IQueryable<Ad> source = adsList;
        var count = await source.CountAsync();
        var items = await source.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

        PageViewModel pageModel = new PageViewModel(count, page, pageSize);
        if (page > pageModel.TotalPages)
        {
            throw new Exception($"Всего доступно { pageModel.TotalPages} страниц!"); 
        }
        //Формируем строку ответа
        var pageAds = new UnitAdsAndParam(items, pageModel.PageNumber, pageModel.HasNextPage, pageModel.HasPreviousPage, pageModel.TotalPages);
       //Отправляем ответ в JSON
        await response.WriteAsJsonAsync(pageAds);

    }
    catch (Exception ex)
    {
        response.StatusCode = 400;
        await response.WriteAsJsonAsync(new { message = $"Не удалось получить список объявлений: {ex.Message}" });
    }
}

//Метод получения пользователя по ID 
async Task GetAdById(string id, HttpResponse response, HttpRequest request)
{
    try
    {
        using var db = new AppDbContext();
        var ad = db.Ads.FirstOrDefault((a) => a.Id == id);

        if (ad != null)
        {
            if (request.Query.ContainsKey("fields"))
            {
                await response.WriteAsJsonAsync(ad);
            }
            else
            {
                var shortAd = new SimpleAd(ad.Id, ad.Title, ad.Price, ad.Image1);
                await response.WriteAsJsonAsync(shortAd);
            }
        }
        else
        {
            throw new Exception("Объявление не найдено");
        }
    }
    catch (Exception ex)
    {
        // если объявление не найдено, отправляем статусный код и сообщение об ошибке
        response.StatusCode = 404;
        await response.WriteAsJsonAsync(new { message = $"Не удалось получить указанное объявление:{ ex.Message}" });
    }
}

//Метод добавления обьявления в базу.
async Task CreateNewAd(HttpResponse response, HttpRequest request)
{
    try
    {
        if (request.HasJsonContentType())
        {
            // Получаем данные нового объявления из запроса
            var newAd = await request.ReadFromJsonAsync<Ad>();

            if (newAd != null) 
            { 
                var context = new ValidationContext(newAd);
                var results = new List<ValidationResult>();
                if (!Validator.TryValidateObject(newAd, context, results, true))
                {
                    response.StatusCode = 400;
                    throw new Exception(results[0].ErrorMessage);
                }
                else
                {
                    // Генерируем id для нового объявления
                    newAd.Id = Guid.NewGuid().ToString();
                    // Указываем дату создания
                    newAd.DateOfCreation = DateTime.Now;

                    // Добавляем объявление в базу
                    using var db = new AppDbContext();
                    await db.Ads.AddAsync(newAd);
                    await db.SaveChangesAsync();

                    response.StatusCode = 200;
                    await response.WriteAsJsonAsync(newAd.Id);
                }
            }
            else
            {
                throw new Exception("Заполните форму");
            }
        }
        else
        {
            throw new Exception("Некорректный запрос");
        }
       
    }
    catch (Exception ex)
    {
        response.StatusCode = 400;
        await response.WriteAsJsonAsync(new { message = ex.Message });
    }
}



// Модель сокращенного объявления
public record SimpleAd(string Id, string Title, float Price, string? Image1);
public record UnitAdsAndParam (List<Ad> Ads, int pageNuber, bool nextPage, bool prevPage, int TotalPages);