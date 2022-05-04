namespace SpinWheel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateSomeToUserClass : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "PhoneNumber", c => c.String());
            AddColumn("dbo.Users", "Email", c => c.String(maxLength: 50));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "Email");
            DropColumn("dbo.Users", "PhoneNumber");
        }
    }
}
