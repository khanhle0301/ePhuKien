namespace Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class backgroundColor : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Colors", "Background", c => c.String());
        }
        
        public override void Down()
        {
            DropColumn("dbo.Colors", "Background");
        }
    }
}
