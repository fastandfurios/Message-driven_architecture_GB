using FluentMigrator;

namespace Restaurant.Kitchen.DAL.Migrations
{
    [Migration(1)]
    public class FirstMigration : Migration
    {
        public override void Up()
        {
            Create.Table("KitchenMessages")
                .WithColumn("MessageId").AsGuid().PrimaryKey()
                .WithColumn("OrderId").AsGuid();

        }

        public override void Down()
        {
            Delete.Table("KitchenMessages");
        }
    }
}
