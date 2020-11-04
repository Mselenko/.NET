using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Assignment2.Models
{
    public class PlaylistEditTracksViewModel
    {
        [Key]
        public int PlaylistId { get; set; }

        public string Name { get; set; }

        public IEnumerable<int> TracksIds { get; set; }
    }
}