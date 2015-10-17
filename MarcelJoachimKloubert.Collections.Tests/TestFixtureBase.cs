/**********************************************************************************************************************
 * Notifiable.NET (https://github.com/mkloubert/Notifiable.NET)                                                       *
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

namespace MarcelJoachimKloubert.Collections.Tests
{
    /// <summary>
    /// A basic test fixture.
    /// </summary>
    [TestFixture]
    public abstract class TestFixtureBase
    {
        #region Constructors (1)

        /// <summary>
        /// Initializes a new instance of the <see cref="TestFixtureBase" /> class.
        /// </summary>
        protected TestFixtureBase()
        {
        }

        #endregion Constructors (1)

        #region Methods (8)

        /// <summary>
        /// The logic for the <see cref="TestFixtureBase.SetupFixture" /> method.
        /// </summary>
        protected virtual void OnSetupFixture()
        {
            // dummy
        }

        /// <summary>
        /// The logic for the <see cref="TestFixtureBase.SetupTest" /> method.
        /// </summary>
        protected virtual void OnSetupTest()
        {
            // dummy
        }

        /// <summary>
        /// The logic for the <see cref="TestFixtureBase.TearDownFixture" /> method.
        /// </summary>
        protected virtual void OnTearDownFixture()
        {
            // dummy
        }

        /// <summary>
        /// The logic for the <see cref="TestFixtureBase.TearDownTest" /> method.
        /// </summary>
        protected virtual void OnTearDownTest()
        {
            // dummy
        }

        /// <summary>
        /// Logic for <see cref="TestFixtureSetUpAttribute" />.
        /// </summary>
        [TestFixtureSetUp]
        public void SetupFixture()
        {
            this.OnSetupFixture();
        }

        /// <summary>
        /// Logic for <see cref="SetUpAttribute" />.
        /// </summary>
        [SetUp]
        public void SetupTest()
        {
            this.OnSetupTest();
        }

        /// <summary>
        /// Logic for <see cref="TestFixtureTearDownAttribute" />.
        /// </summary>
        [TestFixtureTearDown]
        public void TearDownFixture()
        {
            this.OnTearDownFixture();
        }

        /// <summary>
        /// Logic for <see cref="TearDownAttribute" />.
        /// </summary>
        [TearDown]
        public void TearDownTest()
        {
            this.OnTearDownTest();
        }

        #endregion Methods (8)
    }
}