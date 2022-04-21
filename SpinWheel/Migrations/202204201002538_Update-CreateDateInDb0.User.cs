namespace SpinWheel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class UpdateCreateDateInDb0User : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Users", "CreateDate", c => c.DateTime(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Users", "CreateDate");
        }
    }
}
