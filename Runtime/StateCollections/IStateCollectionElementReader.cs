namespace TanitakaTech.StateVariable.StateCollections
{
    public interface IStateCollectionElementReader<TKey, TValue> :
        IStateCollectionReader<TValue>
    {
        TValue ReadElement(TKey id);
        
        // TODO: Return Result<ElementReadResult, TValue>
        ElementReadResult TryReadElement(TKey id, out TValue element);
    }

    public enum ElementReadResult
    {
        Success,
        NotFound,
    }
}