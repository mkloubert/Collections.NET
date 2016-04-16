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
    /// A list with a limited size.
    /// </summary>
    /// <typeparam name="T">Type of the items.</typeparam>
    [DebuggerDisplay("Count = {Count}; MaxCount = {MaxCount}")]
    [DebuggerTypeProxy(typeof(CollectionDebugView<>))]
    public class LimitedList<T> : LimitedCollection<T>, IList<T>, IList
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LimitedList{T}" /> class.
        /// </summary>
        /// <param name="maxCount">The value for the <see cref="LimitedCollection{T}.MaxCount" /> property.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="maxCount" /> is less than 0.
        /// </exception>
        public LimitedList(int maxCount)
            : base(maxCount)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LimitedList{T}" /> class.
        /// </summary>
        /// <param name="maxCount">The value for the <see cref="LimitedCollection{T}.MaxCount" /> property.</param>
        /// <param name="throwOnOverflow">The value for the <see cref="LimitedCollection{T}.ThrowOnOverflow" /> property.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="maxCount" /> is less than 0.
        /// </exception>
        public LimitedList(int maxCount, bool throwOnOverflow)
            : base(maxCount, throwOnOverflow)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LimitedList{T}" /> class.
        /// </summary>
        /// <param name="maxCount">The value for the <see cref="LimitedCollection{T}.MaxCount" /> property.</param>
        /// <param name="throwOnOverflow">The value for the <see cref="LimitedCollection{T}.ThrowOnOverflow" /> property.</param>
        /// <param name="sync">The value for the <see cref="LimitedCollection{T}.SyncRoot" /> property.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="maxCount" /> is less than 0.
        /// </exception>
        public LimitedList(int maxCount, bool throwOnOverflow, object sync)
            : base(maxCount, throwOnOverflow, sync)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LimitedList{T}" /> class.
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
        public LimitedList(int maxCount, IEnumerable<T> seq,
                           bool throwOnOverflow = false, object sync = null)
            : this(maxCount, seq.ToList(),
                   throwOnOverflow: throwOnOverflow, sync: sync)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LimitedList{T}" /> class.
        /// </summary>
        /// <param name="maxCount">The value for the <see cref="LimitedCollection{T}.MaxCount" /> property.</param>
        /// <param name="baseList">The base list.</param>
        /// <param name="throwOnOverflow">The value for the <see cref="LimitedCollection{T}.ThrowOnOverflow" /> property.</param>
        /// <param name="sync">The value for the <see cref="LimitedCollection{T}.SyncRoot" /> property.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="baseList" /> is <see langword="null" />.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="maxCount" /> is less than 0.
        /// </exception>
        public LimitedList(int maxCount, IList<T> baseList,
                           bool throwOnOverflow = false, object sync = null)
            : base(maxCount, baseColl: baseList,
                   throwOnOverflow: throwOnOverflow, sync: sync)
        {
        }

        #endregion Constructors

        #region Properties

        /// <inheriteddoc />
        public virtual bool IsFixedSize
        {
            get
            {
                var list = _BASE_COLLECTION as IList;
                if (list != null)
                {
                    return list.IsFixedSize;
                }

                return IsReadOnly;
            }
        }

        #endregion Properties

        #region Indexers

        object IList.this[int index]
        {
            get { return this[index]; }

            set { this[index] = (T)value; }
        }

        /// <inheriteddoc />
        public T this[int index]
        {
            get { return ((IList<T>)_BASE_COLLECTION)[index]; }

            set { ((IList<T>)_BASE_COLLECTION)[index] = value; }
        }

        #endregion Indexers

        #region Methods

        int IList.Add(object value)
        {
            Add((T)value);
            return Count - 1;
        }

        bool IList.Contains(object value)
        {
            return Contains((T)value);
        }

        int IList.IndexOf(object value)
        {
            return IndexOf((T)value);
        }

        void IList.Insert(int index, object value)
        {
            Insert(index, (T)value);
        }

        void IList.Remove(object value)
        {
            Remove((T)value);
        }

        void IList<T>.Insert(int index, T item)
        {
            Insert(index, item);
        }

        /// <inheriteddoc />
        public int IndexOf(T item)
        {
            throw new NotImplementedException();
        }

        /// <inheriteddoc />
        public bool Insert(int index, T item)
        {
            var result = TryInsert(index, item);

            if (!result && _THROW_ON_OVERFLOW)
            {
                throw new InvalidOperationException("Maximum has reached!");
            }

            return result;
        }

        /// <inheriteddoc />
        public void RemoveAt(int index)
        {
            ((IList<T>)_BASE_COLLECTION).RemoveAt(index);
        }

        /// <summary>
        /// Tries to inset an item.
        /// </summary>
        /// <param name="index">The zero based index.</param>
        /// <param name="item">The item to insert.</param>
        /// <returns>Item was inserted or not.</returns>
        public bool TryInsert(int index, T item)
        {
            if (!MaximumReached)
            {
                ((IList<T>)_BASE_COLLECTION).Insert(index, item);
                return true;
            }

            return false;
        }

        #endregion Methods
    }
}