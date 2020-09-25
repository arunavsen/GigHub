using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using GigHub.Models;

namespace GigHub.ViewModels
{
    public class GIgFormViewModel
    {
        [Required]
        [FutureDate]
        public string Date { get; set; }
        [Required]
        [ValidTime]
        public string Time { get; set; }
        [Required]
        public string Venue { get; set; }
        [Required]
        public int Genre { get; set; }
        public IEnumerable<Genre> Genres { get; set; }

        public DateTime GetDateTime()
        {
            return DateTime.Parse($"{Date} {Time}");
        }
    }
}