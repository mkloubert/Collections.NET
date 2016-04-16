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
    /// A collection with a limited size.
    /// </summary>
    /// <typeparam name="T">Type of the items.</typeparam>
    [DebuggerDisplay("Count = {Count}; MaxCount = {MaxCount}")]
    [DebuggerTypeProxy(typeof(CollectionDebugView<>))]
    public class LimitedCollection<T> : ICollection<T>, ICollection
    {
        #region Fields

        /// <summary>
        /// Stores the inner collection.
        /// </summary>
        protected readonly ICollection<T> _BASE_COLLECTION;

        /// <summary>
        /// Stores the maximum size of the collection.
        /// </summary>
        protected readonly int _MAX_COUNT;

        /// <summary>
        /// Stores the object for thread safe operations.
        /// </summary>
        protected readonly object _SYNC;

        /// <summary>
        /// Stores if an exception should be thrown if more than the supported maximum size of items
        /// is trying to be added or not.
        /// </summary>
        protected readonly bool _THROW_ON_OVERFLOW;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LimitedCollection{T}" /> class.
        /// </summary>
        /// <param name="maxCount">The value for the <see cref="LimitedCollection{T}.MaxCount" /> property.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="maxCount" /> is less than 0.
        /// </exception>
        public LimitedCollection(int maxCount)
            : this(maxCount, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LimitedCollection{T}" /> class.
        /// </summary>
        /// <param name="maxCount">The value for the <see cref="LimitedCollection{T}.MaxCount" /> property.</param>
        /// <param name="throwOnOverflow">The value for the <see cref="LimitedCollection{T}.ThrowOnOverflow" /> property.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="maxCount" /> is less than 0.
        /// </exception>
        public LimitedCollection(int maxCount, bool throwOnOverflow)
            : this(maxCount, throwOnOverflow,
                   sync: null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LimitedCollection{T}" /> class.
        /// </summary>
        /// <param name="maxCount">The value for the <see cref="LimitedCollection{T}.MaxCount" /> property.</param>
        /// <param name="throwOnOverflow">The value for the <see cref="LimitedCollection{T}.ThrowOnOverflow" /> property.</param>
        /// <param name="sync">The value for the <see cref="LimitedCollection{T}.SyncRoot" /> property.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="maxCount" /> is less than 0.
        /// </exception>
        public LimitedCollection(int maxCount, bool throwOnOverflow, object sync)
            : this(maxCount,
                   seq: Enumerable.Empty<T>(), throwOnOverflow: throwOnOverflow, sync: sync)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LimitedCollection{T}" /> class.
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
        public LimitedCollection(int maxCount, IEnumerable<T> seq,
                                 bool throwOnOverflow = false, object sync = null)
            : this(maxCount, seq.ToList(),
                   throwOnOverflow: throwOnOverflow, sync: sync)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LimitedCollection{T}" /> class.
        /// </summary>
        /// <param name="maxCount">The value for the <see cref="LimitedCollection{T}.MaxCount" /> property.</param>
        /// <param name="baseColl">The base collection.</param>
        /// <param name="throwOnOverflow">The value for the <see cref="LimitedCollection{T}.ThrowOnOverflow" /> property.</param>
        /// <param name="sync">The value for the <see cref="LimitedCollection{T}.SyncRoot" /> property.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="baseColl" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="maxCount" /> is less than 0.
        /// </exception>
        public LimitedCollection(int maxCount, ICollection<T> baseColl,
                                 bool throwOnOverflow = false, object sync = null)
        {
            if (maxCount < 0)
            {
                throw new ArgumentOutOfRangeException("maxCount", "Must be 0 at least!");
            }

            if (baseColl == null)
            {
                throw new ArgumentNullException("baseColl");
            }

            _BASE_COLLECTION = baseColl;
            _MAX_COUNT = maxCount;
            _THROW_ON_OVERFLOW = throwOnOverflow;
            _SYNC = sync;
        }

        #endregion Constructors

        #region Properties

        /// <inheriteddoc />
        public int Count
        {
            get { return _BASE_COLLECTION.Count; }
        }

        /// <inheriteddoc />
        public bool IsReadOnly
        {
            get { return _BASE_COLLECTION.IsReadOnly; }
        }

        /// <inheriteddoc />
        public bool IsSynchronized
        {
            get
            {
                var coll = _BASE_COLLECTION as ICollection;
                if (coll != null)
                {
                    return coll.IsSynchronized;
                }

                return false;
            }
        }

        /// <summary>
        /// Gets the maximum number of possible items.
        /// </summary>
        public int MaxCount
        {
            get { return _MAX_COUNT; }
        }

        /// <summary>
        /// Gets if the maximum has been reached or not.
        /// </summary>
        public bool MaximumReached
        {
            get { return _BASE_COLLECTION.Count >= _MAX_COUNT; }
        }

        /// <summary>
        /// Gets the object for thread safe operations.
        /// </summary>
        public virtual object SyncRoot
        {
            get { return _SYNC; }
        }

        /// <summary>
        /// Gets if an exception should be thrown if more than the supported maximum size of items
        /// is trying to be added or not.
        /// </summary>
        public bool ThrowOnOverflow
        {
            get { return _THROW_ON_OVERFLOW; }
        }

        #endregion Properties

        #region Methods

        /// <inheriteddoc />
        public bool Add(T item)
        {
            var result = TryAdd(item);

            if (!result && _THROW_ON_OVERFLOW)
            {
                throw new InvalidOperationException("Maximum has reached!");
            }

            return result;
        }

        /// <inheriteddoc />
        public void Clear()
        {
            _BASE_COLLECTION.Clear();
        }

        /// <inheriteddoc />
        public bool Contains(T item)
        {
            return _BASE_COLLECTION.Contains(item);
        }

        /// <inheriteddoc />
        public void CopyTo(T[] array, int arrayIndex)
        {
            _BASE_COLLECTION.CopyTo(array, arrayIndex);
        }

        /// <inheriteddoc />
        public IEnumerator<T> GetEnumerator()
        {
            return _BASE_COLLECTION.GetEnumerator();
        }

        void ICollection.CopyTo(Array array, int index)
        {
            CopyTo((T[])array, index);
        }

        void ICollection<T>.Add(T item)
        {
            Add(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        /// <inheriteddoc />
        public bool Remove(T item)
        {
            return _BASE_COLLECTION.Remove(item);
        }

        /// <summary>
        /// Tries to add an item.
        /// </summary>
        /// <param name="item">The item to add.</param>
        /// <returns>Item has been added or not.</returns>
        public bool TryAdd(T item)
        {
            if (!MaximumReached)
            {
                _BASE_COLLECTION.Add(item);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Converts / casts an item to the type of the supported items.
        /// </summary>
        /// <param name="obj">The input value.</param>
        /// <returns>The casted / converted item.</returns>
        protected virtual T ConvertObject(object obj)
        {
            return (T)obj;
        }

        #endregion Methods
    }
}