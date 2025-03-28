namespace TanitakaTech.StateVariable.StateCollections
{
    public interface IStateCollectionElementReader<TKey, TValue> : IStateCollectionReader<TValue>
    {
        TValue ReadElement(TKey id);
        
        ElementReadResult TryReadElement(TKey id, out TValue element);
    }

    public enum ElementReadResult
    {
        Success,
        NotFound,
    }
}