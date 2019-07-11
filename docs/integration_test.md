# IntegrationTest

This project uses xUnit for testing.



## Setup

In order to run the test, a couple of things will have to be setup first.

- A Slack workspace
- Access to https://api.slack.com/apps
  - An "app" on https://api.slack.com/apps
  - This app must require certain access scopes
- Access to certain ENV VARS from wherever the test is running



### A Slack workspace

The tests needs to have access to a Workspace in order to run. If one doesn't wanna risk their "production" Workspace, it would be a smart idea to create a separate one, used just for this purpose.

To do that, go to [https://slack.com/get-started#create](https://slack.com/get-started#create) and fill in whatever infomation it requires.

### Access to api.slack.com

Sign in with your newly created credentials from above.

From there on, click on the button "Create New App". Assign it a name, whatever you feel like, and ensure its "Development Slack Workspace" is the workspace you created above. Then click on "Create app".



Now, this app will require certain API "scopes" in order to function with the tests. Click on "OAuth & Permissions" in the side navigation menu. In the "Scopes" box, make sure that the following scopes are added:

- channels:write
- channels:read
- groups:read
- im:read
- mpim:read
- chat:write:user
- chat:write:bot
- pins:write
- usergroups:write
- users:read
- users:read.email

When that is done, click on "Install App to Workspace" on the same page. Assuming that completes successfully, you should've been granted a "OAuth Access Token". Make sure to note it down, as it will be used in the next step.

### Running the test

#### How to run:

Run in following order:

1. CreateChannel_Given_valid_input_Should_create_channel
	Requires following scope:
	
   * channels:write
   
   1. InviteToChannel_Given_valid_input_Should_invite_to_channel
        Requires following scope:

       * channels:read
       * groups:read
       * im:read
       * mpim:read
       * channels:write
       * users:read
       * users:read.email
2. SendNotificationToChannel_Given_valid_input_Should_send_notfication_to_channel
        Requires following scope:
   
       * chat:write:user
       * chat:write:bot
3. PinMessageToChannel_Given_valid_input_Should_send_notfication_to_channel
        Requires following scope:
   
       * chat:write:user
    * chat:write:bot
	    * pins:write
	
1. CreateUserGroup_And_Add_User_Given_valid_input_Should_create_group_with_user
	Requires following scope:
	
	* usergroups:write

The scopes mentioned are in regards to the Slack API and how it grants permissions. You can read more here: [https://api.slack.com/scopes](https://api.slack.com/scopes)

