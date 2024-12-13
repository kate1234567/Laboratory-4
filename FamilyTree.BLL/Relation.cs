using FamilyTree.DAL;
using System.Text;

namespace FamilyTree.BLL
{
    public class Relation
    {
        public Human SelectedHuman { get; set; }
        public string Relations { get; set; }
        public Human ForHuman { get; set; }
        public Human HumanForRelation { get; set; }
        public Human HumanForGetAge { get; set; }
        public Common Common { get; set; } = new Common();

        public static void SetRelation(DBHandler dBHandler, Relation relation)
        {
            if (relation.Relations == "Родители")
            {
                relation.ForHuman.Parents.Add(relation.SelectedHuman);
            }
            else
            {
                relation.ForHuman.Childrens.Add(relation.SelectedHuman);
            }

            dBHandler.CreateOrUpdateHuman(relation.ForHuman);
            dBHandler.SaveChanges();
        }

        public string GetRelation()
        {
            var sb = new StringBuilder();

            sb.Append($"У {HumanForRelation.FIO} ");

            if (HumanForRelation.Parents.Any())
            {
                sb.Append($"родители: {string.Join(", ", HumanForRelation.Parents.Select(x => x.FIO))}");
                sb.AppendLine();
            }
            if (HumanForRelation.Childrens.Any())
            {
                sb.Append($"дети: {string.Join(", ", HumanForRelation.Childrens.Select(x => x.FIO))}");
                sb.AppendLine();
            }

            return sb.ToString();
        }

        public string GetAge()
        {
            var age = HumanForGetAge.Childrens.First().BirthDay.Year - HumanForGetAge.BirthDay.Year;
            if (HumanForGetAge.Childrens.First().BirthDay < HumanForGetAge.BirthDay.AddYears(age))
            {
                age--;
            }
            return $"На меномент рождения '{HumanForGetAge.Childrens.First().FIO}', '{HumanForGetAge.FIO}' было {age}";
        }

        public string GetCommon()
        {
            var inter = Common.C1.Parents.Intersect(Common.C2.Parents).ToList();
            if (inter.Any())
            {
                return $"Общие родители: {string.Join(", ", inter.Select(x => x.FIO))}";
            }
            else
            {
                return "Общие родители не найдены";
            }
        }
    }


    public class Common
    {
        public Human C1 { get; set; }
        public Human C2 { get; set; }
    }

}
