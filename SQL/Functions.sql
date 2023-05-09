CREATE OR REPLACE FUNCTION get_users_summary(id integer DEFAULT NULL)
RETURNS SETOF "UsersSummary" 
SECURITY DEFINER
SET search_path = public, pg_temp
AS $$
BEGIN
    IF id IS NULL THEN
        RETURN QUERY SELECT * FROM "UsersSummary";
    ELSE
        RETURN QUERY SELECT * FROM "UsersSummary" WHERE "Id" = id;
    END IF;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION get_users_summary_by_registration_date(start_date timestamp with time zone, end_date timestamp with time zone)
RETURNS SETOF "UsersSummary"
SECURITY DEFINER
SET search_path = public, pg_temp
AS $$
BEGIN
    RETURN QUERY SELECT * FROM "UsersSummary" WHERE "RegistrationDate" BETWEEN start_date AND end_date;
END;
$$ LANGUAGE plpgsql;



CREATE OR REPLACE FUNCTION get_posts_summary(id integer DEFAULT NULL)
RETURNS SETOF "PostsSummary" 
SECURITY DEFINER
SET search_path = public, pg_temp
AS $$
BEGIN
    IF id IS NULL THEN
        RETURN QUERY SELECT * FROM "PostsSummary";
    ELSE
        RETURN QUERY SELECT * FROM "PostsSummary" WHERE "Id" = id;
    END IF;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION get_posts_summary_by_creation_date(start_date timestamp with time zone, end_date timestamp with time zone)
RETURNS SETOF "PostsSummary" 
SECURITY DEFINER
SET search_path = public, pg_temp
AS $$
BEGIN
    RETURN QUERY SELECT * FROM "PostsSummary" WHERE "CreationTime" BETWEEN start_date AND end_date;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION get_trending_posts()
RETURNS SETOF "PostsSummary" 
SECURITY DEFINER
SET search_path = public, pg_temp
AS $$
BEGIN
    RETURN QUERY SELECT * FROM "PostsSummary" WHERE "CreationTime" > NOW() - INTERVAL '1 week' ORDER BY "LikeCount" DESC LIMIT 50;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION get_reported_posts()
RETURNS SETOF "PostsSummary" 
SECURITY DEFINER
SET search_path = public, pg_temp
AS $$
BEGIN
    RETURN QUERY SELECT * FROM "PostsSummary" WHERE "ReportCount" > 0 ORDER BY "ReportCount" DESC;
END;
$$ LANGUAGE plpgsql;



CREATE OR REPLACE FUNCTION get_comments_on_post(post_id integer)
RETURNS SETOF "Comments"
SECURITY DEFINER
SET search_path = public, pg_temp
AS $$
BEGIN
    RETURN QUERY
    SELECT * FROM "Comments" WHERE "PostId" = post_id;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION get_comments_from_user(user_id integer)
RETURNS SETOF "Comments"
SECURITY DEFINER
SET search_path = public, pg_temp
AS $$
BEGIN
    RETURN QUERY
    SELECT * FROM "Comments" WHERE "UserId" = user_id;
END;
$$ LANGUAGE plpgsql;



CREATE OR REPLACE FUNCTION get_subscriptions_for_publisher(publisher_id integer)
RETURNS SETOF "Subscriptions"
SECURITY DEFINER
SET search_path = public, pg_temp
AS $$
BEGIN
    RETURN QUERY
    SELECT * FROM "Subscriptions" WHERE "PublisherId" = publisher_id;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION get_subscriptions_of_user(subscriber_id integer)
RETURNS SETOF "Subscriptions"
SECURITY DEFINER
SET search_path = public, pg_temp
AS $$
BEGIN
    RETURN QUERY
    SELECT * FROM "Subscriptions" WHERE "SubscriberId" = subscriber_id;
END;
$$ LANGUAGE plpgsql;



CREATE OR REPLACE FUNCTION get_post_likes(post_id integer)
RETURNS SETOF "Likes"
SECURITY DEFINER
SET search_path = public, pg_temp
AS $$
BEGIN
    RETURN QUERY
    SELECT * FROM "Likes" WHERE "PostId" = post_id;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION get_user_likes(user_id integer)
RETURNS SETOF "Likes"
SECURITY DEFINER
SET search_path = public, pg_temp
AS $$
BEGIN
    RETURN QUERY
    SELECT * FROM "Likes" WHERE "UserId" = user_id;
END;
$$ LANGUAGE plpgsql;



CREATE OR REPLACE FUNCTION get_post_reports(post_id integer)
RETURNS SETOF "Reports"
SECURITY DEFINER
SET search_path = public, pg_temp
AS $$
BEGIN
    RETURN QUERY
    SELECT * FROM "Reports" WHERE "PostId" = post_id;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION get_user_reports(user_id integer)
RETURNS SETOF "Reports"
SECURITY DEFINER
SET search_path = public, pg_temp
AS $$
BEGIN
    RETURN QUERY
    SELECT * FROM "Reports" WHERE "UserId" = user_id;
END;
$$ LANGUAGE plpgsql;



CREATE OR REPLACE FUNCTION get_post_shares(post_id integer)
RETURNS SETOF "Shares"
SECURITY DEFINER
SET search_path = public, pg_temp
AS $$
BEGIN
    RETURN QUERY
    SELECT * FROM "Shares" WHERE "PostId" = post_id;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION get_user_shares(user_id integer)
RETURNS SETOF "Shares"
SECURITY DEFINER
SET search_path = public, pg_temp
AS $$
BEGIN
    RETURN QUERY
    SELECT * FROM "Shares" WHERE "UserId" = user_id;
END;
$$ LANGUAGE plpgsql;



CREATE OR REPLACE FUNCTION get_post_tags(post_id integer)
RETURNS SETOF "Tags"
SECURITY DEFINER
SET search_path = public, pg_temp
AS $$
BEGIN
    RETURN QUERY
    SELECT * FROM "Tags" WHERE "PostId" = post_id;
END;
$$ LANGUAGE plpgsql;

CREATE OR REPLACE FUNCTION get_popular_tags()
RETURNS TABLE (Id integer, Text text, PostId integer, Usage integer)
SECURITY DEFINER
SET search_path = public, pg_temp
AS $$
BEGIN
    RETURN QUERY
    SELECT "Tags"."Text", COUNT("Tags"."PostId") AS "Usage"
    FROM "Tags"
    GROUP BY "Tags"."Text"
    ORDER BY COUNT("Tags"."PostId") DESC;
END;
$$ LANGUAGE plpgsql;

REVOKE EXECUTE ON FUNCTION 
	get_users_summary, get_users_summary_by_registration_date,
	get_posts_summary, get_trending_posts, get_reported_posts,
	get_comments_on_post, get_comments_from_user,
	get_subscriptions_for_publisher, get_subscriptions_of_user,
	get_post_likes, get_user_likes,
	get_post_reports, get_user_reports,
	get_post_shares, get_user_shares,
	get_post_tags, get_popular_tags
	FROM PUBLIC;