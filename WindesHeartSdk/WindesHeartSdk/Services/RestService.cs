using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Acr;
using Acr.Collections;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using WindesHeartSdk.MiBand;
using WindesHeartSdk.Model;

namespace WindesHeartSdk.Services
{
    public static class RestService
    {
        public static string ApiUri;
        public static string DsuUri;

        private static string _username;
        private static string _password;
        private static OAuthAccessToken _userAccessToken;

        private static readonly DefaultContractResolver ContractResolver = new DefaultContractResolver
        {
            NamingStrategy = new CamelCaseNamingStrategy()
        };

        /// <summary>
        /// Returns logged in user access token
        /// </summary>
        public static OAuthAccessToken GetUserAccessToken()
        {
            return _userAccessToken;
        }

        /// <summary>
        /// Sync local data to Open mHealth server
        /// </summary>
        public static async Task<int> PushDataPoints()
        {
            var lastSyncTime = await GetLastSyncTime();
            var user = await GetActiveUser();

            if (lastSyncTime == null || user == null)
            {
                return -1;
            }

            var dataPoints = await GetDataPointRequests(((DateTime)lastSyncTime).ToUniversalTime(), user);

            var client = new HttpClient();
            var uri = $"{ApiUri}/pushdatapoints";

            try
            {
                var count = 0;
                foreach (var dataPoint in dataPoints)
                {
                    if (dataPoint.Measurements.IsEmpty())
                    {
                        break;
                    }

                    count += dataPoint.Measurements.Count;

                    var json = JsonConvert.SerializeObject(dataPoint, Formatting.Indented, new JsonSerializerSettings
                    {
                        ContractResolver = ContractResolver,
                        DateTimeZoneHandling = DateTimeZoneHandling.Utc
                    });

                    var content = new StringContent(json, Encoding.UTF8, "application/json");
                    var response = await client.PostAsync(uri, content);

                    if (!response.IsSuccessStatusCode)
                    {
                        Console.WriteLine(response.StatusCode);
                        return -1;
                    }
                }

                return count;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            
            return -1;
        }

        /// <summary>
        /// Returns timestamp from last sync time
        /// </summary>
        public static async Task<DateTime?> GetLastSyncTime()
        {
            var user = await GetActiveUser();

            if (user == null)
            {
                return null;
            }

            var client = new HttpClient();
            var uri = $"{ApiUri}/getlastsync";

            var json = JsonConvert.SerializeObject(user, Formatting.Indented, new JsonSerializerSettings
            {
                ContractResolver = ContractResolver,
            });
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            try
            {
                var response = await client.PostAsync(uri, content);

                if (response.IsSuccessStatusCode)
                {
                    try
                    {
                        var timestamp = await response.Content.ReadAsStringAsync();

                        if (timestamp.IsEmpty())
                        {
                            return DateTime.Now.AddHours(-1);
                        }
                        return DateTime.Parse(timestamp);

                    }
                    catch (Exception e)
                    {
                        Debug.WriteLine(e);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            
            return null;
        }

        /// <summary>
        /// Login to Open mHealth authorization server and returns true if successful.
        /// </summary>
        public static async Task<bool> Login(string username, string password)
        {
            var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Basic", "dGVzdENsaWVudDp0ZXN0Q2xpZW50U2VjcmV0");
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var formData = new MultipartFormDataContent
            {
                {new StringContent("password"), "grant_type"},
                {new StringContent(username), "username"},
                {new StringContent(password), "password"}
            };

            try
            {
                var response = await client.PostAsync(DsuUri, formData);

                if (response.IsSuccessStatusCode)
                {
                    var content = await response.Content.ReadAsStringAsync();

                    var htmlAttributes = JsonConvert.DeserializeObject<Dictionary<string, string>>(content);

                    _username = username;
                    _password = password;
                    var token = htmlAttributes["access_token"];
                    var expires = int.Parse(htmlAttributes["expires_in"]);
                    _userAccessToken = new OAuthAccessToken
                    {
                        AccessToken = new Guid(token),
                        ValidUntil = DateTime.Now.AddSeconds(expires)
                    };

                    return true;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            
            return false;
        }

        private static async Task<List<DataPointRequest>> GetDataPointRequests(DateTime dateTime, User user, int nSize = 100)
        {
            var activitySamples = await MiBandDb.Database.GetSamplesSince(dateTime);

            Console.WriteLine("Number of samples {0}", activitySamples.Count);

            var list = new List<DataPointRequest>();

            // Split datapoints in chunks
            for (int i = 0; i < activitySamples.Count; i += nSize)
            {
                var samples = activitySamples.GetRange(i, Math.Min(nSize, activitySamples.Count - i));
                list.Add(new DataPointRequest { User = user, Measurements = samples });
            }

            return list;
        }

        private static async Task<User> GetActiveUser()
        {
            await UpdateOAuthToken();
            if (_userAccessToken != null)
            {
                return new User { Token = _userAccessToken.AccessToken };
            }

            return null;
        }

        private static async Task UpdateOAuthToken()
        {
            if (_userAccessToken == null || _userAccessToken.IsExpired)
            {
                if (!_username.IsEmpty() && !_password.IsEmpty())
                {
                    await Login(_username, _password);
                }
            }
        }
    }
}
