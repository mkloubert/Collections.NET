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
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;

namespace MarcelJoachimKloubert.Collections.Concurrent
{
    /// <summary>
    /// A thread safe collection.
    /// </summary>
    /// <typeparam name="T">Type of the items.</typeparam>
    [DebuggerDisplay("Count = {Count}")]
    [DebuggerTypeProxy(typeof(CollectionDebugView<>))]
    public class SynchronizedCollection<T> : ICollection<T>, ICollection, INotifyPropertyChanged
    {
        #region Fields (2)

        private readonly ICollection<T> _BASE_COLLECTION;

        /// <summary>
        /// Stores the value for the <see cref="SynchronizedCollection{T}.SyncRoot" /> property.
        /// </summary>
        protected readonly object _SYNC_ROOT;

        #endregion Fields (2)

        #region Constructors (3)

        /// <summary>
        /// Initializes a new instance of the <see cref="SynchronizedCollection{T}" /> class.
        /// </summary>
        /// <param name="syncRoot">The value for the <see cref="SynchronizedCollection{T}.SyncRoot" /> property.</param>
        public SynchronizedCollection(object syncRoot = null)
            : this(coll: new List<T>(),
                   syncRoot: syncRoot)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SynchronizedCollection{T}" /> class.
        /// </summary>
        /// <param name="coll">The value for the <see cref="SynchronizedCollection{T}.BaseCollection" /> property.</param>
        /// <param name="syncRoot">The value for the <see cref="SynchronizedCollection{T}.SyncRoot" /> property.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="coll" /> is <see langword="null" />.
        /// </exception>
        public SynchronizedCollection(ICollection<T> coll, object syncRoot = null)
        {
            if (coll == null)
            {
                throw new ArgumentNullException("coll");
            }

            this._SYNC_ROOT = syncRoot ?? new object();
            this._BASE_COLLECTION = coll;

            if (this._BASE_COLLECTION is INotifyPropertyChanged)
            {
                ((INotifyPropertyChanged)this._BASE_COLLECTION).PropertyChanged += this.SynchronizedCollection_PropertyChanged;
            }
        }

        /// <summary>
        /// Sends the object to the garbage collector.
        /// </summary>
        ~SynchronizedCollection()
        {
            try
            {
                if (this._BASE_COLLECTION is INotifyPropertyChanged)
                {
                    ((INotifyPropertyChanged)this._BASE_COLLECTION).PropertyChanged -= this.SynchronizedCollection_PropertyChanged;
                }
            }
            catch
            {
                // ignore
            }
        }

        #endregion Constructors (3)

        #region Events (1)

        /// <summary>
        /// <see cref="INotifyPropertyChanged.PropertyChanged" />
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Events (1)

        #region Properties (5)

        /// <summary>
        /// Gets the base collection.
        /// </summary>
        public ICollection<T> BaseCollection
        {
            get { return this._BASE_COLLECTION; }
        }

        /// <inheriteddoc />
        public int Count
        {
            get
            {
                lock (this._SYNC_ROOT)
                {
                    return this._BASE_COLLECTION.Count;
                }
            }
        }

        /// <inheriteddoc />
        public bool IsReadOnly
        {
            get
            {
                lock (this._SYNC_ROOT)
                {
                    return this._BASE_COLLECTION.IsReadOnly;
                }
            }
        }

        /// <inheriteddoc />
        public bool IsSynchronized
        {
            get { return true; }
        }

        /// <inheriteddoc />
        public object SyncRoot
        {
            get { return this._SYNC_ROOT; }
        }

        #endregion Properties (5)

        #region Methods (11)

        /// <inheriteddoc />
        public void Add(T item)
        {
            lock (this._SYNC_ROOT)
            {
                this._BASE_COLLECTION.Add(item);
            }
        }

        /// <inheriteddoc />
        public void Clear()
        {
            lock (this._SYNC_ROOT)
            {
                this._BASE_COLLECTION.Clear();
            }
        }

        /// <inheriteddoc />
        public bool Contains(T item)
        {
            lock (this._SYNC_ROOT)
            {
                return this._BASE_COLLECTION.Contains(item);
            }
        }

        /// <inheriteddoc />
        public void CopyTo(T[] array, int arrayIndex)
        {
            lock (this._SYNC_ROOT)
            {
                this._BASE_COLLECTION.CopyTo(array, arrayIndex);
            }
        }

        /// <summary>
        /// Converts an object to the type of the items.
        /// </summary>
        /// <param name="obj">The input value.</param>
        /// <returns>The output value.</returns>
        protected virtual T ConvertItem(object obj)
        {
            return (T)obj;
        }

        /// <inheriteddoc />
        void ICollection.CopyTo(Array array, int index)
        {
            lock (this._SYNC_ROOT)
            {
                var srcArray = this._BASE_COLLECTION as T[];
                if (srcArray == null)
                {
                    srcArray = this._BASE_COLLECTION.ToArray();
                }

                Array.Copy(sourceArray: srcArray, sourceIndex: 0,
                           destinationArray: array, destinationIndex: index,
                           length: srcArray.Length);
            }
        }

        /// <inheriteddoc />
        public IEnumerator<T> GetEnumerator()
        {
            lock (this._SYNC_ROOT)
            {
                return new SynchronizedEnumerator<T>(enumerator: this._BASE_COLLECTION.GetEnumerator(),
                                                     syncRoot: this._SYNC_ROOT);
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <inheriteddoc />
        public bool Remove(T item)
        {
            lock (this._SYNC_ROOT)
            {
                return this._BASE_COLLECTION.Remove(item);
            }
        }

        private void SynchronizedCollection_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        #endregion Methods (11)
    }
}