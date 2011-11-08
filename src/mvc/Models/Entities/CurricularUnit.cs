using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace mvc.Models.Entities
{
    public class CurricularUnit
    {
        private readonly List<CurricularUnit> _precedence = new List<CurricularUnit>();

        [Required]
        public string Name { get; set; }

        [Required]
        public bool Mandatory { get; set; }

        [Required]
        public ushort Semester { get; set; }

        [Required]
        public float Ects { get; set; }

        public IEnumerable<CurricularUnit> Precedence
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