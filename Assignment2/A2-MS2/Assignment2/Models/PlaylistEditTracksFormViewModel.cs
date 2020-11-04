using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Assignment2.Models
{
    public class PlaylistEditTracksFormViewModel
    {
        [Key]
        public int PlaylistId { get; set; }

        [Display(Name = "Playlist name")]
        public string Name { get; set; }

        public int TracksCount { get; set; }

        [Display(Name = "All tracks")]
        public MultiSelectList TrackList { get; set; }

        public IEnumerable<TrackBaseViewModel> Tracks { get; set; }

    }
}