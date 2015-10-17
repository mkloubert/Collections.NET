﻿/**********************************************************************************************************************
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
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace MarcelJoachimKloubert.Collections
{
    /// <summary>
    /// A wrapper for an <see cref="ICollection{T}" /> object.
    /// All required members are virtual an can be overwritten in later context.
    /// </summary>
    /// <typeparam name="T">Type of the items.</typeparam>
    [DebuggerDisplay("Count = {Count}")]
    [DebuggerTypeProxy(typeof(CollectionDebugView<>))]
    public class CollectionWrapper<T> : ICollection<T>, ICollection,
                                        INotifyPropertyChanged, INotifyCollectionChanged,
                                        IDisposable
    {
        #region Fields (1)

        /// <summary>
        /// Stores the wrapped collection.
        /// </summary>
        protected readonly ICollection<T> _BASE_COLLECTION;

        #endregion Fields (1)

        #region Constructors (3)

        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionWrapper{T}" /> class.
        /// </summary>
        public CollectionWrapper()
            : this(coll: new List<T>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionWrapper{T}" /> class.
        /// </summary>
        /// <param name="coll">The base collection.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="coll" /> is <see langword="null" />.
        /// </exception>
        public CollectionWrapper(ICollection<T> coll)
        {
            if (coll == null)
            {
                throw new ArgumentNullException("coll");
            }

            this._BASE_COLLECTION = coll;

            if (this._BASE_COLLECTION is INotifyPropertyChanged)
            {
                ((INotifyPropertyChanged)this._BASE_COLLECTION).PropertyChanged += this.CollectionWrapper_PropertyChanged;
            }

            if (this._BASE_COLLECTION is INotifyCollectionChanged)
            {
                ((INotifyCollectionChanged)this._BASE_COLLECTION).CollectionChanged += this.CollectionWrapper_CollectionChanged;
            }
        }

        /// <summary>
        /// Sends the object to the garbage collector.
        /// </summary>
        ~CollectionWrapper()
        {
            try
            {
                try
                {
                    if (this._BASE_COLLECTION is INotifyPropertyChanged)
                    {
                        ((INotifyPropertyChanged)this._BASE_COLLECTION).PropertyChanged -= this.CollectionWrapper_PropertyChanged;
                    }
                }
                finally
                {
                    try
                    {
                        if (this._BASE_COLLECTION is INotifyCollectionChanged)
                        {
                            ((INotifyCollectionChanged)this._BASE_COLLECTION).CollectionChanged -= this.CollectionWrapper_CollectionChanged;
                        }
                    }
                    finally
                    {
                        if (this._BASE_COLLECTION is IDisposable)
                        {
                            this.OnDispose((IDisposable)this._BASE_COLLECTION, false);
                        }
                    }
                }
            }
            catch
            {
                // ignore
            }
        }

        #endregion Constructors (3)

        #region Events (2)

        /// <summary>
        /// <see cref="INotifyCollectionChanged.CollectionChanged" />
        /// </summary>
        public event NotifyCollectionChangedEventHandler CollectionChanged;

        /// <summary>
        /// <see cref="INotifyPropertyChanged.PropertyChanged" />
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Events (2)

        #region Properties (5)

        /// <summary>
        /// Gets the wrapped collection.
        /// </summary>
        public ICollection<T> BaseCollection
        {
            get { return this._BASE_COLLECTION; }
        }

        /// <inheriteddoc />
        public virtual int Count
        {
            get { return this._BASE_COLLECTION.Count; }
        }

        /// <inheriteddoc />
        public virtual bool IsReadOnly
        {
            get { return this._BASE_COLLECTION.IsReadOnly; }
        }

        /// <inheriteddoc />
        public virtual bool IsSynchronized
        {
            get
            {
                if (this._BASE_COLLECTION is ICollection)
                {
                    return ((ICollection)this._BASE_COLLECTION).IsSynchronized;
                }

                return false;
            }
        }

        /// <inheriteddoc />
        public virtual object SyncRoot
        {
            get
            {
                if (this._BASE_COLLECTION is ICollection)
                {
                    return ((ICollection)this._BASE_COLLECTION).SyncRoot;
                }

                return this;
            }
        }

        #endregion Properties (5)

        #region Methods (18)

        /// <inheriteddoc />
        public virtual void Add(T item)
        {
            this._BASE_COLLECTION.Add(item);
        }

        /// <inheriteddoc />
        public virtual void Clear()
        {
            this._BASE_COLLECTION.Clear();
        }

        private void CollectionWrapper_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var handler = this.CollectionChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void CollectionWrapper_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var handler = this.PropertyChanged;
            if (handler != null)
            {
                if (this.GetType().GetProperties(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic)
                                  .Any(x => x.Name == e.PropertyName))
                {
                    handler(this, e);
                }
            }
        }

        /// <inheriteddoc />
        public virtual bool Contains(T item)
        {
            return this._BASE_COLLECTION.Contains(item);
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
        public virtual void CopyTo(T[] array, int arrayIndex)
        {
            this._BASE_COLLECTION.CopyTo(array, arrayIndex);
        }

        /// <inheriteddoc />
        protected virtual void CopyTo(Array array, int index)
        {
            if (this._BASE_COLLECTION is ICollection)
            {
                ((ICollection)this._BASE_COLLECTION).CopyTo(array, index);
                return;
            }

            var srcArray = this._BASE_COLLECTION as T[];
            if (srcArray == null)
            {
                srcArray = this._BASE_COLLECTION.ToArray();
            }

            Array.Copy(sourceArray: srcArray, sourceIndex: 0,
                       destinationArray: array, destinationIndex: index,
                       length: srcArray.Length);
        }

        void ICollection.CopyTo(Array array, int index)
        {
            this.CopyTo(array, index);
        }

        /// <inheriteddoc />
        public void Dispose()
        {
            if (this._BASE_COLLECTION is IDisposable)
            {
                this.OnDispose((IDisposable)this._BASE_COLLECTION, true);
            }

            GC.SuppressFinalize(this);
        }

        /// <inheriteddoc />
        public override sealed bool Equals(object obj)
        {
            return this._BASE_COLLECTION.Equals(obj);
        }

        /// <inheriteddoc />
        public virtual IEnumerator<T> GetEnumerator()
        {
            return this._BASE_COLLECTION.GetEnumerator();
        }

        /// <inheriteddoc />
        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <inheriteddoc />
        public override sealed int GetHashCode()
        {
            return this._BASE_COLLECTION.GetHashCode();
        }

        /// <summary>
        /// Returns the equality comparer for the items.
        /// </summary>
        /// <returns>The equality comparer.</returns>
        protected virtual IEqualityComparer<T> GetItemEqualityComparer()
        {
            // system default
            return null;
        }

        /// <summary>
        /// The logic for the <see cref="CollectionWrapper{T}.Dispose()" /> method and the destructor.
        /// </summary>
        /// <param name="coll"><see cref="CollectionWrapper{T}._BASE_COLLECTION" /> as <see cref="IDisposable" /> object.</param>
        /// <param name="disposing">
        /// <see cref="CollectionWrapper{T}.Dispose()" /> method was invoked (<see langword="true" />)
        /// or the destructor (<see langword="false" />).
        /// </param>
        protected virtual void OnDispose(IDisposable coll, bool disposing)
        {
            if (disposing)
            {
                coll.Dispose();
            }
        }

        /// <inheriteddoc />
        public virtual bool Remove(T item)
        {
            return this._BASE_COLLECTION.Remove(item);
        }

        /// <inheriteddoc />
        public override string ToString()
        {
            return this._BASE_COLLECTION.ToString();
        }

        #endregion Methods (18)
    }
}