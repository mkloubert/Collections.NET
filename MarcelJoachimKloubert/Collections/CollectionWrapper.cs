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

namespace MarcelJoachimKloubert.Collections
{
    /// <summary>
    /// A wrapper for an <see cref="ICollection{T}" /> object.
    /// All required members are virtual an can be overwritten in later context.
    /// </summary>
    /// <typeparam name="T">Type of the items.</typeparam>
    [DebuggerDisplay("Count = {Count}")]
    [DebuggerTypeProxy(typeof(CollectionDebugView<>))]
    public class CollectionWrapper<T> : ICollection<T>, ICollection, INotifyPropertyChanged, IDisposable
    {
        #region Fields (1)

        /// <summary>
        /// Stores the wrapped collection.
        /// </summary>
        protected readonly ICollection<T> _BASE_COLLECTION;

        #endregion Fields (1)

        #region Constructors (2)

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
        }

        #endregion Constructors (2)

        #region Events (1)

        /// <summary>
        /// <see cref="INotifyPropertyChanged.PropertyChanged" />
        /// </summary>
        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Events (1)

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

        #region Methods (11)

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

        private void CollectionWrapper_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this, e);
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
        public virtual void CopyTo(Array array, int index)
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

        /// <inheriteddoc />
        public void Dispose()
        {
            if (this._BASE_COLLECTION is IDisposable)
            {
                ((IDisposable)this._BASE_COLLECTION).Dispose();
            }

            GC.SuppressFinalize(this);
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
        public virtual bool Remove(T item)
        {
            return this._BASE_COLLECTION.Remove(item);
        }

        #endregion Methods (11)
    }
}