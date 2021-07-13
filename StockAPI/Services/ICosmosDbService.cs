using StockAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockAPI.Services
{
    public interface ICosmosDbService
    {
        Task<IEnumerable<Stock>> GetItemsAsync(string query);
        Task<Stock> GetItemAsync(string id);
        Task AddItemAsync(Stock item);
        Task UpdateItemAsync(string id, Stock item);
        Task DeleteItemAsync(string id);
    }
}
