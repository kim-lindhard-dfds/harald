-- 2020-05-26 11:08:23 : remove-colum
ALTER TABLE "CapabilityMember" 
DROP CONSTRAINT "FK_CapabilityMember_CapabilityId_CapabilitySlackChannelId";
ALTER TABLE "CapabilityMember"
DROP COLUMN "CapabilitySlackChannelId";
