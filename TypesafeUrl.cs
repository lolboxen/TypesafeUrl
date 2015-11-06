using System;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Web.Mvc;
using System.Web.Routing;

namespace com.lolboxen.TypesafeUrl
{
    public class TypesafeUrl
    {
        private readonly RequestContext _requestContext;
        private readonly RouteCollection _routes;

        public TypesafeUrl(RequestContext requestContext, RouteCollection routes)
        {
            _requestContext = requestContext;
            _routes = routes;
        }

        public string Url<T>(Expression<Func<T, ActionResult>> action) where T : Controller
        {
            var callExpression = (MethodCallExpression)action.Body;
            string actionName = callExpression.Method.Name;
            string controllerName = typeof(T).Name.Replace("Controller", "");

            bool isPost = IsPost(callExpression.Method);

            var methodArgs = callExpression.Method.GetParameters();
            var routeValues = new RouteValueDictionary();
            if (!isPost)
            {
                for (int i = 0; i < methodArgs.Length; i++)
                {
                    var param = methodArgs.ElementAt(i);
                    routeValues.Add(param.Name, Expression.Lambda(callExpression.Arguments[i]).Compile().DynamicInvoke().ToString());
                }
            }

            return UrlHelper.GenerateUrl(null, actionName, controllerName, routeValues, _routes, _requestContext, true);
        }

        private bool IsPost(MethodInfo info)
        {
            return info.GetCustomAttribute<HttpPostAttribute>() != null;
        }
    }
}