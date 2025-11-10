using System;
using System.Collections.Generic;
using System.Linq;

namespace Mottu.Api.InMemory
{
    /// <summary>
    /// Armazenamento genérico em memória.
    /// Usado apenas para simulações locais ou testes sem afetar os domínios reais.
    /// </summary>
    public class InMemoryStorage<T> where T : class
    {
        private readonly Dictionary<Guid, T> _store = new();

        public IEnumerable<T> GetAll() => _store.Values;

        public T? GetById(Guid id) =>
            _store.ContainsKey(id) ? _store[id] : null;

        public void Add(Guid id, T entity)
        {
            if (!_store.ContainsKey(id))
                _store[id] = entity;
        }

        public void Update(Guid id, T entity)
        {
            if (_store.ContainsKey(id))
                _store[id] = entity;
        }

        public void Delete(Guid id)
        {
            if (_store.ContainsKey(id))
                _store.Remove(id);
        }

        public void Clear() => _store.Clear();
    }
}
