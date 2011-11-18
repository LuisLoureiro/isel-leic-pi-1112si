using System;
using System.Linq;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace mvc.Models.Entities
{
    public class CurricularUnit : AbstractEntity<string>
    {
        public static readonly byte Maxsemesters = 10;

        [Required(ErrorMessage = "Introduza o nome da Unidade Curricular")]
        [Display(Name = "Nome")]
        public string Name { get; set; }
        
        [Display(Name = "Obrigatoriedade")]
        public bool Mandatory { get; set; }

        [Required(ErrorMessage = "Introduza o(s) semestre(s) onde será leccionada")]
        [Display(Name = "Semestre(s) Curricular(es)")]
        //public ushort Semester { get; set; }
        public int[] Semester { get; set; }

        [Required(ErrorMessage = "Introduza o número de créditos")]
        [Range(1.0, 20.0, ErrorMessage = "O valor dos créditos deve estar entre 1.0 e 20.0")]
        [Display(Name = "Créditos")]
        public float Ects { get; set; }

        [Display(Name = "Pré-Requisito(s)")]
        //public List<CurricularUnit> Precedence { get; set; }
        public List<string> Precedence { get; set; }

        [Required(ErrorMessage = "Introduza a descrição dos objectivos")]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Objectivos")]
        public string Objectives { get; set; }
        
        [Required(ErrorMessage = "Introduza a descrição dos resultados")]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Resultados da Aprendizagem")]
        public string Results { get; set; }

        [Required(ErrorMessage = "Introduza a descrição da avaliação")]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Avaliação dos Resultados")]
        public string Assessment { get; set; }

        [Required(ErrorMessage = "Introduza a descrição do programa")]
        [DataType(DataType.MultilineText)]
        [Display(Name = "Programa Resumido")]
        public string Program { get; set; }

        public CurricularUnit(string name, string acronym, bool mandatory, ushort semester,
                              float ects, IEnumerable<CurricularUnit> precedence) : base(acronym)
        {
            //Precedence = new List<CurricularUnit>();
            Precedence = new List<string>();
            /*  ushort semester:
             *
             *   0 0 0 0 0 0 1 1 1 1 1 1 1 1 1 1
             *  |___________|___________________|
             *       |               |
             *       |               --> 10 lower weight bits representing the 10 possible semesters where
             *       |                   the Curricular Unit can be teached.
             *       |
             *       ----> The 6 remaining bits must be 0.
             *       
             */
            if (((semester >> Maxsemesters) & 0xFF) != 0)
                throw new ArgumentOutOfRangeException("semester",
                                                      "Invalid semester formatting: The 6 greater weight bits should be 0");

            Name = name;
            Mandatory = mandatory;
            //Semester = semester;
            Semester = new int[Maxsemesters];
            for (int i = 0; i < Maxsemesters; i++)
            {
                if ((semester & (0x1 << i)) == (0x1 << i))
                    Semester[i] = i + 1;
            }
            Ects = ects;
            if(precedence != null) AddPrecedences(precedence);
        }

        public CurricularUnit(string name, string acronym, bool mandatory, UInt16 semester, float ects)
            : this(name, acronym, mandatory, semester, ects, null)
        {
        }

        public CurricularUnit() : base(null)
        {
            //Precedence = new List<CurricularUnit>();
            Semester = new int[Maxsemesters];
            Precedence = new List<string>();
        }

        public void AddPrecedence(CurricularUnit cUnit)
        {
                Precedence.Add(cUnit.Key);
        }

        public void AddPrecedences(IEnumerable<CurricularUnit> precedences)
        {
                Precedence.AddRange(precedences.Select(uc => uc.Key));
        }

        public void UpdatePrecedences(IEnumerable<CurricularUnit> precedences)
        {
                Precedence.Clear();
                AddPrecedences(precedences);
        }
    }
}