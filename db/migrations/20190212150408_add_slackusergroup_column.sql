-- 2019-02-12 15:04:08 : add_slackusergroup_column

ALTER TABLE public."Capability"
ADD "SlackUserGroup" varchar(255) NULL;