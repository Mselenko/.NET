using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Assignment5.Models
{
    public class TrackAddFormViewModel 
    {
        [Required]
        [Display(Name = "Track name")]
        public string Name { get; set; }
        public int AlbumId { get; set; }
        public string AlbumName { get; set; }
        public SelectList GenreList { get; set; }

        [Display(Name = "Track genre")]
        public string Genre { get; set; }

        [Required]
        [Display(Name = "Composer names (comma-separated)")]
        public string Composers { get; set; }

        [Required]
        [Display(Name = "Simple clip")]
        [DataType(DataType.Upload)]
        public string ClipUpload { get; set; }

    }
}