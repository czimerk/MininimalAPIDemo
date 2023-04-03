using Newtonsoft.Json;
using System.Reflection;
using System.Web;
using System.Globalization;
using Microsoft.AspNetCore.Mvc;

namespace TestMinApi.Dto
{
    public class FilterDto
    {
        public string? Name { get; set; }

        public double? PriceGreaterThan { get; set; }

        public double? PriceLessThan { get; set; }

        //public static bool TryParse(string s, out FilterDto dto)
        //{
        //    dto = null;
        //    try
        //    {
        //        var dict = HttpUtility.ParseQueryString(s);
        //        string json = JsonConvert.SerializeObject(dict.Cast<string>().ToDictionary(k => k, v => dict[v]));
        //        dto = JsonConvert.DeserializeObject<FilterDto>(json);
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }

        //    return true;
        //}

        //public static ValueTask<FilterDto?> BindAsync(HttpContext context)
        //{
        //    double priceGt;
        //    double priceLt;
        //    FilterDto? result = null;
        //    try
        //    {
        //        result = new FilterDto()
        //        {
        //            Name = context.Request.Query[nameof(Name)],
        //            PriceGreaterThan = double.TryParse(context.Request.Query[nameof(PriceGreaterThan)].ToString(),
        //            NumberStyles.AllowDecimalPoint,  CultureInfo.InvariantCulture, out priceGt) ? priceGt : null,
        //            PriceLessThan = double.TryParse(context.Request.Query[nameof(PriceLessThan)].ToString(),
        //            NumberStyles.AllowDecimalPoint, CultureInfo.InvariantCulture, out priceLt) ? priceLt : null,
        //        };
        //    }
        //    catch (Exception)
        //    {
        //    }

        //    return ValueTask.FromResult(result);
        //}
    }
}
