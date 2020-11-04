using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Assignment5.Models
{
  
        public class MediaItemAddFormViewModel
        {
            public int ArtistId { get; set; }

            [Display(Name = "Artist Descriptor")]
            public string ArtistInfo { get; set; }

            [Display(Name = "Descriptive caption")]
            [Required, StringLength(50)]
            public string Caption { get; set; }

            [Required]
            [Display(Name = "Media item")]
            [DataType(DataType.Upload)]
            public string Upload { get; set; }
        }

        public class MediaItemAddViewModel
        {
            public int ArtistId { get; set; }

            public string Caption { get; set; }

            [Required]
            public HttpPostedFileBase Upload { get; set; }
        }
    }
