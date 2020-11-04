using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Assignment4.Models
{
    public class TrackAddViewModel
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(45)]
        [Display(Name = "Track name")]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Composer names (comma-seperated)")]

        public string Composers { get; set; }

        [Display(Name = "Track genre")]
        public string Genre { get; set; }

        [Display(Name = "Clerk who helps with album tasks")]
        public string Clerk { get; set; }

        [Required]
        public int AlbumId { get; set; }
    }
}