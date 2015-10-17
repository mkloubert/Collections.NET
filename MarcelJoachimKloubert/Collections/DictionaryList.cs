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
using System.Diagnostics;
using System.Linq;

namespace MarcelJoachimKloubert.Collections
{
    /// <summary>
    /// An extension of <see cref="DictionaryList{T}" /> that also works as <see cref="IDictionary{TKey, TValue}" />.
    /// </summary>
    /// <typeparam name="T">Type of the items.</typeparam>
    [DebuggerDisplay("Count = {Count}")]
    [DebuggerTypeProxy(typeof(CollectionDebugView<>))]
    public partial class DictionaryList<T> : ListWrapper<T>, IDictionary<int?, T>, IDictionary, IReadOnlyDictionary<int?, T>
    {
        #region Constructors (2)

        /// <summary>
        /// Initializes a new instance of the <see cref="DictionaryList{T}" /> class.
        /// </summary>
        public DictionaryList()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DictionaryList{T}" /> class.
        /// </summary>
        /// <param name="list">The base list.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="list" /> is <see langword="null" />.
        /// </exception>
        public DictionaryList(IList<T> list)
            : base(list: list)
        {
        }

        #endregion Constructors (2)

        #region Properties (8)

        /// <inheriteddoc />
        public virtual ICollection<int?> Keys
        {
            get
            {
                return Enumerable.Range(0, this.Count)
                                 .Cast<int?>()
                                 .ToList();
            }
        }

        IEnumerable<int?> IReadOnlyDictionary<int?, T>.Keys
        {
            get { return this.Keys; }
        }

        ICollection IDictionary.Keys
        {
            get
            {
                var keys = this.Keys;
                if (keys is ICollection)
                {
                    return (ICollection)keys;
                }

                return new List<int?>(keys);
            }
        }

        /// <inheriteddoc />
        public virtual T this[int? key]
        {
            get { return this[index: this.GetRealKey(key)]; }

            set
            {
                if (key.HasValue)
                {
                    this[index: key.Value] = value;
                }
                else
                {
                    this.Add(value);
                }
            }
        }

        object IDictionary.this[object key]
        {
            get { return this[key: this.ConvertKey(key)]; }

            set { this[key: this.ConvertKey(key)] = this.ConvertItem(value); }
        }

        /// <inheriteddoc />
        public virtual ICollection<T> Values
        {
            get { return ((IList<T>)this).ToList(); }
        }

        IEnumerable<T> IReadOnlyDictionary<int?, T>.Values
        {
            get { return this.Values; }
        }

        ICollection IDictionary.Values
        {
            get
            {
                var values = this.Values;
                if (values is ICollection)
                {
                    return (ICollection)values;
                }

                return new List<T>(values);
            }
        }

        #endregion Properties (8)

        #region Methods (17)

        /// <inheriteddoc />
        public virtual void Add(int? key, T value)
        {
            if (key.HasValue)
            {
                this.Insert(key.Value, value);
            }
            else
            {
                this.Add(value);
            }
        }

        void IDictionary.Add(object key, object value)
        {
            this.Add(this.ConvertKey(key),
                     this.ConvertItem(value));
        }

        void ICollection<KeyValuePair<int?, T>>.Add(KeyValuePair<int?, T> item)
        {
            this.Add(item.Key, item.Value);
        }

        bool ICollection<KeyValuePair<int?, T>>.Contains(KeyValuePair<int?, T> item)
        {
            T value;
            if (this.TryGetValue(item.Key, out value))
            {
                return EqualityComparer<T>.Default.Equals(item.Value, value);
            }

            return false;
        }

        bool IDictionary.Contains(object key)
        {
            return this.ContainsKey(this.ConvertKey(key));
        }

        /// <inheriteddoc />
        public virtual bool ContainsKey(int? key)
        {
            if (key.HasValue)
            {
                return (key > 0) && (key < this.Count);
            }
            else
            {
                return this.Count > 0;
            }
        }

        /// <summary>
        /// Converts an object to a nullable <see cref="int" />.
        /// </summary>
        /// <param name="obj">The input value.</param>
        /// <returns>The output value.</returns>
        protected virtual int? ConvertKey(object obj)
        {
            return (int?)obj;
        }

        void ICollection<KeyValuePair<int?, T>>.CopyTo(KeyValuePair<int?, T>[] array, int arrayIndex)
        {
            var index = -1;
            using (var e = this.GetEnumerator())
            {
                while (e.MoveNext())
                {
                    ++index;

                    array[arrayIndex + index] = new KeyValuePair<int?, T>(index, e.Current);
                }
            }
        }

        IEnumerator<KeyValuePair<int?, T>> IEnumerable<KeyValuePair<int?, T>>.GetEnumerator()
        {
            return new Enumerator(this);
        }

        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            return new DictionaryEnumerator<int?, T>(new Enumerator(this));
        }

        private int GetRealKey(int? key)
        {
            return key.HasValue ? key.Value
                                : (this.Count - 1);
        }

        /// <inheriteddoc />
        public virtual bool RemoveKey(int? key)
        {
            key = this.GetRealKey(key);

            if (!this.ContainsKey(key.Value))
            {
                return false;
            }

            this.RemoveAt(key.Value);
            return true;
        }

        void IDictionary.Remove(object key)
        {
            this.RemoveKey(this.ConvertKey(key));
        }

        bool IDictionary<int?, T>.Remove(int? key)
        {
            return this.RemoveKey(key);
        }

        /// <summary>
        /// <see cref="ICollection{T}.Remove(T)" />
        /// </summary>
        protected virtual bool Remove(KeyValuePair<int?, T> item)
        {
            T value;
            if (this.TryGetValue(item.Key, out value))
            {
                if (EqualityComparer<T>.Default.Equals(item.Value, value))
                {
                    return this.RemoveKey(item.Key);
                }
            }

            return false;
        }

        bool ICollection<KeyValuePair<int?, T>>.Remove(KeyValuePair<int?, T> item)
        {
            return this.Remove(item);
        }

        /// <inheriteddoc />
        public virtual bool TryGetValue(int? key, out T value)
        {
            key = this.GetRealKey(key);

            if (this.ContainsKey(key.Value))
            {
                value = this[key.Value];
                return true;
            }

            value = default(T);
            return false;
        }

        #endregion Methods (17)
    }
}