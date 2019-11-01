using Microsoft.AspNetCore.Mvc;
using Shared.Models;
using Shared.Util;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace OrderApplication.Controllers
{
    [Route("api")]
    [ApiController]
    public class ApiController : ControllerBase
    {

        /*
      * overzicht benodigde endpoints
      * @@@@@@@@@@ BESTEL APPLICATIE @@@@@@@@@@
      * get voorraad
      * get bestel hoeveelheid
      * get voorraad hoeveelheden specifieke dag/week/maand/jaar
      * get voorraad hoeveelheden laatste 1/7/30/365 dagen
      * 
      * post nieuwe voorraad mutatie van een product
      * put bestel hoeveelheid
      * 
      * @@@@@@@@@@ INZICHT APPLICATIE @@@@@@@@@@
      * 
      * get voorraadmutaties specifieke dag/week/maand/jaar
      * get voorraadmutaties laatste 1/7/30/365 dagen
      * 
      * @@@@@@@@@@ VERKOOP API @@@@@@@@@@
      * 
      * post nieuwe voorraad mutatie van een product / eventueel meerdere voorraad mutaties in een keer
      * 
      */

        // GET api/stock/current/123456                    return the current stuck of product with id 123456
        // GET api/orders/nextorder/123456                       return the next scheduled order amount for product with id 123456
        // PUT api/orders/nextorder/123456/24                    change the next order amount for product 123456 to 24
        // POST api/stock/mutations                             post new stockmutation
        // POST api/stock/mutations/bulk                        post new stockmutations in bulk (store multiple mutations in one call)

        // GET api/stock/history/123456/10                  return the stock from the most recent 10 days, one value for every end of the day
        // GET api/stock/history/123456/10/20052018         return the stock at period 20-05-2018 to 29-05-2018, values at the end of the day
        // GET api/stock/mutations/123456/10/20052018       return the stockmutations at period 20-05-2018 to 29-05-2018, values at the end of the day
        // GET api/stock/mutations/123456/10                return the stockmutations from the most recent 10 days


        private string[] endpoints = new string[] { "no endpoints specified"
                                                    };

        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return endpoints;
        }
    }
}