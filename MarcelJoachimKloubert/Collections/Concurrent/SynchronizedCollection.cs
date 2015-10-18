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

namespace MarcelJoachimKloubert.Collections.Concurrent
{
    /// <summary>
    /// A thread safe collection.
    /// </summary>
    /// <typeparam name="T">Type of the items.</typeparam>
    [DebuggerDisplay("Count = {Count}")]
    [DebuggerTypeProxy(typeof(CollectionDebugView<>))]
    public class SynchronizedCollection<T> : CollectionWrapper<T>
    {
        #region Fields (1)

        /// <summary>
        /// Stores the value for the <see cref="SynchronizedCollection{T}.SyncRoot" /> property.
        /// </summary>
        protected readonly object _SYNC_ROOT;

        #endregion Fields (1)

        #region Constructors (3)

        /// <summary>
        /// Initializes a new instance of the <see cref="SynchronizedCollection{T}" /> class.
        /// </summary>
        public SynchronizedCollection()
            : this(syncRoot: null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SynchronizedCollection{T}" /> class.
        /// </summary>
        /// <param name="syncRoot">The value for the <see cref="SynchronizedCollection{T}.SyncRoot" /> property.</param>
        public SynchronizedCollection(object syncRoot)
            : this(coll: new List<T>(),
                   syncRoot: syncRoot)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SynchronizedCollection{T}" /> class.
        /// </summary>
        /// <param name="coll">The value for the <see cref="CollectionWrapper{T}.BaseCollection" /> property.</param>
        /// <param name="syncRoot">The value for the <see cref="SynchronizedCollection{T}.SyncRoot" /> property.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="coll" /> is <see langword="null" />.
        /// </exception>
        public SynchronizedCollection(ICollection<T> coll, object syncRoot = null)
            : base(coll: coll)
        {
            this._SYNC_ROOT = syncRoot ?? new object();
        }

        #endregion Constructors (3)

        #region Properties (4)

        /// <inheriteddoc />
        public override sealed int Count
        {
            get
            {
                lock (this._SYNC_ROOT)
                {
                    return base.Count;
                }
            }
        }

        /// <inheriteddoc />
        public override sealed bool IsReadOnly
        {
            get
            {
                lock (this._SYNC_ROOT)
                {
                    return base.IsReadOnly;
                }
            }
        }

        /// <inheriteddoc />
        public override sealed bool IsSynchronized
        {
            get { return true; }
        }

        /// <inheriteddoc />
        public override sealed object SyncRoot
        {
            get { return this._SYNC_ROOT; }
        }

        #endregion Properties (4)

        #region Methods (12)

        /// <inheriteddoc />
        public override sealed void Add(T item)
        {
            lock (this._SYNC_ROOT)
            {
                base.Add(item);
            }
        }

        /// <inheriteddoc />
        public override sealed void Clear()
        {
            lock (this._SYNC_ROOT)
            {
                base.Clear();
            }
        }

        /// <inheriteddoc />
        public override sealed bool Contains(T item)
        {
            lock (this._SYNC_ROOT)
            {
                return base.Contains(item);
            }
        }

        /// <inheriteddoc />
        public override sealed void CopyTo(T[] array, int arrayIndex)
        {
            lock (this._SYNC_ROOT)
            {
                base.CopyTo(array, arrayIndex);
            }
        }

        /// <inheriteddoc />
        protected override sealed void CopyTo(Array array, int index)
        {
            lock (this._SYNC_ROOT)
            {
                base.CopyTo(array, index);
            }
        }

        /// <inheriteddoc />
        public override bool Equals(object obj)
        {
            lock (this._SYNC_ROOT)
            {
                return base.Equals(obj);
            }
        }

        /// <inheriteddoc />
        public override sealed IEnumerator<T> GetEnumerator()
        {
            lock (this._SYNC_ROOT)
            {
                return new SynchronizedEnumerator<T>(enumerator: base.GetEnumerator(),
                                                     syncRoot: this._SYNC_ROOT);
            }
        }

        /// <inheriteddoc />
        public override int GetHashCode()
        {
            lock (this._SYNC_ROOT)
            {
                return base.GetHashCode();
            }
        }

        /// <inheriteddoc />
        protected override sealed void OnDispose(bool disposing)
        {
            lock (this._SYNC_ROOT)
            {
                base.OnDispose(disposing);
            }
        }

        /// <inheriteddoc />
        public override sealed bool Remove(T item)
        {
            lock (this._SYNC_ROOT)
            {
                return base.Remove(item);
            }
        }

        /// <inheriteddoc />
        public override sealed string ToString()
        {
            lock (this._SYNC_ROOT)
            {
                return base.ToString();
            }
        }

        #endregion Methods (12)
    }
}