using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockAPI.LocalModels
{
    public enum MutationReason
    {
        Sold,           // verkocht
        Bought,         // ingekocht
        Expired,        // over datum
        Broken,         // breuk, kapotte producten
        Etc             // het overige
    }
}
