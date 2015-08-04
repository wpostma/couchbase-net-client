using Couchbase.IO.Operations;
using NUnit.Framework;

using Newtonsoft.Json;
using System.Collections.Generic;

namespace Couchbase.Tests.IO.Operations
{
    [TestFixture]
    public class GetOperationTests
    {
        private Cluster _cluster;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            _cluster = new Cluster("couchbaseClients/couchbase");

            using (var bucket = _cluster.OpenBucket())
            {
                bucket.Upsert(TestKeys.KeyWithInt32Value.Key, TestKeys.KeyWithInt32Value.Value);
                bucket.Upsert(TestKeys.KeyWithStringValue.Key, TestKeys.KeyWithStringValue.Value);

                // ISO-8601 and Microsoft JSON Date format:
                bucket.Upsert(TestKeys.KeyWithIsoDateValue.Key, TestKeys.KeyWithIsoDateValue.Value);
                bucket.Upsert(TestKeys.KeyWithIsoUtcDateValue.Key, TestKeys.KeyWithIsoUtcDateValue.Value);
                bucket.Upsert(TestKeys.KeyWithMsDateValue.Key, TestKeys.KeyWithMsDateValue.Value);

                // De-Serialize const JSON string into a Dictionary<string, dynamic> and write via bucket.Upsert(Couchbase.Document<dynamic>)
                var ADict = JsonConvert.DeserializeObject<Dictionary<string, dynamic>>(TestKeys.KeyWithJsonDocValueStr);
                var couchDoc = new Couchbase.Document<dynamic>()
                {
                    Id = TestKeys.KeyWithJsonDocKey,
                    Content = ADict
                };
                bucket.Upsert(couchDoc);


            }
        }

        [Test]
        public void Test_Get_String()
        {
            using (var bucket = _cluster.OpenBucket("default"))
            {
                var response = bucket.Get<string>(TestKeys.KeyWithStringValue.Key);
                Assert.IsTrue(response.Success);
                Assert.AreEqual(TestKeys.KeyWithStringValue.Value, response.Value);
            }
        }

        [Test]
        public void Test_Get_Int32()
        {
            using (var bucket = _cluster.OpenBucket("default"))
            {
                var response = bucket.Get<int>(TestKeys.KeyWithInt32Value.Key);
                Assert.IsTrue(response.Success);
                Assert.AreEqual(TestKeys.KeyWithInt32Value.Value, response.Value);
            }
        }

        [Test]
        public void Test_Get_Date()
        {
            using (var bucket = _cluster.OpenBucket("default"))
            {
                //Pass!
                var response = bucket.Get<string>(TestKeys.KeyWithIsoUtcDateValue.Key);
                Assert.IsTrue(response.Success);
                Assert.AreEqual(TestKeys.KeyWithIsoUtcDateValue.Value, response.Value);


                // see: https://forums.couchbase.com/t/surprising-transformations-on-iso-8601-values-unique-to-net-sdk/4678/8
                // It's probably NOT a good idea to expect a string round-trip identity check (stringvalue1==stringvalue2) to pass, but the values should be equivalent
                // when converted to System.DateTime. JSON datetimes are a many-headed beast, and so we should only assert that which we expect to really be done.
                var response3 = bucket.Get<dynamic>("KeyWithJsonDocValue"); //  "RAMSOFT.AUDIT.STUDYACCESS.00485ff354cd491a9044aa6143608fa5" ); // ?  
                Newtonsoft.Json.Linq.JObject Container = response3.Value;

                JsonSerializerSettings settings = new JsonSerializerSettings
                {
                    DateFormatHandling = DateFormatHandling.IsoDateFormat,
                    DateTimeZoneHandling = DateTimeZoneHandling.Utc
                };
                System.DateTime AValue = (System.DateTime )Container[TestKeys.IsoDateKeyName]; // Convert JToken to System.DateTime, in a valid JSON style date format (including ISO-8601, maybe others?)
                System.DateTime ARefValue = System.DateTime.Parse(TestKeys.IsoDateValueSample, null, System.Globalization.DateTimeStyles.RoundtripKind);  // Convert constant string in ISO-8601 format, to System.DateTime.
                Assert.AreEqual( AValue, ARefValue);

                // This test would fail as well if we used string check (string1==string2)
                //When you write an ISO-8601 value that does not END in an explicit timezone (like Z), it could automatically assume that it is UTC and add the Z.
                var response2 = bucket.Get<string>(TestKeys.KeyWithIsoDateValue.Key);
                Assert.IsTrue(response2.Success);
                System.DateTime AValue2 = System.DateTime.Parse(response2.Value, null, System.Globalization.DateTimeStyles.RoundtripKind);  // Convert string in response.Value, assumed to be in ISO-8601 format, to System.DateTime.
                System.DateTime ARefValue2 = System.DateTime.Parse(TestKeys.KeyWithIsoDateValue.Value, null, System.Globalization.DateTimeStyles.RoundtripKind);  // Convert constant string in ISO-8601 format, to System.DateTime.
                Assert.AreEqual(AValue2, ARefValue2); // It should not surprise anyone who works a while with DateTime values in JSON that some effort is required to compare Values.




            }
        }



        [Test]
        public void When_Get_Follows_Set_Operation_Is_Correct()
        {
            const string key = "getsetkey";
            const string value = "the value";
            using (var bucket = _cluster.OpenBucket("default"))
            {
                var setResponse = bucket.Upsert(key, value);
                Assert.IsTrue(setResponse.Success);

                var getResponse = bucket.Get<string>(key);
                Assert.IsTrue(getResponse.Success);
                Assert.AreEqual(value, getResponse.Value);
            }
        }

        [Test]
        public void When_Key_Not_Found_Success_Is_False()
        {
            const string keyThatDoesntExist = "keyThatDoesntExist";
            using (var bucket = _cluster.OpenBucket("default"))
            {
                var getResponse = bucket.Get<string>(keyThatDoesntExist);
                Assert.IsFalse(getResponse.Success);
                Assert.AreEqual("Not found", getResponse.Message);
            }
        }

        [TestFixtureTearDown]
        public void TestFixtureTearDown()
        {
            using (var bucket = _cluster.OpenBucket())
            {
                bucket.Remove(TestKeys.KeyWithInt32Value.Key);
            }
            _cluster.Dispose();
        }
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