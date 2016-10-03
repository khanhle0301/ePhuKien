namespace Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ColorProduct : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Feedbacks", "Phone", c => c.String(maxLength: 25));
            AddColumn("dbo.Products", "Colors", c => c.String());
            DropColumn("dbo.Products", "Sizes");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "Sizes", c => c.String());
            DropColumn("dbo.Products", "Colors");
            DropColumn("dbo.Feedbacks", "Phone");
        }
    }
}
