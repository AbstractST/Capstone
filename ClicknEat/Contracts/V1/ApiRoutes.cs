﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ClicknEat.Contracts.V1
{
    public static class ApiRoutes
    {
        public const string Root = "api";
        public const string Version = "v1";
        public const string Base = Root + "/"+ Version;

        public static class Restaurants
        {
            public const string GetAll = Base + "/Restaurants";

            public const string Get = Base + "/Restaurant/{restaurantId}";

            public const string Create = Base + "/Restaurants";

            public const string Update = Base + "/Restaurant/{restaurantId}";

            public const string Delete = Base + "/Restaurant/{restaurantId}";
        }
    }
}
