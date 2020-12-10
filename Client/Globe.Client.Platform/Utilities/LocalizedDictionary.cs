using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;

namespace Globe.Client.Platform.Utilities
{
    public class LocalizedDictionary : IDictionary<string, string>
    {
        #region Data Members

        Dictionary<string, string> _dictionary = new Dictionary<string, string>();

        #endregion

        #region IDictionary Implementation

        public string this[string key]
        {
            get
            {
                if (!_dictionary.ContainsKey(key))
                    return $"### {nameof(key)} ###";

                return _dictionary[key];
            }
            set
            {
                throw new InvalidOperationException("The LocalizedDictionary doesn't permit to set a value");
            }
        }

        public ICollection<string> Keys => _dictionary.Keys;

        public ICollection<string> Values => _dictionary.Values;

        public int Count => _dictionary.Count;

        public bool IsReadOnly => false;

        public void Add(string key, string value)
        {
            _dictionary.Add(key, value);
        }

        public void Add(KeyValuePair<string, string> item)
        {
            _dictionary.Add(item.Key, item.Value);
        }

        public void Clear()
        {
            _dictionary.Clear();
        }

        public bool Contains(KeyValuePair<string, string> item)
        {
            return _dictionary.ContainsKey(item.Key);
        }

        public bool ContainsKey(string key)
        {
            return _dictionary.ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<string, string>[] array, int arrayIndex)
        {
            throw new System.NotImplementedException();
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            return _dictionary.GetEnumerator();
        }

        public bool Remove(string key)
        {
            return _dictionary.Remove(key);
        }

        public bool Remove(KeyValuePair<string, string> item)
        {
            return _dictionary.Remove(item.Key);
        }

        public bool TryGetValue(string key, [MaybeNullWhen(false)] out string value)
        {
            return _dictionary.TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _dictionary.GetEnumerator();
        }

        #endregion
    }
}
