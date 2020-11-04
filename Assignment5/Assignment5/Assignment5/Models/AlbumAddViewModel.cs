using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Assignment5.Models
{
    public class AlbumAddViewModel
    {

        public AlbumAddViewModel()
        {
            ReleaseDate = DateTime.Today;
            ArtistIds = new List<int>();
            TrackIds = new List<int>();
        }

        [Key]
        public int Id { get; set; }
        public string Coordinator { get; set; }

        [Required, StringLength(50)]
        [Display(Name = "Album Name")]
        public string Name { get; set; }

        [Display(Name = "Release Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}", ApplyFormatInEditMode = true)]
        public DateTime ReleaseDate { get; set; }

        [Display(Name = "Album's primary genre")]
        public string Genre { get; set; }

        [Display(Name = "URL to album image (cover art)")]
        public string UrlAlbum { get; set; }

        [Required]
        public IEnumerable<int> ArtistIds { get; set; }

        public IEnumerable<int> TrackIds { get; set; }

        [DataType(DataType.MultilineText)]
        public string Depiction { get; set; }

    }
}