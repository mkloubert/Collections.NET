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
    partial class GeneralDictionaryWrapper<TKey, TValue>
    {
        private struct Enumerator : IEnumerator<KeyValuePair<TKey, TValue>>
        {
            #region Fields (3)

            private bool _isDisposed;
            private readonly GeneralDictionaryWrapper<TKey, TValue> _DICT;
            private readonly IDictionaryEnumerator _ENUMERATOR;

            #endregion Fields (3)

            #region Constructors (1)

            internal Enumerator(GeneralDictionaryWrapper<TKey, TValue> dict)
            {
                this._isDisposed = false;

                this._DICT = dict;
                this._ENUMERATOR = dict._BASE_DICT.GetEnumerator();
            }

            #endregion Constructors (1)

            #region Properties (2)

            public KeyValuePair<TKey, TValue> Current
            {
                get
                {
                    this.ThrowIfDisposed();

                    var entry = this._ENUMERATOR.Entry;
                    return new KeyValuePair<TKey, TValue>(this._DICT.ConvertKey(entry.Key),
                                                          this._DICT.ConvertValue(entry.Value));
                }
            }

            object IEnumerator.Current
            {
                get { return this.Current; }
            }

            #endregion Properties (2)

            #region Methods (4)

            public void Dispose()
            {
                var dispObj = this._ENUMERATOR as IDisposable;
                if (dispObj != null)
                {
                    dispObj.Dispose();
                }

                GC.SuppressFinalize(this);

                this._isDisposed = true;
            }

            public bool MoveNext()
            {
                this.ThrowIfDisposed();

                return this._ENUMERATOR.MoveNext();
            }

            public void Reset()
            {
                this.ThrowIfDisposed();

                this._ENUMERATOR.Reset();
            }

            private void ThrowIfDisposed()
            {
                if (this._isDisposed)
                {
                    throw new ObjectDisposedException(this.GetType().FullName);
                }
            }

            #endregion Methods (4)
        }
    }
}