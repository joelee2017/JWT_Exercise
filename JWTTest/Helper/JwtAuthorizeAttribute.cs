using Jose;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;
using System.Web.Http.Controllers;

namespace JWTTest.Helper
{
    public class JwtAuthorizeAttribute : AuthorizeAttribute
    {
        static string secretKey = "BAD6809DCB5AFBAAA9DC8CABB4F4AB3D7DCA2438A721B3686B6B0D3288239D00";
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            try
            {
                if (actionContext.Request.Headers.Authorization == null ||
              actionContext.Request.Headers.Authorization.Scheme != "Bearer")
                {
                    var response = actionContext.Request.CreateErrorResponse(HttpStatusCode.Unauthorized, "驗證有誤！");
                    actionContext.Response = response;
                    return base.IsAuthorized(actionContext);
                }
                var token = actionContext.Request.Headers.Authorization.Parameter;
                var payload = JWT.Decode(token, Encoding.UTF8.GetBytes(secretKey), JwsAlgorithm.HS256);
                var payloadObject = JsonConvert.DeserializeObject<PayloadModel>(payload);
                var expireUnixTime = payloadObject.exp;
                var nowUnixTime = DateTimeOffset.Now.ToUnixTimeSeconds();
                return nowUnixTime < expireUnixTime;
            }
            catch (Exception ex)
            {
                throw new ArgumentException();
            }

        }

        public class PayloadModel
        {
            public string iss { get; set; }
            public long iat { get; set; }
            public long exp { get; set; }
            public string aud { get; set; }
            public string sub { get; set; }
            public string name { get; set; }
            public string hash { get; set; }
        }
    }
}