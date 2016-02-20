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

namespace MarcelJoachimKloubert.Collections
{
    /// <summary>
    /// Simple implementation of the <see cref="IChunkedList{T}" /> interface.
    /// </summary>
    /// <typeparam name="T">Type of the items.</typeparam>
    public class ChunkedList<T> : IChunkedList<T>
    {
        #region Fields

        private readonly IEnumerator<T> _ENUMERATOR;
        private readonly T[] _NEXT_ITEM;
        private readonly bool _OWNS_ENUMERATOR;
        private readonly int _SIZE;

        #endregion Fields

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="ChunkedList{T}" /> class.
        /// </summary>
        /// <param name="seq">The sequence with the current items.</param>
        /// <param name="size">The chunk size.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="size" /> is invalid.
        /// </exception>
        /// <exception cref="NullReferenceException">
        /// <paramref name="seq" /> is <see langword="null" />.
        /// </exception>
        public ChunkedList(IEnumerable<T> seq, int size = 25)
            : this(e: seq.GetEnumerator(), size: size,
                   ownsEnumerator: true)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ChunkedList{T}" /> class.
        /// </summary>
        /// <param name="e">The underlying enumerator with the current items.</param>
        /// <param name="size">The chunk size.</param>
        /// <param name="ownsEnumerator">Object should own <paramref name="e" /> or not..</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="size" /> is invalid.
        /// </exception>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="e" /> is <see langword="null" />.
        /// </exception>
        public ChunkedList(IEnumerator<T> e, int size, bool ownsEnumerator = false)
        {
            if (e == null)
            {
                throw new ArgumentNullException("e");
            }

            if (size < 1)
            {
                throw new ArgumentOutOfRangeException("size", size, "Must be 1 at least.");
            }

            _ENUMERATOR = e;
            _SIZE = size;
            _OWNS_ENUMERATOR = ownsEnumerator;

            CurrentChunk = CreateChunkStorage() ?? new List<T>();

            var i = 0;
            var mightHasMoreItems = false;
            while ((i < _SIZE) &&
                   (mightHasMoreItems = _ENUMERATOR.MoveNext()))
            {
                CurrentChunk.Add(_ENUMERATOR.Current);
                ++i;
            }

            if (mightHasMoreItems)
            {
                if (_ENUMERATOR.MoveNext())
                {
                    _NEXT_ITEM = new[] { _ENUMERATOR.Current };
                }
            }

            if (null == _NEXT_ITEM)
            {
                _NEXT_ITEM = new T[0];

                if (_OWNS_ENUMERATOR)
                {
                    _ENUMERATOR.Dispose();
                }
            }
        }

        #endregion Constructors

        #region Properties

        /// <inheriteddoc />
        public IList<T> CurrentChunk
        {
            get;
            private set;
        }

        /// <inheriteddoc />
        public bool HasMoreChunks
        {
            get { return _NEXT_ITEM.Length > 0; }
        }

        #endregion Properties

        #region Methods

        /// <inheriteddoc />
        public ChunkedList<T> GetNextChunk()
        {
            if (!HasMoreChunks)
            {
                throw new InvalidOperationException();
            }

            return new ChunkedList<T>(GetNextChunkList(), _SIZE);
        }

        IChunkedList<T> IChunkedList<T>.GetNextChunk()
        {
            return GetNextChunk();
        }

        /// <summary>
        /// Creates the value for the <see cref="ChunkedList{T}.CurrentChunk" /> property.
        /// </summary>
        /// <returns>The created list.</returns>
        protected virtual IList<T> CreateChunkStorage()
        {
            // create default instance
            return null;
        }

        private IEnumerable<T> GetNextChunkList()
        {
            foreach (var nextItem in _NEXT_ITEM)
            {
                yield return nextItem;
            }

            while (_ENUMERATOR.MoveNext())
            {
                yield return _ENUMERATOR.Current;
            }

            if (_OWNS_ENUMERATOR)
            {
                _ENUMERATOR.Dispose();
            }
        }

        #endregion Methods
    }
}