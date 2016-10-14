namespace Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class edit_post : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Posts", "HomeFlag");
            DropColumn("dbo.Posts", "HotFlag");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Posts", "HotFlag", c => c.Boolean());
            AddColumn("dbo.Posts", "HomeFlag", c => c.Boolean());
        }
    }
}
