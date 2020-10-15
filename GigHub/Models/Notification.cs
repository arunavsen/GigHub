using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GigHub.Models
{
    public class Notification
    {
        public int Id { get; private set; }
        public DateTime DateTime { get; private set; }
        public NotificationType Type { get; private set; }
        public DateTime? OriginalDateTime { get; private set; }
        public string OriginalVenue { get; private set; }
        [Required]
        public Gig Gig { get; private set; }

        protected Notification()
        {

        }

        private Notification(Gig gig, NotificationType notificationType)
        {
            Gig = gig ?? throw new NullReferenceException("gig");
            Type = notificationType;
            DateTime = DateTime.Now;
        }

        public static Notification GigCreated(Gig gig)
        {
            return new Notification(gig,NotificationType.GigCreated);
        }

        public static Notification GigUpdated(DateTime originalDateTime, string originalVenue, Gig newGig)
        {
            var notification = new Notification(newGig,NotificationType.GigUpdated);
            notification.OriginalDateTime = originalDateTime;
            notification.OriginalVenue = originalVenue;

            return notification;
        }

        public static Notification GigCancelled(Gig gig)
        {
            return new Notification(gig, NotificationType.GigCancelled);
        }
    }
}