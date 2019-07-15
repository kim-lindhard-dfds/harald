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

Now, depending on how you run your tests, how this is done may differ. The **key** component in this is that two environment variables gets passed to the process running the tests. These two environment variables being:

- SLACK_API_AUTH_TOKEN


  **value**: The "OAuth Access Token" that you received from following the step above goes here.

- SLACK_TESTING_USER_EMAIL


  **value**: This needs to contain an Email address from a user that resides in the created Slack Workspace. Assuming that you've followed the steps from above, this would be the email address you used to register the Workspace/your user. 



#### Running from bash with dotnet CLI

```bash
SLACK_API_AUTH_TOKEN=TokenGoesHere \
SLACK_TESTING_USER_EMAIL=YourEmailGoesHere \
dotnet test
```

#### Running from Powershell with dotnet CLI

```powershell
$env:SLACK_API_AUTH_TOKEN='TokenGoesHere'; $env:SLACK_TESTING_USER_EMAIL='YourEmailGoesHere'; dotnet test; $env:SLACK_API_AUTH_TOKEN=''; $env:SLACK_TESTING_USER_EMAIL='';
```

#### Using Rider

1. File -> Settings -> "Build, Execution, Deployment" -> "Unit Testing" -> "Test Runner"
2. There should be a section called "Environment Variables". Set them as described above.

#### Using other IDEs/editors

I've taken a quick look at VSCODE and VS, but have so far been unable to quickly find an option for providing environment variables to integration tests. Nonetheless the first two provided examples should be a viable alternative until a better solution has been found for VSCODE and VS.

If you've followed all steps so far, there should be at least one test that has failed, namely *CreateUserGroup_And_Add_User_Given_valid_input_Should_create_group_with_user*, due to it requiring a *paid_teams_only* scope, which isn't found in a newly created Workspace that doesn't have a paid plan.


#### Order to run tests in:

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
	
	It also requires a "*paid_teams_only*" scopes, which sadly isn't as easy as the previous scopes. Documentation on said scope is rather light, but from what I've gathered it requires a paid plan for the Slack Workspace.

The scopes mentioned are in regards to the Slack API and how it grants permissions. You can read more here: [https://api.slack.com/scopes](https://api.slack.com/scopes)

