CREATE OR REPLACE VIEW "UsersSummary" AS
	SELECT "Users"."Id", "Users"."Username",
	"Users"."Avatar", "Users"."ProfileColor", "Users"."RegistrationDate", "Users"."Status", "Users"."Role",
		(SELECT COUNT(*) FROM "Posts" WHERE "Posts"."UserId" = "Users"."Id") AS "PostCount",
		(SELECT COUNT(*) FROM "Comments" WHERE "Comments"."UserId" = "Users"."Id") AS "CommentCount",
		(SELECT COUNT(*) FROM "Likes" WHERE "Likes"."UserId" = "Users"."Id") AS "LikeCount",
		(SELECT COUNT(*) FROM "Reports" WHERE "Reports"."UserId" = "Users"."Id") AS "ReportCount",
		(SELECT COUNT(*) FROM "Shares" WHERE "Shares"."UserId" = "Users"."Id") AS "ShareCount",
		(SELECT COUNT(*) FROM "Subscriptions" WHERE "Subscriptions"."SubscriberId" = "Users"."Id") AS "SubscriptionCount"
	FROM "Users";

CREATE OR REPLACE VIEW "PostsSummary" AS
	SELECT "Posts".*, "Users"."Username",
		(SELECT COUNT(*) FROM "Comments" WHERE "Comments"."PostId" = "Posts"."Id") AS "CommentCount",
		(SELECT COUNT(*) FROM "Likes" WHERE "Likes"."PostId" = "Posts"."Id") AS "LikeCount",
		(SELECT COUNT(*) FROM "Reports" WHERE "Reports"."PostId" = "Posts"."Id") AS "ReportCount",
		(SELECT COUNT(*) FROM "Shares" WHERE "Shares"."PostId" = "Posts"."Id") AS "ShareCount",
		(SELECT COUNT(*) FROM "Tags" WHERE "Tags"."PostId" = "Posts"."Id") AS "TagCount"
	FROM "Posts"
	JOIN "Users" ON "Posts"."UserId" = "Users"."Id";