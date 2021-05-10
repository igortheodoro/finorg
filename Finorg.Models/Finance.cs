using System;

namespace Finorg.Models
{
    public class Finance
    {
        public Guid Id { get; private set; } = Guid.NewGuid();
        public long ChatId { get; set; }
        public decimal Transaction { get; set; }
        public DateTime RegisterDate { get; set; }  
    }
}   
