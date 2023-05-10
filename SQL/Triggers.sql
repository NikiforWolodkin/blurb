CREATE TABLE "UserUpdateLogs" (
    "Id" integer GENERATED ALWAYS AS IDENTITY,
    "UserId" integer NOT NULL,
    "UpdatedColumns" text[] NOT NULL,
    "OldValues" text[] NOT NULL,
    "NewValues" text[] NOT NULL,
    "UpdateTimestamp" timestamp with time zone NOT NULL,
    CONSTRAINT "PK_UserUpdateLogs" PRIMARY KEY ("Id")
);

CREATE OR REPLACE FUNCTION log_user_update()
RETURNS TRIGGER AS $$
BEGIN
    IF OLD."Email" = NEW."Email" AND OLD."RegistrationDate" = NEW."RegistrationDate" AND OLD."PasswordHash" = NEW."PasswordHash"
    THEN
        INSERT INTO "UserUpdateLogs" ("UserId", "UpdatedColumns", "OldValues", "NewValues", "UpdateTimestamp")
        VALUES (
            OLD."Id",
            ARRAY(SELECT column_name FROM information_schema.columns 
				  WHERE table_name = 'Users' AND column_name NOT IN ('Id', 'Email', 'RegistrationDate', 'PasswordHash')),
            ARRAY[OLD."Username", OLD."Avatar", OLD."ProfileColor", OLD."Status", OLD."Role"],
            ARRAY[NEW."Username", NEW."Avatar", NEW."ProfileColor", NEW."Status", NEW."Role"],
            NOW()
        );
    END IF;
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER user_update_trigger
AFTER UPDATE ON "Users"
FOR EACH ROW
EXECUTE FUNCTION log_user_update();

CREATE TABLE "CommentUpdateLogs" (
    "Id" integer GENERATED ALWAYS AS IDENTITY,
    "CommentId" integer NOT NULL,
    "OldText" text NOT NULL,
    "NewText" text NOT NULL,
    "UpdateTimestamp" timestamp with time zone NOT NULL,
    CONSTRAINT "PK_CommentUpdateLogs" PRIMARY KEY ("Id")
);

CREATE OR REPLACE FUNCTION log_comment_update()
RETURNS TRIGGER AS $$
BEGIN
    IF OLD."Text" IS DISTINCT FROM NEW."Text"
    THEN
        INSERT INTO "CommentUpdateLogs" ("CommentId", "OldText", "NewText", "UpdateTimestamp")
        VALUES (
            OLD."Id",
            OLD."Text",
            NEW."Text",
            NOW()
        );
    END IF;
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER comment_update_trigger
AFTER UPDATE ON "Comments"
FOR EACH ROW
EXECUTE FUNCTION log_comment_update();

CREATE TABLE "PostUpdateLogs" (
    "Id" integer GENERATED ALWAYS AS IDENTITY,
    "PostId" integer NOT NULL,
    "OldText" text NOT NULL,
    "NewText" text NOT NULL,
    "UpdateTimestamp" timestamp with time zone NOT NULL,
    CONSTRAINT "PK_PostUpdateLogs" PRIMARY KEY ("Id")
);

CREATE OR REPLACE FUNCTION log_post_update()
RETURNS TRIGGER AS $$
BEGIN
    IF OLD."Text" IS DISTINCT FROM NEW."Text"
    THEN
        INSERT INTO "PostUpdateLogs" ("PostId", "OldText", "NewText", "UpdateTimestamp")
        VALUES (
            OLD."Id",
            OLD."Text",
            NEW."Text",
            NOW()
        );
    END IF;
    RETURN NEW;
END;
$$ LANGUAGE plpgsql;

CREATE TRIGGER post_update_trigger
AFTER UPDATE ON "Posts"
FOR EACH ROW
EXECUTE FUNCTION log_post_update();