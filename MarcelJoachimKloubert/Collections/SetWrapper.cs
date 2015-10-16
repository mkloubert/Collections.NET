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

namespace MarcelJoachimKloubert.Collections
{
    /// <summary>
    /// A wrapper for an <see cref="ISet{T}" /> object.
    /// All required members are virtual an can be overwritten in later context.
    /// </summary>
    /// <typeparam name="T">Type of the items.</typeparam>
    [DebuggerDisplay("Count = {Count}")]
    [DebuggerTypeProxy(typeof(CollectionDebugView<>))]
    public class SetWrapper<T> : CollectionWrapper<T>, ISet<T>
    {
        #region Constructors (2)

        /// <summary>
        /// Initializes a new instance of the <see cref="SetWrapper{T}" /> class.
        /// </summary>
        public SetWrapper()
            : this(set: new HashSet<T>())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SetWrapper{T}" /> class.
        /// </summary>
        /// <param name="set">The base list.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="set" /> is <see langword="null" />.
        /// </exception>
        public SetWrapper(ISet<T> set)
            : base(coll: set)
        {
        }

        #endregion Constructors (2)

        #region Properties (1)

        /// <inheriteddoc />
        public new ISet<T> BaseCollection
        {
            get { return (ISet<T>)base._BASE_COLLECTION; }
        }

        #endregion Properties (1)

        #region Methods (13)

        /// <inheriteddoc />
        public override sealed void Add(T item)
        {
            this.AddItem(item);
        }

        /// <inheriteddoc />
        protected virtual bool AddItem(T item)
        {
            return this.BaseCollection.Add(item);
        }

        bool ISet<T>.Add(T item)
        {
            return this.AddItem(item);
        }

        /// <inheriteddoc />
        public virtual void ExceptWith(IEnumerable<T> other)
        {
            this.BaseCollection.ExceptWith(other);
        }

        /// <inheriteddoc />
        public virtual void IntersectWith(IEnumerable<T> other)
        {
            this.BaseCollection.IntersectWith(other);
        }

        /// <inheriteddoc />
        public virtual bool IsProperSubsetOf(IEnumerable<T> other)
        {
            return this.BaseCollection.IsProperSubsetOf(other);
        }

        /// <inheriteddoc />
        public virtual bool IsProperSupersetOf(IEnumerable<T> other)
        {
            return this.BaseCollection.IsProperSupersetOf(other);
        }

        /// <inheriteddoc />
        public virtual bool IsSubsetOf(IEnumerable<T> other)
        {
            return this.BaseCollection.IsSubsetOf(other);
        }

        /// <inheriteddoc />
        public virtual bool IsSupersetOf(IEnumerable<T> other)
        {
            return this.BaseCollection.IsSupersetOf(other);
        }

        /// <inheriteddoc />
        public virtual bool Overlaps(IEnumerable<T> other)
        {
            return this.BaseCollection.Overlaps(other);
        }

        /// <inheriteddoc />
        public virtual bool SetEquals(IEnumerable<T> other)
        {
            return this.BaseCollection.SetEquals(other);
        }

        /// <inheriteddoc />
        public virtual void SymmetricExceptWith(IEnumerable<T> other)
        {
            this.BaseCollection.SymmetricExceptWith(other);
        }

        /// <inheriteddoc />
        public virtual void UnionWith(IEnumerable<T> other)
        {
            this.BaseCollection.UnionWith(other);
        }

        #endregion Methods (13)
    }
}