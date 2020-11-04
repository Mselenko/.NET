using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Assignment5.Models
{
    public class ArtistBaseViewModel
    {
        [Key]
        public int Id { get; set; }

        [Display(Name = "Artist name or stage name")]
        public string Name { get; set; }

        [Display(Name = "If applicable, artist's birth name")]
        public string BirthName { get; set; }

        [Display(Name = "Birth date or start date")]
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime BirthOrStartDate { get; set; }

        [Display(Name = "Artist photo")]
        public string UrlArtist { get; set; }

        [Display(Name = "Artist's primary genre")]
        public string Genre { get; set; }

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

        [DataType(DataType.MultilineText)]
        public string Portrayal { get; set; }
    }

    public class ArtistWithMediaViewModel : ArtistWithDetailViewModel
    {
        public ArtistWithMediaViewModel()
        {
            MediaItems = new List<MediaItemBaseViewModel>();
        }
        public IEnumerable<MediaItemBaseViewModel> MediaItems { get; set; }
    }
}