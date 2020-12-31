using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.Controllers;
using System.Web.Http.Filters;

namespace testwebapi.App_Start
{
    //public class DecodeActionFilter: ActionFilterAttribute
    //{
    //    public override void OnActionExecuting(HttpActionContext actionContext)
    //    {
    //        //using (var stream = new MemoryStream())
    //        //{
    //        //    var context = (HttpContextBase)actionContext.Request.Properties["MS_HttpContext"];
    //        //    context.Request.InputStream.Seek(0, SeekOrigin.Begin);
    //        //    context.Request.InputStream.CopyTo(stream);
    //        //    string requestBody = Encoding.UTF8.GetString(stream.ToArray());
    //        //}
    //        //base.OnActionExecuting(actionContext);

    //        var test = (actionContext.Request.Content as ObjectContent).Value.ToString();
    //        base.OnActionExecuting(actionContext);
    //    }
    //}

    public class DecodeActionFilter : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request,
                                                      CancellationToken cancellationToken)
        {
            var requestBody = "eyJJbmNsdWRlSW5hY3RpdmUiOmZhbHNlLCJVc2VycyI6IjMzNjgyIiwiUm9sZXMiOiJTUF9DTl9NR1IiLCJMb2NhdGlvblR5cGUiOiJDT1VOVFJZIiwiTG9jYXRpb25Db2RlcyI6IkJFLEJELEJSLENOLEpQLEFVLEtILERLLEZSLE1YLERaLERKLEFELEFULEFGLEtFLEtSLEFSLElOIiwiSXNMYXRlc3QiOnRydWV9";// request.Content.ReadAsStringAsync().Result;
            var oldHeaders = request.Content.Headers;
            byte[] data = Convert.FromBase64String(requestBody);
            string decodedString = Encoding.UTF8.GetString(data);
            request.Content = new StringContent(decodedString);
            ReplaceHeaders(request.Content.Headers, oldHeaders);
            // your logging code here
            return base.SendAsync(request, cancellationToken);
        }
        private void ReplaceHeaders(HttpContentHeaders currentHeaders, HttpContentHeaders oldHeaders)
        {
            currentHeaders.Clear();
            foreach (var item in oldHeaders)
                currentHeaders.Add(item.Key, item.Value);
        }
    }

}


//private static bool TryRetrieveToken(HttpRequestMessage request, out string token)
//{
//    token = null;
//    IEnumerable<string> authzHeaders;
//    if (!request.Headers.TryGetValues("Authorization", out authzHeaders) || authzHeaders.Count() > 1)
//    {
//        return false;
//    }
//    var bearerToken = authzHeaders.ElementAt(0);
//    token = bearerToken.StartsWith("Bearer ") ? bearerToken.Substring(7) : bearerToken;
//    return true;
//}


//protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
//{
//    HttpStatusCode statusCode;
//    string token;

//    var authHeader = request.Headers.Authorization;
//    if (authHeader == null)
//    {
//        // missing authorization header
//        return base.SendAsync(request, cancellationToken);
//    }

//    if (!TryRetrieveToken(request, out token))
//    {
//        statusCode = HttpStatusCode.Unauthorized;
//        return Task<HttpResponseMessage>.Factory.StartNew(() => new HttpResponseMessage(statusCode));
//    }

//    try
//    {
//        JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();
//        TokenValidationParameters validationParameters =
//            new TokenValidationParameters()
//            {
//                AllowedAudience = ConfigurationManager.AppSettings["JwtAllowedAudience"],
//                ValidIssuer = ConfigurationManager.AppSettings["JwtValidIssuer"],
//                SigningToken = new BinarySecretSecurityToken(SymmetricKey)
//            };

//        IPrincipal principal = tokenHandler.ValidateToken(token, validationParameters);
//        Thread.CurrentPrincipal = principal;
//        HttpContext.Current.User = principal;

//        return base.SendAsync(request, cancellationToken);
//    }
//    catch (SecurityTokenValidationException e)
//    {
//        statusCode = HttpStatusCode.Unauthorized;
//    }
//    catch (Exception)
//    {
//        statusCode = HttpStatusCode.InternalServerError;
//    }

//    return Task<HttpResponseMessage>.Factory.StartNew(() => new HttpResponseMessage(statusCode));
//}
