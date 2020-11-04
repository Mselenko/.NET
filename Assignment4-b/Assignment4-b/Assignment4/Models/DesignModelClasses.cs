using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace Assignment4.Models
{

    public class RoleClaim
    {
        public int Id { get; set; }

        [Required, StringLength(45)]
        public string Name { get; set; }
    }


    public class Artist
    {
        public Artist()
        {
            BirthOrStartDate = new DateTime();
            Albums = new List<Album>();
        }

        public int Id { get; set; }

        [Required, StringLength(45)]
        public string Name { get; set; }

        public string BirthName { get; set; }

        public string Executive { get; set; }

        public string Genre { get; set; }

        [Required]
        public string UrlArtist { get; set; }

        public DateTime? BirthOrStartDate { get; set; }

        public ICollection<Album> Albums { get; set; }

    }

    public class Album
    {
        public Album()
        {
            ReleaseDate = new DateTime();
            Artists = new List<Artist>();
            Tracks = new List<Track>();
        }

        public int Id { get; set; }

        [Required, StringLength(45)]
        public string Name { get; set; }

        public string Genre { get; set; }

        public DateTime ReleaseDate { get; set; }

        public string Coordinator { get; set; }

        [Required]
        public string UrlAlbum { get; set; }

        public ICollection<Artist> Artists { get; set; }

        public ICollection<Track> Tracks { get; set; }

    }

    public class Track
    {
        public Track()
        {
            Albums = new List<Album>();
        }

        public int Id { get; set; }

        [Required, StringLength(45)]
        public string Name { get; set; }

        public string Composers { get; set; }

        public string Clerk { get; set; }

        public string Genre { get; set; }

        public ICollection<Album> Albums { get; set; }

    }

    public class Genre
    {
        public int Id { get; set; }

        [Required, StringLength(45)]
        public string Name { get; set; }
    }


}
