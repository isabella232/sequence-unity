using Sequence.WaaS;

namespace Sequence.Relayer
{
    public interface IQueueableTransaction
    {
        public Transaction BuildTransaction();
        public string ToString();
    }
}