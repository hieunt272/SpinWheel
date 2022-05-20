namespace SpinWheel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class DeleteMobileToContactClass : DbMigration
    {
        public override void Up()
        {
            DropColumn("dbo.Contacts", "Address");
            DropColumn("dbo.Contacts", "Mobile");
        }
        
        public override void Down()
        {
            AddColumn("dbo.Contacts", "Mobile", c => c.String(nullable: false, maxLength: 20));
            AddColumn("dbo.Contacts", "Address", c => c.String(maxLength: 200));
        }
    }
}
