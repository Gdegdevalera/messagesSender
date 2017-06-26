using SharedModels;
using System;
using System.Collections.Specialized;

namespace WebApi.Extensions
{
    internal static class PaginationExtension
    {
        public static Pagination GetPagination(this NameValueCollection headers)
        {
            var page = headers[Constants.PaginationPageNumberHeaderName];
            var capacity = headers[Constants.PaginationCapacityHeaderName];

            Func<string, int, int> tryParse = new Func<string, int, int>((source, defaultValue) =>
            {
                int result;
                if (!int.TryParse(source, out result))
                {
                    result = defaultValue;
                }
                return result;
            });

            return new Pagination
            {
                Page = tryParse(page, 1),
                Capacity = tryParse(capacity, 100)
            };
        }

        public static void SetPagination(this NameValueCollection headers, Pagination pagination, int total)
        {
            headers[Constants.PaginationPageNumberHeaderName] = pagination.Page.ToString();
            headers[Constants.PaginationCapacityHeaderName] = pagination.Capacity.ToString();
            headers[Constants.PaginationTotalPagesHeaderName] = pagination.GetTotalPages(total).ToString();
        }
    }
}