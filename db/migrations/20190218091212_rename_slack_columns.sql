-- 2019-02-18 09:12:12 : rename_slack_columns

ALTER TABLE public."Capability"
RENAME COLUMN "SlackChannel" TO "SlackChannelId";

ALTER TABLE public."Capability"
RENAME COLUMN "SlackUserGroup" TO "SlackUserGroupId";