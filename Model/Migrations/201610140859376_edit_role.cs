namespace Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class edit_role : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Credentials", "Status");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Credentials", "Status", c => c.Boolean(nullable: false));
        }
    }
}
