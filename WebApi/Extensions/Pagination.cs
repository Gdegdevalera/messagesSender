using System;

namespace WebApi.Extensions
{
    internal class Pagination
    {
        public int Page { get; set; }

        public int Capacity { get; set; }

        public int Skip
        {
            get { return (Page - 1) * Capacity; }
        }

        public int GetTotalPages(int totalResult)
        {
            return (int)Math.Ceiling((double)totalResult / Capacity);
        }
    }
}
