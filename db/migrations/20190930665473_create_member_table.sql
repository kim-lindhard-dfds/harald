CREATE TABLE "CapabilityMember" (
"Id" uuid NOT NULL,
"Email" varchar(255) NULL,
"CapabilityId" uuid NULL,
"CapabilitySlackChannelId" varchar(255) NULL,
CONSTRAINT "PK_CapabilityMember" PRIMARY KEY ("Id"),
CONSTRAINT "FK_CapabilityMember_CapabilityId_CapabilitySlackChannelId" FOREIGN KEY ("CapabilityId", "CapabilitySlackChannelId") REFERENCES "Capability" ("Id", "SlackChannelId") ON DELETE RESTRICT
);