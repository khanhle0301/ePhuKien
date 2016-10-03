namespace Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AccountCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Credentials",
                c => new
                    {
                        UserGroupID = c.String(nullable: false, maxLength: 20, unicode: false),
                        RoleID = c.String(nullable: false, maxLength: 50, unicode: false),
                        Status = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => new { t.UserGroupID, t.RoleID })
                .ForeignKey("dbo.Roles", t => t.RoleID, cascadeDelete: true)
                .ForeignKey("dbo.UserGroups", t => t.UserGroupID, cascadeDelete: true)
                .Index(t => t.UserGroupID)
                .Index(t => t.RoleID);
            
            CreateTable(
                "dbo.Roles",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 50, unicode: false),
                        Name = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.UserGroups",
                c => new
                    {
                        ID = c.String(nullable: false, maxLength: 20, unicode: false),
                        Name = c.String(nullable: false, maxLength: 50),
                        Status = c.Boolean(),
                    })
                .PrimaryKey(t => t.ID);
            
            CreateTable(
                "dbo.Users",
                c => new
                    {
                        ID = c.Int(nullable: false, identity: true),
                        UserName = c.String(nullable: false, maxLength: 50, unicode: false),
                        Password = c.String(nullable: false, maxLength: 50, unicode: false),
                        GroupID = c.String(nullable: false, maxLength: 20, unicode: false),
                        Name = c.String(nullable: false, maxLength: 100),
                        Address = c.String(maxLength: 200),
                        Email = c.String(maxLength: 200),
                        Phone = c.String(maxLength: 50),
                    })
                .PrimaryKey(t => t.ID)
                .ForeignKey("dbo.UserGroups", t => t.GroupID, cascadeDelete: true)
                .Index(t => t.GroupID);
            
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.Users", "GroupID", "dbo.UserGroups");
            DropForeignKey("dbo.Credentials", "UserGroupID", "dbo.UserGroups");
            DropForeignKey("dbo.Credentials", "RoleID", "dbo.Roles");
            DropIndex("dbo.Users", new[] { "GroupID" });
            DropIndex("dbo.Credentials", new[] { "RoleID" });
            DropIndex("dbo.Credentials", new[] { "UserGroupID" });
            DropTable("dbo.Users");
            DropTable("dbo.UserGroups");
            DropTable("dbo.Roles");
            DropTable("dbo.Credentials");
        }
    }
}
