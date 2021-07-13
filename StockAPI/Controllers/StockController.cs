using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StockAPI.Models;
using StockAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class StockController : ControllerBase
    {
        private readonly ICosmosDbService _cosmosDbService;
        private readonly ILogger<StockController> _logger;

        public StockController(ILogger<StockController> logger, ICosmosDbService cosmosDbService)
        {
            _logger = logger;
            _cosmosDbService = cosmosDbService;
        }

        [HttpGet("Stock")]
        public async Task<IActionResult> Get(string code)
        {
            return Ok(await _cosmosDbService.GetItemsAsync($"SELECT * FROM c WHERE c.code = '{code}'"));
        }

        [HttpGet("Stocks")]
        public async Task<IActionResult> GetAll()
        {
            return Ok(await _cosmosDbService.GetItemsAsync("SELECT * FROM c"));
        }

        [HttpPost]
        public async Task<ActionResult> CreateAsync(Stock item)
        {
            if (ModelState.IsValid)
            {
                item.ID = Guid.NewGuid().ToString();
                await _cosmosDbService.AddItemAsync(item);
                return Ok(item);
            }

            return Ok(item);
        }
    }
}
