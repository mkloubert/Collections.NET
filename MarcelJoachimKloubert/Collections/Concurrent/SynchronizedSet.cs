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
using System.Collections.Generic;
using System.Diagnostics;

namespace MarcelJoachimKloubert.Collections.Concurrent
{
    /// <summary>
    /// A thread safe set.
    /// </summary>
    /// <typeparam name="T">Type of the items.</typeparam>
    [DebuggerDisplay("Count = {Count}")]
    [DebuggerTypeProxy(typeof(CollectionDebugView<>))]
    public class SynchronizedSet<T> : SynchronizedCollection<T>, ISet<T>
    {
        #region Constructors (3)

        /// <summary>
        /// Initializes a new instance of the <see cref="SynchronizedSet{T}" /> class.
        /// </summary>
        /// <param name="syncRoot">The value for the <see cref="SynchronizedCollection{T}.SyncRoot" /> property.</param>
        public SynchronizedSet(object syncRoot = null)
            : this(set: new HashSet<T>(),
                   syncRoot: syncRoot)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SynchronizedSet{T}" /> class.
        /// </summary>
        /// <param name="set">The value for the <see cref="SynchronizedSet{T}.BaseCollection" /> property.</param>
        /// <param name="syncRoot">The value for the <see cref="SynchronizedCollection{T}.SyncRoot" /> property.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="set" /> is <see langword="null" />.
        /// </exception>
        public SynchronizedSet(ISet<T> set, object syncRoot = null)
            : base(coll: set, syncRoot: syncRoot)
        {
        }

        #endregion Constructors (3)

        #region Properties (1)

        /// <inheriteddoc />
        public new ISet<T> BaseCollection
        {
            get { return (ISet<T>)base.BaseCollection; }
        }

        #endregion Properties (1)

        #region Method (11)

        /// <inheriteddoc />
        public new bool Add(T item)
        {
            lock (this._SYNC_ROOT)
            {
                return this.BaseCollection.Add(item);
            }
        }

        /// <inheriteddoc />
        public void ExceptWith(IEnumerable<T> other)
        {
            lock (this._SYNC_ROOT)
            {
                this.BaseCollection.ExceptWith(other);
            }
        }

        /// <inheriteddoc />
        public void IntersectWith(IEnumerable<T> other)
        {
            lock (this._SYNC_ROOT)
            {
                this.BaseCollection.IntersectWith(other);
            }
        }

        /// <inheriteddoc />
        public bool IsProperSubsetOf(IEnumerable<T> other)
        {
            lock (this._SYNC_ROOT)
            {
                return this.BaseCollection.IsProperSubsetOf(other);
            }
        }

        /// <inheriteddoc />
        public bool IsProperSupersetOf(IEnumerable<T> other)
        {
            lock (this._SYNC_ROOT)
            {
                return this.BaseCollection.IsProperSupersetOf(other);
            }
        }

        /// <inheriteddoc />
        public bool IsSubsetOf(IEnumerable<T> other)
        {
            lock (this._SYNC_ROOT)
            {
                return this.BaseCollection.IsSubsetOf(other);
            }
        }

        /// <inheriteddoc />
        public bool IsSupersetOf(IEnumerable<T> other)
        {
            lock (this._SYNC_ROOT)
            {
                return this.BaseCollection.IsSupersetOf(other);
            }
        }

        /// <inheriteddoc />
        public bool Overlaps(IEnumerable<T> other)
        {
            lock (this._SYNC_ROOT)
            {
                return this.BaseCollection.Overlaps(other);
            }
        }

        /// <inheriteddoc />
        public bool SetEquals(IEnumerable<T> other)
        {
            lock (this._SYNC_ROOT)
            {
                return this.BaseCollection.SetEquals(other);
            }
        }

        /// <inheriteddoc />
        public void SymmetricExceptWith(IEnumerable<T> other)
        {
            lock (this._SYNC_ROOT)
            {
                this.BaseCollection.SymmetricExceptWith(other);
            }
        }

        /// <inheriteddoc />
        public void UnionWith(IEnumerable<T> other)
        {
            lock (this._SYNC_ROOT)
            {
                this.BaseCollection.UnionWith(other);
            }
        }

        #endregion Method (11)
    }
}