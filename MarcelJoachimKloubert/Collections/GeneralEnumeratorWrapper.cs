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
    /// Wraps an <see cref="IEnumerator" /> object as generic one.
    /// </summary>
    /// <typeparam name="T">Item type.</typeparam>
    public class GeneralEnumeratorWrapper<T> : IEnumerator<T>
    {
        #region Fields

        private readonly IEnumerator _ENUMERATOR;
        private readonly bool _OWNS_ENUMERATOR;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GeneralEnumeratorWrapper{T}" /> class.
        /// </summary>
        /// <param name="enumerator">The enumerator to wrap.</param>
        /// <param name="ownsEnumerator">Own <paramref name="enumerator" /> or not.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="enumerator" /> is <see langword="null" />.
        /// </exception>
        public GeneralEnumeratorWrapper(IEnumerator enumerator,
                                        bool ownsEnumerator = false)
        {
            if (enumerator == null)
            {
                throw new ArgumentNullException("enumerator");
            }

            _ENUMERATOR = enumerator;
            _OWNS_ENUMERATOR = ownsEnumerator;
        }

        #endregion Constructors

        #region Properties

        /// <inheriteddoc />
        public T Current
        {
            get { return CastObject(_ENUMERATOR.Current); }
        }

        object IEnumerator.Current
        {
            get { return Current; }
        }

        #endregion Properties

        #region Methods

        /// <inheriteddoc />
        public void Dispose()
        {
            if (!_OWNS_ENUMERATOR)
            {
                return;
            }

            var dispObj = _ENUMERATOR as IDisposable;
            if (dispObj == null)
            {
                return;
            }

            dispObj.Dispose();
        }

        /// <inheriteddoc />
        public override bool Equals(object obj)
        {
            return _ENUMERATOR.Equals(obj);
        }

        /// <inheriteddoc />
        public override int GetHashCode()
        {
            return _ENUMERATOR.GetHashCode();
        }

        /// <inheriteddoc />
        public bool MoveNext()
        {
            return _ENUMERATOR.MoveNext();
        }

        /// <inheriteddoc />
        public void Reset()
        {
            _ENUMERATOR.Reset();
        }

        /// <inheriteddoc />
        public override string ToString()
        {
            return _ENUMERATOR.ToString();
        }

        /// <summary>
        /// Casts an object to the item type.
        /// </summary>
        /// <param name="obj">The input value.</param>
        /// <returns>The casted value.</returns>
        protected virtual T CastObject(object obj)
        {
            return (T)obj;
        }

        #endregion Methods
    }

    /// <summary>
    /// Simple extension of <see cref="GeneralEnumeratorWrapper{T}" /> class.
    /// </summary>
    public class GeneralEnumeratorWrapper : GeneralEnumeratorWrapper<object>
    {
        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="GeneralEnumeratorWrapper{T}" /> class.
        /// </summary>
        /// <param name="enumerator">The enumerator to wrap.</param>
        /// <param name="ownsEnumerator">Own <paramref name="enumerator" /> or not.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="enumerator" /> is <see langword="null" />.
        /// </exception>
        public GeneralEnumeratorWrapper(IEnumerator enumerator,
                                        bool ownsEnumerator = false)
            : base(enumerator: enumerator,
                   ownsEnumerator: ownsEnumerator)
        {
        }

        #endregion Constructors
    }
}