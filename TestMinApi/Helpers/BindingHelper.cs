using FluentValidation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace TestMinApi.Helpers
{
    public static class BindingHelper
    {
        public static ValueTask<T?> BindFromQueryAsync<T>(HttpContext context) where T : class
        {
            T? q = BindAsyncImpl<T>(context);

            return ValueTask.FromResult(q);
        }

        public static ValueTask<T?> BindFromQueryWithValidationAsync<T>(HttpContext context) where T : class
        {
            T? q = BindAsyncImpl<T>(context);

            var validator = context.RequestServices.GetRequiredService<IValidator<T>>();
            var validationResult = validator.Validate(q);
            if (!validationResult.IsValid)
            {
                throw new BadHttpRequestException(JsonConvert.SerializeObject(validationResult));
            }
            return ValueTask.FromResult(q);
        }

        private static T? BindAsyncImpl<T>(HttpContext context) where T : class
        {
            T? q = null;
            try
            {
                var type = typeof(T);
                JObject jObj = new JObject();
                foreach (var prop in type.GetProperties())
                {
                    var v = context.Request.Query[prop.Name].ToString();
                    jObj[prop.Name] = v;
                }

                q = jObj.ToObject<T>();
            }
            catch (Exception)
            {

            }

            return q;
        }
    }
}
