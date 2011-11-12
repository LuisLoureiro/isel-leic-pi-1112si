using System;
using System.Collections.Generic;
using HttpServer.Model.Entities;

namespace mvc.Models.Entities
{
    public class CurricularUnit : AbstractEntity<string>
    {
        private readonly List<CurricularUnit> _precedence = new List<CurricularUnit>();

        public static readonly byte Maxsemesters = 10;

        public string Name { get; set; }
        public bool Mandatory { get; set; }
        public ushort Semester { get; set; }
        public float Ects { get; set; }

        public IEnumerable<CurricularUnit> Precedence
        {
            get { return _precedence; }
        }

        public string Objectives { get; set; }
        public string Results { get; set; }
        public string Assessment { get; set; }
        public string Program { get; set; }

        public CurricularUnit(string name, string acronym, bool mandatory, ushort semester,
                              float ects, IEnumerable<CurricularUnit> precedence) : base(acronym)
        {
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
            Semester = semester;
            Ects = ects;
            if(precedence != null) AddPrecedences(precedence);
        }

        public CurricularUnit(string name, string acronym, bool mandatory, UInt16 semester, float ects)
            : this(name, acronym, mandatory, semester, ects, null)
        {
        }

        public void AddPrecedence(CurricularUnit cUnit)
        {
                _precedence.Add(cUnit);
        }

        public void AddPrecedences(IEnumerable<CurricularUnit> precedences)
        {
                _precedence.AddRange(precedences);
        }

        public void UpdatePrecedences(IEnumerable<CurricularUnit> precedences)
        {
                _precedence.Clear();
                _precedence.AddRange(precedences);
        }
    }
}