using Client.Models;
using RestSharp;
using SharedModels;
using System.Collections.Generic;
using System.Linq;

namespace Client.Extensions
{
    internal static class PaginationExtension
    {
        public static void SetPagination(this IRestRequest request, int page, int capacity)
        {
            request.AddHeader(Constants.PaginationPageNumberHeaderName, page.ToString());
            request.AddHeader(Constants.PaginationCapacityHeaderName, capacity.ToString());
        }

        public static Pagination GetPagination(this IList<Parameter> headers)
        {
            return new Pagination
            {
                Page = headers.Get(Constants.PaginationPageNumberHeaderName),
                Capacity = headers.Get(Constants.PaginationCapacityHeaderName),
                TotalPages = headers.Get(Constants.PaginationTotalPagesHeaderName)
            };
        }

        private static int Get(this IList<Parameter> parameters, string name)
        {
            return parameters
                .Where(x => x.Name == name)
                .FirstOrDefault()
                .MayBe(x => TryParseInt(x.Value));
        }

        private static int TryParseInt(object value)
        {
            int result;
            int.TryParse(value.ToString(), out result);
            return result;
        }
    }
}