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

using MarcelJoachimKloubert.Extensions;
using NUnit.Framework;
using System;
using System.Linq;

namespace MarcelJoachimKloubert.Collections.Tests.Collections
{
    public class ChunkedListTests : TestFixtureBase
    {
        #region Methods

        [Test]
        public void Test1()
        {
            for (var chunkSize = 1; chunkSize <= 100; chunkSize++)
            {
                for (var itemSize = 1; itemSize <= 1000; itemSize++)
                {
                    var expectedNrOfChunks = (int)Math.Ceiling((float)itemSize / (float)chunkSize);
                    var seq = Enumerable.Range(0, itemSize);

                    var chunkIndex = 1;
                    IChunkedList<int> chunk = new ChunkedList<int>(seq, chunkSize);
                    do
                    {
                        try
                        {
                            var expectedChunkSize = chunkSize;
                            if (chunkIndex == expectedNrOfChunks)
                            {
                                expectedChunkSize = itemSize % chunkSize;
                                if (expectedChunkSize == 0)
                                {
                                    expectedChunkSize = chunkSize;
                                }
                            }

                            var expectedItems = Enumerable.Range((chunkIndex - 1) * chunkSize, expectedChunkSize);

                            Assert.IsTrue(chunk.CurrentChunk
                                               .SequenceEqual(expectedItems));

                            if (chunk.HasMoreChunks)
                            {
                                chunk = chunk.GetNextChunk();
                            }
                            else
                            {
                                break;
                            }
                        }
                        finally
                        {
                            ++chunkIndex;
                        }
                    } while (true);

                    Assert.AreEqual(expectedNrOfChunks, chunkIndex - 1);
                }
            }
        }

        [Test]
        public void TestFlatten()
        {
            for (var chunkSize = 1; chunkSize <= 100; chunkSize++)
            {
                for (var itemSize = 1; itemSize <= 1000; itemSize++)
                {
                    var seq = Enumerable.Range(0, itemSize);
                    var chunkedList = new ChunkedList<int>(seq, chunkSize);
                    var flattenList = chunkedList.Flatten();

                    Assert.True(flattenList.SequenceEqual(seq));
                }
            }
        }

        #endregion Methods
    }
}