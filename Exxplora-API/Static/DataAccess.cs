using Exxplora_API.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Exxplora_API.Static
{
    public static class DataAccess
    {
        private static ExxploraDbContext _DB;

        public static ExxploraDbContext DB
        {
            get
            {
                if (_DB == null) _DB = new ExxploraDbContext(); 
                return _DB;
            }
        }
    }
}