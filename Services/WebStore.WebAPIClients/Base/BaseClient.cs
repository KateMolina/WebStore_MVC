﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace WebStore.WebAPI.Clients.Base
{
   public abstract class BaseClient
    {
        protected HttpClient Http { get; }

        protected string Address { get; }

        public BaseClient(HttpClient client, string address)
        {
            Http = client;
            Address = address;
        }

    }
}
