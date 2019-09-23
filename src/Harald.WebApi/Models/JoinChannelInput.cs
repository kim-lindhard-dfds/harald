using System;
using System.ComponentModel.DataAnnotations;

namespace Harald.WebApi.Models
{
    public class JoinChannelInput
    {
        [Required]

        public Guid CapabilityId { get; set; }

        [Required]
        public string ChannelName { get; set; }
    }
}
