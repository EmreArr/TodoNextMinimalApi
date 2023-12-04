using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.Annotations;

namespace MinAPISeparateFile;

public static class ProductEndpoints
{
    public static void Map(WebApplication app)
    {
        RouteGroupBuilder productItems = app.MapGroup("/products");

        productItems.MapGet("/", GetAllProducts)
        .WithDescription("Some description get")
        .WithMetadata()
        .WithOpenApi();

        productItems.MapGet("/available", GetAvailableProducts);
        productItems.MapGet("/{id}", GetProduct);
        productItems.MapPost("/", CreateProduct)
        .WithMetadata(new SwaggerOperationAttribute("summary001", "description001"))
        // with description not work with metadata, metadata > description
        .WithDescription("Some description post")
        .WithOpenApi();
        productItems.MapPut("/{id}", UpdateProduct);
        productItems.MapDelete("/{id}", DeleteProduct);
    }

    static async Task<IResult> GetAllProducts(ProductDb db)
    {
        return TypedResults.Ok(await db.Products.ToArrayAsync());
    }

    static async Task<IResult> GetAvailableProducts(ProductDb db)
    {
        return TypedResults.Ok(await db.Products.Where(t => t.isAvailable).ToListAsync());
    }

    static async Task<IResult> GetProduct(int id, ProductDb db)
    {
        return await db.Products.FindAsync(id)
            is Product product
                ? TypedResults.Ok(product)
                : TypedResults.NotFound();
    }

    [ProducesResponseType(StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    static async Task<IResult> CreateProduct(Product product, ProductDb db)
    {
        db.Products.Add(product);
        await db.SaveChangesAsync();

        return TypedResults.Created($"/products/{product.Id}", product);
    }

    static async Task<IResult> UpdateProduct(int id, Product inputProduct, ProductDb db)
    {
        Product? product = await db.Products.FindAsync(id);

        if (product is null) return TypedResults.NotFound();

        product.Name = inputProduct.Name;
        product.Price = inputProduct.Price;
        product.isAvailable = inputProduct.isAvailable;

        await db.SaveChangesAsync();

        return TypedResults.NoContent();
    }

    static async Task<IResult> DeleteProduct(int id, ProductDb db)
    {
        if (await db.Products.FindAsync(id) is Product product)
        {
            db.Products.Remove(product);
            await db.SaveChangesAsync();
            return TypedResults.NoContent();
        }

        return TypedResults.NotFound();
    }
}