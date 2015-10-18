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
using System.ComponentModel;

namespace MarcelJoachimKloubert.Collections
{
    internal class CollectionWrapperEventHandler : INotifyPropertyChanged
    {
        #region Fields (2)

        private readonly object _BASE_COLLECTION;
        private readonly object _PARENT_COLLECTION;

        #endregion Fields (2)

        #region Constructors (2)

        internal CollectionWrapperEventHandler(object parentCollection, object baseCollection)
        {
            this._PARENT_COLLECTION = parentCollection;
            this._BASE_COLLECTION = baseCollection;

            if (this._BASE_COLLECTION is INotifyPropertyChanged)
            {
                ((INotifyPropertyChanged)this._BASE_COLLECTION).PropertyChanged += this.CollectionEventHandler_PropertyChanged;
            }
        }

        ~CollectionWrapperEventHandler()
        {
            try
            {
                try
                {
                    if (this._BASE_COLLECTION is INotifyPropertyChanged)
                    {
                        ((INotifyPropertyChanged)this._BASE_COLLECTION).PropertyChanged -= this.CollectionEventHandler_PropertyChanged;
                    }
                }
                finally
                {
                    this.Dispose(this._BASE_COLLECTION as IDisposable, false);
                }
            }
            catch
            {
                // ignore
            }
        }

        #endregion Constructors (2)

        #region Events (1)

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion Events (1)

        #region Methods (3)

        internal void Dispose(bool disposing)
        {
            this.Dispose(this._BASE_COLLECTION as IDisposable, disposing);
        }

        private void Dispose(IDisposable baseCollection, bool disposing)
        {
            if (disposing)
            {
                if (baseCollection != null)
                {
                    baseCollection.Dispose();
                }

                GC.SuppressFinalize(this._PARENT_COLLECTION);
            }
        }

        private void CollectionEventHandler_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            var handler = this.PropertyChanged;
            if (handler != null)
            {
                handler(this._PARENT_COLLECTION, e);
            }
        }

        #endregion Methods (3)
    }
}