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

using NUnit.Framework;
using System.Collections.Generic;

namespace MarcelJoachimKloubert.Collections.Tests.General
{
    /// <summary>
    /// <see cref="DictionaryList{T}" />
    /// </summary>
    public class DictionaryListTests : TestFixtureBase
    {
        #region Methods (2)

        [Test]
        public void TestAdd()
        {
            var newList = new DictionaryList<int>();

            Assert.AreEqual(0, newList.Count);

            newList.Add(1);
            Assert.AreEqual(1, newList.Count);
            Assert.AreEqual(1, newList[0]);

            newList.Add(3);
            Assert.AreEqual(2, newList.Count);
            Assert.AreEqual(1, newList[0]);
            Assert.AreEqual(3, newList[1]);

            newList[null] = 7;
            Assert.AreEqual(3, newList.Count);
            Assert.AreEqual(1, newList[0]);
            Assert.AreEqual(3, newList[1]);
            Assert.AreEqual(7, newList[2]);
            Assert.AreEqual(7, newList[null]);

            newList.Add(1, 9);
            Assert.AreEqual(4, newList.Count);
            Assert.AreEqual(1, newList[0]);
            Assert.AreEqual(9, newList[1]);
            Assert.AreEqual(3, newList[2]);
            Assert.AreEqual(7, newList[3]);

            newList.Add(null, 44);
            Assert.AreEqual(5, newList.Count);
            Assert.AreEqual(1, newList[0]);
            Assert.AreEqual(9, newList[1]);
            Assert.AreEqual(3, newList[2]);
            Assert.AreEqual(7, newList[3]);
            Assert.AreEqual(44, newList[4]);
            Assert.AreEqual(44, newList[null]);

            Assert.AreEqual(5, newList.Count);
            Assert.AreEqual(5, newList.Count);
            Assert.AreEqual(1, newList[0]);
            Assert.AreEqual(9, newList[1]);
            Assert.AreEqual(3, newList[2]);
            Assert.AreEqual(7, newList[3]);
            Assert.AreEqual(44, newList[4]);
        }

        [Test]
        public void TestClear()
        {
            var newList = new List<string>()
                {
                    "A", "B", "C", "D",
                };

            var dictList = new DictionaryList<string>(newList);

            Assert.AreEqual(4, dictList.Count);
            Assert.AreEqual("A", dictList[0]);
            Assert.AreEqual("B", dictList[1]);
            Assert.AreEqual("C", dictList[2]);
            Assert.AreEqual("D", dictList[3]);

            dictList.Clear();

            Assert.AreEqual(0, dictList.Count);
        }

        [Test]
        public void TestContains()
        {
            var newList = new List<string>()
                {
                    "A", "B", "C", "D",
                };

            var dictList = new DictionaryList<string>(newList);

            Assert.AreEqual(4, dictList.Count);

            Assert.IsTrue(dictList.Contains("A"));
            Assert.IsFalse(dictList.Contains("a"));

            Assert.IsTrue(dictList.Contains("B"));
            Assert.IsFalse(dictList.Contains("b"));

            Assert.IsTrue(dictList.Contains("C"));
            Assert.IsFalse(dictList.Contains("c"));

            Assert.IsTrue(dictList.Contains("D"));
            Assert.IsFalse(dictList.Contains("d"));
        }

        [Test]
        public void TestRemove()
        {
            var newList = new List<string>()
                {
                    "A", "B", "C", "D",
                };

            var dictList = new DictionaryList<string>(newList);

            Assert.AreEqual(4, dictList.Count);
            Assert.AreEqual("A", dictList[0]);
            Assert.AreEqual("B", dictList[1]);
            Assert.AreEqual("C", dictList[2]);
            Assert.AreEqual("D", dictList[3]);

            Assert.IsTrue(dictList.Remove("B"));
            Assert.AreEqual(3, dictList.Count);
            Assert.AreEqual("A", dictList[0]);
            Assert.AreEqual("C", dictList[1]);
            Assert.AreEqual("D", dictList[2]);

            Assert.IsFalse(dictList.Remove("a"));
            Assert.AreEqual(3, dictList.Count);
            Assert.AreEqual("A", dictList[0]);
            Assert.AreEqual("C", dictList[1]);
            Assert.AreEqual("D", dictList[2]);
        }

        #endregion Methods (2)
    }
}