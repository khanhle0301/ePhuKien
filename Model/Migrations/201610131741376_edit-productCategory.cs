namespace Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class editproductCategory : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.ProductCategories", "Description");
            DropColumn("dbo.ProductCategories", "HomeFlag");
        }
        
        public override void Down()
        {
            AddColumn("dbo.ProductCategories", "HomeFlag", c => c.Boolean());
            AddColumn("dbo.ProductCategories", "Description", c => c.String(maxLength: 500));
        }
    }
}
