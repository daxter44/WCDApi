﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace WCDApi.Model.Users
{
    public class UpdateModel
    {
        public Guid Id { get; set; }
        [EmailAddress]
        public string EMail { get; set; }
    }
}
