-- 2019-09-30 13:42:36 : change capability pk to id and slack channel id

ALTER TABLE public."Capability" 
DROP CONSTRAINT team_pk;

ALTER TABLE public."Capability"
ADD CONSTRAINT capability_pk PRIMARY KEY ("Id", "SlackChannelId");