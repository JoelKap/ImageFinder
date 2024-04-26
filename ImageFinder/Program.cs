using ImageFinder.Dal;
using ImageFinder.Service;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<ImageDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("ImageFinderConnection")));

builder.Services.AddScoped<IImageService, ImageService>();
builder.Services.AddScoped<IImageDataAccess, ImageDataAccess>();

builder.Services.AddScoped<IUserIdStrategyService>(serviceProvider =>
{
    var httpClient = serviceProvider.GetRequiredService<IHttpClientFactory>().CreateClient();
    var dbService = serviceProvider.GetRequiredService<IImageDataAccess>();

    var config = new UserIdImageResolverConfig
    {
        StaticUrlDigits = new[] { '6', '7', '8', '9' },
        BaseStaticUrl = "https://my-json-server.typicode.com/ck-pacificdev/tech-test/images/",
        DatabaseLookupDigits = new[] { '1', '2', '3', '4', '5' },
        FetchUrlByIdFunc = dbService.GetImageUrlByIdAsync,
        VowelCharacters = new[] { 'a', 'e', 'i', 'o', 'u' },
        VowelBasedStaticUrl = "https://api.dicebear.com/8.x/pixel-art/png?seed={0}&size=150",
        NonAlphaNumericCharacters = new[] { '@', '#', '$', '%' },
        NonAlphaNumericBasedStaticUrl = "https://api.dicebear.com/8.x/pixel-art/png?seed={0}&size=150"
    };

    return new UserIdImageResolverService(httpClient, config);
});

builder.Services.AddHttpClient();
builder.Services.AddHttpClient<UserIdImageResolverService>(client =>
{
    client.BaseAddress = new Uri("https://my-json-server.typicode.com/ck-pacificdev/tech-test/images/");
});

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddCors(options =>
{
    options.AddPolicy("corsapp", builder =>
    {
        builder.WithOrigins("*")
               .AllowAnyMethod()
               .AllowAnyHeader();
    });
});

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();

app.UseCors("corsapp");

app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();

app.MapControllers();

app.Run();
