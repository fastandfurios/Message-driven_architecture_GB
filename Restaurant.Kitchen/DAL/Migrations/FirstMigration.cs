using FluentMigrator;

namespace Restaurant.Kitchen.DAL.Migrations
{
    [Migration(2)]
    public class FirstMigration : Migration
    {
        public override void Up()
        {
            Create.Table("KitchenMessages")
                .WithColumn("OrderId").AsGuid().PrimaryKey()
                .WithColumn("MessageId").AsGuid();
        }

        public override void Down()
        {
            Delete.Table("KitchenMessages");
        }
    }
}
