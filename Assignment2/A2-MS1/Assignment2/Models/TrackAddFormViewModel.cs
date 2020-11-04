using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Assignment2.Models
{
    public class TrackAddFormViewModel
    {
        [Display(Name = "Track name")]
        public string Name { get; set; }

        [Display(Name = "Composer")]
        public string Composer { get; set; }

        [Display(Name = "Length (ms)")]
        public int Milliseconds { get; set; }

        [Display(Name = "Unit price")]
        public decimal UnitPrice { get; set; }

        [Display(Name = "Album")]
        public SelectList AlbumList { get; set; }

        [Display(Name = "Media Type")]
        public SelectList MediaTypeList { get; set; }
    }
}