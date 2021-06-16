using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ORDER_SERVICE_NET.Utilities
{
    public class Constant
    {
        public const string ConnectionString = "ShopicaDatabase";

        public const string BaseAppUrl = "https://localhost:5001";

        public const string HttpClientMediaType = "application/json";

        public const string OrderNotify = "You have a new order";

        public const string SortASC = "ascend";

        public const string SortDESC = "descend";

        //OrderStatus
        public const string PENDING = "PENDING";
        public const string DELIVER = "DELIVER";
        public const string COMPLETE = "COMPLETE";
        public const string CANCEL = "CANCEL";

        //payment method
        public const string CASH = "CASH";
        public const string PAYPAL = "PAYPAL";


    }
}
