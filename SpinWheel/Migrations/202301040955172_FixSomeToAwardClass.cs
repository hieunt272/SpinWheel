namespace SpinWheel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class FixSomeToAwardClass : DbMigration
    {
        public override void Up()
        {
            AlterColumn("dbo.Awards", "Quantity", c => c.Int());
            AlterColumn("dbo.Awards", "Percent", c => c.Int());
        }
        
        public override void Down()
        {
            AlterColumn("dbo.Awards", "Percent", c => c.String());
            AlterColumn("dbo.Awards", "Quantity", c => c.String());
        }
    }
}
