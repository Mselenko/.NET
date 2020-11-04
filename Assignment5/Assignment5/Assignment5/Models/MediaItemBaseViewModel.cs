using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Assignment5.Models
{
  
        public class MediaItemBaseViewModel : MediaItemWithDetailViewModel
        {
            [Key]
            public int Id { get; set; }

            public string StringId { get; set; }

            public DateTime Timestamp { get; set; }

            public string Caption { get; set; }
        }

        public class MediaItemWithDetailViewModel
        {
            public string ContentType { get; set; }

            public byte[] Content { get; set; }
        }
    }
