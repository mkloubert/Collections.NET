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
using System.Diagnostics;

namespace MarcelJoachimKloubert.Collections
{
    /// <summary>
    /// A wrapper for an <see cref="IDictionary{TKey, TValue}" /> object.
    /// All required members are virtual an can be overwritten in later context.
    /// </summary>
    /// <typeparam name="TKey">Type of the keys.</typeparam>
    /// <typeparam name="TValue">Type of the values.</typeparam>
    [DebuggerDisplay("Count = {Count}")]
    [DebuggerTypeProxy(typeof(CollectionDebugView<>))]
    public class DictionaryWrapper<TKey, TValue> : CollectionWrapper<KeyValuePair<TKey, TValue>>,
                                                   IDictionary<TKey, TValue>, IDictionary
    {
        #region Constructors (2)

        /// <summary>
        /// Initializes a new instance of the <see cref="DictionaryWrapper{TKey, TValue}" /> class.
        /// </summary>
        public DictionaryWrapper()
            : this(dict: new Dictionary<TKey, TValue>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DictionaryWrapper{TKey, TValue}" /> class.
        /// </summary>
        /// <param name="dict">The base dictionary.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="dict" /> is <see langword="null" />.
        /// </exception>
        public DictionaryWrapper(IDictionary<TKey, TValue> dict)
            : base(coll: dict)
        {
        }

        #endregion Constructors (2)

        #region Properties (8)

        /// <inheriteddoc />
        public new IDictionary<TKey, TValue> BaseCollection
        {
            get { return (IDictionary<TKey, TValue>)base._BASE_COLLECTION; }
        }

        /// <inheriteddoc />
        public virtual bool IsFixedSize
        {
            get
            {
                if (this._BASE_COLLECTION is IDictionary)
                {
                    return ((IDictionary)this._BASE_COLLECTION).IsFixedSize;
                }

                return this.IsReadOnly;
            }
        }

        /// <inheriteddoc />
        public virtual ICollection<TKey> Keys
        {
            get { return this.BaseCollection.Keys; }
        }

        ICollection IDictionary.Keys
        {
            get
            {
                var keys = this.Keys;
                if (keys is ICollection)
                {
                    return (ICollection)keys;
                }

                return new List<TKey>(keys);
            }
        }

        /// <inheriteddoc />
        public virtual TValue this[TKey key]
        {
            get { return this.BaseCollection[key]; }

            set { this.BaseCollection[key] = value; }
        }

        object IDictionary.this[object key]
        {
            get { return this[this.ConvertKey(key)]; }

            set { this[this.ConvertKey(key)] = this.ConvertValue(value); }
        }

        /// <inheriteddoc />
        public virtual ICollection<TValue> Values
        {
            get { return this.BaseCollection.Values; }
        }

        ICollection IDictionary.Values
        {
            get
            {
                var values = this.Values;
                if (values is ICollection)
                {
                    return (ICollection)values;
                }

                return new List<TValue>(values);
            }
        }

        #endregion Properties (8)

        #region Methods (10)

        /// <inheriteddoc />
        public virtual void Add(TKey key, TValue value)
        {
            this.BaseCollection.Add(key, value);
        }

        void IDictionary.Add(object key, object value)
        {
            this.Add(this.ConvertKey(key),
                     this.ConvertValue(value));
        }

        bool IDictionary.Contains(object key)
        {
            return this.ContainsKey(this.ConvertKey(key));
        }

        /// <inheriteddoc />
        public virtual bool ContainsKey(TKey key)
        {
            return this.BaseCollection.ContainsKey(key);
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
            return new DictionaryEnumerator<TKey, TValue>(this.BaseCollection);
        }

        /// <inheriteddoc />
        public virtual bool Remove(TKey key)
        {
            return this.BaseCollection.Remove(key);
        }

        void IDictionary.Remove(object key)
        {
            this.Remove(this.ConvertKey(key));
        }

        /// <inheriteddoc />
        public virtual bool TryGetValue(TKey key, out TValue value)
        {
            return this.BaseCollection.TryGetValue(key, out value);
        }

        #endregion Methods (10)
    }
}