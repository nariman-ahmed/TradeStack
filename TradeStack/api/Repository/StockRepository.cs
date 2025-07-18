using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Data;
using api.Dtos.Stock;
using api.Helpers;
using api.interfaces;
using api.Models;
using Microsoft.EntityFrameworkCore;

namespace api.Repository
{
    public class StockRepository : iStockRepository
    {
        private readonly ApplicationDBContext _context;

        public StockRepository(ApplicationDBContext context)
        {
            _context = context;
        }

        public async Task<List<Stock>> GetAllStocksAsync(QueryObject query)
        {
            var stocks = _context.Stocks.Include(s => s.Comments).AsQueryable();

            if (!string.IsNullOrWhiteSpace(query.CompanyName))
            {
                stocks = stocks.Where(s => s.CompanyName.Contains(query.CompanyName));
            }

            if (!string.IsNullOrWhiteSpace(query.Symbol))
            {
                stocks = stocks.Where(s => s.Symbol.Contains(query.Symbol));
            }

            if (!string.IsNullOrWhiteSpace(query.SortBy))
            {
                if (query.SortBy.Equals("Symbol"))
                {
                    stocks = query.OrderByDescending ? stocks.OrderByDescending(s => s.Symbol) : stocks.OrderBy(s => s.Symbol);
                }
            }

            if (!string.IsNullOrWhiteSpace(query.SortBy))
            {
                if (query.SortBy.Equals("Company Name"))
                {
                    //sorting is done alphabetically
                    stocks = query.OrderByDescending ? stocks.OrderByDescending(s => s.CompanyName) : stocks.OrderBy(s => s.CompanyName);
                }
            }

            var SkipNumber = (query.PageNumber - 1) * query.PageSize;

            return await stocks.Skip(SkipNumber).Take(query.PageSize).ToListAsync();
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