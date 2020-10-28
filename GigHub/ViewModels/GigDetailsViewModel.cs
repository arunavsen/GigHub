using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using GigHub.Models;

namespace GigHub.ViewModels
{
    public class GigDetailsViewModel
    {
        public Gig Gig { get; set; }
        public bool IsFollowing { get; set; }
        public bool IsAttending { get; set; }
    }
}