using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Assignment2.Models
{
    public class PlaylistBaseViewModel
    {
        public PlaylistBaseViewModel()
        {
            Tracks = new List<TrackBaseViewModel>();
        }

        [Key]
        public int PlaylistId { get; set; }


        [Display(Name = "Playlist name")]
        public string Name { get; set; }

        public IEnumerable<TrackBaseViewModel> Tracks { get; set; }


        [Display(Name = "Number of tracks on this playlist")]
        public int TracksCount { get; set; }
    }
}