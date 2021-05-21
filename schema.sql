CREATE TABLE IF NOT EXISTS "Templates" (
    "TemplateID" INTEGER NOT NULL CONSTRAINT "PK_Templates" PRIMARY KEY AUTOINCREMENT,
    "Name" TEXT NULL
);
CREATE TABLE sqlite_sequence(name,seq);
CREATE TABLE IF NOT EXISTS "TemplateParameters" (
    "TemplateParameterID" INTEGER NOT NULL CONSTRAINT "PK_TemplateParameters" PRIMARY KEY AUTOINCREMENT,
    "TemplateID" INTEGER NOT NULL,
    "Name" TEXT NULL,
    "Type" INTEGER NOT NULL,
    "Value" TEXT NULL,
    "AllowMultiple" INTEGER NOT NULL,
    CONSTRAINT "FK_TemplateParameters_Templates_TemplateID" FOREIGN KEY ("TemplateID") REFERENCES "Templates" ("TemplateID") ON DELETE CASCADE
);
CREATE TABLE IF NOT EXISTS "TemplateParameterDefaults" (
    "ID" INTEGER NOT NULL CONSTRAINT "PK_TemplateParameterDefaults" PRIMARY KEY AUTOINCREMENT,
    "TemplateParameterID" INTEGER NOT NULL,
    "Value" TEXT NULL,
    "DisplayIndex" INTEGER NOT NULL,
    "TemplateParametersTemplateParameterID" INTEGER NULL,
    CONSTRAINT "FK_TemplateParameterDefaults_TemplateParameters_TemplateParametersTemplateParameterID" FOREIGN KEY ("TemplateParametersTemplateParameterID") REFERENCES "TemplateParameters" ("TemplateParameterID") ON DELETE RESTRICT
);
CREATE TABLE IF NOT EXISTS "TemplateParameterValues" (
    "ID" INTEGER NOT NULL CONSTRAINT "PK_TemplateParameterValues" PRIMARY KEY AUTOINCREMENT,
    "TemplateParameterID" INTEGER NOT NULL,
    "Label" TEXT NULL,
    "Value" TEXT NULL,
    "DisplayIndex" INTEGER NOT NULL,
    "TemplateParametersTemplateParameterID" INTEGER NULL,
    CONSTRAINT "FK_TemplateParameterValues_TemplateParameters_TemplateParametersTemplateParameterID" FOREIGN KEY ("TemplateParametersTemplateParameterID") REFERENCES "TemplateParameters" ("TemplateParameterID") ON DELETE RESTRICT
);
CREATE INDEX "IX_TemplateParameterDefaults_TemplateParametersTemplateParameterID" ON "TemplateParameterDefaults" ("TemplateParametersTemplateParameterID");
CREATE INDEX "IX_TemplateParameters_TemplateID" ON "TemplateParameters" ("TemplateID");
CREATE INDEX "IX_TemplateParameterValues_TemplateParametersTemplateParameterID" ON "TemplateParameterValues" ("TemplateParametersTemplateParameterID");
CREATE TABLE IF NOT EXISTS "__EFMigrationsHistory" (
    "MigrationId" TEXT NOT NULL CONSTRAINT "PK___EFMigrationsHistory" PRIMARY KEY,
    "ProductVersion" TEXT NOT NULL
);