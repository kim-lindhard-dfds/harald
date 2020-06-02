-- 2020-05-26 11:08:23 : update CapabilitySlackChannelId with best effort
UPDATE public."CapabilityMember"
SET "CapabilitySlackChannelId" = "SlackChannelId"
FROM public."Capability"
WHERE "CapabilityId" = public."Capability"."Id";