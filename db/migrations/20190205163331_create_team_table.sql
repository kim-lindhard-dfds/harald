-- 2019-02-05 16:33:31 : create team table

CREATE TABLE public."Team" (
    "Id" uuid NOT NULL,
    "Name" varchar(255) NOT NULL,
    "SlackChannel" varchar(255) NOT NULL,
    CONSTRAINT team_pk PRIMARY KEY ("Id")
);
