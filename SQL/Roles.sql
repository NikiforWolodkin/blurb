CREATE ROLE blurb_admin
	WITH LOGIN ENCRYPTED PASSWORD 'SYSTEM' 
	SUPERUSER CREATEDB CREATEROLE
	VALID UNTIL '2023-04-01';
	
CREATE ROLE blurb_moderator
	WITH LOGIN ENCRYPTED PASSWORD 'SYSTEM'
	VALID UNTIL '2023-04-01';
	
CREATE ROLE blurb_user
	WITH LOGIN ENCRYPTED PASSWORD 'SYSTEM'
	VALID UNTIL '2023-04-01';
	
CREATE ROLE blurb_server
	WITH LOGIN ENCRYPTED PASSWORD 'SYSTEM'
	VALID UNTIL '2023-04-01';
	
CREATE GROUP blurb_roles;

ALTER GROUP blurb_roles 
	ADD USER blurb_admin, blurb_moderator, blurb_user, blurb_server;

	GRANT ALL 
		ON "Users", "Likes", "Posts", "Reports", "Shares", "Subscriptions", "Comments", "Tags", 
		"UserUpdateLogs", "PostUpdateLogs", "CommentUpdateLogs"
		TO blurb_server;

	GRANT EXECUTE ON PROCEDURE 
		add_user, update_user, update_user_password_hash, update_user_email,
		add_post, update_post,
		add_comment, update_comment,
		add_tag, add_tags,
		add_like, add_likes, delete_like, delete_likes,
		add_report, add_reports,
		add_share, add_shares,
		add_subscription, delete_subscription
		TO blurb_roles;

	GRANT EXECUTE ON PROCEDURE
		update_user_status, delete_user,
		delete_post, delete_comment, delete_tag, delete_tags,
		delete_report, delete_reports, delete_share, delete_shares
		TO blurb_admin, blurb_moderator;

	GRANT EXECUTE ON PROCEDURE update_user_role TO blurb_admin;

	GRANT EXECUTE ON FUNCTION 
		get_users_summary, get_users_summary_by_registration_date,
		get_posts_summary, get_trending_posts, get_reported_posts,
		get_comments_on_post, get_comments_from_user,
		get_subscriptions_for_publisher, get_subscriptions_of_user,
		get_post_likes, get_user_likes,
		get_post_reports, get_user_reports,
		get_post_shares, get_user_shares,
		get_post_tags, get_popular_tags
		TO blurb_roles;