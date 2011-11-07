using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace mvc.Models
{
    public class PropModel
    {
        private readonly FucModel _info;
        public string Owner { get; set; }
        public int Id { get; private set; }

        private static int _id = 0;

        public PropModel()
        {
            Id = ++_id;
        }
    }
}