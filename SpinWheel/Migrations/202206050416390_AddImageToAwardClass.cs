namespace SpinWheel.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class AddImageToAwardClass : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Awards", "Image", c => c.String(maxLength: 500));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Awards", "Image");
        }
    }
}
