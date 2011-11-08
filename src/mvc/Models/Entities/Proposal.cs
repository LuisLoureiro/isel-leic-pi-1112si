namespace mvc.Models.Entities
{
    public class Proposal
    {
        public CurricularUnit Info { get; private set; }
        public string Owner { get; set; }
        public int Id { get; private set; }

        private static int _id = 0;

        public Proposal(CurricularUnit info)
        {
            Id = ++_id;
            Info = info;
        }
    }
}