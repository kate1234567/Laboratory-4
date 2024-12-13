namespace FamilyTree.DAL
{
    public class FamilyTree
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public FamilyTree()
        {

        }

        public FamilyTree(string title)
        {
            Title = title;
        }
    }
}
