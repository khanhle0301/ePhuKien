namespace Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class edit_userGroup : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.Credentials", "UserGroupID", "dbo.UserGroups");
            DropForeignKey("dbo.Users", "GroupID", "dbo.UserGroups");
            DropIndex("dbo.Credentials", new[] { "UserGroupID" });
            DropIndex("dbo.Users", new[] { "GroupID" });
            DropPrimaryKey("dbo.Credentials");
            DropPrimaryKey("dbo.UserGroups");
            AlterColumn("dbo.Credentials", "UserGroupID", c => c.Int(nullable: false));
            AlterColumn("dbo.UserGroups", "ID", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.Users", "GroupID", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Credentials", new[] { "UserGroupID", "RoleID" });
            AddPrimaryKey("dbo.UserGroups", "ID");
            CreateIndex("dbo.Credentials", "UserGroupID");
            CreateIndex("dbo.Users", "GroupID");
            AddForeignKey("dbo.Credentials", "UserGroupID", "dbo.UserGroups", "ID", cascadeDelete: true);
            AddForeignKey("dbo.Users", "GroupID", "dbo.UserGroups", "ID", cascadeDelete: true);
            DropColumn("dbo.UserGroups", "Status");
        }
        
        public override void Down()
        {
            AddColumn("dbo.UserGroups", "Status", c => c.Boolean());
            DropForeignKey("dbo.Users", "GroupID", "dbo.UserGroups");
            DropForeignKey("dbo.Credentials", "UserGroupID", "dbo.UserGroups");
            DropIndex("dbo.Users", new[] { "GroupID" });
            DropIndex("dbo.Credentials", new[] { "UserGroupID" });
            DropPrimaryKey("dbo.UserGroups");
            DropPrimaryKey("dbo.Credentials");
            AlterColumn("dbo.Users", "GroupID", c => c.String(nullable: false, maxLength: 20, unicode: false));
            AlterColumn("dbo.UserGroups", "ID", c => c.String(nullable: false, maxLength: 20, unicode: false));
            AlterColumn("dbo.Credentials", "UserGroupID", c => c.String(nullable: false, maxLength: 20, unicode: false));
            AddPrimaryKey("dbo.UserGroups", "ID");
            AddPrimaryKey("dbo.Credentials", new[] { "UserGroupID", "RoleID" });
            CreateIndex("dbo.Users", "GroupID");
            CreateIndex("dbo.Credentials", "UserGroupID");
            AddForeignKey("dbo.Users", "GroupID", "dbo.UserGroups", "ID", cascadeDelete: true);
            AddForeignKey("dbo.Credentials", "UserGroupID", "dbo.UserGroups", "ID", cascadeDelete: true);
        }
    }
}
