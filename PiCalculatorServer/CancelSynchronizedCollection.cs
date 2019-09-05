using System;
using System.Collections.Concurrent;
using PiCalculatorServer.MessageModels;

namespace PiCalculatorServer
{
    public class CancelSynchronizedCollection
    {
        private BlockingCollection<Guid> cancelCollection;

        public CancelSynchronizedCollection()
        {
            cancelCollection = new BlockingCollection<Guid>();
        }

        public void AddElement(Guid newElment)
        {
            cancelCollection.Add(newElment);
        }

        public bool IsInCollection(CalculatePiMessage deserialized)
        {
            return IsInCollection(deserialized.Id);
        }

        private bool IsInCollection(Guid id)
        {
            foreach (var itemGuid in cancelCollection)
            {
                if (itemGuid==id)
                {
                    return true;
                }
            }

            return false;
        }

        private void SafeRemove(Guid elrmrnet)
        {
            throw new NotImplementedException();
        }
    }
}
