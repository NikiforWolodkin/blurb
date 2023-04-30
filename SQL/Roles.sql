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
	ON "Users", "Likes", "Posts", "Reports", "Shares", "Subscriptions", "Comments"
	TO blurb_server;
 	
