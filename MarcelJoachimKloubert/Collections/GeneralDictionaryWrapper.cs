/**********************************************************************************************************************
 * Collections.NET (http://github.marcel-kloubert.eu/wiki/index.php/En/Collections.NET)                               *
 *                                                                                                                    *
 * Copyright (c) 2015, Marcel Joachim Kloubert <marcel.kloubert@gmx.net>                                              *
 * All rights reserved.                                                                                               *
 *                                                                                                                    *
 * Redistribution and use in source and binary forms, with or without modification, are permitted provided that the   *
 * following conditions are met:                                                                                      *
 *                                                                                                                    *
 * 1. Redistributions of source code must retain the above copyright notice, this list of conditions and the          *
 *    following disclaimer.                                                                                           *
 *                                                                                                                    *
 * 2. Redistributions in binary form must reproduce the above copyright notice, this list of conditions and the       *
 *    following disclaimer in the documentation and/or other materials provided with the distribution.                *
 *                                                                                                                    *
 * 3. Neither the name of the copyright holder nor the names of its contributors may be used to endorse or promote    *
 *    products derived from this software without specific prior written permission.                                  *
 *                                                                                                                    *
 *                                                                                                                    *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY EXPRESS OR IMPLIED WARRANTIES, *
 * INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE  *
 * DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, *
 * SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF SUBSTITUTE GOODS OR    *
 * SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY,  *
 * WHETHER IN CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF THE   *
 * USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.                                           *
 *                                                                                                                    *
 **********************************************************************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace MarcelJoachimKloubert.Collections
{
    #region CLASS: GeneralDictionaryWrapper<TKey, TValue>

    /// <summary>
    /// A generic wrapper for an <see cref="IDictionary" /> object.
    /// </summary>
    /// <typeparam name="TKey">Type of the keys.</typeparam>
    /// <typeparam name="TValue">Type of the values.</typeparam>
    [DebuggerDisplay("Count = {Count}")]
    [DebuggerTypeProxy(typeof(DictionaryDebugView<,>))]
    public partial class GeneralDictionaryWrapper<TKey, TValue> : IDictionary<TKey, TValue>, IDictionary, IReadOnlyDictionary<TKey, TValue>,
                                                                  INotifyPropertyChanged, INotifyCollectionChanged,
                                                                  IDisposable
    {
        #region Fields (1)

        /// <summary>
        /// Stores the wrapped dictionary.
        /// </summary>
        protected readonly IDictionary _BASE_DICT;

        #endregion Fields (1)

        #region Constructors (3)

        /// <summary>
        /// Initializes a new instance of the <see cref="GeneralDictionaryWrapper{TKey, TValue}" /> class.
        /// </summary>
        public GeneralDictionaryWrapper()
            : this(dict: new Dictionary<TKey, TValue>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeneralDictionaryWrapper{TKey, TValue}" /> class.
        /// </summary>
        /// <param name="dict">The base list.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="dict" /> is <see langword="null" />.
        /// </exception>
        public GeneralDictionaryWrapper(IDictionary dict)
        {
            if (dict == null)
            {
                throw new ArgumentNullException("dict");
            }

            this._BASE_DICT = dict;

            if (this._BASE_DICT is INotifyPropertyChanged)
            {
                ((INotifyPropertyChanged)this._BASE_DICT).PropertyChanged += this.GeneralDictionaryWrapper_PropertyChanged;
            }

            if (this._BASE_DICT is INotifyCollectionChanged)
            {
                ((INotifyCollectionChanged)this._BASE_DICT).CollectionChanged += this.GeneralDictionaryWrapper_CollectionChanged;
            }
        }

        /// <summary>
        /// Sends the object to the garbage collector.
        /// </summary>
        ~GeneralDictionaryWrapper()
        {
            try
            {
                try
                {
                    if (this._BASE_DICT is INotifyPropertyChanged)
                    {
                        ((INotifyPropertyChanged)this._BASE_DICT).PropertyChanged -= this.GeneralDictionaryWrapper_PropertyChanged;
                    }
                }
                finally
                {
                    try
                    {
                        if (this._BASE_DICT is INotifyCollectionChanged)
                        {
                            ((INotifyCollectionChanged)this._BASE_DICT).CollectionChanged -= this.GeneralDictionaryWrapper_CollectionChanged;
                        }
                    }
                    finally
                    {
                        if (this._BASE_DICT is IDisposable)
                        {
                            this.OnDispose((IDisposable)this._BASE_DICT, false);
                        }
                    }
                }
            }
            catch
            {
                // ignore
            }
        }

        #endregion Constructors (3)

        #region Events (2)

        /// <summary>
        /// <see cref="INotifyCollectionChanged.CollectionChanged" />
        /// </summary>
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        /// <summary>
        /// <see cref="INotifyPropertyChanged.PropertyChanged" />
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Events (2)

        #region Properties (14)

        /// <summary>
        /// Gets the wrapped dictionary.
        /// </summary>
        public IDictionary BaseDictionary
        {
            get { return this._BASE_DICT; }
        }

        /// <inheriteddoc />
        public int Count
        {
            get { return this._BASE_DICT.Count; }
        }

        /// <inheriteddoc />
        public bool IsFixedSize
        {
            get { return this._BASE_DICT.IsFixedSize; }
        }

        /// <inheriteddoc />
        public bool IsReadOnly
        {
            get { return this._BASE_DICT.IsReadOnly; }
        }

        /// <inheriteddoc />
        public bool IsSynchronized
        {
            get { return this._BASE_DICT.IsSynchronized; }
        }

        /// <inheriteddoc />
        public ICollection<TKey> Keys
        {
            get
            {
                var keys = this._BASE_DICT.Keys as ICollection<TKey>;
                if (keys != null)
                {
                    return keys;
                }

                if (keys == null)
                {
                    return null;
                }

                return keys.Cast<object>()
                           .Select(x => this.ConvertKey(x))
                           .ToList();
            }
        }

        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys
        {
            get { return this.Keys; }
        }

        ICollection IDictionary.Keys
        {
            get { return this._BASE_DICT.Keys; }
        }

        /// <inheriteddoc />
        public object SyncRoot
        {
            get { return this._BASE_DICT.SyncRoot; }
        }

        /// <inheriteddoc />
        public TValue this[TKey key]
        {
            get { return this.ConvertValue(this._BASE_DICT[key]); }

            set { this._BASE_DICT[key] = value; }
        }

        object IDictionary.this[object key]
        {
            get { return this._BASE_DICT[key]; }

            set { this._BASE_DICT[key] = value; }
        }

        /// <inheriteddoc />
        public ICollection<TValue> Values
        {
            get
            {
                var values = this._BASE_DICT.Values as ICollection<TValue>;
                if (values != null)
                {
                    return values;
                }

                if (values == null)
                {
                    return null;
                }

                return values.Cast<object>()
                             .Select(x => this.ConvertValue(x))
                             .ToList();
            }
        }

        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values
        {
            get { return this.Values; }
        }

        ICollection IDictionary.Values
        {
            get { return this._BASE_DICT.Values; }
        }

        #endregion Properties (14)

        #region Methods (24)

        /// <inheriteddoc />
        public void Add(TKey key, TValue value)
        {
            this._BASE_DICT.Add(key, value);
        }

        /// <inheriteddoc />
        public void Add(KeyValuePair<TKey, TValue> item)
        {
            this.Add(item.Key, item.Value);
        }

        void IDictionary.Add(object key, object value)
        {
            this._BASE_DICT.Add(key, value);
        }

        /// <inheriteddoc />
        public void Clear()
        {
            this._BASE_DICT.Clear();
        }

        /// <inheriteddoc />
        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            this._BASE_DICT
                .CopyTo(array, arrayIndex);
        }

        void ICollection.CopyTo(Array array, int index)
        {
            this._BASE_DICT
                .CopyTo(array, index);
        }

        /// <inheriteddoc />
        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            TValue value;
            if (this.TryGetValue(item.Key, out value))
            {
                var comparer = this.GetValueEqualityComparer() ?? EqualityComparer<TValue>.Default;

                return comparer.Equals(item.Value, value);
            }

            return false;
        }

        bool IDictionary.Contains(object key)
        {
            return this._BASE_DICT.Contains(key);
        }

        /// <inheriteddoc />
        public bool ContainsKey(TKey key)
        {
            return this._BASE_DICT.Contains(key);
        }

        /// <summary>
        /// Converts an object to the type of the keys.
        /// </summary>
        /// <param name="obj">The input value.</param>
        /// <returns>The output value.</returns>
        protected virtual TKey ConvertKey(object obj)
        {
            return (TKey)obj;
        }

        /// <summary>
        /// Converts an object to the type of the values.
        /// </summary>
        /// <param name="obj">The input value.</param>
        /// <returns>The output value.</returns>
        protected virtual TValue ConvertValue(object obj)
        {
            return (TValue)obj;
        }

        /// <inheriteddoc />
        public void Dispose()
        {
            if (this._BASE_DICT is IDisposable)
            {
                this.OnDispose((IDisposable)this._BASE_DICT, true);
            }

            GC.SuppressFinalize(this);
        }

        private void GeneralDictionaryWrapper_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var handler = this.CollectionChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void GeneralDictionaryWrapper_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var handler = this.PropertyChanged;
            if (handler != null)
            {
                if (this.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                                  .Any(x => x.Name == e.PropertyName))
                {
                    handler(this, e);
                }
            }
        }

        /// <inheriteddoc />
        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return new Enumerator(this);
        }

        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            return this._BASE_DICT.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        /// Returns the equality comparer for the keys.
        /// </summary>
        /// <returns>The equality comparer.</returns>
        protected virtual IEqualityComparer<TKey> GetKeyEqualityComparer()
        {
            // system default
            return null;
        }

        /// <summary>
        /// Returns the equality comparer for the values.
        /// </summary>
        /// <returns>The equality comparer.</returns>
        protected virtual IEqualityComparer<TValue> GetValueEqualityComparer()
        {
            // system default
            return null;
        }

        /// <summary>
        /// The logic for the <see cref="GeneralDictionaryWrapper{TKey, TValue}.Dispose()" /> method and the destructor.
        /// </summary>
        /// <param name="coll"><see cref="GeneralDictionaryWrapper{TKey, TValue}._BASE_DICT" /> as <see cref="IDisposable" /> object.</param>
        /// <param name="disposing">
        /// <see cref="GeneralDictionaryWrapper{TKey, TValue}.Dispose()" /> method was invoked (<see langword="true" />)
        /// or the destructor (<see langword="false" />).
        /// </param>
        protected void OnDispose(IDisposable coll, bool disposing)
        {
            if (disposing)
            {
                coll.Dispose();
            }
        }

        /// <inheriteddoc />
        public bool Remove(TKey key)
        {
            var result = this.ContainsKey(key);

            this._BASE_DICT.Remove(key);
            return result;
        }

        /// <inheriteddoc />
        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            TValue value;
            if (this.TryGetValue(item.Key, out value))
            {
                var comparer = this.GetValueEqualityComparer() ?? EqualityComparer<TValue>.Default;

                if (comparer.Equals(item.Value, value))
                {
                    return this.Remove(item.Key);
                }
            }

            return false;
        }

        void IDictionary.Remove(object key)
        {
            this._BASE_DICT.Remove(key);
        }

        /// <inheriteddoc />
        public bool TryGetValue(TKey key, out TValue value)
        {
            if (this.ContainsKey(key))
            {
                value = this[key];
                return true;
            }

            value = default(TValue);
            return false;
        }

        #endregion Methods (24)
    }

    #endregion CLASS: GeneralDictionaryWrapper<TKey, TValue>

    #region CLASS: GeneralDictionaryWrapper

    /// <summary>
    /// A generic wrapper for an <see cref="IDictionary" /> object.
    /// </summary>
    [DebuggerDisplay("Count = {Count}")]
    [DebuggerTypeProxy(typeof(DictionaryDebugView<,>))]
    public class GeneralDictionaryWrapper : GeneralDictionaryWrapper<object, object>
    {
        #region Constructors (2)

        /// <summary>
        /// Initializes a new instance of the <see cref="GeneralDictionaryWrapper" /> class.
        /// </summary>
        public GeneralDictionaryWrapper()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeneralDictionaryWrapper" /> class.
        /// </summary>
        /// <param name="dict">The base list.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="dict" /> is <see langword="null" />.
        /// </exception>
        public GeneralDictionaryWrapper(IDictionary dict)
            : base(dict: dict)
        {
        }

        #endregion Constructors (2)
    }

    #endregion CLASS: GeneralDictionaryWrapper
}