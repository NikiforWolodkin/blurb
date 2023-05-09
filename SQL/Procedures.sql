CREATE OR REPLACE PROCEDURE add_user(email text, password_hash text, username text)
LANGUAGE plpgsql
SECURITY DEFINER
SET search_path = public, pg_temp
AS $$
BEGIN
    INSERT INTO "Users" ("Email", "PasswordHash", "Username", "Avatar", "ProfileColor", "RegistrationDate", "Status", "Role")
    VALUES (email, password_hash, username, 'Basic', 'Red', NOW(), 'ACTIVE', 'USER');
END;
$$;

CREATE OR REPLACE PROCEDURE update_user(id integer, profile_color text DEFAULT NULL, avatar text DEFAULT NULL, username text DEFAULT NULL)
LANGUAGE plpgsql
SECURITY DEFINER
SET search_path = public, pg_temp
AS $$
BEGIN
    UPDATE "Users"
    SET "ProfileColor" = COALESCE(profile_color, "ProfileColor"),
        "Avatar" = COALESCE(avatar, "Avatar"),
        "Username" = COALESCE(username, "Username")
    WHERE "Id" = id;
END;
$$;

CREATE OR REPLACE PROCEDURE delete_user(id integer)
LANGUAGE plpgsql
SECURITY DEFINER
SET search_path = public, pg_temp
AS $$
BEGIN
    DELETE FROM "Users"
    WHERE "Id" = id;
END;
$$;

CREATE OR REPLACE PROCEDURE update_user_status(id integer, status text)
LANGUAGE plpgsql
SECURITY DEFINER
SET search_path = public, pg_temp
AS $$
BEGIN
    UPDATE "Users"
    SET "Status" = status
    WHERE "Id" = id;
END;
$$;

CREATE OR REPLACE PROCEDURE update_user_role(id integer, role text)
LANGUAGE plpgsql
SECURITY DEFINER
SET search_path = public, pg_temp
AS $$
BEGIN
    UPDATE "Users"
    SET "Role" = role
    WHERE "Id" = id;
END;
$$;

CREATE OR REPLACE PROCEDURE update_user_password_hash(id integer, password_hash text)
LANGUAGE plpgsql
SECURITY DEFINER
SET search_path = public, pg_temp
AS $$
BEGIN
    UPDATE "Users"
    SET "PasswordHash" = password_hash
    WHERE "Id" = id;
END;
$$;

CREATE OR REPLACE PROCEDURE update_user_email(id integer, email text)
LANGUAGE plpgsql
SECURITY DEFINER
SET search_path = public, pg_temp
AS $$
BEGIN
    UPDATE "Users"
    SET "Email" = email
    WHERE "Id" = id;
END;
$$;



CREATE OR REPLACE PROCEDURE add_post(userid integer, text text)
LANGUAGE plpgsql
SECURITY DEFINER
SET search_path = public, pg_temp
AS $$
BEGIN
    INSERT INTO "Posts" ("Text", "CreationTime", "UserId")
    VALUES (text, NOW(), userid);
END;
$$;

CREATE OR REPLACE PROCEDURE update_post(id integer, text text)
LANGUAGE plpgsql
SECURITY DEFINER
SET search_path = public, pg_temp
AS $$
BEGIN
    UPDATE "Posts"
    SET "Text" = text
    WHERE "Id" = id;
END;
$$;

CREATE OR REPLACE PROCEDURE delete_post(id integer)
LANGUAGE plpgsql
SECURITY DEFINER
SET search_path = public, pg_temp
AS $$
BEGIN
    DELETE FROM "Posts"
    WHERE "Id" = id;
END;
$$;



CREATE OR REPLACE PROCEDURE add_comment(postid integer, userid integer, text text)
LANGUAGE plpgsql
SECURITY DEFINER
SET search_path = public, pg_temp
AS $$
BEGIN
    INSERT INTO "Comments" ("PostId", "UserId", "Text", "CreationTime")
    VALUES (postid, userid, text, NOW());
END;
$$;

CREATE OR REPLACE PROCEDURE update_comment(id integer, text text)
LANGUAGE plpgsql
SECURITY DEFINER
SET search_path = public, pg_temp
AS $$
BEGIN
    UPDATE "Comments"
    SET "Text" = text
    WHERE "Id" = id;
END;
$$;

CREATE OR REPLACE PROCEDURE delete_comment(id integer)
LANGUAGE plpgsql
SECURITY DEFINER
SET search_path = public, pg_temp
AS $$
BEGIN
    DELETE FROM "Comments"
    WHERE "Id" = id;
END;
$$;

CREATE OR REPLACE PROCEDURE add_tag(postid integer, text text)
LANGUAGE plpgsql
SECURITY DEFINER
SET search_path = public, pg_temp
AS $$
BEGIN
    INSERT INTO "Tags" ("Text", "PostId")
    VALUES (text, postid);
END;
$$;

CREATE OR REPLACE PROCEDURE delete_tag(postid integer, tag text)
LANGUAGE plpgsql
SECURITY DEFINER
SET search_path = public, pg_temp
AS $$
BEGIN
    DELETE FROM "Tags"
    WHERE "Text" = tag AND "PostId" = postid;
END;
$$;

CREATE OR REPLACE PROCEDURE add_tags(postid integer, tags text[])
LANGUAGE plpgsql
SECURITY DEFINER
SET search_path = public, pg_temp
AS $$
BEGIN
    INSERT INTO "Tags" ("Text", "PostId")
    SELECT unnest(tags), postid;
END;
$$;

CREATE OR REPLACE PROCEDURE delete_tags(id integer)
LANGUAGE plpgsql
SECURITY DEFINER
SET search_path = public, pg_temp
AS $$
BEGIN
    DELETE FROM "Tags"
    WHERE "Id" = id;
END;
$$;



CREATE OR REPLACE PROCEDURE add_like(userid integer, postid integer)
LANGUAGE plpgsql
SECURITY DEFINER
SET search_path = public, pg_temp
AS $$
BEGIN
    INSERT INTO "Likes" ("UserId", "PostId")
    VALUES (userid, postid);
END;
$$;

CREATE OR REPLACE PROCEDURE add_likes(userids integer[], postid integer)
LANGUAGE plpgsql
SECURITY DEFINER
SET search_path = public, pg_temp
AS $$
BEGIN
    INSERT INTO "Likes" ("UserId", "PostId")
    SELECT unnest(userids), postid;
END;
$$;

CREATE OR REPLACE PROCEDURE delete_like(userid integer, postid integer)
LANGUAGE plpgsql
SECURITY DEFINER
SET search_path = public, pg_temp
AS $$
BEGIN
    DELETE FROM "Likes"
    WHERE "UserId" = userid AND "PostId" = postid;
END;
$$;

CREATE OR REPLACE PROCEDURE delete_likes(postid integer)
LANGUAGE plpgsql
SECURITY DEFINER
SET search_path = public, pg_temp
AS $$
BEGIN
    DELETE FROM "Likes"
    WHERE "PostId" = postid;
END;
$$;



CREATE OR REPLACE PROCEDURE add_share(userid integer, postid integer)
LANGUAGE plpgsql
SECURITY DEFINER
SET search_path = public, pg_temp
AS $$
BEGIN
    INSERT INTO "Shares" ("UserId", "PostId")
    VALUES (userid, postid);
END;
$$;

CREATE OR REPLACE PROCEDURE add_shares(userids integer[], postid integer)
LANGUAGE plpgsql
SECURITY DEFINER
SET search_path = public, pg_temp
AS $$
BEGIN
    INSERT INTO "Shares" ("UserId", "PostId")
    SELECT unnest(userids), postid;
END;
$$;

CREATE OR REPLACE PROCEDURE delete_share(userid integer, postid integer)
LANGUAGE plpgsql
SECURITY DEFINER
SET search_path = public, pg_temp
AS $$
BEGIN
    DELETE FROM "Shares"
    WHERE "UserId" = userid AND "PostId" = postid;
END;
$$;

CREATE OR REPLACE PROCEDURE delete_shares(postid integer)
LANGUAGE plpgsql
SECURITY DEFINER
SET search_path = public, pg_temp
AS $$
BEGIN
    DELETE FROM "Shares"
    WHERE "PostId" = postid;
END;
$$;



CREATE OR REPLACE PROCEDURE add_report(userid integer, postid integer)
LANGUAGE plpgsql
SECURITY DEFINER
SET search_path = public, pg_temp
AS $$
BEGIN
    INSERT INTO "Reports" ("UserId", "PostId")
    VALUES (userid, postid);
END;
$$;

CREATE OR REPLACE PROCEDURE add_reports(userids integer[], postid integer)
LANGUAGE plpgsql
SECURITY DEFINER
SET search_path = public, pg_temp
AS $$
BEGIN
    INSERT INTO "Reports" ("UserId", "PostId")
    SELECT unnest(userids), postid;
END;
$$;

CREATE OR REPLACE PROCEDURE delete_report(userid integer, postid integer)
LANGUAGE plpgsql
SECURITY DEFINER
SET search_path = public, pg_temp
AS $$
BEGIN
    DELETE FROM "Reports"
    WHERE "UserId" = userid AND "PostId" = postid;
END;
$$;

CREATE OR REPLACE PROCEDURE delete_reports(postid integer)
LANGUAGE plpgsql
SECURITY DEFINER
SET search_path = public, pg_temp
AS $$
BEGIN
    DELETE FROM "Reports"
    WHERE "PostId" = postid;
END;
$$;



CREATE OR REPLACE PROCEDURE add_subscription(subscriberid integer, publisherid integer)
LANGUAGE plpgsql
SECURITY DEFINER
SET search_path = public, pg_temp
AS $$
BEGIN
    INSERT INTO "Subscriptions" ("SubscriberId", "PublisherId")
    VALUES (subscriberid, publisherid);
END;
$$;

CREATE OR REPLACE PROCEDURE delete_subscription(subscriberid integer, publisherid integer)
LANGUAGE plpgsql
SECURITY DEFINER
SET search_path = public, pg_temp
AS $$
BEGIN
    DELETE FROM "Subscriptions"
    WHERE "SubscriberId" = subscriberid AND "PublisherId" = publisherid;
END;
$$;



CREATE OR REPLACE PROCEDURE export_posts_json(path_to_file text)
LANGUAGE plpgsql
AS $$
BEGIN
    EXECUTE format('COPY (SELECT row_to_json(t) FROM (SELECT * FROM "Posts") t) TO %L', path_to_file);
END;
$$;

CREATE OR REPLACE PROCEDURE import_posts_json(path_to_file text)
LANGUAGE plpgsql
AS $$
BEGIN
	CREATE TEMP TABLE temp (data jsonb);

	EXECUTE format('COPY temp (data) FROM %L', path_to_file);

	INSERT INTO "Posts" ("Text", "CreationTime", "UserId")
	SELECT data->>'Text', (data->>'CreationTime')::timestamp with time zone, (data->>'UserId')::integer
	FROM temp;

	DROP TABLE temp;
END;
$$;

REVOKE EXECUTE ON PROCEDURE 
	add_user, update_user, update_user_status, update_user_role, update_user_password_hash, update_user_email, delete_user,
	add_post, update_post, delete_post,
	add_comment, update_comment, delete_comment,
	add_tag, add_tags, delete_tag, delete_tags,
	add_like, add_likes, delete_like, delete_likes,
	add_report, add_reports, delete_report, delete_reports,
	add_share, add_shares, delete_share, delete_shares,
	add_subscription, delete_subscription,
	export_posts_json, import_posts_json
	FROM PUBLIC;