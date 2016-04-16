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
using System.Linq;

namespace MarcelJoachimKloubert.Collections
{
    /// <summary>
    /// A dictonary with a limited size.
    /// </summary>
    /// <typeparam name="TKey">Type of the keys.</typeparam>
    /// <typeparam name="TValue">Type of the values.</typeparam>
    [DebuggerDisplay("Count = {Count}; MaxCount = {MaxCount}")]
    [DebuggerTypeProxy(typeof(CollectionDebugView<>))]
    public class LimitedDictionary<TKey, TValue> : LimitedCollection<KeyValuePair<TKey, TValue>>,
                                                   IDictionary<TKey, TValue>, IDictionary
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LimitedDictionary{TKey,TValue}" /> class.
        /// </summary>
        /// <param name="maxCount">The value for the <see cref="LimitedCollection{T}.MaxCount" /> property.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="maxCount" /> is less than 0.
        /// </exception>
        public LimitedDictionary(int maxCount)
            : base(maxCount)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LimitedDictionary{TKey,TValue}" /> class.
        /// </summary>
        /// <param name="maxCount">The value for the <see cref="LimitedCollection{T}.MaxCount" /> property.</param>
        /// <param name="throwOnOverflow">The value for the <see cref="LimitedCollection{T}.ThrowOnOverflow" /> property.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="maxCount" /> is less than 0.
        /// </exception>
        public LimitedDictionary(int maxCount, bool throwOnOverflow)
            : base(maxCount, throwOnOverflow)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LimitedDictionary{TKey,TValue}" /> class.
        /// </summary>
        /// <param name="maxCount">The value for the <see cref="LimitedCollection{T}.MaxCount" /> property.</param>
        /// <param name="throwOnOverflow">The value for the <see cref="LimitedCollection{T}.ThrowOnOverflow" /> property.</param>
        /// <param name="sync">The value for the <see cref="LimitedCollection{T}.SyncRoot" /> property.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="maxCount" /> is less than 0.
        /// </exception>
        public LimitedDictionary(int maxCount, bool throwOnOverflow, object sync)
            : base(maxCount, throwOnOverflow, sync)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LimitedDictionary{TKey,TValue}" /> class.
        /// </summary>
        /// <param name="maxCount">The value for the <see cref="LimitedCollection{T}.MaxCount" /> property.</param>
        /// <param name="seq">The initial items.</param>
        /// <param name="throwOnOverflow">The value for the <see cref="LimitedCollection{T}.ThrowOnOverflow" /> property.</param>
        /// <param name="sync">The value for the <see cref="LimitedCollection{T}.SyncRoot" /> property.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="seq" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="maxCount" /> is less than 0.
        /// </exception>
        public LimitedDictionary(int maxCount, IEnumerable<KeyValuePair<TKey, TValue>> seq,
                                 bool throwOnOverflow = false, object sync = null)
            : this(maxCount, seq.ToDictionary(x => x.Key,
                                              x => x.Value),
                   throwOnOverflow: throwOnOverflow, sync: sync)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LimitedDictionary{TKey,TValue}" /> class.
        /// </summary>
        /// <param name="maxCount">The value for the <see cref="LimitedCollection{T}.MaxCount" /> property.</param>
        /// <param name="baseDict">The base dictionary.</param>
        /// <param name="throwOnOverflow">The value for the <see cref="LimitedCollection{T}.ThrowOnOverflow" /> property.</param>
        /// <param name="sync">The value for the <see cref="LimitedCollection{T}.SyncRoot" /> property.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="baseDict" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="maxCount" /> is less than 0.
        /// </exception>
        public LimitedDictionary(int maxCount, IDictionary<TKey, TValue> baseDict,
                                 bool throwOnOverflow = false, object sync = null)
            : base(maxCount, baseColl: baseDict,
                   throwOnOverflow: throwOnOverflow, sync: sync)
        {
        }

        #endregion Constructors

        #region Properties

        ICollection IDictionary.Keys
        {
            get
            {
                var keys = ((IDictionary<TKey, TValue>)_BASE_COLLECTION).Keys;

                var keyColl = keys as ICollection;
                if (keyColl != null)
                {
                    return keyColl;
                }

                return keys.Cast<object>()
                           .ToList();
            }
        }

        ICollection IDictionary.Values
        {
            get
            {
                var values = ((IDictionary<TKey, TValue>)_BASE_COLLECTION).Values;

                var valueColl = values as ICollection;
                if (valueColl != null)
                {
                    return valueColl;
                }

                return values.Cast<object>()
                             .ToList();
            }
        }

        /// <inheriteddoc />
        public virtual bool IsFixedSize
        {
            get
            {
                var dict = _BASE_COLLECTION as IDictionary;
                if (dict != null)
                {
                    return dict.IsFixedSize;
                }

                return IsReadOnly;
            }
        }

        /// <inheriteddoc />
        public ICollection<TKey> Keys
        {
            get { return ((IDictionary<TKey, TValue>)_BASE_COLLECTION).Keys; }
        }

        /// <inheriteddoc />
        public ICollection<TValue> Values
        {
            get { return ((IDictionary<TKey, TValue>)_BASE_COLLECTION).Values; }
        }

        #endregion Properties

        #region Indexers

        object IDictionary.this[object key]
        {
            get { return ((IDictionary<TKey, TValue>)_BASE_COLLECTION)[(TKey)key]; }

            set { ((IDictionary<TKey, TValue>)_BASE_COLLECTION)[(TKey)key] = (TValue)value; }
        }

        /// <inheriteddoc />
        public TValue this[TKey key]
        {
            get { return ((IDictionary<TKey, TValue>)_BASE_COLLECTION)[key]; }

            set { ((IDictionary<TKey, TValue>)_BASE_COLLECTION)[key] = value; }
        }

        #endregion Indexers

        #region Methods

        /// <inheriteddoc />
        public bool Add(TKey key, TValue value)
        {
            var result = TryAdd(key, value);

            if (!result && _THROW_ON_OVERFLOW)
            {
                throw new InvalidOperationException("Maximum has reached!");
            }

            return result;
        }

        /// <inheriteddoc />
        public bool ContainsKey(TKey key)
        {
            return ((IDictionary<TKey, TValue>)_BASE_COLLECTION).ContainsKey(key);
        }

        void IDictionary.Add(object key, object value)
        {
            throw new NotImplementedException();
        }

        bool IDictionary.Contains(object key)
        {
            throw new NotImplementedException();
        }

        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            var dict = _BASE_COLLECTION as IDictionary;
            if (dict != null)
            {
                return dict.GetEnumerator();
            }

            return new DictionaryEnumerator<TKey, TValue>(GetEnumerator());
        }

        void IDictionary.Remove(object key)
        {
            ((IDictionary<TKey, TValue>)_BASE_COLLECTION).Remove((TKey)key);
        }

        void IDictionary<TKey, TValue>.Add(TKey key, TValue value)
        {
            Add(key, value);
        }

        /// <inheriteddoc />
        public bool Remove(TKey key)
        {
            return ((IDictionary<TKey, TValue>)_BASE_COLLECTION).Remove(key);
        }

        /// <summary>
        /// Tries to add a key/value pair.
        /// </summary>
        /// <param name="key">The key of the item to add.</param>
        /// <param name="value">The value of the item to add.</param>
        /// <returns>Item has been added or not.</returns>
        public bool TryAdd(TKey key, TValue value)
        {
            if (!MaximumReached)
            {
                ((IDictionary<TKey, TValue>)_BASE_COLLECTION).Add(key, value);
                return true;
            }

            return false;
        }

        /// <inheriteddoc />
        public bool TryGetValue(TKey key, out TValue value)
        {
            return ((IDictionary<TKey, TValue>)_BASE_COLLECTION).TryGetValue(key, out value);
        }

        #endregion Methods
    }
}