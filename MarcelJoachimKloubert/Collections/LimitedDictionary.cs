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
    [DebuggerTypeProxy(typeof(DictionaryDebugView<,>))]
    public class LimitedDictionary<TKey, TValue> : DictionaryWrapper<TKey, TValue>,
                                                   ILimitedCollection
    {
        #region Fields

        /// <summary>
        /// Stores the maximum size of the collection.
        /// </summary>
        protected readonly int _MAX_COUNT;

        /// <summary>
        /// Stores if an exception should be thrown if more than the supported maximum size of items
        /// is trying to be added or not.
        /// </summary>
        protected readonly bool _THROW_ON_OVERFLOW;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LimitedDictionary{TKey,TValue}" /> class.
        /// </summary>
        /// <param name="maxCount">The value for the <see cref="LimitedDictionary{TKey,TValue}" /> property.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="maxCount" /> is less than 0.
        /// </exception>
        public LimitedDictionary(int maxCount)
            : this(maxCount, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LimitedDictionary{TKey,TValue}" /> class.
        /// </summary>
        /// <param name="maxCount">The value for the <see cref="LimitedDictionary{TKey,TValue}.MaxCount" /> property.</param>
        /// <param name="throwOnOverflow">The value for the <see cref="LimitedDictionary{TKey,TValue}.ThrowOnOverflow" /> property.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="maxCount" /> is less than 0.
        /// </exception>
        public LimitedDictionary(int maxCount, bool throwOnOverflow)
            : this(maxCount,
                   seq: Enumerable.Empty<KeyValuePair<TKey, TValue>>(), throwOnOverflow: throwOnOverflow)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LimitedList{T}" /> class.
        /// </summary>
        /// <param name="maxCount">The value for the <see cref="LimitedList{T}.MaxCount" /> property.</param>
        /// <param name="seq">The initial items.</param>
        /// <param name="throwOnOverflow">The value for the <see cref="LimitedList{T}.ThrowOnOverflow" /> property.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="seq" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="maxCount" /> is less than 0.
        /// </exception>
        public LimitedDictionary(int maxCount, IEnumerable<KeyValuePair<TKey, TValue>> seq,
                                 bool throwOnOverflow = false)
            : this(maxCount, seq.ToDictionary(x => x.Key,
                                              x => x.Value),
                   throwOnOverflow: throwOnOverflow)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LimitedDictionary{TKey,TValue}" /> class.
        /// </summary>
        /// <param name="maxCount">The value for the <see cref="LimitedDictionary{TKey,TValue}.MaxCount" /> property.</param>
        /// <param name="baseDict">The base dictionary.</param>
        /// <param name="throwOnOverflow">The value for the <see cref="LimitedDictionary{TKey,TValue}.ThrowOnOverflow" /> property.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="baseDict" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="maxCount" /> is less than 0.
        /// </exception>
        public LimitedDictionary(int maxCount, IDictionary<TKey, TValue> baseDict,
                                 bool throwOnOverflow = false)
            : base(baseDict)
        {
            if (maxCount < 0)
            {
                throw new ArgumentOutOfRangeException("maxCount", maxCount, "Must be 0 at least!");
            }

            _MAX_COUNT = maxCount;
            _THROW_ON_OVERFLOW = throwOnOverflow;
        }

        #endregion Constructors

        #region Properties

        /// <inheriteddoc />
        public int MaxCount
        {
            get { return _MAX_COUNT; }
        }

        /// <inheriteddoc />
        public bool ThrowOnOverflow
        {
            get { return _THROW_ON_OVERFLOW; }
        }

        #endregion Properties

        #region Methods

        /// <inheriteddoc />
        public sealed override void Add(KeyValuePair<TKey, TValue> item)
        {
            Add(item.Key, item.Value);
        }

        /// <inheriteddoc />
        public sealed override void Add(TKey key, TValue value)
        {
            if (!TryAdd(key, value) && _THROW_ON_OVERFLOW)
            {
                throw new InvalidOperationException("Maximum has reached!");
            }
        }

        /// <summary>
        /// Tries to add a key/value pair.
        /// </summary>
        /// <param name="key">The key of the item to add.</param>
        /// <param name="value">The value of the item to add.</param>
        /// <returns>Item has been added or not.</returns>
        public bool TryAdd(TKey key, TValue value)
        {
            if (_BASE_COLLECTION.Count < _MAX_COUNT)
            {
                ((IDictionary<TKey, TValue>)_BASE_COLLECTION).Add(key, value);
                return true;
            }

            return false;
        }

        #endregion Methods
    }
}