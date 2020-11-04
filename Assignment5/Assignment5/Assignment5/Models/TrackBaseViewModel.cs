using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Assignment5.Models
{
    public class TrackBaseViewModel
    {

        public TrackBaseViewModel()
        {
            AlbumNames = new List<String>();
        }

        [Key]
        public int Id { get; set; }

        [Display(Name = "Name")]
        public string Name { get; set; }

        [Display(Name = "Composer names (comma-separated)")]
        public string Composers { get; set; }

        [Display(Name = "Genre")]
        public string Genre { get; set; }

        [Display(Name = "Clerk who helps with album tasks")]
        public string Clerk { get; set; }

        [Display(Name = "Albums with this track")]
        public IEnumerable<String> AlbumNames { get; set; }


    }

    public class TrackWithDetailsViewModel : TrackBaseViewModel
    {
        public TrackWithDetailsViewModel()
        {
            AlbumNames = new List<String>();
            Albums = new List<Album>();
            Artists = new List<Artist>();
        }
        public IEnumerable<Album> Albums { get; set; }

        public IEnumerable<Artist> Artists { get; set; }

        [Display(Name = "Number of albums with this track")]
        public int AlbumsCount { get; set; }

        [Display(Name = "Sample Clip")]
        public string Clip
        {
            get
            {
                return $"/clip/{Id}";
            }
        }
    }


    public class TrackAudioViewModel
    {
        public int Id;

        public string AudioContentType { get; set; }

        public byte[] Audio { get; set; }
    }
}