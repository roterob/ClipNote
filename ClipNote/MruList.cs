using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace eClipx
{
    /// <summary>
    /// Clase que proporcionan una lista de prioridad dónde quedan cacheados los últimos elementos accedidos
    /// de la lista. Útil para implementar cacheo de datos.
    /// </summary>
    /// <typeparam name="TKey"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class MruList<TKey, TValue>: IEnumerable<TValue>
    {
        private SortedList<TKey, TValue> cached;
        private LinkedList<TKey> mru;
        private LinkedList<TKey>.Enumerator enumerator;


        public LinkedList<TKey> OrderedValues { get { return mru; } }

        /// <summary>
        /// Constructor de la clase
        /// </summary>
        /// <param name="capacity">Capacidad inicial de la colección</param>
        public MruList(int capacity)
        {
            cached = new SortedList<TKey, TValue>(capacity);
            mru = new LinkedList<TKey>();
            enumerator = mru.GetEnumerator();
        }

        /// <summary>
        /// Devuelve o inserta un elemento en la lista
        /// </summary>
        /// <param name="key">Clave del elemento</param>
        /// <param name="initializer">Función para obtener el valor del elemento</param>
        /// <returns>El valor cacheado o el resutlatdo de evaluar el "initializer" </returns>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public TValue GetItem(TKey key, TValue item)
        {
            if (!mru.Contains(key))         // => 1. Si no está cacheado ...
            {
                if (cached.Count == cached.Capacity)   // ... haciendo un hueco si es necesario ...
                {
                    cached.Remove(mru.Last.Value);
                    mru.RemoveLast();
                }
                cached[key] = item;
                mru.AddFirst(key);            // ... en la última posición de la lista.
            }
            else                                     // => 2. Si ya estaba cacheado ...
            {
                item = cached[key];         // ... lo cogemos de la cache ...
                mru.Remove(key);              // ... y refrescamos su posición en la lista
                mru.AddFirst(key);
            }

            return item;
        }

        /// <summary>
        /// Determina si la lista está vacía
        /// </summary>
        public bool Empty
        {
            get { return mru.Count == 0; }
        }

        /// <summary>
        /// Determina el número de elemntos de la lista
        /// </summary>
        public int Count
        {
            get { return mru.Count; }
        }

        /// <summary>
        /// Determina si una clave determinada pertenece a la lista
        /// </summary>
        /// <param name="key">Clave</param>
        /// <returns>True/False</returns>
        public bool ContainsKey(TKey key)
        {
            return mru.Contains(key);
        }

        /// <summary>
        /// Elimina una clave de la colección
        /// </summary>
        /// <param name="key">Clave a eliminar</param>
        public void RemoveItem(TKey key)
        {
            mru.Remove(key);
            cached.Remove(key);
        }

        public IEnumerator<TValue> GetEnumerator()
        {
            foreach (var key in mru)
                yield return cached[key];
        }

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
