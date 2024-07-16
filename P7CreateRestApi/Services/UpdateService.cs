// Services/UpdateService.cs
using System;
using System.Threading.Tasks;
using Dot.Net.WebApi.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Dot.Net.WebApi.Services;

public class UpdateService(LocalDbContext context)
{
    private readonly LocalDbContext _context = context;

    public async Task<IActionResult> UpdateEntity<T>(int id, T entity, Func<T, bool> existsFunc, Func<T, int> getIdFunc) where T : class
    {
        if (id != getIdFunc(entity))
        {
            return new BadRequestObjectResult("The Id entered in the parameter is not the same as the Id enter in the body");
        }

        try
        {
            var validationMethod = entity.GetType().GetMethod("Validate");
            if (validationMethod != null)
            {
                validationMethod.Invoke(entity, null);
            }
        }
        catch (Exception ex)
        {
            return new BadRequestObjectResult(ex.Message);
        }

        _context.Entry(entity).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!existsFunc(entity))
            {
                return new NotFoundObjectResult($"{typeof(T).Name} with this Id does not exist");
            }
            else
            {
                throw;
            }
        }

        return new NoContentResult();
    }
}