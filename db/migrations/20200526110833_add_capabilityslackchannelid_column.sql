-- 2020-05-26 11:08:23 : remove-colum
ALTER TABLE "CapabilityMember" 
ADD COLUMN "CapabilitySlackChannelId" varchar(255) NULL;