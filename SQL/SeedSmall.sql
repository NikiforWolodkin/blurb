TRUNCATE TABLE "Users", "Likes", "Posts", "Reports", "Shares", "Subscriptions", "Comments", "Tags" RESTART IDENTITY;



DO $$
DECLARE
    i integer;
BEGIN
    FOR i IN 1..20 LOOP
        CALL add_user('mail' || i || '@example.com', 'password_hash', 'User ' || i);
    END LOOP;
END;
$$ LANGUAGE plpgsql;

DO $$
DECLARE
    i integer;
    user_id integer;
    post_text text;
BEGIN
    FOR i IN 1..10 LOOP
        user_id := (i - 1) % 20 + 1;
        post_text := 'This is post number ' || i || ' by user ' || user_id;
        CALL add_post(user_id, post_text);
    END LOOP;
END;
$$ LANGUAGE plpgsql;

DO $$
DECLARE
    i integer;
    j integer;
    num_likes integer;
BEGIN
    FOR i IN 1..10 LOOP
        num_likes := floor(random() * (20 - 10 + 1) + 10);
        FOR j IN 1..num_likes LOOP
            CALL add_like(j, i);
        END LOOP;
    END LOOP;
END;
$$ LANGUAGE plpgsql;

DO $$
DECLARE
    i integer;
    j integer;
    num_likes integer;
BEGIN
    FOR i IN 1..2 LOOP
        num_likes := floor(random() * (20 - 5 + 1) + 5);
        FOR j IN 1..num_likes LOOP
            CALL add_report(j, i);
        END LOOP;
    END LOOP;
END;
$$ LANGUAGE plpgsql;

DO $$
DECLARE
    i integer;
    j integer;
    num_likes integer;
BEGIN
    FOR i IN 1..3 LOOP
        num_likes := floor(random() * (20 - 5 + 1) + 5);
        FOR j IN 1..num_likes LOOP
            CALL add_share(j, i);
        END LOOP;
    END LOOP;
END;
$$ LANGUAGE plpgsql;

DO $$
DECLARE
    i integer;
    j integer;
    num_comments integer;
    comment_text text;
BEGIN
    FOR i IN 1..3 LOOP
        num_comments := floor(random() * (3 - 1 + 1) + 1);
        FOR j IN 1..num_comments LOOP
            comment_text := 'This is comment number ' || j || ' on post ' || i;
            CALL add_comment(i, j, comment_text);
        END LOOP;
    END LOOP;
END;
$$ LANGUAGE plpgsql;

DO $$
DECLARE
    i integer;
    j integer;
    num_tags integer;
    tag_text text;
BEGIN
    FOR i IN 1..2 LOOP
        num_tags := floor(random() * (5 - 1 + 1) + 1);
        FOR j IN 1..num_tags LOOP
            tag_text := 'Tag ' || j;
            CALL add_tag(i, tag_text);
        END LOOP;
    END LOOP;
END;
$$ LANGUAGE plpgsql;

SELECT * FROM get_posts_summary(1);