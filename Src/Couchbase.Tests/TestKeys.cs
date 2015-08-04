using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace Couchbase.Tests
{
    public static class TestKeys
    {
        public const string IsoDateValueSample = "2014-11-13T18:15:23";
        public const string IsoDateKeyName = "isodatetime";

        public const string KeyWithJsonDocKey = "KeyWithJsonDocValue";
        public const string KeyWithJsonDocValueStr = @"{
  ""uniqueidentifier"": ""1.2.124.113532.19.29269.37783.20120128.72021.7570795"",
  ""isodatetime"": ""2014-11-13T18:15:23"",
  ""shortstring"": ""ADMIN"",
  ""mediumstring"": ""SOME MULTI WORD STRING"",
  ""statusinteger"": 123456789,
  ""digitsinquotes"": ""000408703"",
  ""md5hash"": ""6D417FA9C6A430CD3ED34B69C57361DC""
}";
        static TestKeys()
        {
            KeyWithStringValue = new KeyValuePair<string, string>("Key_With_String_Value", "string value.");
            KeyWithInt32Value = new KeyValuePair<string, int?>("Key_With_Int32_Value", 5242010);
            KeyWithIsoDateValue = new KeyValuePair<string, string>("KeyWithIsoDateValue", "2014-11-28T11:01:18" );
            KeyWithIsoUtcDateValue = new KeyValuePair<string, string>("KeyWithIsoDateValue", "2014-11-28T11:01:18Z" );
            KeyWithMsDateValue = new KeyValuePair<string, string>("KeyWithMsDateValue", "2014-11-28 11:01:18" );

            //KeyWithJsonDocValue = new KeyValuePair<string,string>("KeyWithJsonDocValue", KeyWithJsonDocValueStr );
            //KeyWithJsonDocValue = new KeyValuePair<string, dynamic>("KeyWithJsonDocValue", JsonConvert.DeserializeObject(KeyWithJsonDocValueStr) );

        }
        public static KeyValuePair<string, string> KeyWithStringValue;
        public static KeyValuePair<string, int?> KeyWithInt32Value;

        // ISO-8601 in JSON datetime
        public static KeyValuePair<string, string> KeyWithIsoDateValue;
        public static KeyValuePair<string, string> KeyWithIsoUtcDateValue;
        public static KeyValuePair<string, string> KeyWithMsDateValue;
    
        // Complex JSON document
       //public static KeyValuePair<string, dynamic> KeyWithJsonDocValue;

    }
}

#region [ License information          ]

/* ************************************************************
 *
 *    @author Couchbase <info@couchbase.com>
 *    @copyright 2014 Couchbase, Inc.
 *
 *    Licensed under the Apache License, Version 2.0 (the "License");
 *    you may not use this file except in compliance with the License.
 *    You may obtain a copy of the License at
 *
 *        http://www.apache.org/licenses/LICENSE-2.0
 *
 *    Unless required by applicable law or agreed to in writing, software
 *    distributed under the License is distributed on an "AS IS" BASIS,
 *    WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 *    See the License for the specific language governing permissions and
 *    limitations under the License.
 *
 * ************************************************************/

#endregion