using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Assignment4.Models
{
    public class TrackAddFormViewModel : TrackAddViewModel
    {
        public string AlbumName { get; set; }

        [Display(Name = "Track genre")]
        public SelectList GenreList { get; set; }

        [Display(Name = "Album cover")]
        public string UrlAlbumCover { get; set; }

    }
}