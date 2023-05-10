TRUNCATE TABLE "Users", "Likes", "Posts", "Reports", "Shares", "Subscriptions", "Comments", "Tags" RESTART IDENTITY;



DO $$
DECLARE
    i integer;
BEGIN
    FOR i IN 1..2000 LOOP
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
    FOR i IN 1..100 LOOP
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
    FOR i IN 1..100 LOOP
        num_likes := floor(random() * (2000 - 1000 + 1) + 1000);
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
    FOR i IN 1..10 LOOP
        num_likes := floor(random() * (200 - 50 + 1) + 50);
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
    FOR i IN 1..50 LOOP
        num_likes := floor(random() * (200 - 50 + 1) + 50);
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
    FOR i IN 1..20 LOOP
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
    FOR i IN 1..40 LOOP
        num_tags := floor(random() * (5 - 1 + 1) + 1);
        FOR j IN 1..num_tags LOOP
            tag_text := 'Tag ' || j;
            CALL add_tag(i, tag_text);
        END LOOP;
    END LOOP;
END;
$$ LANGUAGE plpgsql;

