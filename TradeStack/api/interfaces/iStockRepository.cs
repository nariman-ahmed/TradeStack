using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Stock;
using api.Models;

/*An interface is a contract or blueprint that defines the methods that a class MUST implement.
 (like for this class we need a asynchronous methods for get, getall, create, update, delete etc.)
 *In this case, we are defining an interface for the StockRepository class.
 *This allows us to use dependency injection to inject the StockRepository class into our controller.
 *This is useful for testing and for separating the concerns of the application.
*/

namespace api.interfaces
{
    public interface iStockRepository
    {
        //All methods in an interface are public and abstract by default.
        //The return type is a single stock or a list of stock objects that we extract from DB.
        Task<List<Stock>> GetAllStocksAsync();

        //The "?" here is because the return could be NULL
        Task<Stock?> GetByIdAsync(int id);

        Task<Stock> CreateAsync(Stock StockModel);

        Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto updateDto);

        Task<Stock?> DeleteAsync(int id);

        Task<bool> StockExists(int id);

    }
}