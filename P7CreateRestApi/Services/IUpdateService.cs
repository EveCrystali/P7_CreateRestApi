using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace Dot.Net.WebApi.Services
{
    public interface IUpdateService<T> where T : class
    {
        Task<IActionResult> UpdateEntity(int id, T entity, Func<T, bool> existsFunc, Func<T, int> getIdFunc);
    }
}
