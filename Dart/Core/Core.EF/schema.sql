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

CREATE TABLE "tblGameSession" (
    "Id" uuid NOT NULL,
    "CreationDate" timestamp with time zone NOT NULL,
    "UserId" uuid NOT NULL,
    CONSTRAINT "PK_tblGameSession" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_tblGameSession_tblUser_UserId" FOREIGN KEY ("UserId") REFERENCES "tblUser" ("Id") ON DELETE CASCADE
);

CREATE TABLE "tblLeaderboard" (
    "Id" uuid NOT NULL,
    "GameSessionId" uuid NOT NULL,
    "Rank" integer NOT NULL,
    "Score" integer NOT NULL,
    CONSTRAINT "PK_tblLeaderboard" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_tblLeaderboard_tblGameSession_GameSessionId" FOREIGN KEY ("GameSessionId") REFERENCES "tblGameSession" ("Id") ON DELETE CASCADE
);

CREATE TABLE "tblScore" (
    "Id" uuid NOT NULL,
    "CreationDate" timestamp with time zone NOT NULL,
    "GameSessionId" uuid NOT NULL,
    "Point" integer NOT NULL,
    CONSTRAINT "PK_tblScore" PRIMARY KEY ("Id"),
    CONSTRAINT "FK_tblScore_tblGameSession_GameSessionId" FOREIGN KEY ("GameSessionId") REFERENCES "tblGameSession" ("Id") ON DELETE CASCADE
);

CREATE UNIQUE INDEX "IX_tblGameSession_UserId" ON "tblGameSession" ("UserId");

CREATE UNIQUE INDEX "IX_tblLeaderboard_GameSessionId" ON "tblLeaderboard" ("GameSessionId");

CREATE INDEX "IX_tblScore_GameSessionId" ON "tblScore" ("GameSessionId");

INSERT INTO "__EFMigrationsHistory" ("MigrationId", "ProductVersion")
VALUES ('20220627093740_InitialCreate', '6.0.6');

COMMIT;

