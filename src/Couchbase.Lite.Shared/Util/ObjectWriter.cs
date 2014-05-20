//
// ObjectWriter.cs
//
// Author:
//     Zachary Gramana  <zack@xamarin.com>
//
// Copyright (c) 2014 Xamarin Inc
// Copyright (c) 2014 .NET Foundation
//
// Permission is hereby granted, free of charge, to any person obtaining
// a copy of this software and associated documentation files (the
// "Software"), to deal in the Software without restriction, including
// without limitation the rights to use, copy, modify, merge, publish,
// distribute, sublicense, and/or sell copies of the Software, and to
// permit persons to whom the Software is furnished to do so, subject to
// the following conditions:
// 
// The above copyright notice and this permission notice shall be
// included in all copies or substantial portions of the Software.
// 
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND,
// EXPRESS OR IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF
// MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE AND
// NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE
// LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION
// OF CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION
// WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
//
//
// Copyright (c) 2014 Couchbase, Inc. All rights reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file
// except in compliance with the License. You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software distributed under the
// License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND,
// either express or implied. See the License for the specific language governing permissions
// and limitations under the License.
//

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Couchbase.Lite 
{

    public class ObjectWriter 
    {

        readonly Boolean prettyPrintJson;

        public ObjectWriter() : this(false) { }

        public ObjectWriter(Boolean prettyPrintJson)
        {
            this.prettyPrintJson = prettyPrintJson;
        }

        public ObjectWriter WriterWithDefaultPrettyPrinter()
        {
            return new ObjectWriter(true); // Currently doesn't do anything, but could use something like http://www.limilabs.com/blog/json-net-formatter in the future.
        }

        public IEnumerable<Byte> WriteValueAsBytes<T> (T item)
        {
            var json = WriteValueAsString<T>(item);
            return Encoding.UTF8.GetBytes(json);
        }

        public string WriteValueAsString<T> (T item)
        {
            return JsonConvert.SerializeObject(item, prettyPrintJson ? Formatting.Indented : Formatting.None);
        }

        public T ReadValue<T> (String json)
        {
            var item = JsonConvert.DeserializeObject<T>(json);
            return item;
        }

        public T ReadValue<T> (IEnumerable<Byte> json)
        {
            using (var jsonStream = new MemoryStream(json.ToArray())) 
            using (var jsonReader = new JsonTextReader(new StreamReader(jsonStream))) 
            {
                var serializer = new JsonSerializer();
                var item = serializer.Deserialize<T>(jsonReader);
                return item;
            }
        }

        public T ReadValue<T> (Stream jsonStream)
        {
            using (var jsonReader = new JsonTextReader(new StreamReader(jsonStream))) 
            {
                var serializer = new JsonSerializer();
                var item = serializer.Deserialize<T>(jsonReader);
                return item;
            }
        }
    }
}
