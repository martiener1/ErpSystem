using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shared.Models
{
    public class UserData
    {
        public int userId;

        public int? storeId;

        public string username;

        public string firstName;

        public string lastName;

        public DateTime? birthDate;

        public UserData(int userId, int? storeId, string username, string firstName, string lastName, DateTime? birthDate)
        {
            this.userId = userId;
            this.storeId = storeId;
            this.username = username;
            this.firstName = firstName;
            this.lastName = lastName;
            this.birthDate = birthDate;
        }
    }
}
