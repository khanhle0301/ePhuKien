namespace Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class ColorIDProduct : DbMigration
    {
        public override void Up()
        {
            RenameColumn(table: "dbo.ProductColors", name: "SizeID", newName: "ColorID");
            RenameIndex(table: "dbo.ProductColors", name: "IX_SizeID", newName: "IX_ColorID");
        }
        
        public override void Down()
        {
            RenameIndex(table: "dbo.ProductColors", name: "IX_ColorID", newName: "IX_SizeID");
            RenameColumn(table: "dbo.ProductColors", name: "ColorID", newName: "SizeID");
        }
    }
}
