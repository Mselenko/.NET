using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Assignment4.Models
{
    public class ArtistBaseViewModel
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Artist name or Stage name")]
        public string Name { get; set; }

        [Display(Name = "If applicable, artist's Birth name")]
        public string BirthName { get; set; }

        [Display(Name = "Birth date or Start date")]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime BirthOrStartDate { get; set; }

        [Display(Name = "Artist photo")]
        public string UrlArtist { get; set; }

        [Display(Name = "Artist's primary genre")]
        public string Genre { get; set; }

        [Display(Name = "Executive who looks after the artist")]
        public string Executive { get; set; }

    }

    public class ArtistWithDetailViewModel : ArtistBaseViewModel
    {
        public ArtistWithDetailViewModel()
        {
            Albums = new List<Album>();
        }

        [Display(Name = "Album(s)")]
        public IEnumerable<Album> Albums { get; set; }

        [Display(Name = "Number of Albums")]
        public int AlbumsCount { get; set; }

    }
}