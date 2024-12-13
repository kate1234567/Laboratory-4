using System.Data.Entity;
using System.Data.Entity.Migrations;

namespace FamilyTree.DAL
{
    public class DBHandler : DbContext
    {
        public DBHandler() : base()
        {

        }

        public DBHandler(string connectionString) : base(connectionString)
        {
            Database.CreateIfNotExists();
        }

        public DbSet<Human> Humans { get; set; }
        public DbSet<FamilyTree> FamilyTrees { get; set; }

        #region CRUD

        public void CreateOrUpdateHuman(Human human)
        {
            Humans.AddOrUpdate(human);
            SaveChanges();
        }

        public void CreateTree(FamilyTree familyTree)
        {
            FamilyTrees.Add(familyTree);
            SaveChanges();
        }

        public List<Human> GetHumans()
        {
            return Humans.Include("Parents").Include("Childrens").ToList();
        }

        public List<FamilyTree> GetFamilyTrees()
        {
            return FamilyTrees.ToList();
        }

        #endregion
    }
}
