using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace mvc.Models
{
    public class FucModel
    {
        private readonly List<FucModel> _precedence = new List<FucModel>();

        [Required]
        public string Name { get; set; }

        [Required]
        public bool Mandatory { get; set; }

        [Required]
        public ushort Semester { get; set; }

        [Required]
        public float Ects { get; set; }

        public IEnumerable<FucModel> Precedence
        {
            get { return _precedence; }
        }

        [Required]
        public string Objectives { get; set; }

        [Required]
        public string Results { get; set; }

        [Required]
        public string Assessment { get; set; }

        [Required]
        public string Program { get; set; }
    }
}