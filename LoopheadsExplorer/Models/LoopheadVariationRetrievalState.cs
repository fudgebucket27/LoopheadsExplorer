namespace LoopheadsExplorer.Models
{
    public enum VariationRetrievalState
    {
        yetToRetrieve,
        retrieving,
        success,
        error
    }

    public class LoopheadVariationRetrievalState
    {
        public VariationRetrievalState retrievalState { get; set; } = VariationRetrievalState.yetToRetrieve;
        public string cid { get; set; }
    }
}
