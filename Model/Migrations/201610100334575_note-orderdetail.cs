namespace Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class noteorderdetail : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderDetails", "Price", c => c.Decimal(nullable: false, precision: 18, scale: 2));
            AddColumn("dbo.OrderDetails", "NoteOrderDetail", c => c.String(maxLength: 500));
            AlterColumn("dbo.Orders", "PaymentStatus", c => c.Boolean(nullable: false));
            DropColumn("dbo.Orders", "CreatedBy");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Orders", "CreatedBy", c => c.String());
            AlterColumn("dbo.Orders", "PaymentStatus", c => c.String());
            DropColumn("dbo.OrderDetails", "NoteOrderDetail");
            DropColumn("dbo.OrderDetails", "Price");
        }
    }
}
