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
    /// A list with a limited size.
    /// </summary>
    /// <typeparam name="T">Type of the items.</typeparam>
    [DebuggerDisplay("Count = {Count}; MaxCount = {MaxCount}")]
    [DebuggerTypeProxy(typeof(CollectionDebugView<>))]
    public class LimitedList<T> : ListWrapper<T>, ILimitedCollection
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
        /// Initializes a new instance of the <see cref="LimitedList{T}" /> class.
        /// </summary>
        /// <param name="maxCount">The value for the <see cref="LimitedList{T}.MaxCount" /> property.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="maxCount" /> is less than 0.
        /// </exception>
        public LimitedList(int maxCount)
            : this(maxCount, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LimitedList{T}" /> class.
        /// </summary>
        /// <param name="maxCount">The value for the <see cref="LimitedList{T}.MaxCount" /> property.</param>
        /// <param name="throwOnOverflow">The value for the <see cref="LimitedList{T}.ThrowOnOverflow" /> property.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="maxCount" /> is less than 0.
        /// </exception>
        public LimitedList(int maxCount, bool throwOnOverflow)
            : this(maxCount,
                   seq: Enumerable.Empty<T>(), throwOnOverflow: throwOnOverflow)
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
        public LimitedList(int maxCount, IEnumerable<T> seq,
                           bool throwOnOverflow = false)
            : this(maxCount, seq.ToList(),
                   throwOnOverflow: throwOnOverflow)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LimitedList{T}" /> class.
        /// </summary>
        /// <param name="maxCount">The value for the <see cref="LimitedList{T}.MaxCount" /> property.</param>
        /// <param name="baseList">The base list.</param>
        /// <param name="throwOnOverflow">The value for the <see cref="LimitedList{T}.ThrowOnOverflow" /> property.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="baseList" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="maxCount" /> is less than 0.
        /// </exception>
        public LimitedList(int maxCount, IList<T> baseList,
                           bool throwOnOverflow = false)
            : base(baseList)
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
        public sealed override void Add(T item)
        {
            if (!TryAdd(item) && _THROW_ON_OVERFLOW)
            {
                throw new InvalidOperationException("Maximum has reached!");
            }
        }

        /// <inheriteddoc />
        public sealed override void Insert(int index, T item)
        {
            if (!TryInsert(index, item) && _THROW_ON_OVERFLOW)
            {
                throw new InvalidOperationException("Maximum has reached!");
            }
        }

        /// <summary>
        /// Tries to add an item.
        /// </summary>
        /// <param name="item">The item to add.</param>
        /// <returns>Item has been added or not.</returns>
        public bool TryAdd(T item)
        {
            if (_BASE_COLLECTION.Count < _MAX_COUNT)
            {
                base.Add(item);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Tries to inset an item.
        /// </summary>
        /// <param name="index">The zero based index.</param>
        /// <param name="item">The item to insert.</param>
        /// <returns>Item was inserted or not.</returns>
        public bool TryInsert(int index, T item)
        {
            if (_BASE_COLLECTION.Count < _MAX_COUNT)
            {
                base.Insert(index, item);
                return true;
            }

            return false;
        }

        #endregion Methods
    }
}