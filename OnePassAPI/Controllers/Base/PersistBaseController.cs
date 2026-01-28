using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using System;

namespace OnePass.API
{
    /// <summary>
    /// ✅ Base controller to abstract try/catch & response handling for persistence endpoints.
    /// </summary>
    public abstract class PersistBaseController : ControllerBase
    {
        protected async Task<IActionResult> ExecutePersistAsync<TDto, TEntity>(
            TDto request,
            string readActionName,
            string readControllerName,
            Func<Task<TEntity>> persistFunc)
         //   where TDto : class {commenting to allow int as input request type}
            where TEntity : class
        {
            if (request == null)
            {
                return BadRequest($"{typeof(TDto).Name.Replace("Dto", "")} data must not be null.");
            }

            try
            {
                var persistedEntity = await persistFunc();

                var locationUrl = Url.Action(
                    action: readActionName,
                    controller: readControllerName,
                    values: new { id = persistedEntity!.GetType().GetProperty("Id")?.GetValue(persistedEntity) },
                    protocol: Request.Scheme);

                return Created(locationUrl!, persistedEntity);
            }
            catch (Exception ex)
            {
                // ✅ centralized logging for all controllers using this base
                var logger = HttpContext.RequestServices.GetService<ILogger<PersistBaseController>>();
                logger?.LogError(ex, "Error occurred while persisting entity of type {Type}", typeof(TEntity).Name);

                return StatusCode(500, $"An error occurred while creating the {typeof(TEntity).Name}.");
            }
        }
    }
}

