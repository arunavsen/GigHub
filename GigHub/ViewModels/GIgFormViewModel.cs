﻿using System.Collections.Generic;
using GigHub.Models;

namespace GigHub.ViewModels
{
    public class GIgFormViewModel
    {
        public string Date { get; set; }
        public string Time { get; set; }
        public string Venue { get; set; }
        public int Genre { get; set; }
        public IEnumerable<Genre> Genres { get; set; }
    }
}