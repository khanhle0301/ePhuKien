namespace Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class edit_postcategory : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.PostCategories", "Description");
            DropColumn("dbo.PostCategories", "ParentID");
            DropColumn("dbo.PostCategories", "DisplayOrder");
            DropColumn("dbo.PostCategories", "Image");
            DropColumn("dbo.PostCategories", "HomeFlag");
        }
        
        public override void Down()
        {
            AddColumn("dbo.PostCategories", "HomeFlag", c => c.Boolean());
            AddColumn("dbo.PostCategories", "Image", c => c.String(maxLength: 256));
            AddColumn("dbo.PostCategories", "DisplayOrder", c => c.Int());
            AddColumn("dbo.PostCategories", "ParentID", c => c.Int());
            AddColumn("dbo.PostCategories", "Description", c => c.String(maxLength: 500));
        }
    }
}
