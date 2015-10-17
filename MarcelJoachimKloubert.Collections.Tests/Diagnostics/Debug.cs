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

using MarcelJoachimKloubert.Collections.Concurrent;
using NUnit.Framework;
using System;
using System.Collections.Generic;

namespace MarcelJoachimKloubert.Collections.Tests.Diagnostics
{
    public class Debug : TestFixtureBase
    {
        #region Methods (1)

        [Test]
        public void Test1()
        {
            var collectionTypes = new Type[]
            {
                typeof(CollectionWrapper<object>),
                typeof(DictionaryList<object>),
                typeof(ListWrapper<object>),
                typeof(SetWrapper<object>),

                typeof(SynchronizedCollection<object>),
                typeof(SynchronizedList<object>),
                typeof(SynchronizedDictionaryList<object>),
            };

            foreach (var ct in collectionTypes)
            {
                var coll = (ICollection<object>)Activator.CreateInstance(ct);

                coll.Add(1);
                coll.Add("2");
                coll.Add(3.4);

                if (coll == null)
                {
                    continue;
                }
            }

            var dictionaryTypes = new Type[]
            {
                typeof(Dictionary<object, object>),
                typeof(DictionaryWrapper<object, object>),
                typeof(SynchronizedDictionary<object, object>),
            };

            foreach (var dt in dictionaryTypes)
            {
                var dict = (IDictionary<object, object>)Activator.CreateInstance(dt);

                dict.Add('A', 1);
                dict.Add('B', "2");
                dict.Add('C', 3.4);

                if (dict == null)
                {
                    continue;
                }
            }
        }

        #endregion Methods (1)
    }
}