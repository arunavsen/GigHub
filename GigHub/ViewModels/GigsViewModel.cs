﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GigHub.Models;

namespace GigHub.ViewModels
{
    public class GigsViewModel
    {
        public IEnumerable<Gig> LatestGigs { get; set; }
        public bool AutheticatedUser { get; set; }
        public string Heading { get; set; }
        public string SearchTerm { get; set; }
        public ILookup<int, Attendence> Attendences { get; set; }
        public ILookup<string, Following> Following { get; internal set; }
    }
}