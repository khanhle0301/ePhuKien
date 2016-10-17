namespace Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Edit_ProductColor : DbMigration
    {
        public override void Up()
        {
            DropForeignKey("dbo.ProductColors", "ColorID", "dbo.Colors");
            DropIndex("dbo.ProductColors", new[] { "ColorID" });
            DropPrimaryKey("dbo.Colors");
            DropPrimaryKey("dbo.ProductColors");
            AlterColumn("dbo.Colors", "ID", c => c.Int(nullable: false, identity: true));
            AlterColumn("dbo.ProductColors", "ColorID", c => c.Int(nullable: false));
            AddPrimaryKey("dbo.Colors", "ID");
            AddPrimaryKey("dbo.ProductColors", new[] { "ProductID", "ColorID" });
            CreateIndex("dbo.ProductColors", "ColorID");
            AddForeignKey("dbo.ProductColors", "ColorID", "dbo.Colors", "ID", cascadeDelete: true);
            DropColumn("dbo.Products", "Colors");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Products", "Colors", c => c.String());
            DropForeignKey("dbo.ProductColors", "ColorID", "dbo.Colors");
            DropIndex("dbo.ProductColors", new[] { "ColorID" });
            DropPrimaryKey("dbo.ProductColors");
            DropPrimaryKey("dbo.Colors");
            AlterColumn("dbo.ProductColors", "ColorID", c => c.String(nullable: false, maxLength: 50, unicode: false));
            AlterColumn("dbo.Colors", "ID", c => c.String(nullable: false, maxLength: 50, unicode: false));
            AddPrimaryKey("dbo.ProductColors", new[] { "ProductID", "ColorID" });
            AddPrimaryKey("dbo.Colors", "ID");
            CreateIndex("dbo.ProductColors", "ColorID");
            AddForeignKey("dbo.ProductColors", "ColorID", "dbo.Colors", "ID", cascadeDelete: true);
        }
    }
}
