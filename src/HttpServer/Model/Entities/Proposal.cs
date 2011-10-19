using System;
namespace HttpServer.Model.Entities
{
    public class Proposal : AbstractEntity<UInt32>
    {
        private CurricularUnit _info;

        public Proposal(UInt32 id, CurricularUnit info) : base(id)
        {
            _info = info;
        }

        public CurricularUnit Info
        {
            get { return _info; }
        }
    }
}