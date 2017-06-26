using CommonModels;
using System;

namespace WebApi.Models
{
    public class MessageEntity
    {
        public long Id { get; set; }

        public EChannel Channel { get; set; }
        
        public string Body { get; set; }

        public DateTime CreatedAt { get; set; }

        public EState State { get; set; }

        public DateTime? ProcessedAt { get; set; }
    }
}