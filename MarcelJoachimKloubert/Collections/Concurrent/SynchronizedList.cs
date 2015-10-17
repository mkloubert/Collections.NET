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

namespace MarcelJoachimKloubert.Collections.Concurrent
{
    /// <summary>
    /// A thread safe list.
    /// </summary>
    /// <typeparam name="T">Type of the items.</typeparam>
    [DebuggerDisplay("Count = {Count}")]
    [DebuggerTypeProxy(typeof(CollectionDebugView<>))]
    public class SynchronizedList<T> : SynchronizedCollection<T>, IList<T>, IList
    {
        #region Constructors (3)

        /// <summary>
        /// Initializes a new instance of the <see cref="SynchronizedList{T}" /> class.
        /// </summary>
        public SynchronizedList()
            : this(syncRoot: null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SynchronizedList{T}" /> class.
        /// </summary>
        /// <param name="syncRoot">The value for the <see cref="SynchronizedCollection{T}.SyncRoot" /> property.</param>
        public SynchronizedList(object syncRoot)
            : this(list: new List<T>(),
                   syncRoot: syncRoot)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SynchronizedList{T}" /> class.
        /// </summary>
        /// <param name="list">The value for the <see cref="SynchronizedList{T}.BaseCollection" /> property.</param>
        /// <param name="syncRoot">The value for the <see cref="SynchronizedCollection{T}.SyncRoot" /> property.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="list" /> is <see langword="null" />.
        /// </exception>
        public SynchronizedList(IList<T> list, object syncRoot = null)
            : base(coll: list, syncRoot: syncRoot)
        {
        }

        #endregion Constructors (3)

        #region Properties (4)

        /// <inheriteddoc />
        public new IList<T> BaseCollection
        {
            get { return (IList<T>)base.BaseCollection; }
        }

        /// <inheriteddoc />
        public bool IsFixedSize
        {
            get
            {
                if (this.BaseCollection is IList)
                {
                    lock (this._SYNC_ROOT)
                    {
                        return ((IList)this.BaseCollection).IsFixedSize;
                    }
                }

                return this.IsReadOnly;
            }
        }

        /// <inheriteddoc />
        public T this[int index]
        {
            get
            {
                lock (this._SYNC_ROOT)
                {
                    return this.BaseCollection[index];
                }
            }
            set
            {
                lock (this._SYNC_ROOT)
                {
                    this.BaseCollection[index] = value;
                }
            }
        }

        object IList.this[int index]
        {
            get { return this[index]; }

            set { this[index] = this.ConvertItem(value); }
        }

        #endregion Properties (4)

        #region Methods (8)

        int IList.Add(object value)
        {
            lock (this._SYNC_ROOT)
            {
                this.BaseCollection.Add(this.ConvertItem(value));
                return this.BaseCollection.Count - 1;
            }
        }

        bool IList.Contains(object value)
        {
            return this.Contains(this.ConvertItem(value));
        }

        /// <inheriteddoc />
        public int IndexOf(T item)
        {
            lock (this._SYNC_ROOT)
            {
                return this.BaseCollection.IndexOf(item);
            }
        }

        int IList.IndexOf(object value)
        {
            return this.IndexOf(this.ConvertItem(value));
        }

        /// <inheriteddoc />
        public void Insert(int index, T item)
        {
            lock (this._SYNC_ROOT)
            {
                this.BaseCollection.Insert(index, item);
            }
        }

        void IList.Insert(int index, object value)
        {
            this.Insert(index, this.ConvertItem(value));
        }

        void IList.Remove(object value)
        {
            this.Remove(this.ConvertItem(value));
        }

        /// <inheriteddoc />
        public void RemoveAt(int index)
        {
            lock (this._SYNC_ROOT)
            {
                this.BaseCollection.RemoveAt(index);
            }
        }

        #endregion Methods (8)
    }
}