namespace TestMinApi.Helpers
{
    public class EndpointHelper
    {
        public static void Anonymous(params IEndpointConventionBuilder[] ecb)
        {
            foreach (var c in ecb)
            {
                c.AllowAnonymous();
            }
        }

        public static void Admin(params IEndpointConventionBuilder[] ecb)
        {
            foreach (var c in ecb)
            {
                c.RequireAuthorization("admin");
            }
        }
    }
}
