-- Tabela de atribuição de papéis por usuário e escopo (multi-tenant)
CREATE TABLE IF NOT EXISTS "UserRoleAssignments" (
    "Id" uuid PRIMARY KEY,
    "TenantId" uuid NOT NULL,
    "UserId" uuid NOT NULL,
    "Role" int NOT NULL,
    "ScopeType" int NOT NULL,
    "LocalChurchId" uuid NULL,
    "ChurchUnitId" uuid NULL,
    "CreatedAt" timestamptz NOT NULL,
    "CreatedBy" varchar(450) NULL,
    "UpdatedAt" timestamptz NULL,
    "UpdatedBy" varchar(450) NULL,
    "DeletedAt" timestamptz NULL,
    "DeletedBy" varchar(450) NULL,
    CONSTRAINT "CK_UserRoleAssignments_Scope_LocalChurch"
        CHECK (("ScopeType" <> 2) OR ("LocalChurchId" IS NOT NULL)),
    CONSTRAINT "CK_UserRoleAssignments_Scope_ChurchUnit"
        CHECK (("ScopeType" <> 3) OR ("ChurchUnitId" IS NOT NULL))
);

CREATE INDEX IF NOT EXISTS "IX_UserRoleAssignments_Tenant_User"
    ON "UserRoleAssignments" ("TenantId", "UserId");

CREATE INDEX IF NOT EXISTS "IX_UserRoleAssignments_LocalChurch"
    ON "UserRoleAssignments" ("TenantId", "LocalChurchId");

CREATE INDEX IF NOT EXISTS "IX_UserRoleAssignments_ChurchUnit"
    ON "UserRoleAssignments" ("TenantId", "ChurchUnitId");
