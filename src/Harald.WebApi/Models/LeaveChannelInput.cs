using System;
using System.ComponentModel.DataAnnotations;

namespace Harald.WebApi.Models
{
    public class LeaveChannelInput
    {
        [Required]

        public Guid CapabilityId { get; set; }

        [Required]
        public string ChannelId { get; set; }
    }
}
