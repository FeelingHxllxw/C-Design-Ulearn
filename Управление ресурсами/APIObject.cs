using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Memory.API
{
    public class APIObject : IDisposable
    {
        private int id;
        private bool disposedValue;

        public APIObject(int id)
        {
            this.id = id;
            MagicAPI.Allocate(id);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                MagicAPI.Free(id);
                disposedValue = true;
            }
        }

        ~APIObject()
        {
            // Если объект еще не был освобожден
            if (!disposedValue)
            {
                // Освобождение ресурса через внешний API
                MagicAPI.Free(id);
            }
        }

        public void Dispose()
        {
            // Не изменяйте этот код. Разместите код очистки в методе "Dispose(bool disposing)".
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}
