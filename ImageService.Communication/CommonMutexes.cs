using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ImageService.Communication
{
   public class CommonMutexes
    {
        private static Mutex writeLock = null;
        private static Mutex readLock = null;

        public static Mutex GetReadMutex()
        {
            if (readLock == null)
            {
                readLock = new Mutex();
            }

            return readLock;
        }

        public static Mutex GetWriteLock()
        {
            if (writeLock == null)
            {
                writeLock = new Mutex();
            }

            return writeLock;
        }
    }
}
