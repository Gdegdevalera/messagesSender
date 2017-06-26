using SharedModels;
using System.Collections.Generic;
    
namespace Client.Models
{
    public class QueueModel
    {
        public IEnumerable<MessageEntity> Messages { get; set; }

        public int Page { get; set; }

        public int Capacity { get; set; }

        public int TotalPages { get; set; }
    }
}