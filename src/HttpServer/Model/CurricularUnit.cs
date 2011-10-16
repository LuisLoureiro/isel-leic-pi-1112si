using System;
using System.Collections.Generic;

namespace HttpServer.Model
{
    public class CurricularUnit
    {
        private List<CurricularUnit> _precedence;
        
        public string Name { get; private set; }
        public string Acronym { get; private set; }
        public bool Mandatory { get; private set; }
        public ushort Semester { get; private set; }
        public float Ects { get; private set; }
        public IEnumerable<CurricularUnit> Precedence{ get { return _precedence; } }
        public string Objectives { get; set; }
        public string Results { get; set; }
        public string Assessment { get; set; }
        public string Program { get; set; }

        public CurricularUnit(string name, string acronym, bool mandatory, ushort semester, float ects, IEnumerable<CurricularUnit> precedence)
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
            if ( ((semester >> 10) & 0xFF) != 0)
                throw new ArgumentOutOfRangeException("semester", "Invalid semester formatting: The 6 greater weight bits should be 0");

            Name = name;
            Acronym = acronym;
            Mandatory = mandatory;
            Semester = semester;
            Ects = ects;
            AddPrecedences(precedence);
        }

        public CurricularUnit(string name, string acronym, bool mandatory, ushort semester, float ects)
            : this(name, acronym, mandatory, semester, ects, null){}
        
        public void AddPrecedence(CurricularUnit cUnit)
        {   
            EnsureListInitialization();
            _precedence.Add(cUnit);
        }

        public void AddPrecedences(IEnumerable<CurricularUnit> precedence)
        {
            EnsureListInitialization();
            if(precedence != null)
                _precedence.AddRange(precedence);
        }

        private void EnsureListInitialization()
        {
            if (_precedence == null)
                _precedence = new List<CurricularUnit>();
        }
    }
}