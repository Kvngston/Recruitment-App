﻿﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Exam.Domains
{
    public class Advert
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public string Id { get; set; }

        public string AdvertName { get; set; }

        public string  AdvertCompany { get; set; }

        public string AdvertLink { get; set; }

        public string Duration { get; set; }

        public string AdvertImageLocation { get; set; }
    }
}