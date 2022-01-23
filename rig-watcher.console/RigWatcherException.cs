using System;
using System.Runtime.Serialization;

namespace rig_watcher.console
{
    [Serializable]
    public class RigWatcherException : Exception
    {
        public RigWatcherException(string message) : base(message)
        {
        }

        public RigWatcherException(string message, Exception innerException) 
            : base(message, innerException)
        {
        }

        protected RigWatcherException(
            SerializationInfo info,
            StreamingContext context
        ) : base(info, context) { }

        public override void GetObjectData(
            SerializationInfo info,
            StreamingContext context
        )
        {
            base.GetObjectData(info, context);
        }
    }
}