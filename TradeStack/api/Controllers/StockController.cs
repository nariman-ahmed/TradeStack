using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Serialization;
using api.Data;
using Microsoft.AspNetCore.Mvc;
using api.Mappers;
using api.Dtos.Stock;
using Microsoft.EntityFrameworkCore;
using api.Repository;
using api.interfaces;

/*Now this is our Contoller, our API basically*/

namespace api.Controllers
{
    [Route("api/stock")]
    [ApiController]
    public class StockController : ControllerBase
    {
        private readonly ApplicationDBContext _context;
        private readonly iStockRepository _stockRepo;

        //now instead on injecting the database context in the controller, we're going to inject the Stocks interface.
        public StockController(ApplicationDBContext context, iStockRepository stockrepo)
        {
            _stockRepo = stockrepo;
            _context = context;
        }

        //Any database data extraction takes time, so we need to make our methods asynchronous.
        //So we wrap any line that has database interaction (EF methonds) in an async-await keywords.

        [HttpGet]
        public async Task<IActionResult> GetAllStocks()
        {
            //so now we can use _stockRepo instead of _context.
            var stocks = await _stockRepo.GetAllStocksAsync();
            var stocksDto = stocks.Select(s => s.ToStockDto());
            return Ok(stocksDto);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById([FromRoute] int id)
        {
            //Here we are getting a stock by its id from the database
            var stock = await _stockRepo.GetByIdAsync(id);

            if (stock == null)
            {
                //If the stock is not found, we return a 404 status code.
                return NotFound();
            }

            return Ok(stock.ToStockDto());
        }

       
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateStockRequestDto stockDto)
        {
            var StockModel = stockDto.ToStockFromCreatedDto();

            await _stockRepo.CreateAsync(StockModel);

            return CreatedAtAction(nameof(GetById), new { id = StockModel.Id }, StockModel.ToStockDto());
        }

        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto updateDto)
        {
            var StockModel = await _stockRepo.UpdateAsync(id, updateDto);

            if (StockModel == null)
            {
                return NotFound();
            }

            return Ok(StockModel.ToStockDto());
        }

        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var StockModel = await _stockRepo.DeleteAsync(id);

            if (StockModel == null)
            {
                return NotFound();
            }

            return NoContent();
        }
    }
}