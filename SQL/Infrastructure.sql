CREATE TABLESPACE blurb_ts 
	OWNER blurb_admin
	LOCATION '/var/lib/postgresql/data';
	
CREATE DATABASE blurb_db
	WITH OWNER blurb_admin
	ENCODING 'UTF8'
	TABLESPACE blurb_ts;

