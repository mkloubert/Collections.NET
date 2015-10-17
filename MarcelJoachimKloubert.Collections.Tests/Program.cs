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
using System;
using System.Linq;
using System.Reflection;

namespace MarcelJoachimKloubert.Collections.Tests
{
    namespace MarcelJoachimKloubert.Tests
    {
        internal static class Program
        {
            #region Fields (1)

            private static object _SYNC = new object();

            #endregion Fields

            #region Methods (4)

            private static void InvokeConsole(Action action)
            {
                InvokeConsole(action, null);
            }

            private static void InvokeConsole(Action action, ConsoleColor? foreColor)
            {
                InvokeConsole(action, foreColor, null);
            }

            private static void InvokeConsole(Action action, ConsoleColor? foreColor, ConsoleColor? bgColor)
            {
                lock (_SYNC)
                {
                    var oldForeColor = Console.ForegroundColor;
                    var oldBgColor = Console.BackgroundColor;

                    try
                    {
                        if (foreColor.HasValue)
                        {
                            Console.ForegroundColor = foreColor.Value;
                        }

                        if (bgColor.HasValue)
                        {
                            Console.BackgroundColor = bgColor.Value;
                        }

                        action();
                    }
                    finally
                    {
                        Console.ForegroundColor = oldForeColor;
                        Console.BackgroundColor = oldBgColor;
                    }
                }
            }

            private static void Main(string[] args)
            {
                foreach (var type in Assembly.GetExecutingAssembly()
                                             .GetTypes()
                                             .Where(t => t.IsPublic &&
                                                         (t.IsAbstract == false))
                                             .OrderBy(t => t.Name, StringComparer.InvariantCultureIgnoreCase))
                {
                    if (type.GetCustomAttributes(typeof(global::NUnit.Framework.IgnoreAttribute), true)
                            .Any())
                    {
                        continue;
                    }

                    if (type.GetCustomAttributes(typeof(global::NUnit.Framework.TestFixtureAttribute), true)
                            .Any() == false)
                    {
                        continue;
                    }

                    var obj = Activator.CreateInstance(type);
                    Console.WriteLine("{0} ...", obj.GetType().Name);

                    var allMethods = obj.GetType()
                                        .GetMethods(BindingFlags.Instance | BindingFlags.Public)
                                        .OrderBy(m => m.Name, StringComparer.InvariantCultureIgnoreCase);

                    var fixtureSetupMethod = allMethods.SingleOrDefault(m => m.GetCustomAttributes(typeof(global::NUnit.Framework.TestFixtureSetUpAttribute), true)
                                                                              .Any());
                    var fixtureTearDownMethod = allMethods.SingleOrDefault(m => m.GetCustomAttributes(typeof(global::NUnit.Framework.TestFixtureTearDownAttribute), true)
                                                                                 .Any());

                    var setupMethod = allMethods.SingleOrDefault(m => m.GetCustomAttributes(typeof(global::NUnit.Framework.SetUpAttribute), true)
                                                                       .Any());
                    var tearDownMethod = allMethods.SingleOrDefault(m => m.GetCustomAttributes(typeof(global::NUnit.Framework.TearDownAttribute), true)
                                                                          .Any());

                    if (fixtureSetupMethod != null)
                    {
                        fixtureSetupMethod.Invoke(obj, null);
                    }

                    foreach (var method in allMethods)
                    {
                        var testAttribs = method.GetCustomAttributes(typeof(global::NUnit.Framework.TestAttribute), true);
                        if (testAttribs.Length < 1)
                        {
                            // not marked as test
                            continue;
                        }

                        var methodIgnoreAttribs = method.GetCustomAttributes(typeof(global::NUnit.Framework.IgnoreAttribute), true);
                        if (methodIgnoreAttribs.Length > 0)
                        {
                            // is ignored
                            continue;
                        }

                        try
                        {
                            var ta = (TestAttribute)testAttribs[0];

                            Console.Write("\t'{0}' ... ", method.Name);

                            if (setupMethod != null)
                            {
                                setupMethod.Invoke(obj, null);
                            }

                            method.Invoke(obj, null);

                            if (tearDownMethod != null)
                            {
                                tearDownMethod.Invoke(obj, null);
                            }

                            InvokeConsole(() =>
                                {
                                    Console.WriteLine("[OK]");
                                }, ConsoleColor.Green
                                 , ConsoleColor.Black);
                        }
                        catch (Exception ex)
                        {
                            InvokeConsole(() =>
                                {
                                    Console.WriteLine("[ERROR: {0}]",
                                                      (ex.GetBaseException() ?? ex).Message);
                                }, ConsoleColor.Red
                                 , ConsoleColor.Black);
                        }
                    }

                    if (fixtureTearDownMethod != null)
                    {
                        fixtureTearDownMethod.Invoke(obj, null);
                    }

                    Console.WriteLine();
                }

                Console.WriteLine();
                Console.WriteLine();
                Console.WriteLine("===== ENTER =====");

                Console.ReadLine();
            }

            #endregion Methods
        }
    }
}