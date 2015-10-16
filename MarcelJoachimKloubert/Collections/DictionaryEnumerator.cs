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

namespace MarcelJoachimKloubert.Collections
{
    /// <summary>
    /// A <see cref="IDictionaryEnumerator" /> wrapper for an <see cref="IDictionary{TKey, TValue}" /> instance.
    /// </summary>
    /// <typeparam name="TKey">Type of the keys.</typeparam>
    /// <typeparam name="TValue">Type of the values.</typeparam>
    public class DictionaryEnumerator<TKey, TValue> : IDictionaryEnumerator, IEnumerator<KeyValuePair<TKey, TValue>>
    {
        #region Fields (1)

        private readonly IEnumerator<KeyValuePair<TKey, TValue>> _ENUMERATOR;

        #endregion Fields (1)

        #region Constructors (2)

        /// <summary>
        /// Initializes a new instance of the <see cref="DictionaryEnumerator{TKey, TValue}" /> class.
        /// </summary>
        /// <param name="dict">The dictionary from where to get the enumerator.</param>
        /// <exception cref="NullReferenceException">
        /// <paramref name="dict" /> is <see langword="null" />.
        /// </exception>
        public DictionaryEnumerator(IDictionary<TKey, TValue> dict)
            : this(enumerator: dict.GetEnumerator())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DictionaryEnumerator{TKey, TValue}" /> class.
        /// </summary>
        /// <param name="enumerator">The inner enumerator to use.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="enumerator" /> is <see langword="null" />.
        /// </exception>
        public DictionaryEnumerator(IEnumerator<KeyValuePair<TKey, TValue>> enumerator)
        {
            if (enumerator == null)
            {
                throw new ArgumentNullException("enumerator");
            }

            this._ENUMERATOR = enumerator;
        }

        #endregion Constructors (2)

        #region Properties (6)

        /// <inheriteddoc />
        public KeyValuePair<TKey, TValue> Current
        {
            get { return this._ENUMERATOR.Current; }
        }

        /// <inheriteddoc />
        public DictionaryEntry Entry
        {
            get { return new DictionaryEntry(this.Current.Key, this.Current.Value); }
        }

        /// <inheriteddoc />
        object IEnumerator.Current
        {
            get { return this.Entry; }
        }

        /// <inheriteddoc />
        public object Key
        {
            get { return this.Current.Key; }
        }

        /// <inheriteddoc />
        public object Value
        {
            get { return this.Current.Value; }
        }

        #endregion Properties (6)

        #region Methods (1)

        /// <inheriteddoc />
        public void Dispose()
        {
            this._ENUMERATOR.Dispose();
        }

        /// <inheriteddoc />
        public bool MoveNext()
        {
            return this._ENUMERATOR.MoveNext();
        }

        /// <inheriteddoc />
        public void Reset()
        {
            this._ENUMERATOR.Reset();
        }

        #endregion Methods (1)
    }
}