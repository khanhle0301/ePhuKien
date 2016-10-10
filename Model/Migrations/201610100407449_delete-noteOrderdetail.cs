namespace Model.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class deletenoteOrderdetail : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.OrderDetails", "Note", c => c.String(maxLength: 500));
            DropColumn("dbo.OrderDetails", "NoteOrderDetail");
        }
        
        public override void Down()
        {
            AddColumn("dbo.OrderDetails", "NoteOrderDetail", c => c.String(maxLength: 500));
            DropColumn("dbo.OrderDetails", "Note");
        }
    }
}
