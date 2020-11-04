using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Assignment5.Models
{
    public class AlbumAddFormViewModel : AlbumAddViewModel
    {
        [Display(Name = "Album's primary genre")]
        public SelectList GenreList { get; set; }

        [Display(Name = "All artists")]
        public MultiSelectList ArtistList { get; set; }

        [Display(Name = "All tracks")]
        public MultiSelectList TrackList { get; set; }

        public string ArtistName { get; set; }

    }
}