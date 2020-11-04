namespace Assignment5.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class add_media_item_to_artist : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.MediaItems", "Artist_Id", c => c.Int());
            CreateIndex("dbo.MediaItems", "Artist_Id");
            AddForeignKey("dbo.MediaItems", "Artist_Id", "dbo.Artists", "Id");
        }
        
        public override void Down()
        {
            DropForeignKey("dbo.MediaItems", "Artist_Id", "dbo.Artists");
            DropIndex("dbo.MediaItems", new[] { "Artist_Id" });
            DropColumn("dbo.MediaItems", "Artist_Id");
        }
    }
}
