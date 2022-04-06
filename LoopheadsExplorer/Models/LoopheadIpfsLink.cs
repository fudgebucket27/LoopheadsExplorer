namespace LoopheadsExplorer.Models
{
    public class LoopheadIpfsLink
    {
        public string name { get; set; }
        public string cid { get; set; }

        public int randomNumOne { get; set; }
        public int randomNumTwo { get; set; }

        public List<Loophead> loopheads = new List<Loophead>();
    }
}
