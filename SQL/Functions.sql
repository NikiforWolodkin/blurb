CREATE OR REPLACE FUNCTION select_all_from_users()
RETURNS SETOF "Users" AS $$
  SELECT * FROM "Users";
$$ LANGUAGE sql;