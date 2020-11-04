using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Assignment5.Models
{
    public class TrackAddViewModel
    {

        [Required]
        public string Name { get; set; }

        [Required]
        public string Composers { get; set; }

        public string Genre { get; set; }

        public string Clerk { get; set; }

        public int AlbumId { get; set; }

        [Required]
        public HttpPostedFileBase ClipUpload { get; set; }
    }
}