
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

namespace OnePass.API
{
    public abstract class ReadControllerBase : ControllerBase
    {
        protected readonly ILogger _logger;
        protected readonly IMemoryCache _cache;

        protected ReadControllerBase(ILogger logger, IMemoryCache cache)
        {
            _logger = logger;
            _cache = cache;
        }

        protected async Task<ActionResult<TResult>> ExecuteAsync<TResult>(
            Guid id,
            Func<string> cacheKeyFactory,
            Func<Task<TResult>> fetchFunc,
            string notFoundMessage)
        {
            if (id == Guid.Empty)
            {
                _logger.LogWarning("Invalid Guid provided.");
                return BadRequest("Invalid Id.");
            }

            var cacheKey = cacheKeyFactory();

            try
            {
                if (_cache.TryGetValue(cacheKey, out TResult? cached))
                {
                    return Ok(cached);
                }

                var result = await fetchFunc();

                if (result == null ||
                    (result is IEnumerable<object> enumerable && !enumerable.Any()))
                {
                    _logger.LogInformation(notFoundMessage);
                    return NotFound(notFoundMessage);
                }

                _cache.Set(cacheKey, result, TimeSpan.FromMinutes(5));

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while processing request for {CacheKey}.", cacheKey);
                return StatusCode(500, "An error occurred while processing your request.");
            }
        }
    }
}
