﻿namespace LoopheadsExplorer.Models
{
    public class LoopheadIpfsLink
    {
        public string name { get; set; }
        public string cid { get; set; }

        public List<Loophead> loopheads = new List<Loophead>();
    }
}
