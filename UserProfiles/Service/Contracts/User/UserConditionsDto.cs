using Library.Models.Enums;
using Library.Models.QueryConditions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service.Contracts.User
{
    public class UserConditionsDto
    {
        public NumericFilter<int>? MinMmr { get; set; }

        public NumericFilter<int>? MaxMmr { get; set; }

        public SortCondition Sort { get; set; }
    }
}
