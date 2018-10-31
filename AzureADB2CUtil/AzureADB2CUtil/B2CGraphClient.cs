using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;


namespace AADB2C.GraphService
{
    public class B2CGraphClient
    {
        private AuthenticationContext authContext;
        private ClientCredential credential;
        static private AuthenticationResult AccessToken;

        public readonly string aadInstance = "https://login.microsoftonline.com/";
        public readonly string aadGraphResourceId = "https://graph.microsoft.com/";
        public readonly string aadGraphEndpoint = "https://graph.microsoft.com/beta/";

        public string Tenant { get; }
        public string ClientId { get; }
        public string ClientSecret { get; }

        public B2CGraphClient(string tenant, string clientId, string clientSecret)
        {
            this.Tenant = tenant;
            this.ClientId = clientId;
            this.ClientSecret = clientSecret;

            // The AuthenticationContext is ADAL's primary class, in which you indicate the direcotry to use.
            this.authContext = new AuthenticationContext("https://login.microsoftonline.com/" + this.Tenant);

            // The ClientCredential is where you pass in your client_id and client_secret, which are 
            // provided to Azure AD in order to receive an access_token using the app's identity.
            this.credential = new ClientCredential(this.ClientId, this.ClientSecret);
        }

        /// <summary>
        /// Search Azure AD user by signInNames property
        /// </summary>
        public async Task<string> GetAllUsersAsync()
        {
            return await SendGraphRequest("users", "$top=999",
                            null, HttpMethod.Get);
        }


        /// <summary>
        /// Handle Graph user API, support following HTTP methods: GET, POST and PATCH
        /// </summary>
        private async Task<string> SendGraphRequest(string api, string query, string data, HttpMethod method)
        {
            // Get the access toke to Graph API
            string acceeToken = await AcquireAccessToken();

            // Set the Graph url. Including: Graph-endpoint/tenat/users?api-version&query
            string url = $"{this.aadGraphEndpoint}{api}";

            if (!string.IsNullOrEmpty(query))
            {
                url += "?" + query;
            }

            //Trace.WriteLine($"Graph API call: {url}");
            try
            {
                using (HttpClient http = new HttpClient())
                using (HttpRequestMessage request = new HttpRequestMessage(method, url))
                {
                    // Set the authorization header
                    request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", acceeToken);

                    // For POST and PATCH set the request content 
                    if (!string.IsNullOrEmpty(data))
                    {
                        //Trace.WriteLine($"Graph API data: {data}");
                        request.Content = new StringContent(data, Encoding.UTF8, "application/json");
                    }

                    // Send the request to Graph API endpoint
                    using (HttpResponseMessage response = await http.SendAsync(request))
                    {
                        string error = await response.Content.ReadAsStringAsync();

                        // Check the result for error
                        if (!response.IsSuccessStatusCode)
                        {
                            // Throw server busy error message
                            if (response.StatusCode == (HttpStatusCode)429)
                            {
                                // TBD: Add you error handling here
                            }

                            throw new Exception(error);
                        }

                        // Return the response body, usually in JSON format
                        return await response.Content.ReadAsStringAsync();
                    }
                }
            }
            catch (Exception)
            {
                // TBD: Add you error handling here
                throw;
            }
        }

        private async Task<string> AcquireAccessToken()
        {
            // If the access token is null or about to be invalid, acquire new one
            if (AccessToken == null ||
                (AccessToken.ExpiresOn.UtcDateTime > DateTime.UtcNow.AddMinutes(-10)))
            {
                try
                {
                    AccessToken = await authContext.AcquireTokenAsync(this.aadGraphResourceId, credential);
                }
                catch (Exception ex)
                {
                    // TBD: Add you error handling here
                    throw;
                }
            }

            return AccessToken.AccessToken;
        }
    }
}
