CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" character varying(150) NOT NULL,
    "ProductVersion" character varying(32) NOT NULL,
    CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY ("MigrationId")
);

START TRANSACTION;

CREATE TABLE "tblUser" (
    "Id" uuid NOT NULL,
    "Username" text NOT NULL,
    "EndPoint" text NOT NULL,
    CONSTRAINT "PK_tblUser" PRIMARY KEY ("Id")
);

CREATE TABLE "tblGameSeason" (
    "Id" uuid NOT NULL,
    "CreationDate" timestamp with time zone NOT NULL,
    "UserId" uuid NOT NULL,
    CONSTRAINT "PK_tblGameSeason" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_tblGameSeason_tblUser_UserId" FOREIGN KEY ("UserId") REFERENCES "tblUser" ("Id") ON DELETE CASCADE
);

CREATE TABLE "tblLeaderboard" (
    "Id" uuid NOT NULL,
    "GameSeasonId" uuid NOT NULL,
    "Rank" integer NOT NULL,
    "Score" integer NOT NULL,
    CONSTRAINT "PK_tblLeaderboard" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_tblLeaderboard_tblGameSeason_GameSeasonId" FOREIGN KEY ("GameSeasonId") REFERENCES "tblGameSeason" ("Id") ON DELETE CASCADE
);

CREATE TABLE "tblScore" (
    "Id" uuid NOT NULL,
    "CreationDate" timestamp with time zone NOT NULL,
    "GameSeasonId" uuid NOT NULL,
    "Point" integer NOT NULL,
    CONSTRAINT "PK_tblScore" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_tblScore_tblGameSeason_GameSeasonId" FOREIGN KEY ("GameSeasonId") REFERENCES "tblGameSeason" ("Id") ON DELETE CASCADE
);

CREATE UNIQUE INDEX "IX_tblGameSeason_UserId" ON "tblGameSeason" ("UserId");

CREATE UNIQUE INDEX "IX_tblLeaderboard_GameSeasonId" ON "tblLeaderboard" ("GameSeasonId");

CREATE INDEX "IX_tblScore_GameSeasonId" ON "tblScore" ("GameSeasonId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20220626083040_InitialCreate', '6.0.6');

COMMIT;

