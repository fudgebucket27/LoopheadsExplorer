﻿namespace LoopheadsExplorer.Models
{
    public class Loophead
    {
        public string baseCid { get; set; }
        public string baseId { get; set; }
        public string variation { get; set; }
        public string metadataCidLink {get;set;}
        public LoopheadMetadata metadata { get; set; }
    }
}
