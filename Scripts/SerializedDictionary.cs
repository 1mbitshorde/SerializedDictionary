using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace ActionCode.SerializedDictionaries
{
    /// <summary>
    /// Serialized Dictionary.
    /// Simply declare your key/value types and you're good to go - zero boilerplate.
    /// </summary>
    [Serializable]
    public sealed class SerializedDictionary<TKey, TValue> : IDictionary<TKey, TValue>, ISerializationCallbackReceiver
    {
        [SerializeField] private List<KeyValuePair> list;
        [SerializeField, HideInInspector] private Dictionary<TKey, int> indexByKey;
        [SerializeField, HideInInspector] private Dictionary<TKey, TValue> dict;

        public int Count => dict.Count;
        public bool IsReadOnly { get; private set; }

        [field: SerializeField, HideInInspector] public bool HasDuplicateKeys { get; private set; }

        public TValue this[TKey key]
        {
            get => dict[key];
            set
            {
                dict[key] = value;

                if (indexByKey.ContainsKey(key))
                {
                    var index = indexByKey[key];
                    list[index] = new KeyValuePair(key, value);
                }
                else
                {
                    list.Add(new KeyValuePair(key, value));
                    indexByKey.Add(key, list.Count - 1);
                }
            }
        }

        public ICollection<TKey> Keys => dict.Keys;
        public ICollection<TValue> Values => dict.Values;

        public SerializedDictionary(int capacity = 10)
        {
            list = new List<KeyValuePair>(capacity);
            indexByKey = new Dictionary<TKey, int>(capacity);
            dict = new Dictionary<TKey, TValue>(capacity);
        }

        // Lists are serialized natively by Unity, no custom implementation needed.
        public void OnBeforeSerialize() { }

        // Populate dictionary with pairs from list and flag key-collisions.
        public void OnAfterDeserialize()
        {
            dict ??= new Dictionary<TKey, TValue>();
            indexByKey ??= new Dictionary<TKey, int>();

            dict.Clear();
            indexByKey.Clear();
            HasDuplicateKeys = false;

            for (int i = 0; i < list.Count; i++)
            {
                var key = list[i].key;
                if (key != null && !ContainsKey(key))
                {
                    dict.Add(key, list[i].value);
                    indexByKey.Add(key, i);
                }
                else HasDuplicateKeys = true;
            }
        }

        public void Add(TKey key, TValue value)
        {
            dict.TryAdd(key, value);
            list.Add(new KeyValuePair(key, value));
            indexByKey.TryAdd(key, list.Count - 1);
        }

        public bool ContainsKey(TKey key) => dict.ContainsKey(key);

        public bool TryGetValue(TKey key, out TValue value) => dict.TryGetValue(key, out value);

        public bool TryGetValueUsingIndex(int index, out TValue value)
        {
            var hasValue = TryGetKeyValuePairUsingIndex(index, out KeyValuePair pair);
            value = pair.value;
            return hasValue;
        }

        public bool TryGetKeyUsingIndex(int index, out TKey key)
        {
            var hasKey = TryGetKeyValuePairUsingIndex(index, out KeyValuePair pair);
            key = pair.key;
            return hasKey;
        }

        public bool Remove(TKey key)
        {
            if (dict.Remove(key))
            {
                var index = indexByKey[key];
                list.RemoveAt(index);
                UpdateIndexLookup(index);
                indexByKey.Remove(key);
                return true;
            }

            return false;
        }

        public void Add(KeyValuePair<TKey, TValue> pair) => Add(pair.Key, pair.Value);

        public void Clear()
        {
            dict.Clear();
            list.Clear();
            indexByKey.Clear();
        }

        public bool Contains(KeyValuePair<TKey, TValue> pair)
        {
            if (dict.TryGetValue(pair.Key, out TValue value))
                return EqualityComparer<TValue>.Default.Equals(value, pair.Value);

            return false;
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            if (array == null) throw new ArgumentException("The array cannot be null.");
            if (arrayIndex < 0)
                throw new ArgumentOutOfRangeException("The starting array index cannot be negative.");
            if (array.Length - arrayIndex < dict.Count)
                throw new ArgumentException("The destination array has fewer elements than the collection.");

            foreach (var pair in dict)
            {
                array[arrayIndex] = pair;
                arrayIndex++;
            }
        }

        public bool Remove(KeyValuePair<TKey, TValue> pair)
        {
            if (dict.TryGetValue(pair.Key, out TValue value))
            {
                var isValueMatched = EqualityComparer<TValue>.Default.Equals(value, pair.Value);
                if (isValueMatched) return Remove(pair.Key);
            }

            return false;
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => dict.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => dict.GetEnumerator();

        private bool TryGetKeyValuePairUsingIndex(int index, out KeyValuePair value)
        {
            var hasValue = index < list.Count;
            value = hasValue ? list[index] : default;
            return hasValue;
        }

        private void UpdateIndexLookup(int removedIndex)
        {
            for (int i = removedIndex; i < list.Count; i++)
            {
                var key = list[i].key;
                indexByKey[key]--;
            }
        }

        [Serializable]
        private struct KeyValuePair
        {
            public TKey key;
            public TValue value;
            public KeyValuePair(TKey key, TValue value)
            {
                this.key = key;
                this.value = value;
            }
        }
    }
}