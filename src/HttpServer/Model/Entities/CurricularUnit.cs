using System;
using System.Collections.Generic;

namespace HttpServer.Model.Entities
{
    public class CurricularUnit : AbstractEntity<string>
    {
        private readonly List<CurricularUnit> _precedence = new List<CurricularUnit>();

        public static readonly byte Maxsemesters = 10;

        public string Name { get; private set; }
        //public string Acronym { get; private set; }
        public bool Mandatory { get; private set; }
        public ushort Semester { get; private set; }
        public float Ects { get; private set; }

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
            if (((semester >> 10) & 0xFF) != 0)
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

        public string SemesterToText()
        {
            int aux = 0x01;
            string ret = "";
            for (int i = 0; i < 10; i++)
            {
                if ((Semester & aux) == aux)
                    ret += (ret.Length != 0 ? " | " : "") + (i+1) + "º";

                aux = aux << 1;
            }

            return ret;
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