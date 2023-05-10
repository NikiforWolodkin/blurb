DROP INDEX "IX_Likes_PostId";

EXPLAIN SELECT COUNT(*) FROM "Likes" WHERE "PostId" = 100;

CREATE INDEX "IX_Likes_PostId" ON "Likes" ("PostId");

EXPLAIN SELECT COUNT(*) FROM "Likes" WHERE "PostId" = 100;



EXPLAIN SELECT * FROM "Likes" WHERE "PostId" = 65 AND "UserId" = 1200;