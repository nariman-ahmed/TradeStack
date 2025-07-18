using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Dtos.Stock;
using api.Models;

/*basically this is a class that we'll use to create extension methods
we have stock objects in our applications that have attributes {id,.....,comments}
however we dont always want to send or return all these attributes when dealing with requests or responses
so we create a DTO (Data Transfer Object) that contains only the attributes we want to send.
*/

namespace api.Mappers
{
    /*we're creating an extension method hence the static class and method
    this class will contain methods to map Stock model to StockDto */
    public static class StockMappers
    {
        /*for this function, we are transforming stock into DTO
        our stock class is defined and constant to contain all attributes
        we create costum DTOs to fit our needs, check out the DTO classes!*/
        public static StockDto ToStockDto(this Stock stock)  //'this' keyword indicates that this is an extension method 
        {
            //here, the DTO we're using is the one specific for GET methods.
            //the method recieves a stock object and returns a StockDto object.
            return new StockDto
            {
                Id = stock.Id,
                Symbol = stock.Symbol,
                CompanyName = stock.CompanyName,
                Purchase = stock.Purchase,
                LastDiv = stock.LastDiv,
                Industry = stock.Industry,
                MarketCap = stock.MarketCap,
                Comments= stock.Comments.Select(c => c.ToCommentDto()).ToList()
            };
        }
        public static Stock ToStockFromCreatedDto(this CreateStockRequestDto dto)
        {
            //here, the DTO we're using is the one specific for POST methods.
            //the method recieves a DTO object (so not all attributes included) and returns a Stock object.
            return new Stock
            {
                Symbol = dto.Symbol,
                CompanyName = dto.CompanyName,
                Purchase = dto.Purchase,
                LastDiv = dto.LastDiv,
                Industry = dto.Industry,
                MarketCap = dto.MarketCap
            };
        }
    }
}