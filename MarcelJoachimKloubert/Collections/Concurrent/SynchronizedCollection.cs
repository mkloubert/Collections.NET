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
        #region Fields

        /// <summary>
        /// Stores the value for the <see cref="SynchronizedCollection{T}.SyncRoot" /> property.
        /// </summary>
        protected readonly object _SYNC_ROOT;

        #endregion Fields

        #region Constructors

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
            _SYNC_ROOT = syncRoot ?? new object();
        }

        #endregion Constructors

        #region Properties

        /// <inheriteddoc />
        public sealed override int Count
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
        public sealed override bool IsReadOnly
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
        public sealed override bool IsSynchronized
        {
            get { return true; }
        }

        /// <inheriteddoc />
        public sealed override object SyncRoot
        {
            get { return _SYNC_ROOT; }
        }

        #endregion Properties

        #region Methods

        /// <inheriteddoc />
        public sealed override void Add(T item)
        {
            lock (_SYNC_ROOT)
            {
                base.Add(item);
            }
        }

        /// <inheriteddoc />
        public sealed override void Clear()
        {
            lock (_SYNC_ROOT)
            {
                base.Clear();
            }
        }

        /// <inheriteddoc />
        public sealed override bool Contains(T item)
        {
            lock (_SYNC_ROOT)
            {
                return base.Contains(item);
            }
        }

        /// <inheriteddoc />
        public sealed override void CopyTo(T[] array, int arrayIndex)
        {
            lock (_SYNC_ROOT)
            {
                base.CopyTo(array, arrayIndex);
            }
        }

        /// <inheriteddoc />
        public override bool Equals(object obj)
        {
            lock (_SYNC_ROOT)
            {
                return base.Equals(obj);
            }
        }

        /// <inheriteddoc />
        public sealed override IEnumerator<T> GetEnumerator()
        {
            lock (_SYNC_ROOT)
            {
                return new SynchronizedEnumerator<T>(enumerator: base.GetEnumerator(),
                                                     syncRoot: _SYNC_ROOT);
            }
        }

        /// <inheriteddoc />
        public override int GetHashCode()
        {
            lock (_SYNC_ROOT)
            {
                return base.GetHashCode();
            }
        }

        /// <inheriteddoc />
        public sealed override bool Remove(T item)
        {
            lock (_SYNC_ROOT)
            {
                return base.Remove(item);
            }
        }

        /// <inheriteddoc />
        public sealed override string ToString()
        {
            lock (_SYNC_ROOT)
            {
                return base.ToString();
            }
        }

        /// <inheriteddoc />
        protected sealed override void CopyTo(Array array, int index)
        {
            lock (_SYNC_ROOT)
            {
                base.CopyTo(array, index);
            }
        }

        /// <inheriteddoc />
        protected sealed override void OnDispose(bool disposing)
        {
            lock (_SYNC_ROOT)
            {
                base.OnDispose(disposing);
            }
        }

        #endregion Methods
    }
}