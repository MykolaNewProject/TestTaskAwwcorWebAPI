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
        // ��������� ���� ���������
        await GetAllAds(response, request);
    }
    else if (Regex.IsMatch(path, expressionForGuid) && request.Method == "GET")
    {
        // ��������� id �� ���� �������
        string? id = path.Value?.Split("/")[3];

        // ��������� ����������� ���������� �� id
        await GetAdById(id, response, request);
    }
    else if (path == "/api/ads" && request.Method == "POST")
    {
        // ���������� ������ ���������� � ���� ������
        await CreateNewAd(response, request);
    }
    else if (path == "/AddNewAd.html" && request.Method == "GET")
    {
        // ���������� ������ ���������� � ���� ������
        response.ContentType = "text/html; charset=utf-8";
        await response.SendFileAsync("html/AddNewAd.html");
    }
    else if (path == "/ViewAdFull.html" && request.Method == "GET")
    {
        // ���������� ������ ���������� � ���� ������
        response.ContentType = "text/html; charset=utf-8";
        await response.SendFileAsync("html/ViewAdFull.html");
    }
    else
    {
        // �������� html �������� ��-���������
        response.ContentType = "text/html; charset=utf-8";
        await response.SendFileAsync("html/index.html");
    }
});

app.Run();

// ����� ��������� ������ ����������
async Task GetAllAds(HttpResponse response, HttpRequest request)
{
    try
    {
        // �������� �������� ������������� ����������� - ����� ��������
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

        /*��������� ��������� �������� ���������� � ��������  ��������������� ������
        � ���� ������ */
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
        
        //���������� ��������� ������������� ������ (pagenation)
        int pageSize = 10;
        IQueryable<Ad> source = adsList;
        var count = await source.CountAsync();
        var items = await source.Skip((page - 1) * pageSize).Take(pageSize).ToListAsync();

        PageViewModel pageModel = new PageViewModel(count, page, pageSize);
        if (page > pageModel.TotalPages)
        {
            throw new Exception($"����� �������� { pageModel.TotalPages} �������!"); 
        }
        //��������� ������ ������
        var pageAds = new UnitAdsAndParam(items, pageModel.PageNumber, pageModel.HasNextPage, pageModel.HasPreviousPage, pageModel.TotalPages);
       //���������� ����� � JSON
        await response.WriteAsJsonAsync(pageAds);

    }
    catch (Exception ex)
    {
        response.StatusCode = 400;
        await response.WriteAsJsonAsync(new { message = $"�� ������� �������� ������ ����������: {ex.Message}" });
    }
}

//����� ��������� ������������ �� ID 
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
            throw new Exception("���������� �� �������");
        }
    }
    catch (Exception ex)
    {
        // ���� ���������� �� �������, ���������� ��������� ��� � ��������� �� ������
        response.StatusCode = 404;
        await response.WriteAsJsonAsync(new { message = $"�� ������� �������� ��������� ����������:{ ex.Message}" });
    }
}

//����� ���������� ���������� � ����.
async Task CreateNewAd(HttpResponse response, HttpRequest request)
{
    try
    {
        if (request.HasJsonContentType())
        {
            // �������� ������ ������ ���������� �� �������
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
                    // ���������� id ��� ������ ����������
                    newAd.Id = Guid.NewGuid().ToString();
                    // ��������� ���� ��������
                    newAd.DateOfCreation = DateTime.Now;

                    // ��������� ���������� � ����
                    using var db = new AppDbContext();
                    await db.Ads.AddAsync(newAd);
                    await db.SaveChangesAsync();

                    response.StatusCode = 200;
                    await response.WriteAsJsonAsync(newAd.Id);
                }
            }
            else
            {
                throw new Exception("��������� �����");
            }
        }
        else
        {
            throw new Exception("������������ ������");
        }
       
    }
    catch (Exception ex)
    {
        response.StatusCode = 400;
        await response.WriteAsJsonAsync(new { message = ex.Message });
    }
}



// ������ ������������ ����������
public record SimpleAd(string Id, string Title, float Price, string? Image1);
public record UnitAdsAndParam (List<Ad> Ads, int pageNuber, bool nextPage, bool prevPage, int TotalPages);