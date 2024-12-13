using FamilyTree.DAL;

namespace TestProject1
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestMethod1()
        {
            DBHandler db = new DBHandler("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=FamilyTree;Integrated Security=True;Connect Timeout=30;Encrypt=False;");
            db.Database.CreateIfNotExists();

            var p1 = new Human()
            {
                Id = 4,
                FIO = "Полина",
                BirthDay = DateTime.Now,
                Gender = Human.GenderEnum.Female,
            };
            var p2 = new Human()
            {
                Id = 3,
                FIO = "Кирилл",
                BirthDay = DateTime.Now,
                Gender = Human.GenderEnum.Male,
            };

            var human = new Human()
            {
                Id = 1,
                BirthDay = DateTime.Now,
                FIO = "Ира",
                Gender = 0,
                Childrens = new List<Human>() { p1, p2 },
                //Parents = new List<Human>() { p1,p2 },
            };

            db.CreateOrUpdateHuman(human);
            db.SaveChanges();
        }
    }
}