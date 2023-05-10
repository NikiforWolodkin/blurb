CREATE TABLE "Users" (
    "Id" integer GENERATED ALWAYS AS IDENTITY,
    "Email" text UNIQUE NOT NULL,
    "PasswordHash" text NOT NULL,
    "Username" text NOT NULL,
    "Avatar" text NOT NULL,
    "ProfileColor" text NOT NULL,
    "RegistrationDate" timestamp with time zone NOT NULL,
    "Status" text NOT NULL,
    "Role" text NOT NULL,
    CONSTRAINT "PK_Users" PRIMARY KEY ("Id")
);

CREATE TABLE "Posts" (
	"Id" integer GENERATED ALWAYS AS IDENTITY,
	"Text" text NOT NULL,
	"CreationTime" timestamp with time zone NOT NULL,
	"UserId" integer NULL,
	CONSTRAINT "PK_Posts" PRIMARY KEY ("Id"),
	CONSTRAINT "FK_Posts_Users_UserId" 
		FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE
);

CREATE TABLE "Subscriptions" (
    "SubscriberId" integer NOT NULL,
    "PublisherId" integer NOT NULL,
    CONSTRAINT "PK_Subscriptions" PRIMARY KEY ("SubscriberId", "PublisherId"),
    CONSTRAINT "FK_Subscriptions_Users_PublisherId" 
		FOREIGN KEY ("PublisherId") REFERENCES "Users" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_Subscriptions_Users_SubscriberId" 
		FOREIGN KEY ("SubscriberId") REFERENCES "Users" ("Id") ON DELETE CASCADE
);

CREATE TABLE "Comments" (
    "Id" integer GENERATED ALWAYS AS IDENTITY,
    "PostId" integer NOT NULL,
    "UserId" integer NULL,
    "Text" text NOT NULL,
    "CreationTime" timestamp with time zone NOT NULL,
    CONSTRAINT "PK_Comments" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Comments_Posts_PostId" 
		FOREIGN KEY ("PostId") REFERENCES "Posts" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_Comments_Users_UserId" 
		FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE
);

CREATE TABLE "Likes" (
    "UserId" integer NOT NULL,
    "PostId" integer NOT NULL,
    CONSTRAINT "PK_Likes" PRIMARY KEY ("UserId", "PostId"),
    CONSTRAINT "FK_Likes_Posts_PostId" 
		FOREIGN KEY ("PostId") REFERENCES "Posts" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_Likes_Users_UserId" 
		FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE
);

CREATE TABLE "Reports" (
    "UserId" integer NOT NULL,
    "PostId" integer NOT NULL,
    CONSTRAINT "PK_Reports" PRIMARY KEY ("UserId", "PostId"),
    CONSTRAINT "FK_Reports_Posts_PostId" 
		FOREIGN KEY ("PostId") REFERENCES "Posts" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_Reports_Users_UserId" 
		FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE
);

CREATE TABLE "Shares" (
    "UserId" integer NOT NULL,
    "PostId" integer NOT NULL,
    CONSTRAINT "PK_Shares" PRIMARY KEY ("UserId", "PostId"),
    CONSTRAINT "FK_Shares_Posts_PostId" 
		FOREIGN KEY ("PostId") REFERENCES "Posts" ("Id") ON DELETE CASCADE,
    CONSTRAINT "FK_Shares_Users_UserId" 
		FOREIGN KEY ("UserId") REFERENCES "Users" ("Id") ON DELETE CASCADE
);

CREATE TABLE "Tags" (
    "Id" integer GENERATED ALWAYS AS IDENTITY,
    "Text" text NOT NULL,
    "PostId" integer NULL,
    CONSTRAINT "PK_Tags" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_Tags_Posts_PostId" 
		FOREIGN KEY ("PostId") REFERENCES "Posts" ("Id") ON DELETE CASCADE
);

CREATE INDEX "IX_Users_RegistrationDate" ON "Users" ("RegistrationDate");

CREATE INDEX "IX_Posts_CreationTime" ON "Posts" ("CreationTime");
CREATE INDEX "IX_Posts_UserId" ON "Posts" ("UserId");

CREATE INDEX "IX_Comments_CreationTime" ON "Comments" ("CreationTime");
CREATE INDEX "IX_Comments_UserId" ON "Comments" ("UserId");
CREATE INDEX "IX_Comments_PostId" ON "Comments" ("PostId");

CREATE INDEX "IX_Likes_UserId" ON "Likes" ("UserId");
CREATE INDEX "IX_Likes_PostId" ON "Likes" ("PostId");

CREATE INDEX "IX_Reports_UserId" ON "Reports" ("UserId");
CREATE INDEX "IX_Reports_PostId" ON "Reports" ("PostId");

CREATE INDEX "IX_Shares_UserId" ON "Shares" ("UserId");
CREATE INDEX "IX_Shares_PostId" ON "Shares" ("PostId");

CREATE INDEX "IX_Subscriptions_SubscriberId" ON "Subscriptions" ("SubscriberId");
CREATE INDEX "IX_Subscriptions_PublisherId" ON "Subscriptions" ("PublisherId");

CREATE INDEX "IX_Tags_PostId" ON "Tags" ("PostId");
CREATE INDEX "IX_Tags_Text" ON "Tags" ("Text");