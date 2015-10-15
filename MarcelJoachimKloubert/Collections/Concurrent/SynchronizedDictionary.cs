/**********************************************************************************************************************
 * Collections.NET (https://github.com/mkloubert/Collections.NET)                                                     *
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
using System.Diagnostics;

namespace MarcelJoachimKloubert.Collections.Concurrent
{
    /// <summary>
    /// A thread safe dictionary.
    /// </summary>
    /// <typeparam name="TKey">Type of the keys.</typeparam>
    /// <typeparam name="TValue">Type of the value.</typeparam>
    [DebuggerDisplay("Count = {Count}")]
    [DebuggerTypeProxy(typeof(CollectionDebugView<>))]
    public class SynchronizedDictionary<TKey, TValue> : SynchronizedCollection<KeyValuePair<TKey, TValue>>,
                                                        IDictionary<TKey, TValue>, IDictionary, IReadOnlyDictionary<TKey, TValue>
    {
        #region Constructors (2)

        /// <summary>
        /// Initializes a new instance of the <see cref="SynchronizedDictionary{TKey, TValue}" /> class.
        /// </summary>
        /// <param name="syncRoot">The value for the <see cref="SynchronizedCollection{T}.SyncRoot" /> property.</param>
        public SynchronizedDictionary(object syncRoot = null)
            : this(dict: new Dictionary<TKey, TValue>(),
                   syncRoot: syncRoot)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SynchronizedDictionary{TKey, TValue}" /> class.
        /// </summary>
        /// <param name="dict">The value for the <see cref="SynchronizedDictionary{TKey, TValue}.BaseCollection" /> property.</param>
        /// <param name="syncRoot">The value for the <see cref="SynchronizedCollection{T}.SyncRoot" /> property.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="dict" /> is <see langword="null" />.
        /// </exception>
        public SynchronizedDictionary(IDictionary<TKey, TValue> dict, object syncRoot = null)
            : base(coll: dict,
                   syncRoot: syncRoot)
        {
        }

        #endregion Constructors (2)

        #region Properties (10)

        /// <inheriteddoc />
        public new IDictionary<TKey, TValue> BaseCollection
        {
            get { return (IDictionary<TKey, TValue>)base.BaseCollection; }
        }

        /// <inheriteddoc />
        public bool IsFixedSize
        {
            get
            {
                if (this.BaseCollection is IDictionary)
                {
                    lock (this._SYNC_ROOT)
                    {
                        return ((IDictionary)this.BaseCollection).IsFixedSize;
                    }
                }

                return this.IsReadOnly;
            }
        }

        /// <inheriteddoc />
        public ICollection<TKey> Keys
        {
            get { return new SynchronizedCollection<TKey>(coll: this.BaseCollection.Keys, syncRoot: this._SYNC_ROOT); }
        }

        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys
        {
            get { return this.Keys; }
        }

        ICollection IDictionary.Keys
        {
            get { return (ICollection)this.Keys; }
        }

        /// <inheriteddoc />
        public TValue this[TKey key]
        {
            get
            {
                lock (this._SYNC_ROOT)
                {
                    return this.BaseCollection[key];
                }
            }
            set
            {
                lock (this._SYNC_ROOT)
                {
                    this.BaseCollection[key] = value;
                }
            }
        }

        object IDictionary.this[object key]
        {
            get { return this[this.ConvertKey(key)]; }

            set { this[this.ConvertKey(key)] = this.ConvertValue(value); }
        }

        /// <inheriteddoc />
        public ICollection<TValue> Values
        {
            get { return new SynchronizedCollection<TValue>(coll: this.BaseCollection.Values, syncRoot: this._SYNC_ROOT); }
        }

        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values
        {
            get { return this.Values; }
        }

        ICollection IDictionary.Values
        {
            get { return (ICollection)this.Values; }
        }

        #endregion Properties (10)

        #region Methods (10)

        /// <inheriteddoc />
        public void Add(TKey key, TValue value)
        {
            lock (this._SYNC_ROOT)
            {
                this.BaseCollection.Add(key, value);
            }
        }

        void IDictionary.Add(object key, object value)
        {
            this.Add(this.ConvertKey(key),
                     this.ConvertValue(value));
        }

        /// <inheriteddoc />
        public bool ContainsKey(TKey key)
        {
            lock (this._SYNC_ROOT)
            {
                return this.BaseCollection.ContainsKey(key);
            }
        }

        bool IDictionary.Contains(object key)
        {
            return this.ContainsKey(this.ConvertKey(key));
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

        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            if (this.BaseCollection is IDictionary)
            {
                lock (this._SYNC_ROOT)
                {
                    return ((IDictionary)this.BaseCollection).GetEnumerator();
                }
            }

            return new DictionaryEnumerator<TKey, TValue>(this);
        }

        /// <inheriteddoc />
        public bool Remove(TKey key)
        {
            lock (this._SYNC_ROOT)
            {
                return this.BaseCollection.Remove(key);
            }
        }

        void IDictionary.Remove(object key)
        {
            this.Remove(this.ConvertKey(key));
        }

        /// <inheriteddoc />
        public bool TryGetValue(TKey key, out TValue value)
        {
            lock (this._SYNC_ROOT)
            {
                return this.BaseCollection.TryGetValue(key, out value);
            }
        }

        #endregion Methods (10)
    }
}