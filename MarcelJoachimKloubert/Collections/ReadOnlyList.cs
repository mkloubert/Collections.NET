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
    /// A read-only wrapper for an <see cref="IList{T}" /> object.
    /// </summary>
    /// <typeparam name="T">Type of the items.</typeparam>
    [DebuggerDisplay("Count = {Count}")]
    [DebuggerTypeProxy(typeof(CollectionDebugView<>))]
    public class ReadOnlyList<T> : ListWrapper<T>
    {
        #region Constructors (2)

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyList{T}" /> class.
        /// </summary>
        public ReadOnlyList()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyList{T}" /> class.
        /// </summary>
        /// <param name="list">The base collection.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="list" /> is <see langword="null" />.
        /// </exception>
        public ReadOnlyList(IList<T> list)
            : base(list: list)
        {
        }

        #endregion Constructors (2)

        #region Properies (3)

        /// <inheriteddoc />
        public override sealed bool IsFixedSize
        {
            get { return true; }
        }

        /// <inheriteddoc />
        public override sealed bool IsReadOnly
        {
            get { return true; }
        }

        /// <inheriteddoc />
        public override sealed T this[int index]
        {
            get { return base[index]; }

            set { throw new NotSupportedException(); }
        }

        #endregion Properies (3)

        #region Methods (6)

        /// <inheriteddoc />
        public override sealed void Add(T item)
        {
            throw new NotSupportedException();
        }

        /// <inheriteddoc />
        protected override sealed int AddItem(object value)
        {
            throw new NotSupportedException();
        }

        /// <inheriteddoc />
        public override sealed void Clear()
        {
            throw new NotSupportedException();
        }

        /// <inheriteddoc />
        public override sealed void Insert(int index, T item)
        {
            throw new NotSupportedException();
        }

        /// <inheriteddoc />
        public override sealed bool Remove(T item)
        {
            throw new NotSupportedException();
        }

        /// <inheriteddoc />
        public override sealed void RemoveAt(int index)
        {
            throw new NotSupportedException();
        }

        #endregion Methods (6)
    }
}