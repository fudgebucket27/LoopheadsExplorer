namespace LoopheadsExplorer.Models
{
    public enum RetrievalState
    {
        yetToRetrieve,
        retrieving,
        success,
        error
    }

    public class LoopheadMetadataRetrievalState
    {
        public RetrievalState retrievalState { get; set; } = RetrievalState.yetToRetrieve;
        public string metadataCid { get; set; }
    }
}
