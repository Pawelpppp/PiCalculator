using System;
using System.Collections.Concurrent;
using PiCalculatorServer.MessageModels;

namespace PiCalculatorServer
{
    public class CancelSynchronizedCollection
    {
        private ConcurrentBag<Guid> cancelCollection;

        public CancelSynchronizedCollection()
        {
            cancelCollection = new ConcurrentBag<Guid>();
        }

        public void AddElement(Guid newElment)
        {
            cancelCollection.Add(newElment);
        }

        public bool IsInCollection(CalculatePiMessage deserialized)
        {
            return false;
        }

        private void SafeRemove(Guid elrmrnet)
        {
            throw new NotImplementedException();
        }
    }
}
