using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.Dtos.Stock
{
    /*This DTO class if=s created specifically for POST methods. Unlike GET, we can expect the user to provide all
    attributes of a certain stock, so for example when creating a new stock, user can't decide the id.
    so this class is specifically made to copy the Stocks object class just minus the id attribute.
    (we still also dont want to include comments for now)*/
    public class CreateStockRequestDto
    {
        public string Symbol { get; set; } = string.Empty;

        public string CompanyName { get; set; } = string.Empty;

        public decimal Purchase { get; set; }

        public decimal LastDiv { get; set; }

        public string Industry { get; set; } = string.Empty;

        public long MarketCap { get; set; }
    }
}