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

namespace MarcelJoachimKloubert.Collections
{
    /// <summary>
    /// A wrapper for an <see cref="IList{T}" /> object.
    /// All required members are virtual an can be overwritten in later context.
    /// </summary>
    /// <typeparam name="T">Type of the items.</typeparam>
    [DebuggerDisplay("Count = {Count}")]
    [DebuggerTypeProxy(typeof(CollectionDebugView<>))]
    public class ListWrapper<T> : CollectionWrapper<T>, IList<T>, IList, IReadOnlyList<T>
    {
        #region Constructors (2)

        /// <summary>
        /// Initializes a new instance of the <see cref="ListWrapper{T}" /> class.
        /// </summary>
        public ListWrapper()
            : this(list: new List<T>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ListWrapper{T}" /> class.
        /// </summary>
        /// <param name="list">The base list.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="list" /> is <see langword="null" />.
        /// </exception>
        public ListWrapper(IList<T> list)
            : base(coll: list)
        {
        }

        #endregion Constructors (2)

        #region Properties (4)

        /// <inheriteddoc />
        public new IList<T> BaseCollection
        {
            get { return (IList<T>)base._BASE_COLLECTION; }
        }

        /// <inheriteddoc />
        public virtual bool IsFixedSize
        {
            get
            {
                if (this._BASE_COLLECTION is IList)
                {
                    return ((IList)this._BASE_COLLECTION).IsFixedSize;
                }

                return this.IsReadOnly;
            }
        }

        /// <inheriteddoc />
        public virtual T this[int index]
        {
            get { return this.BaseCollection[index]; }

            set { this.BaseCollection[index] = value; }
        }

        object IList.this[int index]
        {
            get { return this[index]; }

            set { this[index] = this.ConvertItem(value); }
        }

        #endregion Properties (4)

        #region Methods (9)

        /// <summary>
        /// <see cref="IList.Add(object)" />
        /// </summary>
        protected virtual int Add(object value)
        {
            this.Add(this.ConvertItem(value));
            return this.Count;
        }

        int IList.Add(object value)
        {
            return this.Add(value);
        }

        bool IList.Contains(object value)
        {
            return this.Contains(this.ConvertItem(value));
        }

        /// <inheriteddoc />
        public virtual int IndexOf(T item)
        {
            return this.BaseCollection.IndexOf(item);
        }

        /// <inheriteddoc />
        public virtual void Insert(int index, T item)
        {
            this.BaseCollection.Insert(index, item);
        }

        void IList.Insert(int index, object value)
        {
            this.Insert(index, this.ConvertItem(value));
        }

        int IList.IndexOf(object value)
        {
            return this.IndexOf(this.ConvertItem(value));
        }

        void IList.Remove(object value)
        {
            this.Remove(this.ConvertItem(value));
        }

        /// <inheriteddoc />
        public virtual void RemoveAt(int index)
        {
            this.BaseCollection.RemoveAt(index);
        }

        #endregion Methods (9)
    }
}