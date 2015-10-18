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

namespace MarcelJoachimKloubert.Collections.Concurrent
{
    /// <summary>
    /// A wrapper that can be used to handle a <see cref="IEnumerator{T}" /> thread safe.
    /// </summary>
    /// <typeparam name="T">Type of the items.</typeparam>
    public class SynchronizedEnumerator<T> : IEnumerator<T>
    {
        #region Fields (2)

        /// <summary>
        /// Stores the value for the <see cref="SynchronizedEnumerator{T}.SyncRoot" /> property.
        /// </summary>
        protected readonly object _SYNC_ROOT = new object();

        private readonly IEnumerator<T> _ENUMERATOR;

        #endregion Fields (2)

        #region Constructors (1)

        /// <summary>
        /// INitializes a new instance of the <see cref="SynchronizedEnumerator{T}" /> class.
        /// </summary>
        /// <param name="enumerator">The value for the <see cref="SynchronizedEnumerator{T}.BaseEnumerator" /> property.</param>
        /// <param name="syncRoot">The value for the <see cref="SynchronizedEnumerator{T}.SyncRoot" /> property.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="enumerator" /> is <see langword="null" />.
        /// </exception>
        public SynchronizedEnumerator(IEnumerator<T> enumerator, object syncRoot = null)
        {
            if (enumerator == null)
            {
                throw new ArgumentNullException("enumerator");
            }

            this._SYNC_ROOT = syncRoot ?? new object();
            this._ENUMERATOR = enumerator;
        }

        #endregion Constructors (1)

        #region Properties (4)

        /// <summary>
        /// Gets the wrapped enumerator.
        /// </summary>
        public IEnumerator<T> BaseEnumerator
        {
            get { return this._ENUMERATOR; }
        }

        /// <inheriteddoc />
        public T Current
        {
            get
            {
                lock (this._SYNC_ROOT)
                {
                    return this._ENUMERATOR.Current;
                }
            }
        }

        object IEnumerator.Current
        {
            get { return this.Current; }
        }

        /// <summary>
        /// Gets the object for thread safe operations.
        /// </summary>
        public object SyncRoot
        {
            get { return this._SYNC_ROOT; }
        }

        #endregion Properties (4)

        #region Methods (6)

        /// <inheriteddoc />
        public void Dispose()
        {
            lock (this._SYNC_ROOT)
            {
                this._ENUMERATOR.Dispose();
            }
        }

        /// <inheriteddoc />
        public override bool Equals(object obj)
        {
            lock (this._SYNC_ROOT)
            {
                return this._ENUMERATOR.Equals(obj);
            }
        }

        /// <inheriteddoc />
        public override int GetHashCode()
        {
            lock (this._SYNC_ROOT)
            {
                return this._ENUMERATOR.GetHashCode();
            }
        }

        /// <inheriteddoc />
        public bool MoveNext()
        {
            lock (this._SYNC_ROOT)
            {
                return this._ENUMERATOR.MoveNext();
            }
        }

        /// <inheriteddoc />
        public void Reset()
        {
            lock (this._SYNC_ROOT)
            {
                this._ENUMERATOR.Reset();
            }
        }

        /// <inheriteddoc />
        public override string ToString()
        {
            lock (this._SYNC_ROOT)
            {
                return this._ENUMERATOR.ToString();
            }
        }

        #endregion Methods (6)
    }
}