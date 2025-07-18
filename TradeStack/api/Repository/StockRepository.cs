using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

/*We're going to use this class to implement the iStockRepository interface
 Index order to implement an interface, we need to provide the implementation for all the methods defined in the interface.
 This class will be used to just interact with the database and perform CRUD operations on the Stock model.
*/

namespace api.Repository
{
    public class StockRepository : iStockRepository
    {
        //now we need to implement dependency injection for the ApplicationDBContext
        //we actually did that in the StockController, so we can use the same approach here.
        private readonly ApplicationDBContext _context;

        //dependency injection happens through the constructor of the class.
        public StockRepository(ApplicationDBContext context)
        {
            //this is going to bring in the database before we use it, AKA preheat the oven
            _context = context;
        }

        //ALL METHODS ASYNC

        public async Task<List<Stock>> GetAllStocksAsync()
        {
            return await _context.Stocks.Include(s => s.Comments).ToListAsync();
        }

        public async Task<Stock?> GetByIdAsync(int id)
        {
            return await _context.Stocks.Include(c => c.Comments).FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<Stock> CreateAsync(Stock StockModel)
        {
            await _context.Stocks.AddAsync(StockModel);
            await _context.SaveChangesAsync();
            return StockModel;
        }

        public async Task<Stock?> UpdateAsync(int id, UpdateStockRequestDto updateDto)
        {
            var StockModel = await _context.Stocks.FindAsync(id);

            if (StockModel == null)
            {
                return null;
            }

            //then we directly copy the values from the request body to the StockModel object.
            StockModel.Symbol = updateDto.Symbol;
            StockModel.CompanyName = updateDto.CompanyName;
            StockModel.Purchase = updateDto.Purchase;
            StockModel.LastDiv = updateDto.LastDiv;
            StockModel.Industry = updateDto.Industry;
            StockModel.MarketCap = updateDto.MarketCap;

            await _context.SaveChangesAsync();

            return StockModel;
        }

        public async Task<Stock?> DeleteAsync(int id)
        {
            var StockModel = await _context.Stocks.FindAsync(id);

            if (StockModel == null)
            {
                return null;
            }

            _context.Stocks.Remove(StockModel);
            await _context.SaveChangesAsync();

            return StockModel;
        }

        public Task<bool> StockExists(int stockid)
        {
            return _context.Stocks.AnyAsync(s => s.Id == stockid);
        }
    }
}