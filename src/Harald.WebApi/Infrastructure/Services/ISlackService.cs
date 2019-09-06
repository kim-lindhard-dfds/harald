using System;
using System.Linq;
using System.Threading.Tasks;
using Harald.WebApi.Domain;
using Harald.WebApi.Infrastructure.Facades.Slack;
using Microsoft.Extensions.Logging;

namespace Harald.WebApi.Infrastructure.Services
{
    public interface ISlackService
    {
        Task<UserGroup> EnsureUserGroupExists(string capabilityName);
    }
    
    
    public class SlackService : ISlackService
    {
        private readonly ISlackFacade _slackFacade;
        private readonly ILogger<SlackService> _logger;

        public SlackService(ISlackFacade slackFacade, ILogger<SlackService> logger)
        {
            _slackFacade = slackFacade;
            _logger = logger;
        }
        public async Task<UserGroup> EnsureUserGroupExists(string capabilityName)
        {
            UserGroup userGroup = null;
            try
            {
                var existingUserGroups = await _slackFacade.GetUserGroups();
                var defaultHandle = UserGroupHandle.Create(capabilityName);

                var userGroupFromSlack = existingUserGroups?.SingleOrDefault(grp => grp.Handle == defaultHandle);

                if (userGroupFromSlack != null)
                    return userGroupFromSlack;
            
                
                var response = await _slackFacade.CreateUserGroup(
                    name: $"{capabilityName} user group",
                    handle: capabilityName,
                    description: $"User group for capability {capabilityName}.");

                userGroup = response.UserGroup;
            }
            catch (SlackFacade.SlackFacadeException ex)
            {
                _logger.LogError($"Issue with Slack API: {ex} : {ex.Message}");
            }

            return userGroup;
        }
    }
}