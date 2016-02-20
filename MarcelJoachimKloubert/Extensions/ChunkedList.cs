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

using MarcelJoachimKloubert.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MarcelJoachimKloubert.Extensions
{
    static partial class MJKCollectionExtensionMethods
    {
        #region Methods

        /// <summary>
        /// Returns a sequence as chunked list.
        /// </summary>
        /// <typeparam name="T">Type of the items.</typeparam>
        /// <param name="seq">The sequence.</param>
        /// <param name="size">The custom size to use.</param>
        /// <param name="ofType">Remove <see langword="null" /> references or not.</param>
        /// <returns>The sequence as chunked list.</returns>
        /// <remarks>
        /// <see langword="null" /> is returned if <paramref name="seq" /> is <see langword="null" />.
        /// </remarks>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="size" /> is invalid.
        /// </exception>
        public static IChunkedList<T> AsChunk<T>(this IEnumerable<T> seq, int? size = null, bool ofType = false)
        {
            if (seq == null)
            {
                return null;
            }

            if (ofType)
            {
                seq = seq.Where(x => x != null);
            }

            IChunkedList<T> result;
            if (size.HasValue)
            {
                result = new ChunkedList<T>(seq, size.Value);
            }
            else
            {
                result = seq as IChunkedList<T>;
            }

            if (result == null)
            {
                result = new ChunkedList<T>(seq);
            }

            return result;
        }

        /// <summary>
        /// Flattens a chunked list.
        /// </summary>
        /// <typeparam name="T">Type of the items.</typeparam>
        /// <param name="list">The list.</param>
        /// <returns>The flatten version of <paramref name="list" />.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="list" /> is <see langword="null" />.
        /// </exception>
        public static IEnumerable<T> Flatten<T>(this IChunkedList<T> list)
        {
            if (list == null)
            {
                throw new ArgumentNullException("list");
            }

            do
            {
                var chunk = list.CurrentChunk;
                if (chunk != null)
                {
                    using (var e = chunk.GetEnumerator())
                    {
                        while (e.MoveNext())
                        {
                            yield return e.Current;
                        }
                    }
                }

                if (list.HasMoreChunks)
                {
                    list = list.GetNextChunk();
                }
                else
                {
                    list = null;
                }
            }
            while (list != null);
        }

        /// <summary>
        /// Invokes an <see cref="IChunkedList{T}.GetNextChunk()" /> method async.
        /// </summary>
        /// <typeparam name="T">Type of the items.</typeparam>
        /// <param name="list">The list.</param>
        /// <returns>The running task.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="list" /> is <see langword="null" />.
        /// </exception>
        public static Task<IChunkedList<T>> GetNextChunkAsync<T>(this IChunkedList<T> list)
        {
            if (list == null)
            {
                throw new ArgumentNullException("list");
            }

            return Task.Factory
                       .StartNew((state) => ((IChunkedList<T>)state).GetNextChunk(),
                                 state: list);
        }

        #endregion Methods
    }
}