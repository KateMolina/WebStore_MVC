﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebStore.Domain.Entities.Base;
using WebStore.Domain.Entities.Base.Interfaces;

namespace WebStore.Domain.Entities
{
    public class Employee: NamedEntity
    {
        //public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Age { get; set; }

        public int Order => throw new NotImplementedException();
    }
}
