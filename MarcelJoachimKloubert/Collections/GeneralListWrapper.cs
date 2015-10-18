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
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Reflection;

namespace MarcelJoachimKloubert.Collections
{
    #region CLASS: GeneralListWrapper<T>

    /// <summary>
    /// A generic wrapper for an <see cref="IList" /> object.
    /// </summary>
    /// <typeparam name="T">Type of the items.</typeparam>
    [DebuggerDisplay("Count = {Count}")]
    [DebuggerTypeProxy(typeof(CollectionDebugView<>))]
    public class GeneralListWrapper<T> : IList<T>, IList, IReadOnlyList<T>,
                                         INotifyPropertyChanged, INotifyCollectionChanged,
                                         IDisposable
    {
        #region Fields (1)

        /// <summary>
        /// Stores the wrapped list.
        /// </summary>
        protected readonly IList _BASE_LIST;

        #endregion Fields (1)

        #region Constructors (3)

        /// <summary>
        /// Initializes a new instance of the <see cref="GeneralListWrapper{T}" /> class.
        /// </summary>
        public GeneralListWrapper()
            : this(list: new List<T>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeneralListWrapper{T}" /> class.
        /// </summary>
        /// <param name="list">The base list.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="list" /> is <see langword="null" />.
        /// </exception>
        public GeneralListWrapper(IList list)
            : base()
        {
            if (list == null)
            {
                throw new ArgumentNullException("list");
            }

            this._BASE_LIST = list;

            if (this._BASE_LIST is INotifyPropertyChanged)
            {
                ((INotifyPropertyChanged)this._BASE_LIST).PropertyChanged += this.GeneralListWrapper_PropertyChanged;
            }

            if (this._BASE_LIST is INotifyCollectionChanged)
            {
                ((INotifyCollectionChanged)this._BASE_LIST).CollectionChanged += this.GeneralListWrapper_CollectionChanged;
            }
        }

        /// <summary>
        /// Sends the object to the garbage collector.
        /// </summary>
        ~GeneralListWrapper()
        {
            try
            {
                try
                {
                    if (this._BASE_LIST is INotifyPropertyChanged)
                    {
                        ((INotifyPropertyChanged)this._BASE_LIST).PropertyChanged -= this.GeneralListWrapper_PropertyChanged;
                    }
                }
                finally
                {
                    try
                    {
                        if (this._BASE_LIST is INotifyCollectionChanged)
                        {
                            ((INotifyCollectionChanged)this._BASE_LIST).CollectionChanged -= this.GeneralListWrapper_CollectionChanged;
                        }
                    }
                    finally
                    {
                        if (this._BASE_LIST is IDisposable)
                        {
                            this.OnDispose((IDisposable)this._BASE_LIST, false);
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

        #region Properties (8)

        /// <summary>
        /// Gets the wrapped list.
        /// </summary>
        public IList BaseList
        {
            get { return this._BASE_LIST; }
        }

        /// <inheriteddoc />
        public int Count
        {
            get { return this._BASE_LIST.Count; }
        }

        /// <inheriteddoc />
        public bool IsFixedSize
        {
            get { return this._BASE_LIST.IsFixedSize; }
        }

        /// <inheriteddoc />
        public bool IsReadOnly
        {
            get { return this._BASE_LIST.IsReadOnly; }
        }

        /// <inheriteddoc />
        public bool IsSynchronized
        {
            get { return this._BASE_LIST.IsSynchronized; }
        }

        /// <inheriteddoc />
        public object SyncRoot
        {
            get { return this._BASE_LIST.SyncRoot; }
        }

        /// <inheriteddoc />
        public T this[int index]
        {
            get { return this.ConvertItem(this._BASE_LIST[index]); }

            set { this._BASE_LIST[index] = value; }
        }

        object IList.this[int index]
        {
            get { return this._BASE_LIST[index]; }

            set { this._BASE_LIST[index] = value; }
        }

        #endregion Properties (8)

        #region Methods (24)

        /// <inheriteddoc />
        public void Add(T item)
        {
            this._BASE_LIST.Add(item);
        }

        int IList.Add(object value)
        {
            return this._BASE_LIST.Add(value);
        }

        /// <inheriteddoc />
        public void Clear()
        {
            this._BASE_LIST.Clear();
        }

        /// <inheriteddoc />
        public bool Contains(T item)
        {
            return this._BASE_LIST.Contains(item);
        }

        bool IList.Contains(object value)
        {
            return this._BASE_LIST.Contains(value);
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
        public void CopyTo(T[] array, int arrayIndex)
        {
            this._BASE_LIST.CopyTo(array, arrayIndex);
        }

        void ICollection.CopyTo(Array array, int index)
        {
            this._BASE_LIST.CopyTo(array, index);
        }

        /// <inheriteddoc />
        public void Dispose()
        {
            if (this._BASE_LIST is IDisposable)
            {
                this.OnDispose((IDisposable)this._BASE_LIST, true);
            }

            GC.SuppressFinalize(this);
        }

        /// <inheriteddoc />
        public override bool Equals(object obj)
        {
            return this._BASE_LIST.Equals(obj);
        }

        private void GeneralListWrapper_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            var handler = this.CollectionChanged;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        private void GeneralListWrapper_PropertyChanged(object sender, PropertyChangedEventArgs e)
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
        public IEnumerator<T> GetEnumerator()
        {
            return this._BASE_LIST
                       .Cast<object>()
                       .Select(x => this.ConvertItem(x))
                       .GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <inheriteddoc />
        public override int GetHashCode()
        {
            return this._BASE_LIST.GetHashCode();
        }

        /// <inheriteddoc />
        public int IndexOf(T item)
        {
            return this._BASE_LIST.IndexOf(item);
        }

        int IList.IndexOf(object value)
        {
            return this._BASE_LIST.IndexOf(value);
        }

        /// <inheriteddoc />
        public void Insert(int index, T item)
        {
            this._BASE_LIST.Insert(index, item);
        }

        void IList.Insert(int index, object value)
        {
            this._BASE_LIST.Insert(index, value);
        }

        /// <summary>
        /// The logic for the <see cref="GeneralDictionaryWrapper{TKey, TValue}.Dispose()" /> method and the destructor.
        /// </summary>
        /// <param name="coll"><see cref="GeneralDictionaryWrapper{TKey, TValue}._BASE_DICT" /> as <see cref="IDisposable" /> object.</param>
        /// <param name="disposing">
        /// <see cref="GeneralDictionaryWrapper{TKey, TValue}.Dispose()" /> method was invoked (<see langword="true" />)
        /// or the destructor (<see langword="false" />).
        /// </param>
        protected void OnDispose(IDisposable coll, bool disposing)
        {
            if (disposing)
            {
                coll.Dispose();
            }
        }

        /// <inheriteddoc />
        public bool Remove(T item)
        {
            var result = this.Contains(item);

            this._BASE_LIST.Remove(item);
            return result;
        }

        void IList.Remove(object value)
        {
            this._BASE_LIST.Remove(value);
        }

        /// <inheriteddoc />
        public void RemoveAt(int index)
        {
            this._BASE_LIST.RemoveAt(index);
        }

        /// <inheriteddoc />
        public override string ToString()
        {
            return this._BASE_LIST.ToString();
        }

        #endregion Methods (24)
    }

    #endregion CLASS: GeneralListWrapper<T>

    #region CLASS: GeneralListWrapper

    /// <summary>
    /// A generic wrapper for an <see cref="IList" /> object.
    /// </summary>
    [DebuggerDisplay("Count = {Count}")]
    [DebuggerTypeProxy(typeof(CollectionDebugView<>))]
    public class GeneralListWrapper : GeneralListWrapper<object>
    {
        #region Constructors (2)

        /// <summary>
        /// Initializes a new instance of the <see cref="GeneralListWrapper" /> class.
        /// </summary>
        public GeneralListWrapper()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GeneralListWrapper" /> class.
        /// </summary>
        /// <param name="list">The base list.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="list" /> is <see langword="null" />.
        /// </exception>
        public GeneralListWrapper(IList list)
            : base(list: list)
        {
        }

        #endregion Constructors (2)
    }

    #endregion CLASS: GeneralListWrapper
}