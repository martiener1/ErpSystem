using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StockAPI.LocalModels
{
    public class StockMutation
    {
        public int? id;
        public int storeId;
        public int productId;
        public int amount;
        public MutationReason reason;
        public DateTime dateTime;

        public StockMutation(int? id, int storeId, int productId, int amount, MutationReason reason, DateTime dateTime)
        {
            this.id = id;
            this.productId = productId;
            this.amount = amount;
            this.reason = reason;
            this.dateTime = dateTime;
        }

        public StockMutation() { }

        public override bool Equals(object obj)
        {
            StockMutation mutation = obj as StockMutation;
            if (mutation == null) return false;

            return this.id == mutation.id &&
                    this.storeId == mutation.storeId &&
                    this.productId == mutation.productId &&
                    this.amount == mutation.amount &&
                    this.reason == mutation.reason;
            // not counting on dateTime, because minor differences can occur
        }
    }
}
