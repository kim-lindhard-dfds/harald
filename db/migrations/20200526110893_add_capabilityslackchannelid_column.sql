-- 2020-05-26 11:08:23 : remove-colum
ALTER TABLE "CapabilityMember" 
ADD CONSTRAINT "FK_CapabilityMember_CapabilityId_CapabilitySlackChannelId" FOREIGN KEY ("CapabilityId", "CapabilitySlackChannelId") REFERENCES "Capability" ("Id", "SlackChannelId") ON DELETE RESTRICT;
