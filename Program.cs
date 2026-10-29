using ProductosADO.Api.Repositories;

var builder = WebApplication.CreateBuilder(args);

// 1. Agregamos controllers y swagger para probar
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 2. PATRÓN REPOSITORY
// Cuando el controlador pida IProductoRepository, le damos ProductoRepository
builder.Services.AddScoped<IProductoRepository, ProductoRepository>();

var app = builder.Build();

// 3. swagger solo en desarrollo
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();
app.MapControllers();
app.Run();